# Quick start

> Starting with the next x.x.x release, `ShareItem(...)` returns `ItemShare` instead of `void`.

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
