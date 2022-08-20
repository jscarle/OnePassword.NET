namespace OnePassword.Vaults;

[AttributeUsage(AttributeTargets.Field)]
public class IconAttribute : Attribute
{
    public string Name { get; }
        
    public IconAttribute(string name)
    {
        Name = name;
    }
}