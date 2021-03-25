using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnePassword.Items
{
    public class UrlFieldList : List<UrlField>
    {
        /// <summary>Gets or sets the value associated with the specified label.</summary>
        /// <param name="label">The label of the value to get or set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="label" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="label" /> does not exist in the collection.</exception>
        /// <returns>The value associated with the specified label. If the specified label is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
        public UrlField this[string label]
        {
            get
            {
                if (label == null)
                    throw new ArgumentNullException(nameof(label));

                foreach (UrlField urlField in this)
                    if (urlField.Label == label)
                        return urlField;

                throw new KeyNotFoundException();
            }
            set
            {
                bool labelExists = false;
                for (int index = 0; index < Count; index++)
                {
                    if (this[index].Label != label)
                        continue;

                    labelExists = true;
                    this[index] = value;
                    break;
                }
                if (!labelExists)
                    Add(value);
            }
        }

        /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the labels of the <see cref="T:UrlFieldList" />.</summary>
        /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the labels of the <see cref="T:UrlFieldList" />.</returns>
        ICollection Labels
        {
            get
            {
                Collection<string> collection = new Collection<string>();
                foreach (UrlField urlField in this)
                    collection.Add(urlField.Label);
                return collection;
            }
        }

        /// <summary>Determines whether the <see cref="T:UrlFieldList" /> contains the specified label.</summary>
        /// <param name="label">The label to locate in the <see cref="T:UrlFieldList" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="label" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the <see cref="T:UrlFieldList" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        public bool ContainsLabel(string label)
        {
            bool labelExists = false;
            for (int index = 0; index < Count; index++)
            {
                if (this[index].Label != label)
                    continue;

                labelExists = true;
                break;
            }
            return labelExists;
        }
    }
}
