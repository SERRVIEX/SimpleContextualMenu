namespace SimpleContextualMenu.Items
{
    using UnityEngine.Events;

    public abstract class ItemDataBase
    {
        public bool IsActive { get; protected set; }
        public UnityAction OnClick { get; protected set; }

        // Constructors

        public ItemDataBase(bool isActive, UnityAction onClick)
        {
            IsActive = isActive;
            OnClick = onClick;
        }
    }
}