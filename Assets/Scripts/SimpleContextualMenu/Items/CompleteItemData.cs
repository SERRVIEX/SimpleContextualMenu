namespace SimpleContextualMenu.Items
{
    using UnityEngine;
    using UnityEngine.Events;

    public class CompleteItemData : ItemDataBase
    {
        public Sprite Icon { get; private set; }
        public string HotKey { get; private set; }

        // Methods

        public CompleteItemData(bool isActive, UnityAction onClick) : base(isActive, onClick) { }

        public CompleteItemData(Sprite icon, bool isActive, UnityAction onClick) : base(isActive, onClick)
        {
            Icon = icon;
        }

        public CompleteItemData(string hotkey, bool isActive, UnityAction onClick) : base(isActive, onClick)
        {
            HotKey = hotkey;
        }

        public CompleteItemData(Sprite icon, string hotkey, bool isActive, UnityAction onClick) : base(isActive, onClick)
        {
            Icon = icon;
            HotKey = hotkey;
        }
    }
}