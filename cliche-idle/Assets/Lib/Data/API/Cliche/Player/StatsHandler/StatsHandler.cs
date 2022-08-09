using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cliche.System;

public class StatsHandler : MonoBehaviour
{
    [HeaderAttribute("General stats")]
    // FIXME: max health is the upper limit, enforce it
    public PlayerHealth Health;

    //[field: Header("Item stats")]
    public int Defence {
        get
        {
            List<GearSocket<Armour>> sockets = gameObject.GetComponent<InventoryHandler>().Armour.Sockets.FindAll(socket => socket.EquippedItem != null);
            int defence = Mathf.FloorToInt((float)sockets.Average(socket => socket.EquippedItem.MainStatValue));
            return defence;
        }
    }

    public int Attack {
        get
        {
            List<GearSocket<Weapon>> sockets = gameObject.GetComponent<InventoryHandler>().Weapons.Sockets.FindAll(socket => socket.EquippedItem != null);
            int attack = Mathf.FloorToInt((float)sockets.Average(socket => socket.EquippedItem.MainStatValue));
            return attack;
        }
    }

    [field: HeaderAttribute("Core stats")]
    public Intelligence Intelligence;

    public Dexterity Dexterity;

    public Strength Strength;

    public Vitality Vitality;

    public int GetFreeStatPoints()
    {
        int CurrentLevelPoints = Manifests.GetByID<IntervalValueModifier>("StatPointsPerLevel").GetAmountFloored(gameObject.GetComponent<ProgressionHandler>().Level);
        string raceID = gameObject.GetComponent<CharacterHandler>().Race.ToString();
        Race raceData = Manifests.GetByID<Race>(raceID);
        int AllocatedStatPoints = ((Intelligence.Value - raceData.Intelligence) + (Dexterity.Value - raceData.Dexterity) + (Strength.Value - raceData.Strength) + (Vitality.Value - raceData.Vitality));
        return (CurrentLevelPoints - AllocatedStatPoints);
    }

    public int GetRacialBaseStatAmount(string statName)
    {
        string raceID = gameObject.GetComponent<CharacterHandler>().Race.ToString();
        Race raceData = Manifests.GetByID<Race>(raceID);
        int stat = (int)raceData.GetType().GetProperty(statName).GetValue(raceData);
        Debug.Log(stat);
        return stat;
    }
}

public abstract class CoreStat
{
    // FIXME: add intrinsic manifest access

    [field: SerializeField]
    public int Value { get; private set; }

    public int MinimumValue {
        get
        {
            string StatName = GetType().Name;
            int racialStatPoints = GameObject.Find("Player").GetComponent<StatsHandler>().GetRacialBaseStatAmount(StatName);
            return racialStatPoints;
        }
    }

    public void Grant (int amount)
    {
        int freeStatPoints = GameObject.Find("Player").GetComponent<StatsHandler>().GetFreeStatPoints();
        if (freeStatPoints >= Mathf.Abs(amount))
        {
            Value += Mathf.Abs(amount);
        }
    }

    public void Take (int amount)
    {
        string raceID = GameObject.Find("Player").GetComponent<CharacterHandler>().Race.ToString();
        Race raceData = Manifests.GetByID<Race>(raceID);
        string StatName = GetType().Name;
        int racialStatPoints = GameObject.Find("Player").GetComponent<StatsHandler>().GetRacialBaseStatAmount(StatName);
        if ((Value - racialStatPoints) >= Mathf.Abs(amount))
        {
            Value -= Mathf.Abs(amount);
        }
        else
        {
            Debug.LogError($"Can not TAKE stat point from category {StatName}. It would be either 0 or below the racial default.");
        }
    } 
}

[System.Serializable]
public class Intelligence : CoreStat { }

[System.Serializable]
public class Dexterity : CoreStat { }

[System.Serializable]
public class Strength : CoreStat { }

[System.Serializable]
public class Vitality : CoreStat { }
