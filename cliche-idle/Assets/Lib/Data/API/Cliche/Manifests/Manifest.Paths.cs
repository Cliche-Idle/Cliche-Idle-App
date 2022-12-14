using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cliche.System
{
    public partial class Manifests
    {
        public static readonly Dictionary<Type, string> Paths = new Dictionary<Type, string>()
        {
            { typeof(SingleValueModifier), "Manifests/Singles" },
            { typeof(RangeValueModifier), "Manifests/Ranges" },
            { typeof(IntervalValueModifier), "Manifests/Intervals" },
            { typeof(AdventureManifest), "Activities/Adventures" },
            { typeof(LootTable), "Activities/Rewards" },
            { typeof(ArmourManifest), "Armour" },
            { typeof(ConsumableManifest), "Consumables" },
            { typeof(WeaponManifest), "Weapons" },
            { typeof(Race), "Player/Races" },
        };
    }
}