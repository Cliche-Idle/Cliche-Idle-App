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

    public Weapon(string id, int attack) : base(id)
    {
        MainStatValue = attack;
    }

    protected override void LoadFromManifest()
    {
        WeaponManifest manifest = Manifests.GetByID<WeaponManifest>(ID);
        ItemType = manifest.ItemType;
        ItemSubTypeHash = manifest.SubTypeHash;
        MainStatType = manifest.MainStatType;
        IsInstanceItem = manifest.IsInstanceItem;
    }
}