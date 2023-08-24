namespace SimpleContextualMenu
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using SimpleContextualMenu.Items;

    public sealed class ContextualMenuManager : MonoBehaviour
    {
        public static ContextualMenuManager Instance { get; private set; }

        public static Camera Camera { get; private set; }
        [SerializeField] private Camera _camera;

        public static RectTransform Content { get; private set; }
        [SerializeField] private RectTransform _content;

        [SerializeField, Space(10)] private ContextualMenu _contextualMenuPrefab;
        [SerializeField] private GameObject _separatorPrefab;
        [SerializeField] private List<ItemViewBase> _itemPrefabs = new List<ItemViewBase>();

        // Constructors

        private ContextualMenuManager() { }

        // Methods

        public void Awake()
        {
            Instance = this;
            Camera = _camera;
            Content = _content;
        }

        public static ContextualMenu Create()
        {
            return Instance.CreateImpl();
        }

        private ContextualMenu CreateImpl()
        {
            ContextualMenu menu = Instantiate(_contextualMenuPrefab, _content);
            return menu;
        }

        public static GameObject GetSeparator()
        {
            return Instance._separatorPrefab;
        }

        public static T GetItem<T>() where T : ItemViewBase
        {
            return Instance.GetItemImpl<T>();
        }

        private T GetItemImpl<T>() where T : ItemViewBase
        {
            for (int i = 0; i < _itemPrefabs.Count; i++)
                if (_itemPrefabs[i] is T)
                    return _itemPrefabs[i] as T;

            throw new Exception($"Class '{typeof(T)} was not found.'");
        }
    }
}