namespace OnePassword.Common;

public interface IResultBase<T> : IEquatable<IResultBase<T>>, IComparable<IResultBase<T>>, IComparable
{
    string Id { get; }

    string Name { get; }

    void Deconstruct(out string id, out string name);
}