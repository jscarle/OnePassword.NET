# OnePassword.NET - 1Password CLI Wrapper
This library serves as a .NET wrapper for the [1Password](https://1password.com/) command-line tool op.exe ( [Download](https://app-updates.agilebits.com/product_history/CLI2) | [Documentation](https://developer.1password.com/docs/cli/reference) ).

![develop](https://github.com/jscarle/OnePassword.NET/actions/workflows/develop.yml/badge.svg)
![release](https://github.com/jscarle/OnePassword.NET/actions/workflows/release.yml/badge.svg)

## References
This library targets .NET 5.0 and .NET 6.0.

## Dependencies
This library has no dependencies.

## Quick Start

### Creating an instance of the manager
```csharp
var onePassword = new OnePasswordManager();
```

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

### Getting all items in a vault
```csharp
var items = onePassword.GetItems(vault);
```

### Selecting a specific item
```csharp
var item = items.First(x => x.Title == "Your Item's Title");
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
