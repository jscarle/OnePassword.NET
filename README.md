[![Banner](https://raw.githubusercontent.com/jscarle/OnePassword.NET/main/Banner.png)](https://github.com/jscarle/OnePassword.NET)

# OnePassword.NET - 1Password CLI Wrapper

This library serves as a .NET wrapper for the [1Password](https://1password.com/) command-line tool
op.exe ( [Download](https://app-updates.agilebits.com/product_history/CLI2) | [Documentation](https://developer.1password.com/docs/cli/reference) ).

[![main](https://img.shields.io/github/actions/workflow/status/jscarle/OnePassword.NET/main.yml?logo=github)](https://github.com/jscarle/OnePassword.NET)
[![nuget](https://img.shields.io/nuget/v/OnePassword.NET)](https://www.nuget.org/packages/OnePassword.NET)
[![downloads](https://img.shields.io/nuget/dt/OnePassword.NET)](https://www.nuget.org/packages/OnePassword.NET)

## References

This library targets .NET 6.0, .NET 7.0, and .NET 8.0.

## Dependencies

This library has no dependencies.

## Quick start

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

```csharp
var onePassword = new OnePasswordManager(serviceAccountToken: token);
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

_Note: If you want to reuse the same template for several items, make sure you clone the instance to avoid reference
issues._

```csharp
var server1 = serverTemplate.Clone();
var server2 = serverTemplate.Clone();
```

### Getting all items in a vault

```csharp
var items = onePassword.GetItems(vault);
```

### Selecting a specific item

```csharp
var item = items.First(x => x.Title == "Your Item's Title");
```

### Editing a specific item

```csharp
item.Fields.First(x => x.Label == "password").Value = "newpass";
onePassword.EditItem(item, vault);
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

## Running tests

Due to the fact that this library acts as a wrapper for the CLI and in order for tests to have any significant value,
the majority of tests are integration tests which must run against an active 1Password account (preferably a Business
account).

### Effects on the active account

The integration tests will sign in to the specified account, then create and update a test group, a test user
(optional), and a test vault. Items will then be created and update in the test vault. Finally, the test vault, user,
and group will be deleted. _Note: If the integration tests fail, test data may remain in the specified account._

### Configuration

The integration tests are configured using the environment variables which are prefixed with `OPT_` (**O**ne**P**assword
**T**ests).
Environment variables which are integers must contain only numbers and those which are booleans must contain `true` or
`false` as a string.

| Environment Variable          | Description                                                                   | Type   | Default Value         |
|-------------------------------|-------------------------------------------------------------------------------|--------|-----------------------|
| OPT_COMMAND_TIMEOUT           | The timeout (in minutes) for each CLI command.                                | int    | `2`                   |
| OPT_RATE_LIMIT                | The rate (in milliseconds) at which commands are executed.                    | int    | `250`                 |
| OPT_RUN_LIVE_TESTS            | Activates or deactivates integration tests.                                   | bool   | `false`               |
| OPT_CREATE_TEST_USER          | Activates or deactivates the creation of the test user and its related tests. | bool   | `false`               |
| OPT_ACCOUNT_ADDRESS           | The account address. Should be the host name only.                            | string |                       |
| OPT_ACCOUNT_EMAIL             | The email to use when authenticating.                                         | string |                       |
| OPT_ACCOUNT_NAME              | The account name. Used to test account related commands.                      | string |                       |
| OPT_ACCOUNT_PASSWORD          | The password to use when authenticating.                                      | string |                       |
| OPT_ACCOUNT_SECRET_KEY        | The secret key to use when authenticating.                                    | string |                       |
| OPT_TEST_USER_EMAIL           | The test user's email address.                                                | string |                       |
| OPT_TEST_USER_CONFIRM_TIMEOUT | The time (in minutes) to wait for manual confirmation of the test user.       | int    | `OPT_COMMAND_TIMEOUT` |

> #### Disclaimer
>
> While the use of the 1Password name and logo is authorized, it's important to note that this library is not an official product developed or maintained by AgileBits, Inc.
