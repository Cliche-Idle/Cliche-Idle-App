using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cliche.System;
using UnityEditor;

public partial class AdventureHandler : MonoBehaviour
{
    /// <summary>
    /// The number of adventures that are available at the same time.
    /// </summary>
    [field: SerializeField]
    public int MaxHandlerAdventures { get; private set; }

    /// <summary>
    /// Contains the list of available activities in the user's feed.
    /// </summary>
    public List<string> AvailableAdventures;


    /// <summary>
    /// The number of adventures that can be active at the same time.
    /// </summary>
    [field: SerializeField]
    public int MaxQueuedAdventures { get; private set; }

    /// <summary>
    /// Contains the list of currenctly active activities.
    /// </summary>
    public List<AdventureQueueItem> AdventureQueue;

    /// <summary>
    /// Transfers an adventure from the AvailableAdventures to the AdventureQueue. 
    /// </summary>
    /// <param name="adventureID"></param>
    /// <param name="adventureEndTime"></param>
    public void TransferAdventureToActiveQueue(string adventureID, double adventureEndTime)
    {
        int adventureQueueCount = AdventureQueue.Count;
        if (adventureQueueCount + 1 <= MaxQueuedAdventures)
        {
            AvailableAdventures.Remove(adventureID);
            AdventureQueue.Add(new AdventureQueueItem(adventureID, adventureEndTime));
        }
        else
        {
            Debug.LogWarning($"Adventure ({adventureID}) could not be transferred to queue because it is full.");
        }
    }

    /// <summary>
    /// Refills the available adventures list (takes the adventure queue into account too when calculating size). If there aren't enough adventures available to the player at the time, it may not completely refill it.
    /// </summary>
    public void RefillAvailableList()
    {
        AdventureManifest[] allAdventures = Resources.LoadAll<AdventureManifest>(Manifests.Paths[typeof(AdventureManifest)]);
        List<string> userAvailableAdventures = new List<string>();
        foreach (var item in allAdventures)
        {
            if (item.Requirements == null || item.Requirements.IsFulfilled())
            {
                userAvailableAdventures.Add(item.ID);
            }
        }
        // TODO: refactor so the fill amount is explicitly calculated before
        while (true)
        {
            // Stop when refilled the Available list / there are not more adventures left
            // Takes into account that the Queue items are part of the max limit
            if (((AvailableAdventures.Count - AdventureQueue.Count) == (MaxHandlerAdventures - AdventureQueue.Count)) || (userAvailableAdventures.Count == 0))
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
        //  Debug.LogWarning($"Could not refill available adventures list; it is already full or there aren't any available adventures for this character.");
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
