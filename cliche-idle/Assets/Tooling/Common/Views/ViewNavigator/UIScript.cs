using System;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;

/// <summary>
/// A MonoBehaviour that is designed to control ViewNavigator views.
/// </summary>
public abstract class UIScript : MonoBehaviour
{
    /// <summary>
    /// The ID of the View this script controls.
    /// </summary>
    [field: Header("UI Navigator setup")]
    [field: SerializeField]
    public string ViewID { get; private set; }

    /// <summary>
    /// The View this script controls.
    /// </summary>
    public View View { get; private set;}

    /// <summary>
    /// The ViewNavigator instance this script is hooked up to.
    /// </summary>
    [field: SerializeField]
    public ViewNavigator Navigator { get; private set; }

    /// <summary>
    /// Gets the target container reference this script is aimed at.
    /// </summary>
    /// <returns></returns>
    public VisualElement GetViewContainer()
    {
        return Navigator.GetTargetContainer(View.ContainerID);
    }

    /// <summary>
    /// Shorthand convenience call to display the attached View.
    /// See <see cref="ViewNavigator.SwitchToView(string)"/> for details.
    /// </summary>
    public void DisplayView()
    {
        Navigator.SwitchToView(ViewID);
    }

    /// <summary>
    /// Shorthand convenience call to clear the attached View. 
    /// See <see cref="ViewNavigator.ClearUpViewContainer(string)"/> for details.
    /// </summary>
    public void ClearView()
    {
        Navigator.ClearUpViewContainer(View.ContainerID);
    }

    private void Awake() {
        View = Navigator.GetView(ViewID);
        if (View != null)
        {
            View.OnEnterFocus += OnEnterFocus;
            View.OnLeaveFocus += OnLeaveFocus;
        }
    }

    private void FixedUpdate() {
        if (View != null)
        {
            if (View.Enabled)
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