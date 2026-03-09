# TODO

Captured review notes on 2026-03-08 so the current GitHub issue/PR analysis is preserved before digging further into the test failures.

## Priority Order

Recommended implementation order based on current evidence:

1. Merge or replicate PR #92.
2. Fix the sharing API gaps raised by Issues #90 and #94 together.
3. Fix Issue #93.
4. Clarify and possibly improve item hydration behavior related to Issue #91.
5. Reproduce Issue #85 before committing to a code change.
6. Revisit PR #84 only if attachment metadata matters soon.

## Open Pull Requests

### PR #92 - Replace items deletion with archiving

Link:
- https://github.com/jscarle/OnePassword.NET/pull/92

Conclusion:
- Valid bug fix.
- High priority.
- Safe and narrowly scoped.

Why it is valid:
- `ArchiveDocument(IDocument document, IVault vault)` currently calls `DeleteDocument(document.Id, vault.Id)` instead of delegating to `ArchiveDocument(document.Id, vault.Id)`.
- The string overload is already correct and executes `document delete {documentId} --vault {vaultId} --archive`.

Code evidence:
- `OnePassword.NET/OnePasswordManager.Documents.cs`
- Current bug location: `ArchiveDocument(IDocument, IVault)` at line 224 currently forwards to `DeleteDocument(...)` at line 231.
- Correct archive behavior already exists in `ArchiveDocument(string, string)` at line 235.

Impact:
- Callers who ask to archive a document through the object overload may permanently delete it instead.
- This is a behavioral bug, not just a code-style inconsistency.

Recommendation:
- Merge the PR or implement the same fix locally.
- Add a regression test for both archive overloads.
- Consider adding analogous archive tests for items as well, because there are currently no archive-specific tests in the suite.

Additional note:
- The PR title mentions items, but the actual diff only fixes document archiving.

### PR #84 - Add Support for Referencing Files Object in Item Class

Link:
- https://github.com/jscarle/OnePassword.NET/pull/84

Conclusion:
- Reasonable additive feature.
- Lower priority than the current bug fixes.
- Not merge-ready as-is if the bar includes tests and docs.

What it changes:
- Adds `OnePassword.NET/Items/File.cs`.
- Adds `TrackedList<File> Files` to `OnePassword.NET/Items/ItemBase.cs`.

Strengths:
- The change is additive and should not break existing consumers.
- It appears aligned with actual 1Password item JSON containing file metadata.
- Read-only access to attachment metadata is a useful capability.

Gaps and concerns:
- No tests were added.
- No documentation or README examples were added.
- The public type name `File` is easy to confuse with `System.IO.File`.
- The PR explicitly does not attempt editing support for files, so the feature is limited to inspection/deserialization.
- There are no CI checks reported on the branch.

Code evidence:
- Added model: `OnePassword.NET/Items/File.cs`
- Added property: `OnePassword.NET/Items/ItemBase.cs`

Recommendation:
- Only prioritize this if attachment metadata is important in near-term work.
- Before merging:
  - Add at least one deserialization test proving the `files` JSON payload round-trips into `Item.Files`.
  - Add a short README or docfx note so consumers know this is read-only metadata.
  - Consider whether the public type name should remain `File` or whether documentation should explicitly call out namespace qualification.

## Open Issues

### Issue #94 - ShareItem requires email

Link:
- https://github.com/jscarle/OnePassword.NET/issues/94

Conclusion:
- Valid issue.
- Should be fixed together with Issue #90 because both problems come from the same API design.

Why it is valid:
- `ShareItem(string itemId, string vaultId, IReadOnlyCollection<string> emailAddresses, ...)` always appends `--emails {joinedEmails}`.
- There is no code path that omits `--emails` when the caller wants an unrestricted link.

Code evidence:
- `OnePassword.NET/OnePasswordManager.Items.cs`
- Relevant implementation: line 285.
- Unconditional `--emails` emission: line 292.
- Public API only exposes email-requiring overloads in `OnePassword.NET/IOnePasswordManager.Items.cs` lines 146-180.

Impact:
- Consumers cannot create a general share link without binding it to one or more email addresses.
- Passing an empty collection still produces `--emails `, which is likely invalid or at least not intentionally supported.

Recommendation:
- Add overloads that do not require email addresses.
- In the collection overload, omit `--emails` when the collection is null or empty.
- Preserve existing overloads for backward compatibility.
- Add tests for:
  - no emails
  - one email
  - multiple emails
  - `expiresIn`
  - `viewOnce`
- Update docs with examples for both unrestricted shares and email-restricted shares.

Preferred implementation shape:
- Keep the current `ShareItem(...)` methods for compatibility if needed.
- Add new additive APIs for share creation that return the created share data.

### Issue #90 - ShareItem has no return value

Link:
- https://github.com/jscarle/OnePassword.NET/issues/90

Conclusion:
- Valid issue.
- Best solved in the same change as Issue #94.

