using System;
using UnityEngine;
using Cliche.System;

[Serializable]
public class CoreStat : AdjustableIntProperty
{
    public string StatName { get; protected set; }

    public CoreStat(string statID)
    {
        StatName = statID;
    }

    public int MinimumValue
    {
        get
        {
            int racialStatPoints = GameObject.Find("Player").GetComponent<StatsHandler>().GetRacialBaseStatAmount(StatName);
            return racialStatPoints;
        }
    }

    public override void Grant(int amount)
    {
        int freeStatPoints = GameObject.Find("Player").GetComponent<StatsHandler>().GetFreeStatPoints();
        if (freeStatPoints >= Mathf.Abs(amount))
        {
            Value += Mathf.Abs(amount);
        }
    }

    public override void Take(int amount)
    {
        //string raceID = GameObject.Find("Player").GetComponent<CharacterHandler>().Race.ToString();
        //Race raceData = Manifests.GetByID<Race>(raceID);
        int racialStatPoints = GameObject.Find("Player").GetComponent<StatsHandler>().GetRacialBaseStatAmount(StatName);
        if ((Value - racialStatPoints) >= Mathf.Abs(amount))
        {
            Value -= Mathf.Abs(amount);
        }
        else
        {
            Debug.LogError($"Can not TAKE stat point from category {StatName}. It would be either below 0 or the racial minimum.");
        }
    }
}
