using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewNavigator : MonoBehaviour
{
    /// <summary>
    /// The target UIDocument of this Navigator instance. 
    /// </summary>
    public UIDocument Target;

    /// <summary>
    /// Contains the list of registered views.
    /// </summary>
    [field: SerializeField]
    public List<ViewEntry> Views { get; private set; }

    /// <summary>
    /// Switches the view in the target GameObject's UIDocument, at a specified VisualElement.
    /// </summary>
    /// <param name="viewID">The viewID to be switched in</param>
    /// <exception cref="NullReferenceException">Thrown when the specified containerID is not found on the target document.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the specified viewID is not registered.</exception>
    public void SwitchToView(string viewID)
    {
        // Get the ViewData assigned to the given ID
        var viewData = GetView(viewID);
        // Grab and clear the target container
        VisualElement targetContainer = GetTargetContainer(viewData.containerID);
        // Clear the view with the same container ID if it's in focus, and trigger its OnLeaveFocus event
        var outOfFocusView = Views.Find(view => view.containerID == viewData.containerID && view.InFocus == true);
        if (outOfFocusView != null)
        {
            outOfFocusView.SetState(false);
            if (outOfFocusView.OnLeaveFocus != null)
            {
                outOfFocusView.OnLeaveFocus.Invoke(this, null);
            }
        }
        targetContainer.Clear();
        // TODO: Test this, we don't want any shared references if an instance is updated.
        // * Instantiate should make a new unique clone, so this is probably a non-issue.
        targetContainer.Add(viewData.UXMLDocument.Instantiate());
        // Fire update event
        viewData.SetState(true);
        if (viewData.OnEnterFocus != null)
        {
            viewData.OnEnterFocus.Invoke(this, null);
        }
    }

    public void ClearViewContainer(string viewID)
    {
        // Get the ViewData assigned to the given ID
        var viewData = GetView(viewID);
        // Grab and clear the target container
        VisualElement targetContainer = GetTargetContainer(viewData.containerID);
        targetContainer.Clear();
        // Fire update event
        if (viewData.InFocus == true)
        {
            viewData.SetState(false);
            if (viewData.OnLeaveFocus != null)
            {
                viewData.OnLeaveFocus.Invoke(this, null);
            }
        }
    }

    public VisualElement GetTargetContainer(string containerID)
    {
        VisualElement targetContainer = Target.rootVisualElement.Q(containerID);
        if (targetContainer != null)
        {
            return targetContainer;
        }
        else
        {
            // Handle invalid containerID
            throw new NullReferenceException($"ViewNavigator could not find targetcontainer `{containerID}` on document {Target}.");
        }
    }

    public ViewEntry GetView(string viewID)
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

    /// <summary>
    /// The current state of the view.
    /// </summary>
    public bool InFocus { get; private set; }

    /// <summary>
    /// Sets the state of the view.
    /// </summary>
    /// <param name="state"></param>
    public void SetState(bool state)
    {
        InFocus = state;
    }

    /// <summary>
    /// Event that fires when the view is created.
    /// </summary>
    public EventHandler OnEnterFocus;

    /// <summary>
    /// Event that fires when the view is destroyed.
    /// </summary>
    public EventHandler OnLeaveFocus;
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