Why it is valid:
- The public interface returns `void` for all share methods.
- The implementation calls `Op(command)` and discards the CLI output.

Code evidence:
- `OnePassword.NET/IOnePasswordManager.Items.cs` lines 146-180
- `OnePassword.NET/OnePasswordManager.Items.cs` lines 252-297

Impact:
- Callers cannot access the created share link or any share metadata returned by the CLI.
- This severely limits the usefulness of the feature because the primary output of item sharing is the generated share link.

Recommendation:
- Introduce a result model, for example `ItemShareResult`, and return it from a new additive API.
- Avoid changing the existing `void` signatures in place unless a breaking API change is acceptable.

Preferred design:
- Add a new method family, for example:
  - `CreateItemShare(...)`
  - `GetItemShare(...)`
  - or a `ShareItem(...)` overload set that returns a result type while keeping old `void` methods for compatibility
- Parse and expose at least:
  - the share URL or link
  - expiry information if returned
  - recipient information if returned
  - view-once state if returned

Why this should be designed with Issue #94:
- The library should support both unrestricted and email-restricted shares.
- The library should return the created share data in both cases.
- Treating these as separate fixes risks duplicating the API churn.

### Issue #93 - trailing /n for version on windows

Link:
- https://github.com/jscarle/OnePassword.NET/issues/93

Conclusion:
- Valid issue.
- Small and low-risk fix.

Why it is valid:
- `GetVersion()` returns `Op("--version")` directly.
- `Op(...)` returns raw stdout for `--version`.
- No trimming is applied.

Code evidence:
- `OnePassword.NET/OnePasswordManager.cs`
- `GetVersion()` is at lines 172-176.
- The `Op(...)` special-case for `--version` is at lines 207-210.
- The raw output is returned at lines 279-280 when no `[ERROR]` is present.

Impact:
- On Windows, the `Version` property can include a trailing CRLF.
- This can cause equality checks, formatting, or serialization surprises.

Recommendation:
- Trim only the `GetVersion()` result, for example `return Op(command).Trim();`
- Do not globally trim all CLI output because some commands may legitimately depend on exact output formatting.
- Add a regression test for the `Version` property shape if feasible.

### Issue #91 - Unable to retrieve Fields using Service Account Token

Link:
- https://github.com/jscarle/OnePassword.NET/issues/91

Conclusion:
- The report is partly valid, but the title likely misidentifies the root cause.
- The missing-fields behavior looks more like a "summary vs. hydrated item" issue than a service-account-only bug.

Why this conclusion is likely correct:
- `GetItems(string vaultId)` runs `item list --vault {vaultId}`.
- Item list calls typically return summary items, not fully hydrated item details with all fields.
- The issue comment saying "Try using .GetItem recursively, the fieldvalues will be retrieved correctly" matches the current implementation pattern.

Code evidence:
- `OnePassword.NET/OnePasswordManager.Items.cs`
- `GetItems(string vaultId)` at lines 21-28 runs `item list --vault ...`
- `GetItem(string itemId, string vaultId)` at lines 72-80 runs `item get ... --vault ...`
- README example currently shows fetching items with `GetItems(vault)` and then selecting one, which can mislead users into assuming those items are fully hydrated.

Current documentation concern:
- `README.md` lines 97-113 show `GetItems(vault)`, selecting an item, and then editing fields.
- That example does not clearly tell users that they may need `GetItem(...)` before relying on the `Fields` collection.

Impact:
- Users may assume a bug in service-account mode when the real behavior is that `GetItems` returns partial item data.
- This can produce repeated support issues and confusion.

Recommendation:
- Minimum fix:
  - Document clearly that `GetItems` returns summary items and `GetItem` should be used when field data is required.
  - Update README and docfx examples accordingly.
- Possible API improvement:
  - Add an explicit helper or option for hydrated retrieval, such as `GetItemsWithDetails(...)`, if the extra CLI calls and rate-limit implications are acceptable.

Open question to verify later:
- Whether service-account mode and interactive mode differ in field hydration behavior in current CLI versions.

Important test-harness note discovered during this investigation:
- The test suite reads `OPT_SERVICE_ACCOUNT_TOKEN`, not `OP_SERVICE_ACCOUNT_TOKEN`.
- Setting only `OP_SERVICE_ACCOUNT_TOKEN` will not put the test harness into service-account mode.

Relevant test code:
- `OnePassword.NET.Tests/Common/TestsBase.cs`
- `ServiceAccountToken` is read from `OPT_SERVICE_ACCOUNT_TOKEN` at line 25.
- The manager is created with `options.ServiceAccountToken = ServiceAccountToken` at line 72.

### Issue #85 - Example on how to add Fields to an existing item?

Link:
- https://github.com/jscarle/OnePassword.NET/issues/85

Conclusion:
- Not enough evidence yet to justify a production code change.
- Needs a targeted reproduction first.

