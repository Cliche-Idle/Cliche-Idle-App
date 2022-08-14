using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class PopUpBase : UIScript
{
    public VisualTreeAsset PopUpBaseUXML;
    private VisualElement PopUpElement;
    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        PopUpBaseUXML.CloneTree(Navigator.Target.rootVisualElement);
        PopUpElement = Navigator.Target.rootVisualElement.Q<VisualElement>("PopUpBase");
        PopUpElement.Q<Button>("PopUpCloseButton").clicked += ClosePopUp;
    }

    protected override void OnLeaveFocus(object sender, EventArgs e)
    {
        Navigator.Target.rootVisualElement.Remove(PopUpElement);
    }

    private void ClosePopUp()
    {
        ClearView();
    }
}