using System;
using Cliche.System;

[Serializable]
public class Armour : Item
{
    public WeaponType ItemSubType {
        get {
            return (WeaponType)ItemSubTypeHash;
        }
    }
    public Armour(string id, int defence)
    {
        ID = id;
        MainStatValue = defence;
        LoadFromManifest();
    }

    public Armour(string id, string variantID, int defence) : this(id, defence)
    {
        AttachVariantID(variantID);
    }

    protected override void LoadFromManifest()
    {
        ArmourItem manifest = Manifests.GetByID<ArmourItem>(ID);
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