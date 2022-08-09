using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public ItemBucket<Weapon> Weapons;

    public ItemBucket<Armour> Armour;

    public StackedItemBucket<Consumable> Consumables;

    private void Awake() {
        // Populate weapon sockets:
        Weapons.Sockets.Add(new GearSocket<Weapon>("WeaponSocket"));
        // Populate armour sockets:
        Armour.Sockets.Add(new GearSocket<Armour>("HelmetSocket", (int)ArmourType.Helmet));
        Armour.Sockets.Add(new GearSocket<Armour>("ChestSocket", (int)ArmourType.Chest));
        Armour.Sockets.Add(new GearSocket<Armour>("LegSocket", (int)ArmourType.Leg));
    }
}