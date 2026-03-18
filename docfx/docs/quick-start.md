# Quick start

> Starting with 2.5.0, `ShareItem(...)` returns `ItemShare` instead of `void`.

### Creating an instance of the manager

```csharp
var onePassword = new OnePasswordManager();
```

The default executable name is `op.exe` on Windows and `op` on other platforms. If the CLI isn't in the current working directory, set `options.Path` and, if needed, `options.Executable`.

### Adding your account and signing in for the first time

```csharp
var domain = "my.1password.com";
var email = "your@email.com";
var secretKey = "A3-YOUR-SECRET-KEY";
var password = "yourpassword";

onePassword.AddAccount(domain, email, secretKey, password);
onePassword.SignIn(password);
```

### Signing in for subsequent connections

```csharp
onePassword.UseAccount(domain);
onePassword.SignIn(password);
```

### Using Service Accounts

To signin using a Service Account, a token must be provided as part of the options.

```csharp
var onePassword = new OnePasswordManager(options => {
    options.ServiceAccountToken = "yourtoken";
});
```

Subsequently, the following commands are not used or supported when using service accounts.

- GetAccounts, GetAccount, AddAccount, UseAccount, ForgetAccount
- SignIn, SignOut

For more information, see the documentation
on [1Password Service Accounts](https://developer.1password.com/docs/service-accounts).

### Getting all vaults

```csharp
var vaults = onePassword.GetVaults();
```

### Selecting a specific vault

```csharp
var vault = vaults.First(x => x.Name == "Private");
```

### Creating an item using a template

```csharp
var serverTemplate = onePassword.GetTemplate(Category.Server);

serverTemplate.Title = "Your Item's Title";
serverTemplate.Fields.First(x => x.Label == "username").Value = "secretuser";
serverTemplate.Fields.First(x => x.Label == "password").Value = "secretpass";

var serverItem = onePassword.CreateItem(serverTemplate, vault);
```

_Note: If you want to reuse the same template for several items, make sure you clone the instance to avoid reference
issues._

```csharp
var server1 = serverTemplate.Clone();
var server2 = serverTemplate.Clone();
```

### Getting all items in a vault

`GetItems(...)` returns summary items. Fetch the item details with `GetItem(...)` before relying on `Fields`, `Sections`, or other hydrated properties.

```csharp
var items = onePassword.GetItems(vault);
```

### Selecting a specific item

```csharp
var itemSummary = items.First(x => x.Title == "Your Item's Title");
var item = onePassword.GetItem(itemSummary, vault);
```

### Editing a specific item

```csharp
item.Fields.First(x => x.Label == "password").Value = "newpass";
onePassword.EditItem(item, vault);
```

### Adding a field to an existing item

```csharp
var itemToExtend = onePassword.GetItem(itemSummary, vault);
itemToExtend.Fields.Add(new Field("Environment", FieldType.String, "Production"));

onePassword.EditItem(itemToExtend, vault);
```

The same hydrate-edit-save flow applies to items based on custom templates. Fetch the hydrated item with `GetItem(...)` before editing so the wrapper preserves the item's template-backed `category_id` in the edit payload.

### Adding file attachments to an item

```csharp
var itemToAttach = onePassword.GetItem(itemSummary, vault);
itemToAttach.FileAttachments.Add(new FileAttachment(@"C:\Files\Production.env", "Production Env"));

onePassword.EditItem(itemToAttach, vault);
```

### Removing a file attachment from an item

```csharp
var itemToUpdate = onePassword.GetItem(itemSummary, vault);
var attachment = itemToUpdate.FileAttachments.First(x => x.Name == "Production Env");
itemToUpdate.FileAttachments.Remove(attachment);

onePassword.EditItem(itemToUpdate, vault);
```

### Reading file attachment metadata and content

`FileAttachments` are hydrated by `GetItem(...)` and expose attachment metadata returned by the CLI. Use `SaveFileAttachmentContent(...)` to download the attachment content, or `GetFileAttachmentReference(...)` when you need the secret reference built from the vault, item, and attachment IDs.

```csharp
var itemWithAttachments = onePassword.GetItem(itemSummary, vault);
var attachment = itemWithAttachments.FileAttachments.First();
onePassword.SaveFileAttachmentContent(attachment, itemWithAttachments, vault, @"C:\Files\Production.env");
```

### Reading variables from a 1Password Environment

This feature requires the beta 1Password CLI `2.33.0-beta.02` or later.

```csharp
var variables = onePassword.GetEnvironmentVariables("environment-id");

foreach (var variable in variables)
    Console.WriteLine($"{variable.Name}={variable.Value}");
```

### Saving variables from a 1Password Environment

This writes the Environment output to disk using the same `KEY=value` format returned by the CLI.

```csharp
onePassword.SaveEnvironmentVariables("environment-id", @".env");
```

### Sharing an item without email restrictions

```csharp
var share = onePassword.ShareItem(item, vault);

Console.WriteLine(share.Url);
```

### Sharing an item with email restrictions

```csharp
var share = onePassword.ShareItem(
    item,
    vault,
    "recipient@example.com",
    expiresIn: TimeSpan.FromDays(7),
    viewOnce: true);

Console.WriteLine(share.Url);
```

### Sharing an item with multiple email restrictions

```csharp
var share = onePassword.ShareItem(
    item,
    vault,
    new[] { "recipient1@example.com", "recipient2@example.com" });

Console.WriteLine(share.Url);
```

### Archiving an item

```csharp
onePassword.ArchiveItem(item, vault);
```

### Deleting an item

```csharp
onePassword.DeleteItem(item, vault);
```

### Signing out

```csharp
onePassword.SignOut();
```
