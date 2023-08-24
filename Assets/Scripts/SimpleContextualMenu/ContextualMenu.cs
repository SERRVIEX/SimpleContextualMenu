namespace SimpleContextualMenu
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using SimpleContextualMenu.Items;
    using SimpleContextualMenu.Internal;

    public sealed class ContextualMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static ContextualMenu Selected { get; private set; }

        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public Image Background { get; private set; }
        [field: SerializeField] public GameObject Lock { get; private set; }
        [field: SerializeField] public MenuView View { get; private set; }
        public MenuData Data { get; private set; }

        [field: SerializeField] public ContextualMenu Parent { get; private set; }
        [field: SerializeField] public ContextualMenu Child { get; private set; }

        // Constructors

        private ContextualMenu() { }

        // Methods

        public static ContextualMenu Create(Vector3 mousePosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ContextualMenuManager.Content, mousePosition, ContextualMenuManager.Camera, out Vector2 value);
            Vector3 localPosition = value;
            localPosition.z = 0;
            ContextualMenu menu = ContextualMenuManager.Create();
            menu.View.RectTransform.SetAnchor(Anchor.TopLeft);
            menu.View.RectTransform.SetPivot(Pivot.TopLeft);
            menu.View.RectTransform.localPosition = localPosition;
            return menu;
        }

        public enum Position
        {
            Left, Right, Top, Down
        }

        public static ContextualMenu Create(RectTransform referenceRect, Position position, Vector2 offset = default)
        {
            Vector3 localPosition = CalculatePosition(referenceRect, position, offset);
            ContextualMenu menu = ContextualMenuManager.Create();
            menu.View.RectTransform.SetAnchor(Anchor.TopLeft);
            menu.View.RectTransform.SetPivot(Pivot.TopLeft);
            menu.View.RectTransform.localPosition = localPosition;
            return menu;
        }

        private static Vector3 CalculatePosition(RectTransform referenceRect, Position position, Vector2 offset)
        {
            Vector3 positionOffset = new Vector3(offset.x, offset.y, 0);
            Vector3 worldPosition = referenceRect.TransformPoint(referenceRect.rect.center);
            Vector3 localPosition = ContextualMenuManager.Content.InverseTransformPoint(worldPosition);
            localPosition.z = 0;

            switch (position)
            {
                case Position.Left:
                    localPosition.y += referenceRect.rect.height / 2f;
                    localPosition += Vector3.left * (referenceRect.rect.width / 2 + positionOffset.x);
                    break;

                case Position.Right:
                    localPosition.y += referenceRect.rect.height / 2f;
                    localPosition += Vector3.right * (referenceRect.rect.width / 2 + positionOffset.x);
                    break;

                case Position.Top:
                    localPosition.x += referenceRect.rect.width / 2f;
                    localPosition += Vector3.up * (referenceRect.rect.height / 2 + positionOffset.y);
                    break;

                case Position.Down:
                    localPosition.x += referenceRect.rect.width / 2f;
                    localPosition += Vector3.down * (referenceRect.rect.height / 2 + positionOffset.y);
                    break;
            }

            return localPosition;
        }

        public void Set(ContextualMenu parent, MenuData data)
        {
            Data = data;
            Parent = parent;
            Parent.Child = this;

            transform.SetParent(parent.RectTransform);
            Background.raycastTarget = false;
            Lock.SetActive(false);
        }

        public void Add<T1, T2>(string path, T1 data, T2 view) 
            where T1 : ItemDataBase
            where T2 : ItemViewBase
        {
            if(Data == null)
                Data = new MenuData();

            if (string.IsNullOrEmpty(path))
                throw new Exception("Path can't be null or empty.");

            if (path.Contains('/'))
            {
                string[] items = path.Split('/');
                for (int i = 0; i < items.Length; i++)
                    if (string.IsNullOrEmpty(items[i]))
                        throw new Exception("Path can't have a null or empty node.");

                CreateTree(Data, data, view, 0, items);
            }
            else
                CreateTree(Data, data, view, 0, path);
        }

        private void CreateTree<T1, T2>(MenuData parent, T1 data, T2 view, int index, params string[] items)
            where T1 : ItemDataBase
            where T2 : ItemViewBase
        {
            {
                // Go through the menu items.
                for (int j = 0; j < parent.Children.Count; j++)
                {
                    ItemMetadata metadata = parent.Children[j];

                    // Verify if the menu already has an item with this name. 
                    if (metadata.Title == items[index])
                    {
                        index++;
                        if (index == items.Length)
                            throw new Exception(string.Join('/', items) + " already exists.");

                        // Go to the next submenu.
                        MenuData submenu = GetOrCreateMenu(metadata);
                        CreateTree(submenu, data, view, index, items);
                        return;
                    }
                }
            }

            {
                // Create new menu item.
                ItemMetadata metadata = new ItemMetadata(items[index], data, view, parent);
                // Add it to menu list.
                parent.Children.Add(metadata);

                index++;
                if (index == items.Length)
                    return;

                // Go to the next submenu.
                MenuData submenu = GetOrCreateMenu(metadata);
                CreateTree(submenu, data, view, index, items);
            }
        }

        private MenuData GetOrCreateMenu(ItemMetadata menuItemData)
        {
            if (menuItemData.Submenu == null)
                menuItemData.Submenu = new MenuData();
            return menuItemData.Submenu;
        }

        public void AddSeparator(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                Data.Separators.Add(Data.Children.Count);
                return;
            }
            ItemMetadata menuItemData = GetMenuItem(path);
            MenuData menuData = menuItemData.Submenu;
            menuData.Separators.Add(menuData.Children.Count);
        }

        private ItemMetadata GetMenuItem(string path)
        {
            ItemMetadata menuItemData;
            if (path.Contains('/'))
            {
                string[] items = path.Split('/');
                for (int i = 0; i < items.Length; i++)
                    if (string.IsNullOrEmpty(items[i]))
                        throw new Exception("Path can't have a null or empty node.");

                menuItemData = GetMenuItem(Data, 0, items);
            }
            else
                menuItemData = GetMenuItem(Data, 0, path);

            return menuItemData;
        }

        private ItemMetadata GetMenuItem(MenuData menu, int index, params string[] items)
        {
            for (int i = 0; i < menu.Children.Count; i++)
            {
                if (menu.Children[i].Title == items[index])
                {
                    if (index == items.Length - 1)
                        return menu.Children[i];

                    index++;
                    return GetMenuItem(menu.Children[i].Submenu, index, items);
                }
            }

            throw new Exception($"Path '{string.Join('/', items)}' wasn't found.");
        }

        public void Build()
        {
            Build(Data);
        }

        private void Build(MenuData menuData)
        {
            for (int i = 0; i < menuData.Children.Count; i++)
            {
                ItemMetadata metadata = menuData.Children[i];
                ItemViewBase view = Instantiate(metadata.View, View.transform);
                view.Link(this, metadata);
                view.Set(metadata.Title, metadata.Data);
            }

            for (int i = menuData.Separators.Count - 1; i >= 0; i--)
            {
                Transform separator = Instantiate(ContextualMenuManager.GetSeparator(), View.transform).transform;
                separator.SetSiblingIndex(menuData.Separators[i]);
            }
        }

        private void OnValidate()
        {
            if (RectTransform == null)
                RectTransform = GetComponent<RectTransform>();

            if (Background == null)
                Background = GetComponent<Image>();
        }

        public void Dismiss()
        {
            Destroy(gameObject);
        }

        public void DismissRecursive()
        {
            Dismiss(this);
        }

        private void Dismiss(ContextualMenu menu)
        {
            if(menu.Parent == null)
            {
                Destroy(menu.gameObject);
                return;
            }

            Dismiss(menu.Parent);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Selected = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Selected = null;
        }
    }
}