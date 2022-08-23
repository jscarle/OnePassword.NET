namespace OnePassword.Items;

public interface IItem
{
    string? Id { get; init; }

    string Title { get; set; }
}