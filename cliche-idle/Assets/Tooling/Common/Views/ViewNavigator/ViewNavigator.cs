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
        private List<UIScript> Views = new List<UIScript>();

        /// <summary>
        /// Switches the view in the target GameObject's UIDocument, at a specified VisualElement.
        /// </summary>
        /// <param name="viewID">The viewID to be switched in</param>
        public void SwitchToView(string viewID)
        {
            // Get the view assigned to the given ID
            var view = GetView(viewID);
            if (view != null)
            {
                view.DisplayView();
            }
        }

        /// <summary>
        /// Registers a view to the navigator. Used internally for UIScripts to register themselves.
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
                // TODO: instead of clearing the entire container, ask for the target and sender ID so if a container has multiple active views, only one of them is removed
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
        private UIScript GetView(string viewID)
        {
            var viewData = Views.Find(view => view.ID == viewID);
            if (viewData == null)
            {
                Debug.LogWarning($"ViewNavigator could not find a registered view with the key <color=blue>{viewID}</color>.");
            }
            return viewData;
        }
    }
}