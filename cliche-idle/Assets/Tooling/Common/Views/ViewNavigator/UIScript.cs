using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UIViews;


/// <summary>
/// A MonoBehaviour that is designed to control ViewNavigator views.
/// </summary>
public class UIScript : MonoBehaviour
{
    /// <summary>
    /// The ViewNavigator instance this script is hooked up to.
    /// </summary>
    [field: Header("UI Navigator setup")]
    [field: Tooltip("The ViewNavigator instance this script is hooked up to.")]
    [field: SerializeField]
    public ViewNavigator Navigator { get; private set; }



    /// <summary>
    /// The ID of the View this script controls.
    /// </summary>
    [field: Header("View setup")]
    [field: Tooltip("")]
    [field: SerializeField]
    public string ID { get; private set; }

    public string WrapperVisualElementName {
        get {
            return $"UIView__{ID}";
        }
    }

    /// <summary>
    /// The UXML Document file containing the view to be switched in.
    /// </summary>
    [field: Tooltip("")]
    [field: SerializeField]
    public VisualTreeAsset UXMLDocument { get; private set; }

    /// <summary>
    /// Sets whether or not the view is static. If set to <see langword="true"/>, <see cref="UIUpdate()"/> will never run. Default is <see langword="false"/>.
    /// </summary>
    [field: Tooltip("")]
    [field: SerializeField]
    public bool IsStatic { get; private set; } = false;

    /// <summary>
    /// The current state of the view.
    /// </summary>
    [field: Tooltip("")]
    [field: SerializeField]
    public bool IsViewActive { get; private set; }



    /// <summary>
    /// The dependency of this view, which must be present before this can be loaded. If none is specified, the view will be spawned in the Navigator target's root.
    /// </summary>
    [field: Header("Dependency setup")]
    [field: Tooltip("")]
    [field: SerializeField]
    public UIScript Dependency { get; private set; }

    /// <summary>
    /// The ID of the view container in the target document. 
    /// The VisualElement with this ID will be cleared, and the contents of this view will be copied to its tree.
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("")]
    public string ContainerID { get; private set; }

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
    /// Shorthand convenience call to display the attached View.
    /// See <see cref="ViewNavigator.SwitchToView(string)"/> for details.
    /// </summary>
    public void DisplayView()
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
                        WalkDependencyStack();

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
                                OnEnterFocus(null, null);
                                IsViewActive = true;
                            });

                            // OnLeaveFocus
                            viewWrapperContainer.RegisterCallback<DetachFromPanelEvent>(evt => {
                                OnLeaveFocus(null, null);
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
    /// Shorthand convenience call to clear the attached View. 
    /// See <see cref="ViewNavigator.ClearUpViewContainer(string)"/> for details.
    /// </summary>
    public void HideView()
    {
        Navigator.ClearUpViewContainer(ContainerID);
    }

    /// <summary>
    /// Compiles the dependency stack of a view into a list. Items are sorted in reverse order, where the lowest non active dependency is first.
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    public List<UIScript> GetDependencyList()
    {
        List<UIScript> dependencies = new List<UIScript>();

        UIScript dependency = Dependency;
        // TODO: does active checking make sense here? This looks like a generic function that would probably be expected to return the full dependency stack
        while ((dependency != null && dependency.IsViewActive == false))
        {
            if (dependency != null)
            {
                dependencies.Add(dependency);
            }
            dependency = dependency.Dependency;
        }
        // TODO: don't reverse the list
        // Reverse the list so the lowest inactive dependencies are at the start
        dependencies.Reverse();

        return dependencies;
    }

    // TODO: rename this function to make sense
    private void WalkDependencyStack()
    {
        var deps = GetDependencyList();
        deps.Reverse();
        if (deps.Count > 0)
        {
            foreach(UIScript dependency in deps)
            {
                if (dependency.IsViewActive == false)
                {
                    dependency.DisplayView();
                }
            }
        }
    }

    private void Awake() {
        Navigator.RegisterView(this);
    }

    private void Update() {
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
    protected virtual void OnEnterFocus(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Runs when the view leaves focus (Destroyed).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnLeaveFocus(object sender, EventArgs e)
    {
        
    }
}


[CustomEditor(typeof(UIScript), true)]
public class UIScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var uiscript = target as UIScript;
        string infoMessageText = "";
        if (uiscript.UXMLDocument == null)
        {
            // TODO: check if this is needed
            infoMessageText += "Because no UXML document has been added, this view will not trigger attach / detach events. ";
        }
        if (uiscript.Dependency == null)
        {
            if (uiscript.ContainerID == null)
            {
                infoMessageText += "Because no dependency or container have been specified, this view will be attached directly to the root visual element. ";
            }
            else
            {
                infoMessageText += "Because no dependency has been specified, this view will be attached as a child to the specified container on the root visual element. ";
            }
        }
        if (infoMessageText.Length != 0)
        {
            EditorGUILayout.HelpBox(infoMessageText, MessageType.Info);
        }
    }
}