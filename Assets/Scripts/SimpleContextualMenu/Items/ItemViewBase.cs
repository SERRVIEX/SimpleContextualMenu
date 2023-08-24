namespace SimpleContextualMenu.Items
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using TMPro;

    using SimpleContextualMenu.Internal;

    public abstract class ItemViewBase : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        /// <summary>
        /// Reference to the menu.
        /// </summary>
        protected ContextualMenu Menu { get; private set; }

        /// <summary>
        /// All the data intended for this item.
        /// </summary>
        protected ItemMetadata Metadata { get; private set; }

        /// <summary>
        /// Opened submenu if it has it.
        /// </summary>
        protected ContextualMenu Submenu;

        [field: SerializeField] protected RectTransform RectTransform { get; private set; }
        [field: SerializeField] protected Image Background { get; private set; }
        [field: SerializeField] protected TextMeshProUGUI Title { get; private set; }
        [field: SerializeField] protected Image Arrow { get; private set; }

        // Methods

        internal void Link(ContextualMenu menu, ItemMetadata metadata)
        {
            Menu = menu;
            Metadata = metadata;
        }
            
        /// <summary>
        /// Fill the item with the data.
        /// </summary>
        public abstract void Set(string title, ItemDataBase data);

        public abstract void OnPointerEnter(PointerEventData eventData);
        public abstract void OnPointerClick(PointerEventData eventData);
        public abstract void OnPointerExit(PointerEventData eventData);

        protected virtual void OnValidate()
        {
            if(RectTransform == null)
                RectTransform = GetComponent<RectTransform>();

            RectTransform.SetAnchor(Anchor.TopLeft);
            RectTransform.SetPivot(Pivot.TopLeft);

            if (Background == null) 
                Background = GetComponent<Image>();
        }
    }
}