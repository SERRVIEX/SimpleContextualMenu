namespace SimpleContextualMenu.Items
{
    using System.Collections.Generic;

    public class MenuData
    {
        public List<ItemMetadata> Children;
        public List<int> Separators;

        // Constructors

        public MenuData()
        {
            Children = new List<ItemMetadata>();
            Separators = new List<int>();
        }
    }
}