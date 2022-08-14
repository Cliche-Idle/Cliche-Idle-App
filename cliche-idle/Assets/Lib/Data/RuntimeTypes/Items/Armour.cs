using System;
using Cliche.System;

[Serializable]
public class Armour : Item
{
    public ArmourType ItemSubType {
        get {
            return (ArmourType)ItemSubTypeHash;
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
        ArmourManifest manifest = Manifests.GetByID<ArmourManifest>(ID);
        ItemType = manifest.ItemType;
        ItemSubTypeHash = manifest.SubTypeHash;
        MainStatType = manifest.MainStatType;
        IsInstanceItem = manifest.IsInstanceItem;
    }
}