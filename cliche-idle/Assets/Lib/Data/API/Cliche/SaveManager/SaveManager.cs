using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class SaveManager : MonoBehaviour
{
    [Header("Settings")]
    
    public readonly string SaveSubPath = "Saves";
    public readonly string DEBUGSaveSubPath = "DEBUG_Saves";
    public List<Component> SaveObjects;

    /// <summary>
    /// Controls wether or not auto save is enabled. This setting is only applied on startup.
    /// </summary>
    [field: SerializeField]
    public bool AutoSaveEnabled { get; private set; } = false;


    [field: SerializeField]
    public double LastSaveTime { get; private set; }

    /// <summary>
    /// The auto save inerval in seconds.
    /// </summary>
    [field: SerializeField]
    public float SaveInterval { get; private set; } = 120;

    private void Start()
    {
        // Start auto save loop
        if (AutoSaveEnabled)
        {
            StartCoroutine(SaveGameLoop());
        }
    }

    IEnumerator SaveGameLoop()
    {
        while (true)
        {
            SaveUserState();
            yield return new WaitForSecondsRealtime(SaveInterval);
        }
    }
    

    public void SaveUserState()
    {
        if (Directory.Exists($"{Application.persistentDataPath}/{SaveSubPath}") == false)
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/{SaveSubPath}");
        }

        JObject save = new JObject();
        save.Add("BundleVersion", $"{PlayerSettings.bundleVersion}");
        save.Add("SaveDate_UNIX", $"{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}");
        save.Add("SaveDate_WINSTRING", $"{DateTime.UtcNow}");

        foreach (var item in SaveObjects)
        {
            // Funky double parsing but JSON.NET doesn't seem to like Unity (especially Colours)
            save.Add(GetComponentKey(item), JToken.Parse(JsonUtility.ToJson(item)));
        }

        string saveJSON = save.ToString();

        if (Application.isEditor)
        {
            // Indent the JSON file for better readability when running in the editor
            saveJSON = save.ToString(Formatting.Indented);
        }

        File.WriteAllText($"{Application.persistentDataPath}/{SaveSubPath}/gamesave.json", saveJSON);
        
        Debug.Log($"<color=green>Player data saved to save-files.</color>");
    }

    public void LoadUserState()
    {
        if (Directory.Exists($"{Application.persistentDataPath}/{SaveSubPath}") == false)
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/{SaveSubPath}");
        }

        JObject save = JObject.Parse(File.ReadAllText($"{Application.persistentDataPath}/{SaveSubPath}/gamesave.json"));

        foreach (var item in save)
        {
            var loadObject = SaveObjects.Find(element => GetComponentKey(element) == item.Key);
            if (loadObject != null)
            {
                JsonUtility.FromJsonOverwrite(item.Value.ToString(), loadObject);
            }
        }

        Debug.Log($"<color=green>Player data loaded from save-files.</color>\nVersion: {save["BundleVersion"]}\nSaved on: {save["SaveDate_WINSTRING"]}");
    }

    private string GetComponentKey (Component comp)
    {
        string key = comp.GetType().ToString(); //comp.GetInstanceID().ToString();
        return key;
    }
}


[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var saveManager = target as SaveManager;
        EditorGUILayout.LabelField("DEBUG", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Save location: ", $"{Application.persistentDataPath}/{saveManager.SaveSubPath}/");
        //
        if (GUILayout.Button("Open save location"))
        {
            EditorUtility.RevealInFinder($"{ Path.Join(Application.persistentDataPath, saveManager.SaveSubPath) }");
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Trigger manual save"))
        {
            saveManager.SaveUserState();
            Debug.Log("<color=yellow>Manual player data save triggered.</color>");
        }
        GUILayout.Space(8);
        if (GUILayout.Button("Trigger manual load"))
        {
            saveManager.LoadUserState();
            Debug.Log("<color=yellow>Manual player data load triggered.</color>");
        }
    }
}