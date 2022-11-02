using System;
using UnityEngine;
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
    [field: SerializeField]
    public ViewNavigator Navigator { get; private set; }



    /// <summary>
    /// The ID of the View this script controls.
    /// </summary>
    [field: Header("View setup")]
    [field: SerializeField]
    public string ID { get; private set; }

    /// <summary>
    /// The UXML Document file containing the view to be switched in.
    /// </summary>
    [field: SerializeField]
    public VisualTreeAsset UXMLDocument { get; private set; }

    /// <summary>
    /// Sets whether or not the view is static. If set to <see langword="true"/>, <see cref="UIUpdate()"/> will never run. Default is <see langword="false"/>.
    /// </summary>
    [field: SerializeField]
    public bool IsStatic { get; private set; } = false;

    /// <summary>
    /// The current state of the view.
    /// </summary>
    [field: SerializeField]
    public bool IsViewActive { get; private set; }


    /// <summary>
    /// The dependency of this view, which must be present before this can be loaded. If none is specified, the view will be spawned in the Navigator target's root.
    /// </summary>
    [field: Header("Dependency setup")]
    [field: SerializeField]
    public UIScript Dependency { get; private set; }

    /// <summary>
    /// The ID of the view container in the target document. 
    /// The VisualElement with this ID will be cleared, and the contents of this view will be copied to its tree.
    /// </summary>
    [field: SerializeField]
    public string ContainerID { get; private set; }

    /// <summary>
    /// Gets the dependency container ID this script is aimed at.
    /// </summary>
    /// <returns></returns>
    public VisualElement GetViewContainer()
    {
        // Grab visual element via Navigator
        // Todo: Check if grabbing the dependency container or this view's UXMLDocument root would be better
        return Navigator.GetTargetContainer(ContainerID);
    }

    /// <summary>
    /// Shorthand convenience call to display the attached View.
    /// See <see cref="ViewNavigator.SwitchToView(string)"/> for details.
    /// </summary>
    public void DisplayView()
    {
        if (ID.Length != 0)
        {
            // FIXME: the first event doesn't trigger because SwitchToView already attaches the view
            Navigator.SwitchToView(ID);
            IsViewActive = true;

            VisualElement container = GetViewContainer();
            if (container != null)
            {
                
                // The normal event syntax is more useful here than the Unity one, so lambda it 
                // TODO: Make the sender the lowest exiting UIScript that is detaching 

                // OnEnterFocus
                /*container.RegisterCallback<AttachToPanelEvent>(evt => {
                    OnEnterFocus(null, null);
                });*/

                // OnLeaveFocus
                container.RegisterCallback<DetachFromPanelEvent>(evt => {
                    OnLeaveFocus(null, null);
                    IsViewActive = false;
                });

                OnEnterFocus(null, null);
            }
        }
    }

    /// <summary>
    /// Shorthand convenience call to clear the attached View. 
    /// See <see cref="ViewNavigator.ClearUpViewContainer(string)"/> for details.
    /// </summary>
    public void ClearView()
    {
        Navigator.ClearUpViewContainer(ContainerID);
    }

    private void Awake() {
        //View = Navigator.GetView(ID);
        Navigator.RegisterView(this);
        DisplayView();
    }

    private void FixedUpdate() {
        if (IsStatic == false)
        {
            //todo: Don't run update if the UI element is static
            if (IsViewActive)
            {
                UIUpdate();
            }
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled, and the attached View is in focus.
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