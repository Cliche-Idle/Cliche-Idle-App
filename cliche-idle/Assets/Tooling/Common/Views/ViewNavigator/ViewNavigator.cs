using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewNavigator : MonoBehaviour
{
    /// <summary>
    /// Contains the list of registered views.
    /// </summary>
    public List<ViewEntry> Views;

    /// <summary>
    /// An Event that fires whenever a view is changed / switched / hidden / updated.
    /// </summary>
    public event EventHandler<ViewNavigatorEventArgs> OnViewUpdate;

    // TODO: at startup the uniqueness of viewID keys should be ensured. Append a count number to them and log the change.
    private void Start() {
        
    }

    /// <summary>
    /// Switches the view in the target GameObject's UIDocument, at a specified VisualElement.
    /// </summary>
    /// <param name="target">The GameObject with the target UIDocument</param>
    /// <param name="viewID">The viewID to be switched in</param>
    /// <exception cref="NullReferenceException">Thrown when the specified containerID is not found on the target document.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the specified viewID is not registered.</exception>
    public void SwitchToView(GameObject target, string viewID)
    {
        // Grab the target's UIDocument
        UIDocument targetDocument = target.GetComponent<UIDocument>();
        // Get the ViewData assigned to the given ID
        var viewData = GetViewData(viewID);
        // Grab and clear the target container
        VisualElement targetContainer = GetTargetContainer(targetDocument, viewData.containerID);
        targetContainer.Clear();
        // TODO: Test this, we don't want any shared references if an instance is updated.
        // * Instantiate should make a new unique clone, so this is probably a non-issue.
        targetContainer.Add(viewData.UXMLDocument.Instantiate());
        // Fire update event
        OnViewUpdate.Invoke(null, new ViewNavigatorEventArgs(viewData.viewID, viewData.containerID));
    }

    public void ClearViewContainer(GameObject target, string viewID)
    {
        // Grab the target's UIDocument
        UIDocument targetDocument = target.GetComponent<UIDocument>();
        // Get the ViewData assigned to the given ID
        var viewData = GetViewData(viewID);
        // Grab and clear the target container
        VisualElement targetContainer = GetTargetContainer(targetDocument, viewData.containerID);
        targetContainer.Clear();
        // Fire update event
        OnViewUpdate.Invoke(null, new ViewNavigatorEventArgs(viewData.viewID, viewData.containerID));
    }

    private VisualElement GetTargetContainer(UIDocument targetDocument, string containerID)
    {
        VisualElement targetContainer = targetDocument.rootVisualElement.Q(containerID);
        if (targetContainer != null)
        {
            return targetContainer;
        }
        else
        {
            // Handle invalid containerID
            throw new NullReferenceException($"ViewNavigator could not find targetcontainer `{containerID}` on document {targetDocument}.");
        }
    }

    private ViewEntry GetViewData(string viewID)
    {
        var viewData = Views.Find(view => view.viewID == viewID);
        if (viewData != null)
        {
            return viewData;
        }
        else
        {
            // Handle invalid ViewID
            throw new KeyNotFoundException($"ViewNavigator could not find a registered view with key `{viewID}`.");
        }
    }
    
    [Serializable]
    public class ViewEntry
    {
        /// <summary>
        /// Unique ID referencing the view. This is used for calling a view switch.
        /// </summary>
        public string viewID;
        /// <summary>
        /// The ID of the view container in the target document. 
        /// The VisualElement with this ID will be cleared, and the contents of this view will be copied to its tree.
        /// </summary>
        public string containerID;
        /// <summary>
        /// The UXML Document file containing the view to be switched in.
        /// </summary>
        public VisualTreeAsset UXMLDocument;
    }
}


public class ViewNavigatorEventArgs : EventArgs
{
    private string viewID;
    private string containerID;

    public ViewNavigatorEventArgs(string calledViewID, string updatedContainerID)
    {
        viewID = calledViewID;
        containerID = updatedContainerID;
    }

    /// <summary>
    /// The ViewID that was called for update.
    /// </summary>
    public string ViewID 
    {
        get { return viewID; }
    }

    /// <summary>
    /// The ContainerID that got modified.
    /// </summary>
    public string ContainerID
    {
        get { return containerID; }
    }
}
