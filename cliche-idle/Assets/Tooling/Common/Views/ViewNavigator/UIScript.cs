using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A MonoBehaviour that is designed to control ViewNavigator views.
/// </summary>
public abstract class UIScript : MonoBehaviour 
{
    [field: Header("UI Navigator setup")]
    [field: SerializeField]
    public string ViewID { get; protected set; }

    /// <summary>
    /// The View this script controls.
    /// </summary>
    public ViewEntry View { 
        get
        {
            return Navigator.GetView(ViewID);
        }
    }

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
        return Navigator.GetTargetContainer(View.containerID);
    }

    private void Awake() {
        View.OnEnterFocus += OnEnterFocus;
        View.OnLeaveFocus += OnLeaveFocus;
    }

    /// <summary>
    /// Runs when the view enters focus (Created).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public abstract void OnEnterFocus(object sender, EventArgs e);

    /// <summary>
    /// Runs when the view leaves focus (Destroyed).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public abstract void OnLeaveFocus(object sender, EventArgs e);
}