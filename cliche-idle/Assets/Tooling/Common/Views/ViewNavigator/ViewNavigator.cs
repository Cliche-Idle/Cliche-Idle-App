using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIViews
{
    public class ViewNavigator : MonoBehaviour
    {
        /// <summary>
        /// The target UIDocument of this Navigator instance. 
        /// </summary>
        [field: SerializeField]
        public UIDocument Target { get; private set; }

        /// <summary>
        /// Contains the list of registered views.
        /// </summary>
        [field: SerializeField]
        public List<View> Views { get; private set; }

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
            if (viewData.Enabled == false)
            {
                // Grab and clear the target container
                VisualElement targetContainer = GetTargetContainer(viewData.ContainerID);
                if (viewData.UXMLDocument != null && targetContainer != null)
                {
                    // Clear the view container
                    ClearUpViewContainer(viewData.ContainerID);
                    viewData.UXMLDocument.CloneTree(targetContainer);
                    //targetContainer.Add(viewData.UXMLDocument.Instantiate());
                }
                // Fire update event
                viewData.SetState(true);
            }
            else
            {
                // View is already in focus
            }
        }

        /// <summary>
        /// Clears the targeted View's container, and triggers its and every active sub-View's OnLeaveFocus event.
        /// </summary>
        /// <param name="viewID"></param>
        public void ClearUpViewContainer(string containerID)
        {
            var baseTargetView = Views.Find(view => view.ContainerID == containerID && view.Enabled == true);
            // Grab and clear the target container
            VisualElement baseTargetContainer = GetTargetContainer(containerID);
            // Fire OnLeaveFocus event for the cleared view
            if (baseTargetView != null)
            {
                baseTargetView.SetState(false);
            }
            // Clear the view with the same container ID if it's in focus, and trigger its OnLeaveFocus event
            if (baseTargetContainer != null)
            {
                // Check and trigger the OnLeaveFocus for every view that may be active inside this one
                // * Assumes containerIDs are completely unique
                // * https://docs.unity3d.com/ScriptReference/UIElements.VisualElement-name.html
                foreach (var view in Views)
                {
                    if (view.Enabled)
                    {
                        // Check if the view's container is inside the base one and push it out of focus
                        var subViewContainer = baseTargetContainer.Q(view.ContainerID);
                        if (subViewContainer != null)
                        {
                            view.SetState(false);
                        }
                    }
                }
                // Clear the base view container
                baseTargetContainer.Clear();
            }
        }

        /// <summary>
        /// Gets the VisualElement with the given name.
        /// </summary>
        /// <param name="containerID"></param>
        /// <returns></returns>
        public VisualElement GetTargetContainer(string containerID)
        {
            VisualElement targetContainer = Target.rootVisualElement.Q(containerID);
            if (targetContainer == null)
            {
                Debug.LogWarning($"ViewNavigator could not find targetcontainer `{containerID}` on document {Target}.");   
            }
            return targetContainer;
        }

        /// <summary>
        /// Gets the View with the given ID.
        /// </summary>
        /// <param name="viewID"></param>
        /// <returns></returns>
        public View GetView(string viewID)
        {
            var viewData = Views.Find(view => view.ID == viewID);
            if (viewData == null)
            {
                Debug.LogWarning($"ViewNavigator could not find a registered view with key `{viewID}`.");
            }
            return viewData;
        }
    }

    [Serializable]
    public class View
    {
        /// <summary>
        /// Unique ID referencing the view. This is used for calling a view switch.
        /// </summary>
        [field: SerializeField]
        public string ID { get; private set; }
        /// <summary>
        /// The ID of the view container in the target document. 
        /// The VisualElement with this ID will be cleared, and the contents of this view will be copied to its tree.
        /// </summary>
        [field: SerializeField]
        public string ContainerID { get; private set; }
        /// <summary>
        /// The UXML Document file containing the view to be switched in.
        /// </summary>
        [field: SerializeField]
        public VisualTreeAsset UXMLDocument { get; private set; }

        /// <summary>
        /// The current state of the view.
        /// </summary>
        [field: SerializeField]
        public bool Enabled { get; private set; }

        /// <summary>
        /// Sets the state of the view.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="parentDocument"></param>
        public void SetState(bool state)
        {
            if (Enabled == true && state == false)
            {
                if (OnLeaveFocus != null)
                {
                    OnLeaveFocus.Invoke(this, null);
                }
            }
            else if (Enabled == false && state == true)
            {
                if (OnEnterFocus != null)
                {
                    OnEnterFocus.Invoke(this, null);
                }
            }
            Enabled = state;
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
}