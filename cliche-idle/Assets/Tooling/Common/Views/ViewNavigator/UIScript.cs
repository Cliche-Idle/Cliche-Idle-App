using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace UIViews
{
    /// <summary>
    /// Used to create <see cref="ViewNavigator"/> views.
    /// </summary>
    public class UIScript : MonoBehaviour
    {
        /// <summary>
        /// The ViewNavigator instance this script is hooked up to.
        /// </summary>
        [field: SerializeField]
        [field: HideInInspector]
        public ViewNavigator Navigator { get; protected set; }

        /// <summary>
        /// The ID of the View this script controls.
        /// </summary>
        [field: SerializeField]
        [field: HideInInspector]
        public string ID { get; protected set; }

        /// <summary>
        /// The name of the VisualElement this view will be wrapped into when displayed.
        /// </summary>
        public string WrapperVisualElementName
        {
            get
            {
                return $"UIView__{ID}";
            }
        }

        /// <summary>
        /// The UXML Document file containing the view to be switched in.
        /// </summary>
        [field: SerializeField]
        [field: HideInInspector]
        public VisualTreeAsset UXMLDocument { get; protected set; }

        /// <summary>
        /// Sets whether or not the view is static. If set to <see langword="true"/>, <see cref="UIUpdate()"/> will never run. Default is <see langword="false"/>.
        /// </summary>
        [field: SerializeField]
        [field: HideInInspector]
        public bool IsStatic { get; protected set; } = false;

        /// <summary>
        /// The current state of the view.
        /// </summary>
        [field: SerializeField]
        [field: HideInInspector]
        public bool IsViewActive { get; protected set; } = false;

        /// <summary>
        /// The dependency of this view, which must be present before this can be loaded. If none is specified, the view will be spawned in the Navigator target's root.
        /// </summary>
        [field: SerializeField]
        [field: HideInInspector]
        public UIScript Dependency { get; protected set; }

        /// <summary>
        /// The ID of the view container in the target document. 
        /// The VisualElement with this ID will be cleared, and the contents of this view will be copied to its tree.
        /// </summary>
        [field: SerializeField]
        [field: HideInInspector]
        public string ContainerID { get; protected set; }

        /// <summary>
        /// Gets the view's base VisualElement.
        /// </summary>
        /// <returns></returns>
        public VisualElement GetViewContainer()
        {
            // Grab visual element via Navigator
            return Navigator.GetTargetContainer(WrapperVisualElementName);
        }

        /// <summary>
        /// Displays the view on the attached Navigator's target.
        /// See <see cref="ViewNavigator.SwitchToView(string)"/> for details.
        /// </summary>
        public virtual void DisplayView()
        {
            if (ID != null && ID.Length != 0)
            {
                //Navigator.SwitchToView(ID);
                //IsViewActive = true;

                if (UXMLDocument != null)
                {
                    if (IsViewActive == false)
                    {
                        if (ContainerID != null && ContainerID.Length != 0)
                        {
                            // Spawn dependencies
                            SetDependenciesActive();

                            // Get target container
                            VisualElement viewTargetContainer = Navigator.GetTargetContainer(ContainerID);
                            if (viewTargetContainer != null)
                            {
                                // Clear the view container
                                viewTargetContainer.Clear();

                                VisualElement viewWrapperContainer = new VisualElement();
                                UXMLDocument.CloneTree(viewWrapperContainer);
                                viewWrapperContainer.name = WrapperVisualElementName;

                                // The normal event syntax is more useful here than the Unity one, so lambda it 
                                // TODO: Make the sender the lowest exiting UIScript that is detaching 

                                // OnEnterFocus
                                viewWrapperContainer.RegisterCallback<AttachToPanelEvent>(evt => {
                                    OnEnterFocus();
                                    IsViewActive = true;
                                });

                                // OnLeaveFocus
                                viewWrapperContainer.RegisterCallback<DetachFromPanelEvent>(evt => {
                                    OnLeaveFocus();
                                    IsViewActive = false;
                                });

                                viewTargetContainer.Add(viewWrapperContainer);
                            }
                        }
                        else
                        {
                            UXMLDocument.CloneTree(Navigator.Target.rootVisualElement);
                        }
                    }
                    else
                    {
                        //warning to console
                    }
                }
                else
                {
                    //See UIScriptEditor TODO
                }
            }
            else
            {
                // throw error
            }
        }

        /// <summary>
        /// Hides the view, and removes it from the hierarchy. 
        /// </summary>
        public virtual void HideView()
        {
            Navigator.ClearUpViewContainer(ContainerID);
        }

        /// <summary>
        /// Compiles the dependency stack of this view into a list.
        /// </summary>
        /// <returns></returns>
        public List<UIScript> GetDependencyList()
        {
            List<UIScript> dependencies = new List<UIScript>();
            UIScript dependency = Dependency;
            while (dependency != null)
            {
                dependencies.Add(dependency);
                dependency = dependency.Dependency;
            }
            return dependencies;
        }

        /// <summary>
        /// Runs through the dependency stack and calls every inactive inluded view's <see cref="DisplayView()"/>.
        /// </summary>
        private void SetDependenciesActive()
        {
            var deps = GetDependencyList();
            // Reverse the dependency list so the lowest dependency is first
            deps.Reverse();
            if (deps.Count > 0)
            {
                foreach (UIScript dependency in deps)
                {
                    // Check if the dependency is not active
                    if (dependency.IsViewActive == false)
                    {
                        dependency.DisplayView();
                    }
                }
            }
        }

        private void Awake()
        {
            if (Navigator != null)
            {
                Navigator.RegisterView(this);
            }
            else
            {
                throw new NullReferenceException($"{ID} does not have a UI Navigator instance assigned.");
            }
        }

        private void Update()
        {
            // Don't run updates if the view is marked as static
            if (IsStatic == false)
            {
                // Don't run updates if the view is not active
                if (IsViewActive)
                {
                    UIUpdate();
                }
            }
        }

        /// <summary>
        /// Identical to Update(), except it only runs when <see cref="IsStatic"/> is set to <see langword="false"/> and <see cref="IsViewActive"/> is <see langword="true"/>.
        /// </summary>
        protected virtual void UIUpdate()
        {

        }

        /// <summary>
        /// Runs when the view enters focus (Created).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnEnterFocus()
        {

        }

        /// <summary>
        /// Runs when the view leaves focus (Destroyed).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLeaveFocus()
        {

        }


        [CustomEditor(typeof(UIScript), true, isFallback = true)]
        public class UIScriptEditor : Editor
        {
            public string[] DependencyContainerIDs = new string[0];
            public int SelectedContainerIDIndex = 0;

            private bool IsFoldoutOpen = true;

            public override void OnInspectorGUI()
            {
                // Get the view data
                var view = target as UIScript;

                // View setup is in its own foldout so it doesn't take up too much space on derived scripts
                IsFoldoutOpen = EditorGUILayout.BeginFoldoutHeaderGroup(IsFoldoutOpen, "View setup");
                if (IsFoldoutOpen)
                {
                    // Increase ident by 1 so the foldout is more visually separated
                    EditorGUI.indentLevel++;
                    // Set the UI Navigator instance
                    view.Navigator = (ViewNavigator)EditorGUILayout.ObjectField("UI Navigator", view.Navigator, typeof(ViewNavigator), true);

                    EditorGUILayout.Space(10);

                    view.ID = EditorGUILayout.TextField("ID", view.ID);
                    view.UXMLDocument = (VisualTreeAsset)EditorGUILayout.ObjectField("UI Document", view.UXMLDocument, typeof(VisualTreeAsset), true);
                    view.IsStatic = EditorGUILayout.Toggle("Is Static", view.IsStatic);
                    view.IsViewActive = EditorGUILayout.Toggle("Is View Active", view.IsViewActive);

                    // Dependency handling
                    EditorGUILayout.Space(10);

                    EditorGUILayout.LabelField("Dependency setup", EditorStyles.boldLabel);
                    view.Dependency = (UIScript)EditorGUILayout.ObjectField("Dependency", view.Dependency, typeof(UIScript), true);

                    if (view.Navigator != null)
                    {
                        // FIXME: Potential performance and memory impact, as this runs multiple times, not just when the dependency changes
                        // Get the container IDs from the dependency, if there is one; otherwise get the Navigator target's root
                        if (view.Dependency != null)
                        {
                            UpdateDependencyContainerIDs(view.Dependency.UXMLDocument);
                        }
                        else
                        {
                            UpdateDependencyContainerIDs(view.Navigator.Target.visualTreeAsset);
                        }

                        SelectedContainerIDIndex = Array.IndexOf<string>(DependencyContainerIDs, view.ContainerID);
                        SelectedContainerIDIndex = EditorGUILayout.Popup("Target container ID: ", SelectedContainerIDIndex, DependencyContainerIDs);
                        if (SelectedContainerIDIndex != -1)
                        {
                            view.ContainerID = DependencyContainerIDs[SelectedContainerIDIndex];
                        }
                    }

                    // Display informational warning on the bottom of the foldout
                    ShowInformationWarnings(view);

                    // Reset ident to normal
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space(10);

                // Draw the default inspector last so the script's UI can be separated. This will draw the auto UI as normal for derived scripts
                DrawDefaultInspector();
            }

            /// <summary>
            /// Checks the view setup and displays informational warnings.
            /// </summary>
            /// <param name="view"></param>
            /// <returns></returns>
            private void ShowInformationWarnings(UIScript view)
            {
                if (view.UXMLDocument == null)
                {
                    EditorGUILayout.HelpBox("No UXML document has been added, so this view will not receive attach / detach events.", MessageType.Info);
                }
                if (view.Dependency == null)
                {
                    if (view.ContainerID == null || view.ContainerID.Length == 0)
                    {
                        EditorGUILayout.HelpBox("No dependency or container have been specified, so this view will be attached directly to the root visual element.", MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No dependency has been specified, so this view will be attached to the selected container on the root visual element.", MessageType.Info);
                    }
                }
            }


            /// <summary>
            /// Compiles and sets the list of container IDs in the dependency's VisualTreeAsset
            /// </summary>
            /// <param name="dependencyUXML"></param>
            private void UpdateDependencyContainerIDs(VisualTreeAsset dependencyUXML)
            {
                // Get every VisualElement in the dependency's hierarchy
                VisualElement layout = dependencyUXML.Instantiate();
                List<VisualElement> childList = layout.Query<VisualElement>().ToList();
                // We use instantiate, so remove the TemplateContainer's ID.
                childList.RemoveAt(0);
                var selectableContainerIDs = new List<string>();
                // Get the names of every VisualElement that has one, so they can be targeted
                foreach (var element in childList)
                {
                    if (element.name != null)
                    {
                        selectableContainerIDs.Add(element.name);
                    }
                }
                DependencyContainerIDs = selectableContainerIDs.ToArray();
            }
        }
    }
}