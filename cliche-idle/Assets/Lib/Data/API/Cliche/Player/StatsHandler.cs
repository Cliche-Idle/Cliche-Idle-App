using System.Linq;
using UnityEngine;
using Cliche.System;

public class StatsHandler : MonoBehaviour
{
    [HeaderAttribute("General stats")]
    public AdjustableProperty Health;

    public int MaxHealth
    {
        get {
            int level = gameObject.GetComponent<ProgressionHandler>().Level;
            int levelBonus = Manifests.GetByID<IntervalValueModifier>("HealthBonusPerLevel").GetAmountFloored(level);
            int vitalityBonus = Manifests.GetByID<IntervalValueModifier>("Vitality").GetAmountFloored(Vitality.Value);
            return (levelBonus + vitalityBonus);
        }
    }

    [field: Header("Item stats")]
    public int Defence {
        get
        {
            return (int)gameObject.GetComponent<InventoryHandler>().Armour.Sockets.Average(socket => socket.EquippedItem.MainStatValue); 
        }
    }

    public int Attack {
        get
        {
            return (int)gameObject.GetComponent<InventoryHandler>().Weapons.Sockets.Average(socket => socket.EquippedItem.MainStatValue);
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
        Race raceData = Resources.Load<Race>($"Player/Races/{raceID}");
        int AllocatedStatPoints = ((Intelligence.Value - raceData.Intelligence) + (Dexterity.Value - raceData.Dexterity) + (Strength.Value - raceData.Strength) + (Vitality.Value - raceData.Vitality));
        return (CurrentLevelPoints - AllocatedStatPoints);
    }

    public int GetRacialBaseStatAmount(string statName)
    {
        string raceID = gameObject.GetComponent<CharacterHandler>().Race.ToString();
        Race raceData = Resources.Load<Race>($"Player/Races/{raceID}");
        int stat = (int)raceData.GetType().GetProperty(statName).GetValue(raceData);
        Debug.Log(stat);
        return stat;
    }
}

public abstract class CoreStat
{
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
