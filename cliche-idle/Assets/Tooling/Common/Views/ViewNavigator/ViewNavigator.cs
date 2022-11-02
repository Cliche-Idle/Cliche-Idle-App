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
        private List<UIScript> Views = new List<UIScript>();

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
            if (viewData.UXMLDocument != null)
            {
                if (viewData.IsViewActive == false)
                {
                    // If there isn't a dependency specified, spawn the tree into the Nav target root
                    if (viewData.Dependency != null || viewData.ContainerID.Length != 0)
                    {
                        // Grab and clear the target container
                        VisualElement targetContainer = GetTargetContainer(viewData.ContainerID);
                        if (targetContainer != null)
                        {
                            // Clear the view container
                            ClearUpViewContainer(viewData.ContainerID);
                            viewData.UXMLDocument.CloneTree(targetContainer);
                            //targetContainer.Add(viewData.UXMLDocument.Instantiate());
                        }
                    }
                    else
                    {
                        viewData.UXMLDocument.CloneTree(Target.rootVisualElement);
                    }
                }
            }
        }

        /// <summary>
        /// Registers a new view to the navigator. Used internally for UIScripts to register themselves.
        /// </summary>
        /// <param name="view"></param>
        public void RegisterView(UIScript view)
        {
            // Todo: check for duplicates. Since this is only used internally, throw a warning and otherwise ignore.
            if (view != null)
            {
                Views.Add(view);
            }
        }

        /// <summary>
        /// Clears the targeted View's container, and triggers its and every active sub-View's OnLeaveFocus event.
        /// </summary>
        /// <param name="viewID"></param>
        public void ClearUpViewContainer(string containerID)
        {
            // Grab and clear the target container
            VisualElement baseTargetContainer = GetTargetContainer(containerID);

            // Clear the view with the same container ID if it's in focus, and trigger its OnLeaveFocus event
            if (baseTargetContainer != null)
            {
                // TODO: instead of clearing the entire container, ask for the target and sender ID so of a container has multiple active views, only one of them is removed
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
                Debug.LogWarning($"ViewNavigator could not find container <color=blue>{containerID}</color> on the targeted document.");   
            }
            return targetContainer;
        }

        /// <summary>
        /// Gets the View with the given ID.
        /// </summary>
        /// <param name="viewID"></param>
        /// <returns></returns>
        public UIScript GetView(string viewID)
        {
            var viewData = Views.Find(view => view.ID == viewID);
            if (viewData == null)
            {
                Debug.LogWarning($"ViewNavigator could not find a registered view with the key <color=blue>{viewID}</color>.");
            }
            return viewData;
        }

        private List<UIScript> CompileDependencyList(string viewID)
        {
            List<UIScript> dependencies = new List<UIScript>();

            UIScript dependency = GetView(viewID).Dependency;
            dependencies.Add(dependency);

            while (dependency != null)
            {
                dependency = dependency.Dependency;
                if (dependency != null)
                {
                    dependencies.Add(dependency);
                }
            }

            return dependencies;
        }

        private void ClimbDependencyStack()
        {

        }
    }
}