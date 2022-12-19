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

    public void GiveItem(Item item, int consumableQuantity = 1)
    {
        var itemType = Item.GetInternalItemType(item);
        switch (itemType)
        {
            case ItemTypes.Weapon:
                Weapons.Add((Weapon)item);
                break;
            case ItemTypes.Armour:
                Armour.Add((Armour)item);
                break;
            case ItemTypes.Consumable:
                Consumables.Add((Consumable)item);
                break;
        }
    }

    public void RemoveItem(Item item, int consumableQuantity = 1)
    {
        var itemType = Item.GetInternalItemType(item);
        switch (itemType)
        {
            case ItemTypes.Weapon:
                Weapons.Remove((Weapon)item);
                break;
            case ItemTypes.Armour:
                Armour.Remove((Armour)item);
                break;
            case ItemTypes.Consumable:
                Consumables.Remove((Consumable)item, consumableQuantity);
                break;
        }
    }

    public void EquipItem(Item item)
    {
        var itemType = Item.GetInternalItemType(item);
        switch (itemType)
        {
            case ItemTypes.Weapon:
                Weapons.Equip((Weapon)item);
                break;
            case ItemTypes.Armour:
                Armour.Equip((Armour)item);
                break;
            default:
                throw new System.Exception($"Item of type {itemType} can not be equipped.");
        }
    }

    public bool IsEquipped(Item item)
    {
        var itemType = Item.GetInternalItemType(item);
        switch (itemType)
        {
            case ItemTypes.Weapon:
                var wskt = Weapons.Sockets.Find(socket => socket.EquippedItem.ID == item.ID && socket.EquippedItem.VariantID == item.VariantID);
                return (wskt != null);
            case ItemTypes.Armour:
                var askt = Weapons.Sockets.Find(socket => socket.EquippedItem.ID == item.ID && socket.EquippedItem.VariantID == item.VariantID);
                return (askt != null);
            default:
                return false;
        }
    }
}