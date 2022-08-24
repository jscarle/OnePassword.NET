using System.Collections.Immutable;
using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Template> GetTemplates()
    {
        var command = "item template list";
        return Op<ImmutableList<Template>>(command);
    }

    public Item GetTemplate(ITemplate template)
    {
        if (template.Id.Length == 0)
            throw new ArgumentException($"{nameof(template.Id)} cannot be empty.", nameof(template));

        var command = $"item template get \"{template.Name}\"";
        return Op<Item>(command);
    }

    public Item GetTemplate(Category category)
    {
        if (category is Category.Unknown or Category.Custom)
            throw new ArgumentException($"{nameof(category)} cannot be {nameof(Category.Unknown)} or {nameof(Category.Custom)}.", nameof(category));

        var command = $"item template get \"{category.ToEnumString().Replace("_", " ")}\"";
        return Op<Item>(command);
    }
}