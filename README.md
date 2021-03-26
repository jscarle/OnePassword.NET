# OnePassword.NET - 1Password CLI Wrapper
This library serves as a .NET wrapper for the [1Password](https://1password.com/) command-line tool op.exe ( [Download](https://app-updates.agilebits.com/product_history/CLI) | [Documentation](https://support.1password.com/command-line-reference/) ).

## References
This library targets .NET Framework 2.0, 3.5, 4.0, 4.5, .NET 5.0, and .NET Standard 2.0.

## Dependencies
[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) is a required dependency.

## Quick Start

### Creating an instance of the manager and signing in
```csharp
string domain = "my.1password.com";
string email = "your@email.com";
string secretKey = "A3-YOUR-SECRET-KEY";
string password = "yourpassword";

OnePasswordManager onePassword = new OnePasswordManager();

onePassword.SignIn(domain, email, secretKey, password);
```

### Listing all items
```csharp
ItemList items = onePassword.ListItems();
```

### Finding a specific item
```csharp
Item item = items["Your Item's Title"];
```

### Listing all templates
```csharp
TemplateList templates = onePassword.ListTemplates();
```

### Creating an item using a template
```csharp
Template loginTemplate = onePassword.GetTemplate(templates["Login"]);

loginTemplate.Title = "Your Item's Title";
loginTemplate.Details.Fields["username"].Value = "theitemusername";
loginTemplate.Details.Fields["password"].Value = "theitempassword";
loginTemplate.Url = "https://the.item.url/";

onePassword.CreateItem(loginTemplate);
```

### Signing out
```csharp
onePassword.SignOut();
```
