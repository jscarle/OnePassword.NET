namespace OnePassword.Templates;

public interface ITemplate : IEquatable<ITemplate>, IComparable<ITemplate>, IComparable
{
    string Name { get; }
}