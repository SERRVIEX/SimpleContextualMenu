namespace SimpleContextualMenu.Examples
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    public class Test : MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                ContextualMenu menu = ContextualMenu.Create(Input.mousePosition);
                menu.Add("Open", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.Add("Save", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.AddSeparator(string.Empty);
                menu.Add("Assign", new TextItemData(false, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.Add("Maximize", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());

                menu.Add("General/Scene", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.Add("General/Game", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.AddSeparator("General");
                menu.Add("General/Inspector", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.Add("General/Hierarchy", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());

                menu.Add("Rendering/Lightning", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.Add("Rendering/Light Explorer", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.AddSeparator("Rendering");
                menu.Add("Rendering/Ambient Occlusion", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                
                menu.Add("Layouts/2 by 3", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.Add("Layouts/4 split", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());

                menu.Add("Layouts/Delete Layout/2 by 3", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());
                menu.Add("Layouts/Delete Layout/4 split", new TextItemData(true, () => { }), ContextualMenuManager.GetItem<TextItemView>());

                menu.Build();
            }
        }
    }
}