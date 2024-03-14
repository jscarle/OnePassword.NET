using OnePassword.Items;
using OnePassword.Templates;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets the templates.</summary>
    /// <returns>The list of templates.</returns>
    public ImmutableList<TemplateInfo> GetTemplates();

    /// <summary>Gets a template.</summary>
    /// <param name="template">The template to retrieve.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Template GetTemplate(ITemplate template);

    /// <summary>Gets a template.</summary>
    /// <param name="name">The template name to retrieve.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Template GetTemplate(string name);

    /// <summary>Gets a template.</summary>
    /// <param name="category">The template category.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Template GetTemplate(Category category);
}