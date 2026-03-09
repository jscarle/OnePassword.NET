# TODO

Current snapshot as of 2026-03-08.

Historical debugging notes and completed issue/PR triage were removed from this file after the related work landed. The remaining current items are below.

## Priority Order

1. Decide whether to merge, supersede, or close PR #84.
2. Verify whether Issue #85 still reproduces for custom-template items.

## Open Pull Requests

### PR #84 - Add Support for Referencing Files Object in Item Class

Link:
- https://github.com/jscarle/OnePassword.NET/pull/84

Current assessment:
- Still lower priority than the bug-fix work already merged.
- Additive and potentially useful, but not merge-ready as-is.
- The PR only adds read-only file metadata exposure.

Outstanding concerns:
- No tests were added.
- No README or docfx documentation was added.
- The public type name `File` is easy to confuse with `System.IO.File`.

Recommended next step:
- If attachment metadata matters soon, either update PR #84 or reimplement it with:
  - deserialization coverage for the `files` payload
  - documentation that the feature is read-only metadata
  - explicit consideration of the public type name
- Otherwise, leave it deferred.

## Open Issues

### Issue #85 - Example on how to add Fields to an existing item?

Link:
- https://github.com/jscarle/OnePassword.NET/issues/85

Current assessment:
- The built-in item flow is now covered and documented:
  - `OnePassword.NET.Tests/TestItems.cs` includes `EditItemAddsNewField()`
  - `README.md` and `docfx/docs/quick-start.md` show hydrating with `GetItem(...)` before adding a field
- The remaining uncertainty is the issue comment pointing to custom-template behavior related to PR #81.

Recommended next step:
- Reproduce the scenario specifically against a custom-template item.
- If it does not reproduce, close the issue as stale or already addressed by current behavior and docs.
- If it does reproduce, treat it as a targeted custom-template compatibility bug and add focused regression coverage before changing product code.
