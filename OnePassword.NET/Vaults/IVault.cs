namespace OnePassword.Vaults;

public interface IVault
{
    string Id { get; init; }

    string Name { get; init; }
}