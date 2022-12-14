using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cliche.System;

public class StatsHandler : MonoBehaviour
{
    [field: Header("Player health")]
    [field: SerializeField]
    public PlayerHealth Health { get; private set; } = new PlayerHealth();

    [field: Header("Core stats")]
    [field: SerializeField]
    public CoreStat Intelligence { get; private set; }

    [field: SerializeField]
    public CoreStat Dexterity { get; private set; }

    [field: SerializeField]
    public CoreStat Strength { get; private set; }

    [field: SerializeField]
    public CoreStat Vitality { get; private set; }

    private void Awake()
    {
        Intelligence = new CoreStat("Intelligence");
        Dexterity = new CoreStat("Dexterity");
        Strength = new CoreStat("Strength");
        Vitality = new CoreStat("Vitality");
    }

    /// <summary>
    /// The player's overall defence, based on the currently equipped armour sets.
    /// </summary>
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

    /// <summary>
    /// The player's overall defence, based on the currently equipped armour sets.
    /// </summary>
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

    /// <summary>
    /// Gets the number of currently unallocated stat points.
    /// </summary>
    /// <returns></returns>
    public int GetFreeStatPoints()
    {
        int CurrentLevelPoints = Manifests.GetByID<IntervalValueModifier>("StatPointsPerLevel").GetAmountFloored(gameObject.GetComponent<ProgressionHandler>().Level);
        string raceID = gameObject.GetComponent<CharacterHandler>().CharacterSheet.Race.ToString();
        Race raceData = Manifests.GetByID<Race>(raceID);
        int AllocatedStatPoints = ((Intelligence.Value - raceData.Intelligence) + (Dexterity.Value - raceData.Dexterity) + (Strength.Value - raceData.Strength) + (Vitality.Value - raceData.Vitality));
        return (CurrentLevelPoints - AllocatedStatPoints);
    }

    /// <summary>
    /// Gets the base amount of base stat points in a given category based on the player's race.
    /// </summary>
    /// <param name="statName"></param>
    /// <returns></returns>
    public int GetRacialBaseStatAmount(string statName)
    {
        string raceID = gameObject.GetComponent<CharacterHandler>().CharacterSheet.Race.ToString();
        Race raceData = Manifests.GetByID<Race>(raceID);
        int stat = (int)raceData.GetType().GetProperty(statName).GetValue(raceData);
        return stat;
    }
}
