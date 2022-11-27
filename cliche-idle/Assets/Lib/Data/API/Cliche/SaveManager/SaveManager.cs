using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnityEngine.Serialization;


public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// The foldername for production builds.
    /// </summary>
    private readonly string _saveSubPath = "Saves";

    /// <summary>
    /// The foldername for Debug and Dev builds (when ran from the editor).
    /// </summary>
    private readonly string _debugSaveSubPath = "DEBUG_Saves";

    /// <summary>
    /// The filepath where saves are stored. Changes depending on platform, or if the game is running in a build or in the editor.
    /// </summary>
    public string SavePath { get; private set; }

    /// <summary>
    /// Controls wether or not auto save is enabled. This setting is only applied on startup.
    /// </summary>
    [field: SerializeField]
    [field: Header("Settings")]
    public bool AutoSaveEnabled { get; private set; } = false;

    /// <summary>
    /// The auto save interval in seconds.
    /// </summary>
    [field: SerializeField]
    public float AutoSaveInterval { get; private set; } = 120;

    /// <summary>
    /// Controls wether or not the save file is loaded on startup. Defaults to true.
    /// </summary>
    [field: SerializeField]
    public bool AutoLoadEnabled { get; private set; } = true;

    /// <summary>
    /// The list of components that's state will be saved.
    /// </summary>
    [field: SerializeField]
    [field: FormerlySerializedAs("SaveObjects")]
    private List<Component> SaveObjects;

    private void Start()
    {
        // Set save path based on editor
        if (Application.isEditor)
        {
            SavePath = $"{Application.persistentDataPath}/{_debugSaveSubPath}";
        }
        else
        {
            SavePath = $"{Application.persistentDataPath}/{_saveSubPath}";
        }
        // Load save
        if (AutoLoadEnabled)
        {
            LoadUserState();
        }
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
            yield return new WaitForSecondsRealtime(AutoSaveInterval);
        }
    }
    
    /// <summary>
    /// Overwrites the last savefile with the current player data.
    /// </summary>
    public void SaveUserState()
    {
        if (Directory.Exists(SavePath) == false)
        {
            Directory.CreateDirectory(SavePath);
        }

        // Create backup of the save before overwriting
        if (File.Exists($"{SavePath}/gamesave.json"))
        {
            File.Copy($"{SavePath}/gamesave.json", $"{SavePath}/gamesave.bak", true);
        }

        JObject save = new JObject();
        // Add custom metadata
        save.Add("BundleVersion", $"{PlayerSettings.bundleVersion}");
        save.Add("SaveDate_UNIX", $"{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}");
        save.Add("SaveDate_WINSTRING", $"{DateTime.UtcNow}");

        foreach (var item in SaveObjects)
        {
            // Funky double parsing but JSON.NET doesn't seem to like Unity (especially Colours)
            save.Add(GetComponentKey(item), JToken.Parse(JsonUtility.ToJson(item)));
        }

        string saveJSON;
        if (Application.isEditor)
        {
            // Indent the JSON file for better readability when running in the editor
            saveJSON = save.ToString(Formatting.Indented);
        }
        else
        {
            saveJSON = save.ToString();
        }

        File.WriteAllText($"{SavePath}/gamesave.json", saveJSON);
        
        Debug.Log($"<color=green>Player data saved to save-files.</color>");
    }

    /// <summary>
    /// Loads and overwrites the current player data with the last saved information.
    /// </summary>
    public void LoadUserState()
    {
        if (Directory.Exists(SavePath) == false)
        {
            Directory.CreateDirectory(SavePath);
        }

        try
        {
            LoadSaveFileContents($"{SavePath}/gamesave.json");
        }
        catch (Exception ex)
        {
            Debug.LogError("Main save file could not be loaded:");
            Debug.LogError(ex.Message);
            Debug.LogError("Attempting to load backup save now.");
            try
            {
                LoadSaveFileContents($"{SavePath}/gamesave.bak");
                Debug.LogWarning("Backup save has been successfully loaded.");
            }
            catch (Exception ex2)
            {
                Debug.LogError("Backup save file could not be loaded:");
                Debug.LogError(ex2.Message);
                Debug.LogError("User data can not be loaded.");
                throw ex2;
            }   
        }
    }

    private void LoadSaveFileContents(string saveFilePath)
    {
        if (File.Exists(saveFilePath))
        {
            JObject save = JObject.Parse(File.ReadAllText(saveFilePath));

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
    }

    /// <summary>
    /// Gets the identifier (its Typename as a string) of the given component.
    /// </summary>
    /// <param name="comp"></param>
    /// <returns></returns>
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
        EditorGUILayout.LabelField("Save location: ", $"{Application.persistentDataPath}/");
        //
        if (GUILayout.Button("Open save location"))
        {
            EditorUtility.RevealInFinder($"{Application.persistentDataPath}/");
        }
        if (Application.isPlaying)
        {
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
}