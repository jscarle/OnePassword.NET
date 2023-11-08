using OnePassword.Common;

namespace OnePassword.Vaults;

/// <summary>
/// Common base class that represents a 1Password vault.
/// </summary>
public abstract class VaultBase : ResultBase<IVault>, IVault;