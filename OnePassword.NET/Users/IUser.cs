namespace OnePassword.Users;

public interface IUser
{
    string Id { get; init; }

    string Name { get; init; }
}