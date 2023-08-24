namespace SimpleContextualMenu.Examples
{
    using UnityEngine;

    using SimpleContextualMenu.Items;

    public class Test : MonoBehaviour
    {
        [SerializeField] private Sprite _open;
        [SerializeField] private Sprite _save;
        [SerializeField] private Sprite _render;
        [SerializeField] private Sprite _layout;

        // Methods

        private void LateUpdate()
        {
            if(Input.GetMouseButtonDown(1))
            {
                ContextualMenu menu = ContextualMenu.Create(Input.mousePosition);
                menu.Add("Open", new CompleteItemData(_open, "CTRL+O", true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("Save", new CompleteItemData(_save, "CTRL+S", true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.AddSeparator(string.Empty);
                menu.Add("Assign", new CompleteItemData(false, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("Maximize", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());

                menu.Add("General", new CompleteItemData("CTRL+G", true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());

                menu.Add("General/Scene", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("General/Game", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.AddSeparator("General");
                menu.Add("General/Inspector", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("General/Hierarchy", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());

                menu.Add("Rendering", new CompleteItemData(_render, true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("Rendering/Lightning", new CompleteItemData("CTRL+L", true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("Rendering/Light Explorer", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.AddSeparator("Rendering");
                menu.Add("Rendering/Ambient Occlusion", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());

                menu.Add("Layouts", new CompleteItemData(_layout, true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("Layouts/2 by 3", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("Layouts/4 split", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());

                menu.Add("Layouts/Delete Layout/2 by 3", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());
                menu.Add("Layouts/Delete Layout/4 split", new CompleteItemData(true, () => { }), ContextualMenuManager.GetItem<CompleteItemView>());

                menu.Build();
            }
        }
    }
}