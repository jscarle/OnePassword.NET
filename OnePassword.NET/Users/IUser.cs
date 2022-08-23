using OnePassword.Common;

namespace OnePassword.Users;

public interface IUser : IIdentifiable
{
    string Name { get; init; }
}