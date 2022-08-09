using UnityEngine;
using System;
using Cliche.System;

[Serializable]
public class Consumable : StackableItem
{
    public ConsumableType ItemSubType {
        get {
            return (ConsumableType)ItemSubTypeHash;
        }
    }

    public Consumable(string id)
    {
        ID = id;
        LoadFromManifest();
    }

    public Consumable(string id, int quantity) : this(id)
    {
        Grant(quantity);
    }

    protected override void LoadFromManifest()
    {
        ConsumableManifest manifest = Manifests.GetByID<ConsumableManifest>(ID);
        ItemType = manifest.ItemType;
        ItemSubTypeHash = manifest.SubTypeHash;
        MainStatType = manifest.MainStatType;
        IsInstanceItem = manifest.IsInstanceItem;
    }
}