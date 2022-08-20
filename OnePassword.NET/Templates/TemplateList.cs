using System.Collections;
using System.Collections.ObjectModel;

namespace OnePassword.Templates;

public class TemplateList : List<Template>
{
    /// <summary>Gets or sets the value associated with the specified name.</summary>
    /// <param name="name">The name of the value to get or set.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="name" /> does not exist in the collection.</exception>
    /// <returns>The value associated with the specified name. If the specified name is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
    public Template this[string name]
    {
        get
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            foreach (var template in this)
                if (template.Name == name)
                    return template;

            throw new KeyNotFoundException();
        }
        set
        {
            var nameExists = false;
            for (var index = 0; index < Count; index++)
            {
                if (this[index].Name != name)
                    continue;

                nameExists = true;
                this[index] = value;
                break;
            }
            if (!nameExists)
                Add(value);
        }
    }

    /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the names of the <see cref="T:TemplateList" />.</summary>
    /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the names of the <see cref="T:TemplateList" />.</returns>
    public ICollection Names
    {
        get
        {
            var collection = new Collection<string>();
            foreach (var template in this)
                collection.Add(template.Name);
            return collection;
        }
    }

    /// <summary>Determines whether the <see cref="T:TemplateList" /> contains the specified name.</summary>
    /// <param name="name">The name to locate in the <see cref="T:TemplateList" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:TemplateList" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
    public bool ContainsName(string name)
    {
        var nameExists = false;
        for (var index = 0; index < Count; index++)
        {
            if (this[index].Name != name)
                continue;

            nameExists = true;
            break;
        }
        return nameExists;
    }
}