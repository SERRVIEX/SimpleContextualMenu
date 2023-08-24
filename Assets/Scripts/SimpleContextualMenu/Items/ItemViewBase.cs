namespace SimpleContextualMenu.Items
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using TMPro;

    public abstract class ItemViewBase : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        protected ContextualMenu Menu { get; private set; }
        protected ItemMetadata Metadata { get; private set; }

        [field: SerializeField] protected RectTransform RectTransform { get; private set; }
        [field: SerializeField] protected Image Background { get; private set; }
        [field: SerializeField] protected TextMeshProUGUI Title { get; private set; }
        [field: SerializeField] protected Image Arrow { get; private set; }

        // Methods

        public void Link(ContextualMenu menu, ItemMetadata metadata)
        {
            Menu = menu;
            Metadata = metadata;
        }
            
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