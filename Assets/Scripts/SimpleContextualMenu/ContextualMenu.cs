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

        [SerializeField] private GameObject _lock;
        [SerializeField] private MenuView _view;
        private MenuData _data;

        public ContextualMenu Parent { get; private set; }
        public ContextualMenu Child { get; private set; }

        // Constructors

        private ContextualMenu() { }

        // Methods

        /// <summary>
        /// Create a contextual menu at mouse position.
        /// </summary>
        public static ContextualMenu Create(Vector3 mousePosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ContextualMenuManager.Content, mousePosition, ContextualMenuManager.Camera, out Vector2 value);
            Vector3 localPosition = value;
            localPosition.z = 0;
            ContextualMenu menu = ContextualMenuManager.Create();
            menu._view.RectTransform.SetAnchor(Anchor.TopLeft);
            menu._view.RectTransform.SetPivot(Pivot.TopLeft);
            menu._view.RectTransform.localPosition = localPosition;
            return menu;
        }

        /// <summary>
        /// Create contextual menu toward another rect.
        /// </summary>
        public static ContextualMenu Create(RectTransform referenceRect, Position position)
        {
            ContextualMenu menu = ContextualMenuManager.Create();
            Vector3 localPosition = menu.CalculatePosition(referenceRect, position);
            menu._view.RectTransform.SetAnchor(Anchor.TopLeft);
            menu._view.RectTransform.SetPivot(Pivot.TopLeft);
            menu._view.RectTransform.localPosition = localPosition;
            return menu;
        }

        private Vector3 CalculatePosition(RectTransform referenceRect, Position position)
        {
            Vector3 positionOffset = new Vector3(0, 2, 0);
            Vector3 worldPosition = referenceRect.TransformPoint(referenceRect.rect.center);
            Vector3 localPosition = ContextualMenuManager.Content.InverseTransformPoint(worldPosition);
            localPosition.z = 0;

            switch (position)
            {
                case Position.Left:
                    localPosition.y += referenceRect.rect.height / 2f + positionOffset.y / 2;
                    localPosition += Vector3.left * (referenceRect.rect.width / 2 + positionOffset.x);
                    break;

                case Position.Right:
                    localPosition.y += referenceRect.rect.height / 2f + positionOffset.y / 2;
                    localPosition += Vector3.right * (referenceRect.rect.width / 2 + positionOffset.x);
                    break;

                case Position.Top:
                    localPosition.x -= referenceRect.rect.width / 2f + positionOffset.x / 2;
                    localPosition += Vector3.up * (referenceRect.rect.height / 2 + positionOffset.y);
                    break;

                case Position.Down:
                    localPosition.x -= referenceRect.rect.width / 2f + positionOffset.x / 2;
                    localPosition += Vector3.down * (referenceRect.rect.height / 2 + positionOffset.y);
                    break;
            }

            return localPosition;
        }

        public void Set(ContextualMenu parent, MenuData data)
        {
            _data = data;
            Parent = parent;
            Parent.Child = this;

            transform.SetParent(parent.RectTransform);
            Background.raycastTarget = false;
            _lock.SetActive(false);
        }

        public void Add<T1, T2>(string path, T1 data, T2 view) 
            where T1 : ItemDataBase
            where T2 : ItemViewBase
        {
            if(_data == null)
                _data = new MenuData();

            if (string.IsNullOrEmpty(path))
                throw new Exception("Path can't be null or empty.");

            if (path.Contains('/'))
            {
                string[] items = path.Split('/');
                for (int i = 0; i < items.Length; i++)
                    if (string.IsNullOrEmpty(items[i]))
                        throw new Exception("Path can't have a null or empty node.");

                CreateTree(_data, data, view, 0, items);
            }
            else
                CreateTree(_data, data, view, 0, path);
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
                _data.Separators.Add(_data.Children.Count);
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

                menuItemData = GetMenuItem(_data, 0, items);
            }
            else
                menuItemData = GetMenuItem(_data, 0, path);

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
            Build(_data);
        }

        private void Build(MenuData menuData)
        {
            for (int i = 0; i < menuData.Children.Count; i++)
            {
                ItemMetadata metadata = menuData.Children[i];
                ItemViewBase view = Instantiate(metadata.View, _view.transform);
                view.Link(this, metadata);
                view.Set(metadata.Title, metadata.Data);
            }

            for (int i = menuData.Separators.Count - 1; i >= 0; i--)
            {
                Transform separator = Instantiate(ContextualMenuManager.GetSeparator(), _view.transform).transform;
                separator.SetSiblingIndex(menuData.Separators[i] + 1);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Selected = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Selected = null;
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
            if (menu.Parent == null)
            {
                Destroy(menu.gameObject);
                return;
            }

            Dismiss(menu.Parent);
        }

        private void OnValidate()
        {
            if (RectTransform == null)
                RectTransform = GetComponent<RectTransform>();

            if (Background == null)
                Background = GetComponent<Image>();
        }

        // Other

        public enum Position
        {
            Left, 
            Right, 
            Top, 
            Down
        }
    }
}