Why it is still open:
- The current integration tests verify editing and removing existing fields, but they do not verify adding a brand-new field to an existing item.
- The only comment points at a custom-template edge case tied to PR #81.
- PR #81 has already been merged, but there is no follow-up confirming whether the original behavior still reproduces on current `main`.

Code evidence:
- Existing field-edit tests:
  - `OnePassword.NET.Tests/TestItems.cs`
  - Edit path verified in `EditItem()` starting at line 69.
  - Removal of an existing field is asserted at line 93.
- Missing coverage:
  - No test currently adds a new field to an existing item and then verifies persistence.

Related historical context:
- PR #81 added template UUID support and was merged:
  - https://github.com/jscarle/OnePassword.NET/pull/81
- That history suggests custom-template behavior has been problematic before, but the current open issue is not specific enough to justify changing `EditItem` blindly.

Recommendation:
- Do not implement a fix yet.
- First reproduce the behavior with two cases:
  - standard built-in template item
  - custom template item
- If only custom templates fail:
  - treat it as a custom-template compatibility issue
  - design a targeted workaround
- If both fail:
  - investigate whether `item edit` JSON serialization is missing a required structure for newly added fields
  - then add a focused fix and regression test

## Cross-Cutting Recommendations

### 1. Add archive regression tests

Current state:
- No archive-specific tests were found for items or documents.

Reason:
- PR #92 exposed a real archive bug that survived because the test suite does not cover archive semantics.

Recommendation:
- Add tests covering:
  - `ArchiveDocument(IDocument, IVault)`
  - `ArchiveDocument(string, string)`
  - `ArchiveItem(IItem, IVault)`
  - `ArchiveItem(string, string)`

### 2. Redesign the share API as one cohesive change

Reason:
- Issues #90 and #94 are symptoms of the same problem: the share API is currently shaped around side effects only, not around the actual share artifact returned by the CLI.

Recommendation:
- Introduce a result model.
- Support both unrestricted and email-restricted shares.
- Keep compatibility by leaving old `void` methods in place initially if desired.
- Add docs and examples immediately so users do not rediscover the same problems.

### 3. Clarify item hydration behavior

Reason:
- The code distinction between `item list` and `item get` is correct, but the docs make it easy to misunderstand what data `GetItems` returns.

Recommendation:
- Update docs to say:
  - `GetItems` is a summary listing
  - `GetItem` is the details call
- Consider a helper method for hydrated retrieval if support volume justifies it.

### 4. Reproduce before fixing custom-template field creation

Reason:
- Issue #85 may be real, but the current evidence is weak and may be limited to custom templates.

Recommendation:
- Add a regression/repro test first.
- Do not guess at the fix until the failing case is pinned down.

## Captured Test Investigation Context

These notes are included because they may matter when returning to the failing live tests.

### Current live-test environment findings

Present:
- `OPT_RUN_LIVE_TESTS=true`
- `OPT_CREATE_TEST_USER=false`
- `OPT_ACCOUNT_ADDRESS`
- `OPT_ACCOUNT_EMAIL`
- `OPT_ACCOUNT_NAME`
- `OPT_ACCOUNT_PASSWORD`
- `OPT_ACCOUNT_SECRET_KEY`
- `OPT_TEST_USER_EMAIL`

Unset:
- `OPT_COMMAND_TIMEOUT`
- `OPT_RATE_LIMIT`
- `OPT_TEST_USER_CONFIRM_TIMEOUT`
- `OPT_SERVICE_ACCOUNT_TOKEN`
- `OP_SERVICE_ACCOUNT_TOKEN`
- `OP_CONNECT_HOST`
- `OP_CONNECT_TOKEN`
- `OP_DEVICE`

What the test harness actually uses:
- `OPT_*` variables, not `OP_*`
- Environment lookup order in the tests is:
  - Machine
  - User
  - Process

Code evidence:
- `OnePassword.NET.Tests/Common/TestsBase.cs` lines 114-116

Defaulted values because the variables are unset:
- `OPT_COMMAND_TIMEOUT` defaults to `2` minutes
- `OPT_RATE_LIMIT` defaults to `250` ms
- `OPT_TEST_USER_CONFIRM_TIMEOUT` defaults to `OPT_COMMAND_TIMEOUT`

### Current test execution result

Command run:
- `dotnet test OnePassword.NET.Tests\\OnePassword.NET.Tests.csproj`

Observed result:
- `42` total
- `32` failed
- `2` passed
- `8` skipped

First blocking failure:
- `SignIn`
- Error: `No accounts configured for use with 1Password CLI.`

What that implies:
- The suite is still taking the interactive account path, not service-account mode.
- The account/session bootstrap is failing before the rest of the suite can proceed.
- Many later `OperationCanceledException` failures are cascade failures from that initial setup problem.

