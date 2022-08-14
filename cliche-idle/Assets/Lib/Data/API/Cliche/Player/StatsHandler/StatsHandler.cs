using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cliche.System;

public class StatsHandler : MonoBehaviour
{
    private void Awake() {
        Health = gameObject.GetComponent<PlayerHealth>();
    }

    [field: HeaderAttribute("General stats")]
    // FIXME: Keep an eye out on this, an instance ID is saved into savefiles and if that changes, it might mess things up when it's loaded back.
    public PlayerHealth Health { get; private set; }

    //[field: Header("Item stats")]
    public int Defence {
        get
        {
            int defence = 0;
            List<GearSocket<Armour>> sockets = gameObject.GetComponent<InventoryHandler>().Armour.Sockets.FindAll(socket => socket.EquippedItem != null);
            if (sockets.Count != 0)
            {
                defence = Mathf.FloorToInt((float)sockets.Average(socket => socket.EquippedItem.MainStatValue));
            }
            return defence;
        }
    }

    public int Attack {
        get
        {
            int attack = 0;
            List<GearSocket<Weapon>> sockets = gameObject.GetComponent<InventoryHandler>().Weapons.Sockets.FindAll(socket => socket.EquippedItem != null);
            if (sockets.Count != 0)
            {
                attack = Mathf.FloorToInt((float)sockets.Average(socket => socket.EquippedItem.MainStatValue));
            }
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
            Debug.LogError($"Can not TAKE stat point from category {StatName}. It would be either below 0 or the racial minimum.");
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
