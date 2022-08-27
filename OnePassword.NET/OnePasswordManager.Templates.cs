using System.Collections.Immutable;
using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<TemplateSimple> GetTemplates()
    {
        var command = "item template list";
        return Op<ImmutableList<TemplateSimple>>(command);
    }

    public Template GetTemplate(ITemplate template)
    {
        if (template.Name.Length == 0)
            throw new ArgumentException($"{nameof(template.Name)} cannot be empty.", nameof(template));

        var command = $"item template get \"{template.Name}\"";
        var result = Op<Template>(command);

        result.Name = template.Name;

        return result;
    }

    public Template GetTemplate(Category category)
    {
        if (category is Category.Unknown or Category.Custom)
            throw new ArgumentException($"{nameof(category)} cannot be {nameof(Category.Unknown)} or {nameof(Category.Custom)}.", nameof(category));

        var templateName = category.ToEnumString();

        var command = $"item template get \"{templateName}\"";
        var result = Op<Template>(command);

        result.Name = templateName;

        return result;
    }
}