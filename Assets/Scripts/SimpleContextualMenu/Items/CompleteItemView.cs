namespace SimpleContextualMenu.Items
{
    using UnityEngine;
    using UnityEngine.UI;

    using TMPro;

    public class CompleteItemView : ItemView<CompleteItemData>
    {
        [field: SerializeField] protected Image Icon { get; private set; }
        [field: SerializeField] protected TextMeshProUGUI HotKey { get; private set; }

        // Methods

        public override void Set(string title, ItemDataBase data)
        {
            base.Set(title, data);

            Icon.sprite = Data.Icon;
            Icon.gameObject.SetActive(Icon.sprite != null);

            HotKey.text = Data.HotKey;
            HotKey.gameObject.SetActive(!string.IsNullOrEmpty(HotKey.text) && Metadata.Submenu == null);
        }
    }
}