namespace SimpleContextualMenu.Internal
{
    using SimpleContextualMenu.Items;

    public class ItemMetadata
    {
        public string Title { get; private set; }

        public ItemDataBase Data { get; private set; }
        public ItemViewBase View { get; private set; }

        public MenuData Parent { get; private set; }

        /// <summary>
        /// Submenu can be set up after initializing metadata.
        /// </summary>
        public MenuData Submenu;

        // Constructors

        public ItemMetadata(string title, ItemDataBase data, ItemViewBase view, MenuData parent)
        {
            Title = title;
            Data = data;
            View = view;
            Parent = parent;
        }
    }
}