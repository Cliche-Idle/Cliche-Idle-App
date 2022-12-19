using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cliche.System;
using UnityEditor;

public partial class AdventureHandler : MonoBehaviour
{
    /// <summary>
    /// The maximum total number of adventures that can be present at a time in <see cref="AvailableAdventures"/> and <see cref="AdventureQueue"/> combined.
    /// </summary>
    [field: SerializeField]
    public int MaxHandlerAdventures { get; private set; } = 3;

    /// <summary>
    /// Contains the list of available activities in the user's feed.
    /// </summary>
    public List<string> AvailableAdventures;

    /// <summary>
    /// Event that fires when either Adventure lists receive a change.
    /// </summary>
    public EventHandler OnAdventuresUpdate;

    /// <summary>
    /// The maximum number of advantures that can be active at a time. Effectively the maximum count for <see cref="AdventureQueue"/>.
    /// </summary>
    [field: SerializeField]
    public int MaxQueuedAdventures { get; private set; } = 1;

    /// <summary>
    /// Contains the list of currenctly active activities.
    /// </summary>
    public List<AdventureQueueItem> AdventureQueue;

    /// <summary>
    /// Transfers an adventure from <see cref="AvailableAdventures"/> to the <see cref="AdventureQueue"/>. 
    /// </summary>
    /// <param name="adventureID"></param>
    /// <param name="adventureEndTime"></param>
    private void TransferAdventureToActiveQueue(string adventureID, double adventureEndTime)
    {
        int adventureQueueCount = AdventureQueue.Count;
        if (adventureQueueCount + 1 <= MaxQueuedAdventures)
        {
            AvailableAdventures.Remove(adventureID);
            AdventureQueue.Add(new AdventureQueueItem(adventureID, adventureEndTime));
            FireUpdateEvent();
        }
        else
        {
            throw new Exception($"Adventure ({adventureID}) could not be transferred to queue because it is full.");
        }
    }

    /// <summary>
    /// Refills <see cref="AvailableAdventures"/>. If there aren't enough adventures available to the player at the time, or the number of adventures would exceed <see cref="MaxHandlerAdventures"/>, it may not completely refill the list.
    /// </summary>
    public void RefillAvailableList()
    {
        AdventureManifest[] allAdventures = Resources.LoadAll<AdventureManifest>(Manifests.ManifestPaths[typeof(AdventureManifest)]);
        List<string> userAvailableAdventures = new List<string>();
        foreach (var item in allAdventures)
        {
            if (item.Requirements == null || item.Requirements.IsFulfilled())
            {
                userAvailableAdventures.Add(item.ID);
            }
        }
        // TODO: refactor so the full amount is explicitly calculated before
        while (true)
        {
            // Stop when refilled the Available list / there are not more adventures left
            // Takes into account that the Queue items are part of the max limit
            if (((AvailableAdventures.Count + AdventureQueue.Count) == (MaxHandlerAdventures)) || (userAvailableAdventures.Count == 0))
            {
                break;
            }
            // Grab random adventure
            int adventureIndex = UnityEngine.Random.Range(0, userAvailableAdventures.Count-1);
            string adventureID = userAvailableAdventures[adventureIndex];
            // Add adventure to Available list if it's not there already
            if (AvailableAdventures.Contains(adventureID) == false)
            {
                AvailableAdventures.Add(adventureID);
            }
            // Remove adventure from list (regardless whether or not it was added)
            userAvailableAdventures.RemoveAt(adventureIndex);
        }
        FireUpdateEvent();
        //  Debug.LogWarning($"Could not refill available adventures list; it is already full or there aren't any available adventures for this character.");
    }

    private void FireUpdateEvent()
    {
        if (OnAdventuresUpdate != null)
        {
            OnAdventuresUpdate.Invoke(this, null);
        }
    }

    private void Start()
    {
        // Attempts refill on startup
        RefillAvailableList();
    }

    /// <summary>
    /// Runs the update loop that checks if adventures are completed.
    /// </summary>
    private void FixedUpdate() {
        if (AdventureQueue.Count != 0)
        {
            foreach (var item in AdventureQueue)
            {
                if (item.Finished == false)
                {
                    item.CheckIfFinished();
                }
            }
        }
    }
}


[CustomEditor(typeof(AdventureHandler))]
public class AdventureHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var adventureHandler = target as AdventureHandler;
        EditorGUILayout.LabelField("DEBUG", EditorStyles.boldLabel);
        //
        GUILayout.Space(10);
        if (GUILayout.Button("Trigger finish adventure"))
        {
            var adventureID = adventureHandler.AdventureQueue[0].ID;
            adventureHandler.FinishAdventure(adventureID);
            Debug.Log("<color=yellow>Manual adventure finish triggered.</color>");
        }
    }
}
