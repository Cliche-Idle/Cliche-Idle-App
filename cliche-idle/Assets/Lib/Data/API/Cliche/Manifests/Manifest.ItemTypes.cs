using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cliche.System
{
    public partial class Manifests
    {
        public static readonly Dictionary<ItemTypes, Type> ItemManifestTypes = new Dictionary<ItemTypes, Type>()
        {
            { ItemTypes.Armour, typeof(ArmourManifest) },
            { ItemTypes.Consumable, typeof(ConsumableManifest) },
            { ItemTypes.Weapon, typeof(WeaponManifest) }
        };
    }
}