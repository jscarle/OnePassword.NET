using System.Collections;
using System.Collections.ObjectModel;

namespace OnePassword.Groups;

public class GroupList : List<Group>
{
    /// <summary>Gets or sets the value associated with the specified name.</summary>
    /// <param name="name">The name of the value to get or set.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="name" /> does not exist in the collection.</exception>
    /// <returns>The value associated with the specified name. If the specified name is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
    public Group this[string name]
    {
        get
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            foreach (var group in this)
                if (group.Name == name)
                    return group;

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

    /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the names of the <see cref="T:GroupList" />.</summary>
    /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the names of the <see cref="T:GroupList" />.</returns>
    public ICollection Names
    {
        get
        {
            var collection = new Collection<string>();
            foreach (var group in this)
                collection.Add(group.Name);
            return collection;
        }
    }

    /// <summary>Determines whether the <see cref="T:GroupList" /> contains the specified name.</summary>
    /// <param name="name">The name to locate in the <see cref="T:GroupList" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:GroupList" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
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