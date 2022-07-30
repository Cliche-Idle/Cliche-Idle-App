using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField]
    public ItemBucket<Weapon> Weapons;

    [SerializeField]
    public ItemBucket<Armour> Armour;

    [SerializeField]
    public StackedItemBag Consumables;

    private void Awake() {
        // Populate weapon sockets:
        Weapons.Sockets.Add(new GearSocket<Weapon>("WeaponSocket"));
        // Populate armour sockets:
        Armour.Sockets.Add(new GearSocket<Armour>("HelmetSocket", (int)ArmourType.Helmet));
        Armour.Sockets.Add(new GearSocket<Armour>("ChestSocket"));
        Armour.Sockets.Add(new GearSocket<Armour>("LegSocket"));
    }
}
/*
[Serializable]
public class Consumable : IItem
{
    [field: SerializeField]
    public string ID { get; private set; }

    public Consumable(string id)
    {
        ID = id;
    }

    public ConsumableItem GetManifestData()
    {
        return Resources.Load<ConsumableItem>($"Consumables/{ID}");
    }
}
*/