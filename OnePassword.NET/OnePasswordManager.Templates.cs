using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <summary>
    /// Gets the templates.
    /// </summary>
    /// <returns>The list of templates.</returns>
    public ImmutableList<TemplateInfo> GetTemplates()
    {
        const string command = "item template list";
        return Op<ImmutableList<TemplateInfo>>(command);
    }

    /// <summary>
    /// Gets a template.
    /// </summary>
    /// <param name="template">The template to retrieve.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Template GetTemplate(ITemplate template)
    {
        if (template.Name.Length == 0)
            throw new ArgumentException($"{nameof(template.Name)} cannot be empty.", nameof(template));

        var command = $"item template get \"{template.Name}\"";
        var result = Op<Template>(command);

        result.Name = template.Name;

        return result;
    }

    /// <summary>
    /// Gets a template.
    /// </summary>
    /// <param name="category">The template category.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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