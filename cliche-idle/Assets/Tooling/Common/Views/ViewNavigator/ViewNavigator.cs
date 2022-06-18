using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewNavigator : MonoBehaviour
{
    public List<ViewEntry> Views;

    public void SwitchToView(GameObject target, string viewID)
    {
        UIDocument targetDocument = target.GetComponent<UIDocument>();
        var viewData = Views.Find(view => view.viewID == viewID);
        if (viewData != null)
        {
            VisualElement targetContainer = targetDocument.rootVisualElement.Q(viewData.containerID);
            targetContainer.Clear();
            // TODO: Test this, we don't want any shared references if an instance is updated.
            targetContainer.Add(viewData.view.rootVisualElement);
        }
        else
        {
            // Handle incorrect ViewID
            throw new KeyNotFoundException($"No view with key {viewID} is registered.");
        }
    }
    
    [Serializable]
    public class ViewEntry
    {
        public string viewID;
        public string containerID;
        public UIDocument view;

        public ViewEntry(string ViewID, string ContainerID, UIDocument ViewDocument)
        {
            viewID = ViewID;
            containerID = ContainerID;
            view = ViewDocument;
        }
    }
}