Relevant test flow:
- `OnePassword.NET.Tests/AccountPreTests.cs`
  - `AddAccount()` at line 22
  - `SignIn()` at line 30
- `OnePassword.NET.Tests/SetUpAccount.cs`
  - `AddAccount()` at line 11
  - `SignIn()` at line 19

Relevant product code:
- `OnePassword.NET/OnePasswordManager.Accounts.cs`
  - `AddAccount(...)` lines 40-84
  - `_account` is set at line 83
  - `SignIn(...)` lines 100-117

### Important caution for later debugging

The test harness downloads and runs its own `op` binary:
- `OnePassword.NET.Tests/Common/TestsBase.cs`
- Download URL currently points to `v2.26.0`
- The executable is extracted to a temporary working directory
- `OnePasswordManager` is then configured to run that downloaded binary

Implication:
- The failing tests are not using any globally installed `op` executable.
- Future test debugging should compare behavior against the test-downloaded `op` version, not just the system CLI.

### New debugging result from direct CLI reproduction

Date:
- 2026-03-08

What was tested:
- Downloaded the latest stable 1Password CLI for Windows (`2.32.1`) from the official release history.
- Ran `op account add`, `op account list`, and `op signin` manually against an isolated config directory using the current `OPT_ACCOUNT_*` values.
- Also ran the exact wrapper-shaped command line that `OnePasswordManager` currently constructs:
  - `account add --address ... --email ... --secret-key ... --format json --no-color`
  - followed by `signin --force --raw --format json --no-color --account ...`

Observed behavior with the latest CLI:
- `account add` fails immediately with:
  - `Couldn't sign in. Check your sign-in details then try again.`
- `account list` then returns an empty list.
- `signin` then fails with:
  - `No accounts configured for use with 1Password CLI.`

Why this matters:
- This reproduces the same failure sequence seen in the tests, but outside the test harness.
- The underlying problem is not only in `signin`; the manual account-add step is already failing.

Important library bug exposed by this:
- `OnePasswordManager.AddAccount(...)` currently calls:
  - `var result = Op(command, trimmedPassword, true);`
- Because `returnError` is `true`, `Op(...)` returns the CLI error string instead of throwing.
- `AddAccount(...)` only special-cases the `"No saved device ID."` message.
- For every other error, `AddAccount(...)` still continues and sets `_account` as if the account was added successfully.

Code evidence:
- `OnePassword.NET/OnePasswordManager.Accounts.cs`
- `AddAccount(...)` starts at line 40.
- The error-returning call is at line 75.
- The device-ID special case is at lines 76-81.
- `_account` is set unconditionally at line 83.

Practical consequence:
- The account-add step can fail silently.
- Tests then proceed to `SignIn`, which fails with `No accounts configured for use with 1Password CLI.`
- This explains why the initial visible test failure is `SignIn` even though the true first problem is `AddAccount`.

Recommended follow-up fix:
- Change `AddAccount(...)` so that it throws when `Op(..., returnError: true)` returns any error other than the specific recoverable `"No saved device ID."` case.
- At minimum:
  - detect non-empty returned errors
  - retry only for the device-ID case
  - otherwise throw `InvalidOperationException` with the returned CLI error

Additional interpretation:
- There may still be a real environment or credential problem behind the failed `account add` call.
- But the library should not hide that failure.
- Fixing `AddAccount(...)` error handling should make the test suite fail at the actual root step instead of cascading into misleading later failures.

### Updated result after correcting the OPT account values

Date:
- 2026-03-08

What changed:
- The `OPT_ACCOUNT_*` values were updated again after the previous failed reproductions.

New direct CLI result using the latest stable CLI (`2.32.1`):
- `account add` now succeeds.
- `account list` shows the configured account.
- `signin --force --raw --account ...` also succeeds.

Meaning:
- The earlier `account add` failure was caused by bad or mismatched account input values, not by a fundamental incompatibility with the latest CLI.
- The test suite can now reach the next layer of failures.

New test-suite status:
- `dotnet test OnePassword.NET.Tests\\OnePassword.NET.Tests.csproj`
- `42` total
- `25` failed
- `9` passed
- `8` skipped

This is important progress because:
- The original auth/bootstrap problem is no longer the primary blocker.
- The remaining failures now reveal real code and compatibility issues that were previously hidden behind the sign-in failure cascade.

## New Test Findings After Auth Started Working

### 1. `GetAccount` is failing due to enum deserialization

Observed failure:
- `GetAccount`
- Exception:
  - `System.NotImplementedException : Could not convert string value to its enum representation.`

Code path:
- `OnePassword.Common.JsonStringEnumConverterEx<T>.Read(...)`
- `OnePassword.OnePasswordManager.GetAccount(...)`

Interpretation:
- The JSON returned by the current CLI contains an enum string value that the library does not currently know how to map.
- This is likely a schema drift problem between the library's enum definitions and newer CLI output.

