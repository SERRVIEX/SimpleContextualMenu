namespace SimpleContextualMenu.Items
{
    public class ItemMetadata
    {
        public string Title;

        public ItemDataBase Data;
        public ItemViewBase View;

        public MenuData Parent;
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