using System;
using Cliche.System;

[Serializable]
public class Weapon : Item
{
    public WeaponType ItemSubType {
        get {
            return (WeaponType)ItemSubTypeHash;
        }
    }
    public Weapon(string id, int attack)
    {
        ID = id;
        MainStatValue = attack;
        LoadFromManifest();
    }

    public Weapon(string id, string variantID, int attack) : this(id, attack)
    {
        AttachVariantID(variantID);
    }

    protected override void LoadFromManifest()
    {
        WeaponItem manifest = Manifests.GetByID<WeaponItem>(ID);
        ItemType = manifest.ItemType;
        ItemSubTypeHash = manifest.SubTypeHash;
        MainStatType = manifest.MainStatType;
        IsInstanceItem = manifest.IsInstanceItem;
        // Item manifest data
        Icon = manifest.Icon;
        Name = manifest.Name;
        Description = manifest.Description;
        Price = manifest.Price;
    }
}