Recommendation:
- Capture the raw JSON from `op account get --format json` and identify which enum property is failing.
- Then update the relevant enum or the enum converter behavior.
- This is now one of the highest-value debugging targets because it is the first real test failure after auth succeeds.

### 2. `ForgetAccount` no longer matches current CLI behavior

Observed failure:
- `ForgetAccount`
- CLI error:
  - `You are currently logged in to the account you are trying to forget. Use 'op signout --account my --forget' instead.`

Interpretation:
- The CLI behavior has changed relative to what the wrapper and tests expect.
- The current implementation still issues `account forget "<account>"` directly.

Code evidence:
- `OnePassword.NET/OnePasswordManager.Accounts.cs`
- `ForgetAccount(bool all = false)` builds `account forget` commands at lines 132-150.

Recommendation:
- Update the implementation and tests to align with current CLI behavior.
- Likely options:
  - explicitly sign out before forgetting
  - or support the newer `signout --forget` style when forgetting the active account

### 3. Many later failures are still cancellation cascades

Observed pattern:
- After the first real failure, `TestsBase.Run(...)` cancels the relevant token source.
- Many later tests then fail with `OperationCanceledException`.

Meaning:
- The most useful next failures to focus on are the first non-cancellation failures in execution order.
- Right now those are:
  - `GetAccount` enum conversion
  - `ForgetAccount` behavior drift
  - then setup-dependent failures further down

### 4. `CreateItem` is now failing because `TestVault` was never established

Observed failure:
- `CreateItem`
- Exception:
  - `ArgumentException : Id cannot be empty. (Parameter 'vault')`

Interpretation:
- This is probably downstream of an earlier vault-setup failure rather than an `Item` implementation bug.
- The vault setup tests are still being canceled before they can populate `TestVault`.

Recommendation:
- Do not debug `CreateItem` yet.
- Fix earlier account/setup failures first so vault setup can complete cleanly.

## Revised Next Debugging Order

After auth is working, the best next investigation order is:

1. Capture and inspect the raw JSON for `op account get --format json`.
2. Fix the enum deserialization failure in account details.
3. Update `ForgetAccount` behavior for current CLI semantics.
4. Rerun tests and then inspect the first vault/group setup failure that remains.
5. Only after setup is stable, revisit item/document test failures.

## Operational Memory Dump - Latest CLI Compatibility, Live Tests, and Debugging Notes

This section is intentionally redundant and operational. The goal is that if all conversational memory is lost, the future agent can restart from this file and avoid repeating the same dead ends.

### What was accomplished after the investigation above

The library and tests were updated to work with the current 1Password CLI behavior that surfaced once live authentication started succeeding.

The important end state:

- `dotnet test OnePassword.NET.Tests\OnePassword.NET.Tests.csproj` completed successfully.
- Final observed result:
  - `29` passed
  - `13` skipped
  - `0` failed
- A later rerun still produced the same result even after the shell process environment drifted, because the test harness prefers user-scope environment variables over process-scope variables.

This matters because some values visible in the shell were not the values actually used by the tests.

### The single most important test-environment trap

The test harness does **not** simply read the current process environment. It resolves every `OPT_*` variable in this order:

1. `Machine`
2. `User`
3. `Process`

Code evidence:

- `OnePassword.NET.Tests/Common/TestsBase.cs`
- `GetEnv(...)`:
  - `EnvironmentVariableTarget.Machine`
  - then `EnvironmentVariableTarget.User`
  - then `EnvironmentVariableTarget.Process`

Practical consequence:

- If the shell process shows one value, but a user-level environment variable exists, the tests will use the user-level value instead.
- This caused real confusion during debugging because the shell appeared to say one thing while the tests behaved according to another value.

### Effective live-test environment at the time the suite passed

These are the **effective** values because of the `Machine -> User -> Process` lookup order. Do not trust the shell process environment alone.

Observed scoped values:

- `OPT_RUN_LIVE_TESTS`
  - `Machine`: unset
  - `User`: `true`
  - `Process`: `true`
  - Effective value used by tests: `true`
- `OPT_CREATE_TEST_USER`
  - `Machine`: unset
  - `User`: `false`
  - `Process`: `true`
  - Effective value used by tests: `false`
- `OPT_ACCOUNT_ADDRESS`
  - `Machine`: unset
  - `User`: `my.1password.com`
  - `Process`: `my.1password.com`
  - Effective value used by tests: `my.1password.com`
- `OPT_ACCOUNT_EMAIL`
  - `Machine`: unset
  - `User`: `ac266132@gmail.com`
  - `Process`: `jscarle@sentios.ca`
  - Effective value used by tests: `ac266132@gmail.com`
- `OPT_ACCOUNT_NAME`
  - `Machine`: unset
  - `User`: `Jean-Sebastien Carle`
  - `Process`: `Jean-Sebastien Carle`
  - Effective value used by tests: `Jean-Sebastien Carle`
