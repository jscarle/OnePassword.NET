using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnePassword.Documents
{
    public class DocumentList : List<Document>
    {
        /// <summary>Gets or sets the value associated with the specified title.</summary>
        /// <param name="title">The title of the value to get or set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="title" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="title" /> does not exist in the collection.</exception>
        /// <returns>The value associated with the specified title. If the specified title is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
        public Document this[string title]
        {
            get
            {
                if (title == null)
                    throw new ArgumentNullException(nameof(title));

                foreach (Document document in this)
                    if (document.Overview != null && document.Overview.Title == title)
                        return document;

                throw new KeyNotFoundException();
            }
            set
            {
                bool titleExists = false;
                for (int index = 0; index < Count; index++)
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

        /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the titles of the <see cref="T:DocumentList" />.</summary>
        /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the titles of the <see cref="T:DocumentList" />.</returns>
        ICollection Titles
        {
            get
            {
                Collection<string> collection = new Collection<string>();
                foreach (Document document in this)
                {
                    if (document.Overview != null)
                        collection.Add(document.Overview.Title);
                }
                return collection;
            }
        }

        /// <summary>Determines whether the <see cref="T:DocumentList" /> contains the specified title.</summary>
        /// <param name="title">The title to locate in the <see cref="T:DocumentList" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="title" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the <see cref="T:DocumentList" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        public bool ContainsTitle(string title)
        {
            bool titleExists = false;
            for (int index = 0; index < Count; index++)
            {
                if (this[index].Overview == null || this[index].Overview.Title != title)
                    continue;

                titleExists = true;
                break;
            }
            return titleExists;
        }
    }
}
