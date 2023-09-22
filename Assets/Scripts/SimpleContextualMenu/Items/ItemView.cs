namespace SimpleContextualMenu.Items
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.EventSystems;

    public abstract class ItemView<T> : ItemViewBase where T : ItemDataBase
    {
        protected T Data { get; private set; }
       
        // Methods

        public override void Set(string label, ItemDataBase data)
        {
            Data = data as T;
            Label.text = label;

            Background.color = Color.clear;
            Label.color = data.IsActive ? new Color32(176, 176, 176, 255) : new Color32(97, 97, 97, 255);
            Arrow.color = data.IsActive ? new Color32(97, 97, 97, 255) : new Color32(65, 65, 65, 255);
            Arrow.gameObject.SetActive(Metadata.Submenu != null);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!Data.IsActive)
                return;

            // If the menu has a submenu open from another item tthen dismiss it.
            if(Menu.Child != null && Menu.Child != Submenu)
                Menu.Child.Dismiss();

            // If this item has a not opened submenu, then show it.
            if(Metadata.Submenu != null && Submenu == null)
            {
                Submenu = ContextualMenu.Create(RectTransform, ContextualMenu.Position.Right);
                Submenu.Set(Menu, Metadata.Submenu);
                Submenu.Build();
            }

            Background.color = new Color32(87, 87, 87, 255);
            Label.color = Color.white;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!Data.IsActive)
                return;

            // We can't press on item with submenu, this is not a button.
            if (Metadata.Submenu != null)
                return;

            Data.OnClick.Invoke();
            Menu.DismissRecursive();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!Data.IsActive)
                return;

            StartCoroutine(OnPointerExitImpl());

            Background.color = Color.clear;
            Label.color = new Color32(154, 154, 154, 255);
        }

        private IEnumerator OnPointerExitImpl()
        {
            yield return new WaitForEndOfFrame();

            if (Submenu != null)
                if (ContextualMenu.Selected != Submenu)
                    Submenu.Dismiss();
        }
    }
}