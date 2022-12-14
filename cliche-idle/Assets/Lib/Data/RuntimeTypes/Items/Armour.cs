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

    public Armour(string id, int defence) : base(id)
    {
        MainStatValue = defence;
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