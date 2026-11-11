# 1Password CLI compatibility review

The referenced stable CLI version is **2.42.0**, reviewed on November 10, 2026. The [release archive](https://app-updates.agilebits.com/product_history/CLI2) dates its binary release November 3; the [release announcement](https://releases.1password.com/developers/cli/) was updated November 4. The archive also lists `2.44.0-beta.01`, which is a prerelease and is not the compatibility target.

This documentation and source review covered the complete published CLI 2 history: **116 entries, comprising 60 stable releases and 56 prereleases**, from `2.0.0-beta.3` through the entries available on the review date. All 20 command families in the current reference were compared with the wrapper. The compatibility review itself did not run tests, account operations, project builds, or 1Password CLI commands. Subsequent release validation is described below. The last recorded live validation remains CLI `2.32.1`.

## Changes since the previous CLI reference

These changes affect compatibility or explain functionality outside the wrapper. Earlier releases were also checked for command syntax, JSON changes, authentication, attachments, permissions, and secret references. Source: [complete CLI 2 release history](https://app-updates.agilebits.com/product_history/CLI2).

| Release | Relevant change | Wrapper assessment |
| --- | --- | --- |
| 2.33.0 | Tag replacement and clearing; unset date defaults; Account Trust Log support. | Tracked item tags already support replacement and clearing. Missing field values remain empty; no zero date is synthesized. Trust Log behavior belongs to the CLI. |
| 2.33.1 | Desktop authentication fixes; passkey warning for JSON edits. | Passkey limitation now appears in API and usage documentation. |
| 2.34.0 and 2.34.1 | Shell plugins, subprocess cancellation, and clearer vault Travel Mode help. | Plugin and subprocess commands are not wrapped. Existing vault boolean mapping matches the documented flag. |
| 2.35.0 | Plugin additions and deletion fixes for invited users. | Existing user deletion delegates to the corrected CLI. |
| 2.38.1 and 2.39.0 | Faster reads, improved diagnostics, and vault lookup failures. | Existing read paths benefit automatically; command failures now honor exit status. |
| 2.40.0 | Google Workspace synchronization and dependency security fixes. | Synchronization has no wrapper API. |
| 2.41.0 and 2.41.1 | Environment support in `run`; security updates. | `run` has no wrapper API. |
| 2.42.0 | Environment secret listing; address fixes; plugin changes. | Environment commands are not wrapped. Item and secret reads use the corrected CLI behavior. |

## Command coverage

Coverage describes existing public APIs and does not imply complete flag or workflow coverage. Sources: [command reference](https://www.1password.dev/cli/reference), the individual command pages, and the manager implementations.

| Command family | Existing wrapper coverage |
| --- | --- |
| `account` | Add, list, get, and forget. Account selection is maintained by the manager. |
| `completion` | None. |
| `connect` | No management API. Ambient Connect configuration can affect CLI authentication. |
| `document` | Create, list, get, edit, delete, and archive. |
| `environment` | None, including the new secret listing command. |
| `events-api` | None. |
| `group` | Create, list, get, edit, delete, and membership management. |
| `inject` | None. |
| `item` | List, get, create, edit, move, share, delete, archive, templates, and attachments. Several generation, filtering, and output options are not exposed. |
| `plugin` | None. |
| `provisioning` | None. |
| `read` | Read text secrets or save secret and attachment content to a file. Query parameters pass through in secret references. |
| `run` | None, including Environment loading. |
| `service-account` | Authentication with an existing token; no creation or rate-limit API. |
| `signin` | Manual and desktop integration modes. |
| `signout` | Current account or all accounts. |
| `update` | Download and replace the executable using the stable channel. |
| `user` | List, get, provision, confirm, edit, suspend, reactivate, and delete. Recovery is not exposed. |
| `vault` | List, get, create, edit, delete, and user/group permission management. Permission-filtered listing and omission of permissions to revoke all access are not exposed. |
| `whoami` | None. |

Ten of the 20 command families have direct wrapper methods. Service account authentication is configuration rather than a `service-account` management method. New command families and optional workflows were not added as part of compatibility corrections.

## Confirmed issues corrected

### Command execution and authentication

* **Failures:** The command runner previously decided success from a stderr prefix alone and could return normally after a nonzero exit. It now waits for the process, checks its exit code, and preserves diagnostics without removing a fixed number of characters. Both output streams are drained concurrently to avoid pipe deadlocks. Standard input explicitly uses UTF-8, matching the [CLI encoding contract](https://www.1password.dev/cli/reference#alternative-character-encoding).
* **Account registration:** `AddAccount` used an undocumented `--secret-key` flag. The Secret Key now travels only in the child process's `OP_SECRET_KEY` environment variable. Passwords retain their exact contents instead of losing surrounding whitespace. See [account commands](https://www.1password.dev/cli/reference/management-commands/account) and the documented Secret Key environment variable in the CLI 2.0.1 release notes.
* **Account selection:** An explicit `GetAccount` argument could be followed by a second, conflicting `--account` flag. Selection now reaches the runner as one override. `SignOut` also emits only one account selector. See [multiple account selection](https://www.1password.dev/cli/use-multiple-accounts).
* **Authentication environment:** An explicit service account could silently use an inherited Connect backend, which has higher precedence. Service account mode removes Connect variables from the child environment. Human modes remove an inherited service account token, and the selected desktop integration setting is explicit. See [service account precedence and supported commands](https://www.1password.dev/service-accounts/use-with-1password-cli).
* **Service account restrictions:** A blanket rejection of all user and group commands also blocked documented read operations. User/group get and list can now reach the CLI; unsupported management operations remain blocked. The same service account guide discourages using these read operations as a provisioning workflow.
* **Verbose output:** Verbose mode previously printed the session token and complete secret-bearing responses. It now prints the base command without appended authentication tokens or response bodies.

### Items and files

* **Secret references and argument quoting:** References containing spaces were split into separate arguments. References are now quoted, and the existing argument quoting routine handles embedded quotes and trailing backslashes. Names, descriptions, paths, titles, tags, and template names use that routine. See [secret reference syntax](https://www.1password.dev/cli/secret-reference-syntax).
* **Attachment assignment targets:** Literal periods, equals signs, and backslashes in attachment or section names were interpreted as assignment syntax. These characters are now escaped before argument quoting. See [item assignment syntax](https://www.1password.dev/cli/reference/management-commands/item#item-create).
* **URL edits:** Editing a primary URL interpolated the `Url` object, producing its type name. The command now uses `Href`; changes elsewhere in the URL collection do not emit an accidental empty primary URL override. See [item editing flags](https://www.1password.dev/cli/reference/management-commands/item#item-edit).
* **Document tags and modes:** An empty tag collection was ignored during replacement, preventing tag removal. It now emits an empty `--tags` value. Blank optional file modes are omitted. See [document editing and download flags](https://www.1password.dev/cli/reference/management-commands/document).
* **Share expiry:** Converting a duration greater than seven days to weeks discarded its hours, minutes, and seconds. The conversion now subtracts complete weeks and preserves the remainder. See [share duration units](https://www.1password.dev/cli/reference/management-commands/item#item-share).
* **Known model values:** The models now recognize the documented Crypto Wallet category, service account user type, and recovery-started state. Existing enum numeric values are preserved. Sources: [item categories](https://www.1password.dev/cli/item-fields), [service account user details](https://www.1password.dev/service-accounts/use-with-1password-cli), and [user examples](https://www.1password.dev/cli/reference/management-commands/user).

### Vaults and updates

* **Permission names:** Grant/revoke methods sent names such as `View Items` where the CLI expects `view_items`. All four methods now use lowercase names with underscores and disable interactive input. Permission dependencies remain the caller's responsibility; no extra privileges are added automatically. See [vault command flags](https://www.1password.dev/cli/reference/management-commands/vault) and [permission dependencies](https://www.1password.dev/cli/vault-permissions).
* **Updates:** The updater explicitly selects `stable` and cleans its temporary directory on failure as well as success. See [update options](https://www.1password.dev/cli/reference/commands/update).

## Remaining limitations

**Passkey preservation is a CLI limitation.** `EditItem` sends JSON, and the CLI cannot preserve passkeys in JSON template edits. This method must not be used on passkey-bearing items. The documented JSON schema does not provide a reliable passkey-presence check, so the wrapper cannot claim automatic protection. The API remarks, README, and quick start now explain this limitation. SSH Key edits are rejected before starting the process because that operation is also unsupported. See [editing restrictions](https://www.1password.dev/cli/item-edit).

**Moving changes the item's ID.** Existing `MoveItem` methods return `void`, so a supplied model retains its original ID. Their API remarks now document that callers must retrieve the destination item. Returning the new item would require a public API change. See [item move behavior](https://www.1password.dev/cli/reference/management-commands/item#item-move).

**Documentation does not fully specify every JSON response.** `ItemShare` keeps optional metadata optional, enum conversion retains its unknown fallback, and document creation retains the existing `uuid` mapping. No response properties were guessed. The updater still recognizes a human-readable availability message; the update reference does not promise a structured response schema.

**Live behavior remains unverified.** The download pin and simulated CLI version fixtures now reference `2.42.0`; existing assertions were adjusted for corrected command strings. Static review cannot establish account capabilities, server policy, MFA interactions, or undocumented response shapes. Package, assembly, and file versions are managed separately during release preparation.

## Static verification

Roslyn syntax parsing of all **88 C# source files** found no syntax errors. Project XML and SDK JSON parsed successfully, and `git diff --check` found no patch whitespace errors. MSBuild configuration evaluation resolved both projects to `net11.0`, SDK `11.0.100`, and the default C# `15.0`. Project builds, Rider inspections, and tests were excluded from the original compatibility review.

## Release validation

Subsequent preparation of release **2.6.0** included a Release solution build with **zero compiler warnings and errors**, Rider 2026.3 solution inspections with **zero warnings**, and creation of the NuGet and symbol packages. The package contains the `net11.0` library. No tests were executed locally; CI test results are recorded in the release pull request.
