using System.Collections;
using System.Collections.ObjectModel;

namespace OnePassword.Items;

public class ItemList : List<Item>
{
    /// <summary>Gets or sets the value associated with the specified title.</summary>
    /// <param name="title">The title of the value to get or set.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="title" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="title" /> does not exist in the collection.</exception>
    /// <returns>The value associated with the specified title. If the specified title is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
    public Item this[string title]
    {
        get
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            foreach (var item in this)
                if (item.Overview.Title == title)
                    return item;

            throw new KeyNotFoundException();
        }
        set
        {
            var titleExists = false;
            for (var index = 0; index < Count; index++)
            {
                if (this[index].Overview.Title != title)
                    continue;

                titleExists = true;
                this[index] = value;
                break;
            }
            if (!titleExists)
                Add(value);
        }
    }

    /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the titles of the <see cref="T:ItemList" />.</summary>
    /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the titles of the <see cref="T:ItemList" />.</returns>
    public ICollection Titles
    {
        get
        {
            var collection = new Collection<string>();
            foreach (var item in this)
            {
                if (item.Overview.Title.Length > 0)
                    collection.Add(item.Overview.Title);
            }
            return collection;
        }
    }

    /// <summary>Determines whether the <see cref="T:ItemList" /> contains the specified title.</summary>
    /// <param name="title">The title to locate in the <see cref="T:ItemList" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="title" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:ItemList" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
    public bool ContainsTitle(string title)
    {
        var titleExists = false;
        for (var index = 0; index < Count; index++)
        {
            if (this[index].Overview.Title != title)
                continue;

            titleExists = true;
            break;
        }
        return titleExists;
    }
}