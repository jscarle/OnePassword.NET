using OnePassword.Accounts;
using OnePassword.Documents;
using OnePassword.Groups;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword.Common;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ImmutableList<Account>))]
[JsonSerializable(typeof(AccountDetails))]
[JsonSerializable(typeof(ImmutableList<DocumentDetails>))]
[JsonSerializable(typeof(Document))]
[JsonSerializable(typeof(ImmutableList<Group>))]
[JsonSerializable(typeof(ImmutableList<VaultGroup>))]
[JsonSerializable(typeof(ImmutableList<UserGroup>))]
[JsonSerializable(typeof(GroupDetails))]
[JsonSerializable(typeof(ImmutableList<Item>))]
[JsonSerializable(typeof(Item))]
[JsonSerializable(typeof(ImmutableList<TemplateInfo>))]
[JsonSerializable(typeof(Template))]
[JsonSerializable(typeof(ImmutableList<User>))]
[JsonSerializable(typeof(ImmutableList<GroupUser>))]
[JsonSerializable(typeof(ImmutableList<VaultUser>))]
[JsonSerializable(typeof(UserDetails))]
[JsonSerializable(typeof(ImmutableList<Vault>))]
[JsonSerializable(typeof(VaultDetails))]
internal sealed partial class JsonContext : JsonSerializerContext;
