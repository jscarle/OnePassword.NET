namespace OnePassword.Accounts;

public interface IAccount : IEquatable<IAccount>, IComparable<IAccount>, IComparable
{
    string Id { get; }

    void Deconstruct(out string id, out string name);
}