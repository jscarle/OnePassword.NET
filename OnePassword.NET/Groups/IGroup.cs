using OnePassword.Common;

namespace OnePassword.Groups;

public interface IGroup : IIdentifiable
{
    string Name { get; init; }
}