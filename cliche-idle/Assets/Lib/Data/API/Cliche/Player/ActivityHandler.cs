using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityHandler : MonoBehaviour
{
    /// <summary>
    /// Contains the list of available activities in the user's feed.
    /// </summary>
    public List<string> AvailableAdventures;

    /// <summary>
    /// Contains the list of currencty active activities.
    /// </summary>
    public List<ActivityQueueItem> AdventureQueue;
}

[System.Serializable]
public class ActivityQueueItem
{
    public ActivityQueueItem(string activityID, int adventureLength)
    {
        ID = activityID;
        Length = adventureLength;
    }

    public ActivityQueueItem() {
    
    }

    /// <summary>
    /// The ID of the activity.
    /// </summary>
    [field: SerializeField]
    public string ID { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [field: SerializeField]
    public int Length { get; private set; }
}
