using System;
using UnityEngine;

[System.Serializable]
public class AdventureQueueItem
{
    public AdventureQueueItem(string adventureID, double adventureEndTime)
    {
        ID = adventureID;
        EndTime = adventureEndTime;
        Finished = false;
    }

    /// <summary>
    /// The ID of the activity.
    /// </summary>
    [field: SerializeField]
    public string ID { get; private set; }

    /// <summary>
    /// The time (in UNIX format) when the adventure completes.
    /// </summary>
    [field: SerializeField]
    public double EndTime { get; private set; }

    /// <summary>
    /// Shorthand for checking if the adventure is finished.
    /// </summary>
    [field: SerializeField]
    public bool Finished { get; private set; }

    /// <summary>
    /// Checks and sets if the adventure is finished.
    /// </summary>
    /// <returns></returns>
    public bool CheckIfFinished()
    {
        var baseTime = DateTime.UnixEpoch;
        baseTime = baseTime.AddSeconds(EndTime);
        if (Finished == false)
        {
            if (baseTime < DateTime.UtcNow)
            {
                Finished = true;
            }
        }
        return Finished;
    }
}
