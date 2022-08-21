#if !NET20
using System.Collections.Generic;
using OnePassword.Items;

namespace OnePassword
{
    public static class Extensions
    {
        public static Group FindByName(this IEnumerable<Group> groups, string name)
        {
            foreach (Group group in groups)
                if (group.Name == name)
                    return group;
            return null;
        }

        public static ItemField FindByName(this IEnumerable<ItemField> fields, string name)
        {
            foreach (ItemField field in fields)
                if (field.Name == name)
                    return field;
            return null;
        }

        public static Section FindByName(this IEnumerable<Section> sections, string name)
        {
            foreach (Section section in sections)
                if (section.Name == name)
                    return section;
            return null;
        }

        public static SectionField FindByName(this IEnumerable<SectionField> fields, string name)
        {
            foreach (SectionField field in fields)
                if (field.Name == name)
                    return field;
            return null;
        }

        public static Template FindByName(this IEnumerable<Template> templates, string name)
        {
            foreach (Template template in templates)
                if (template.Name == name)
                    return template;
            return null;
        }

        public static User FindByName(this IEnumerable<User> users, string name)
        {
            foreach (User user in users)
                if (user.Name == name)
                    return user;
            return null;
        }

        public static Vault FindByName(this IEnumerable<Vault> vaults, string name)
        {
            foreach (Vault vault in vaults)
                if (vault.Name == name)
                    return vault;
            return null;
        }

        public static Document FindByTitle(this IEnumerable<Document> documents, string title)
        {
            foreach (Document document in documents)
                if (document.Overview.Title == title)
                    return document;
            return null;
        }

        public static Item FindByTitle(this IEnumerable<Item> items, string title)
        {
            foreach (Item item in items)
                if (item.Overview.Title == title)
                    return item;
            return null;
        }
    }
}
#endif