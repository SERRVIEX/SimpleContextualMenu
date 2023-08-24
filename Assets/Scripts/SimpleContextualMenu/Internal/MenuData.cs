namespace SimpleContextualMenu.Internal
{
    using System.Collections.Generic;

    public class MenuData
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public List<ItemMetadata> Children { get; private set; }

        /// <summary>
        /// List of the sibling index of the separators.
        /// </summary>
        public List<int> Separators { get; private set; }

        // Constructors

        public MenuData()
        {
            Children = new List<ItemMetadata>();
            Separators = new List<int>();
        }
    }
}