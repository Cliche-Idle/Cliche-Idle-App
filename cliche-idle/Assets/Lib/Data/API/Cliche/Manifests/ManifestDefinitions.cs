using System;
using System.Collections.Generic;
using UnityEngine;
using Cliche.GameModes;

namespace Cliche.System
{
    public partial class Manifests
    {
        public static Dictionary<Type, string> Paths = new Dictionary<Type, string>()
        {
            { typeof(SingleValueModifier), "Manifests/Singles" },
            { typeof(RangeValueModifier), "Manifests/Ranges" },
            { typeof(IntervalValueModifier), "Manifests/Intervals" },
            { typeof(Adventure), "Activities/Adventures" },
            { typeof(LootTable), "Activities/Rewards" },
            { typeof(ArmourItem), "Armour" },
            { typeof(ConsumableItem), "Consumables" },
            { typeof(WeaponItem), "Weapons" },
            { typeof(Race), "Player/Races" },
        };
    }
}