- `OPT_TEST_USER_EMAIL`
  - `Machine`: unset
  - `User`: `optest@supremus.com`
  - `Process`: `optest@supremus.com`
  - Effective value used by tests: `optest@supremus.com`
- `OPT_SERVICE_ACCOUNT_TOKEN`
  - unset in all scopes
  - Effective mode: **not** service-account mode
- `OP_SERVICE_ACCOUNT_TOKEN`
  - irrelevant to the tests
- `OP_CONNECT_HOST`
  - irrelevant to the tests
- `OP_CONNECT_TOKEN`
  - irrelevant to the tests

Secret values were present where needed:

- `OPT_ACCOUNT_PASSWORD`
- `OPT_ACCOUNT_SECRET_KEY`

Do not store the secret values in this file. It is enough to remember that the test harness was able to use them successfully once the effective account identity values were correct.

### What the skipped tests actually mean

The final `13` skipped tests were expected and should not be misread as regressions.

Breakdown:

- `8` user-management tests were skipped because `OPT_CREATE_TEST_USER` was effectively `false` at **user scope**, even though the process environment later showed `true`.
- `5` group-management tests were skipped because the active account is not authorized for group management and the suite now skips cleanly instead of failing/canceling.

This is important:

- The user-management skips were caused by environment precedence.
- The group-management skips were caused by feature authorization.
- These are different causes and should not be conflated.

### Current account capability assumptions that matched reality

The account that successfully drove the suite is effectively an individual/personal-style account, not a business account.

Relevant observations:

- Latest CLI `account get --format json` returned an account type of `INDIVIDUAL`.
- The previous hardcoded test expectation that the account type must be `Business` was wrong for the real environment.
- Group-management operations returned `403 Forbidden`.
- Vault creation and the rest of the core item/document/template flows still worked.

Raw latest-CLI account JSON captured during investigation:

```json
{
  "id": "YU4CHZYEQFHOBOIIKBXZPNXYKI",
  "name": "Jean-Sebastien Carle",
  "domain": "my",
  "type": "INDIVIDUAL",
  "state": "ACTIVE",
  "created_at": "2020-03-28T20:11:37Z"
}
```

Why this matters:

- Future tests or assertions must not assume `Business`.
- Any feature that requires a team/business-style account may need conditional handling in tests.

### Important distinction between the globally installed CLI and the test CLI

The live test harness downloads and runs its **own** `op` binary. It does **not** use whatever `op` happens to be installed globally.

Current harness behavior:

- `OnePassword.NET.Tests/Common/TestsBase.cs`
- Downloads:
  - `https://cache.agilebits.com/dist/1P/op2/pkg/v2.26.0/op_windows_amd64_v2.26.0.zip` on Windows
  - `https://cache.agilebits.com/dist/1P/op2/pkg/v2.26.0/op_linux_amd64_v2.26.0.zip` on Linux
- Extracts the executable into a random temporary working directory
- Configures `OnePasswordManager` with:
  - `options.Path = WorkingDirectory`
  - `options.Executable = ExecutableName`

Implications:

- A passing or failing global `op` command is useful for diagnosis, but it is **not** the same execution path as the tests.
- Any future CLI-compatibility debugging should check both:
  - the system/global CLI if using it for reproduction
  - the test-downloaded CLI if debugging the suite

Additional current-shell observation:

- At the time these notes were written, `op` was **not** on `PATH` in the current shell anymore.
- That did **not** matter for the test suite because the tests use the downloaded binary.

### Latest-CLI reproduction findings that led to the fixes

The latest stable CLI used during direct manual debugging was `2.32.1`.

The important timeline:

1. Direct manual reproduction with bad or mismatched account values:
   - `op account add ...` failed with:
     - `Couldn't sign in. Check your sign-in details then try again.`
   - `op account list` was empty.
   - `op signin --force --raw --account ...` then failed with:
     - `No accounts configured for use with 1Password CLI.`

2. This exposed a real library bug:
   - `AddAccount(...)` did not throw for most CLI errors.
   - It only special-cased the `"No saved device ID."` message.
   - It still set `_account` even after a failed `account add`.
   - The suite then surfaced the later `SignIn` failure instead of the real root cause.

3. After correcting the effective account values:
   - `account add` succeeded
   - `account list` showed the account
   - `signin --force --raw --account ...` succeeded

4. Once auth worked, the hidden compatibility issues became visible:
   - enum deserialization failed on new account type output
   - forgetting an active account no longer matched current CLI expectations
   - test assumptions around account type and unauthorized features were too rigid
   - one vault edit overload had an unrelated forwarding bug that the suite exposed once it got far enough

### Exact code changes applied in the product code

#### 1. Added support for `Individual` account type

File:

- `OnePassword.NET/Accounts/AccountType.cs`

Change:

- Added:

```csharp
[EnumMember(Value = "Individual")]
Individual,
```

Reason:

