namespace SimpleContextualMenu.Examples
{
    using UnityEngine.Events;

    using SimpleContextualMenu.Items;

    public class TextItemData : ItemDataBase
    {
        public TextItemData(bool isActive, UnityAction onClick) : base(isActive, onClick) { }
    }
}