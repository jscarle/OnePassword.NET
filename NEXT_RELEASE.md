# Draft release description for x.x.x

## Breaking changes

- `ShareItem(...)` now returns `ItemShare` instead of `void`.

## Highlights

- Merged PR `#92` to fix `ArchiveDocument(IDocument, IVault)` so it archives documents instead of deleting them.
- Redesigned item sharing so unrestricted links are supported and the created share URL is returned to the caller.
- Trimmed the CLI version string returned on Windows so `Version` no longer includes a trailing newline.
- Clarified in the docs that `GetItems(...)` returns summary items and `GetItem(...)` should be used before working with hydrated fields.
- Added regression coverage for archive behavior, item sharing, version handling, and adding a new field to an existing built-in item.

## Migration

Before:

```csharp
onePassword.ShareItem(item, vault, "recipient@example.com");
```

After:

```csharp
var restrictedShare = onePassword.ShareItem(item, vault, "recipient@example.com");
var unrestrictedShare = onePassword.ShareItem(item, vault);
```
