using OnePassword.Common;

namespace OnePassword.Vaults;

public interface IVault : IIdentifiable
{
    string Name { get; init; }
}