- The latest CLI returned `"INDIVIDUAL"` / `Individual`-style account type data that the library did not previously map.
- Without this, `GetAccount()` could fail on modern account JSON.

#### 2. Made enum conversion degrade to `Unknown` when possible

File:

- `OnePassword.NET/Common/JsonStringEnumConverterEx.cs`

Change:

- The converter now returns the enum member mapped to `"UNKNOWN"` if it exists instead of immediately throwing for every unrecognized string.

Current behavior added:

```csharp
if (_stringToEnum.TryGetValue("UNKNOWN", out var unknownValue))
    return unknownValue;

throw new NotImplementedException($"Could not convert string value '{stringValue}' to its enum representation.");
```

Reason:

- The library was too brittle against CLI schema drift.
- If an enum already has an `Unknown` member, returning it is safer than hard-failing on new values.

Why this is a good general rule:

- It preserves forward compatibility.
- It still throws if no `Unknown` member exists, so truly unsupported enums still surface loudly.

#### 3. Fixed `AddAccount(...)` so it no longer hides real CLI failures

File:

- `OnePassword.NET/OnePasswordManager.Accounts.cs`

Previous bad behavior:

- `AddAccount(...)` called `Op(command, password, true)`.
- `returnError: true` caused `Op(...)` to return the CLI error string instead of throwing.
- `AddAccount(...)` only handled `"No saved device ID."`.
- It still set `_account` for other failures.

New behavior:

```csharp
else if (result.StartsWith("[ERROR]", StringComparison.InvariantCulture))
{
    throw new InvalidOperationException(result.Length > 28 ? result[28..].Trim() : result.Trim());
}
```

Reason:

- This turns a silent account-add failure into the real root exception.
- It prevents misleading follow-on failures such as `SignIn` reporting `No accounts configured`.

This is one of the most important fixes in this whole round of work.

#### 4. Updated `SignOut(...)` for current account semantics

File:

- `OnePassword.NET/OnePasswordManager.Accounts.cs`

Change:

- When `all == false`, `SignOut(...)` now targets the cached account explicitly.
- It also clears `_session` after sign-out.

Current logic added:

```csharp
if (all)
    command += " --all";
else if (_account.Length > 0)
    command += $" --account \"{_account}\"";
Op(command);
_session = "";
```

Reason:

- The modern CLI is stricter and account-scoped sign-out behavior matters.
- Clearing `_session` avoids stale in-memory state after sign-out.

#### 5. Updated `ForgetAccount(...)` for current CLI behavior

File:

- `OnePassword.NET/OnePasswordManager.Accounts.cs`

Observed CLI drift:

- Forgetting the currently logged-in account directly produced an error telling the user to sign out first.

Chosen implementation:

- If `_session` is populated, call `SignOut(all)` first.
- Then run `account forget`.
- Clear `_account` after success.

Current logic added:

```csharp
if (_session.Length > 0)
    SignOut(all);

...

_account = "";
```

Reason:

- This aligned the wrapper with current CLI expectations in a simple, predictable way.
- It was more reliable in practice than trying to depend directly on a `signout --forget` path for the wrapper behavior.

#### 6. Fixed a real, unrelated vault edit forwarding bug

File:

- `OnePassword.NET/OnePasswordManager.Vaults.cs`

Bug:

- `EditVault(IVault vault, ...)` validated its arguments and then incorrectly called:

```csharp
EditVault(vault.Id);
```

- That dropped every edit parameter and effectively made the object overload broken.

Fix:

```csharp
EditVault(vault.Id, name, description, icon, travelMode);
```

Reason:

- Once the suite got far enough to exercise vault operations, this surfaced as a real bug.
- This was not strictly a latest-CLI compatibility issue, but it was blocking correct live behavior and needed to be fixed.

### Exact test changes applied

#### 1. Relaxed the account type assertion

File:

- `OnePassword.NET.Tests/AccountPreTests.cs`

Old assertion:

```csharp
Assert.That(account.Type, Is.EqualTo(AccountType.Business));
```

New assertion:

```csharp
Assert.That(account.Type, Is.Not.EqualTo(AccountType.Unknown));
```

Reason:

- The real environment is not guaranteed to be `Business`.
- The important invariant is that the type is recognized, not that it is business-specific.

#### 2. Prevented `Assert.Ignore()` from poisoning the suite

File:

- `OnePassword.NET.Tests/Common/TestsBase.cs`

Critical fix:

- `Run(...)` previously caught all exceptions and canceled the relevant token source.
- In NUnit, `Assert.Ignore()` throws `IgnoreException`.
- That meant an intentional skip could incorrectly cancel the rest of the setup/test phase and create cascaded failures.

New logic:

```csharp
catch (NUnit.Framework.IgnoreException)
{
    throw;
}
catch (Exception)
{
    tokenSource.Cancel();
    throw;
}
```

Why this matters beyond the current issue:

