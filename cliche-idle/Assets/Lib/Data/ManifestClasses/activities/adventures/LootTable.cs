using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "GameData/Activities/Loot table")]
public class LootTable : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    public List<WeaponItem> Weapons;

    public List<ArmourItem> Armour;

    public List<ConsumableItem> Items;

    // TODO: get loot   (Singe or composite category)
}