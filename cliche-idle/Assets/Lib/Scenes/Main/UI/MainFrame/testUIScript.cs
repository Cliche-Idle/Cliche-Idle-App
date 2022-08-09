using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class testUIScript : UIScript
{
    // Start is called before the first frame update
    void Start()
    {
        Navigator.SwitchToView(ViewID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEnterFocus(object sender, EventArgs e)
    {
        Debug.Log($"View {ViewID} entered focus.");
        GetViewContainer().Q<Button>().clicked += BtnClick;
    }

    public override void OnLeaveFocus(object sender, EventArgs e)
    {
        Debug.Log($"View {ViewID} left focus.");
    }

    private void BtnClick()
    {
        Debug.Log($"View {ViewID} button clicked.");
        Navigator.ClearViewContainer(ViewID);
    }
}

[CustomEditor(typeof(testUIScript))]
public class testUIScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var uihandler = target as testUIScript;
        EditorGUILayout.LabelField("DEBUG", EditorStyles.boldLabel);
        //
        GUILayout.Space(10);
        if (GUILayout.Button("Trigger view switch"))
        {
            uihandler.Navigator.SwitchToView(uihandler.ViewID);
        }
    }
}