- Future skip-based feature gating in the suite must never trigger cancellation side effects.
- This is a general testing hygiene fix, not just a point fix.

#### 3. Added explicit group-feature gating

Files:

- `OnePassword.NET.Tests/Common/TestsBase.cs`
- `OnePassword.NET.Tests/SetUpGroup.cs`
- `OnePassword.NET.Tests/TearDownGroup.cs`

New shared state:

```csharp
private protected static bool GroupManagementSupported = true;
```

Behavior:

- The first group-creation attempt catches `InvalidOperationException` containing `"(403) Forbidden"`.
- It sets `GroupManagementSupported = false`.
- It calls `Assert.Ignore("Group management is not authorized for the current account.");`
- Later group setup/teardown tests short-circuit immediately if group management is unsupported.

Why this was necessary:

- The active account is valid for most live tests but not for group administration.
- The test suite should treat missing authorization as an unsupported capability, not as a product failure.

Important side note:

- This pattern may be worth copying for any other capability that depends on account tier or tenant permissions.

### Final test result that should be treated as the current baseline

Command:

```powershell
dotnet test OnePassword.NET.Tests\OnePassword.NET.Tests.csproj
```

Observed result after the fixes:

- `Passed!`
- `Failed: 0`
- `Passed: 29`
- `Skipped: 13`
- `Total: 42`

This result was observed again after later environment drift in the shell because the effective values still came from user-scope environment variables.

### What to remember if tests fail again in the future

#### First debugging question: which environment scope is actually winning?

Do not assume the current shell process values are the values the tests are using.

Check all three scopes for at least:

- `OPT_RUN_LIVE_TESTS`
- `OPT_CREATE_TEST_USER`
- `OPT_ACCOUNT_ADDRESS`
- `OPT_ACCOUNT_EMAIL`
- `OPT_ACCOUNT_NAME`
- `OPT_ACCOUNT_PASSWORD`
- `OPT_ACCOUNT_SECRET_KEY`
- `OPT_TEST_USER_EMAIL`
- `OPT_SERVICE_ACCOUNT_TOKEN`

The future agent should explicitly compare:

- `Machine`
- `User`
- `Process`

If the shell and the test behavior disagree, this is the first place to look.

#### Second debugging question: is the failure happening in the test-downloaded CLI or only in the global CLI?

Remember:

- The tests run a downloaded `op` binary from a temp directory.
- A manually reproduced global CLI failure is useful signal but not perfect proof of what the suite is doing.

#### Third debugging question: is the first real failure hidden by cascade cancellations?

Look for:

- the first non-cancellation failure
- the first non-skip failure
- any place where `Assert.Ignore()` or a setup exception might poison later tests

The current `IgnoreException` handling fix reduced this problem, but it is still the right mental model for triage.

#### Fourth debugging question: is the account tier allowed to do the operation?

Do not assume:

- business account
- user administration
- group management

The environment used here was real, authenticated, and still not authorized for all administrative features.

### What to remember if future work touches account handling again

- `AddAccount(...)` must not hide real CLI failures.
- `SignOut(...)` should keep account/session state in sync with the CLI.
- `ForgetAccount(...)` must respect the CLI's active-account behavior.
- Account type handling must remain tolerant of new enum values.
- Tests should validate behavior without overfitting to one account tier.

### What to remember if future work touches test writing again

- Prefer capability-based assertions over tenant-tier assumptions.
- If a feature is unauthorized but not inherently broken, prefer an explicit skip over a hard failure.
- Never let an intentional skip trigger cancellation of later tests.
- If the tests use live external systems, isolate account-tier assumptions early and visibly.
- When a fixture depends on setup side effects, the first failing setup test often invalidates many later results; debug the earliest real failure first.

### What to remember if future work returns to the GitHub issues and PRs above

The earlier sections in this file still contain the issue and PR triage. Those conclusions remain useful even after the latest-CLI fixes:

- PR #92 is still a valid archive bug fix and should be merged or replicated.
- Issues #90 and #94 should still be addressed together as a share-API redesign.
- Issue #93 is still a valid low-risk version-trimming fix if it has not already been handled separately.
- Issue #91 still needs documentation clarity around summary items vs hydrated items.
- Issue #85 still needs a targeted reproduction before a product code change.
- PR #84 is still lower-priority and should ideally add tests before merging.

### Short future restart checklist

If starting from zero later, do this in order:

1. Read this file top to bottom.
2. Check `git diff` to see whether the compatibility fixes described here are already present or still only local.
3. Check all `OPT_*` variables across `Machine`, `User`, and `Process`.
4. Remember that the test harness uses its own downloaded `op` binary.
5. Run `dotnet test OnePassword.NET.Tests\OnePassword.NET.Tests.csproj`.
6. If it fails, identify the first real failure, not the later cascades.
7. If it skips unexpectedly, verify whether the skip is due to env precedence, account tier, or explicit capability gating.
