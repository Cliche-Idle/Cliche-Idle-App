using System;
using UnityEngine;
using Cliche.System;

[Serializable]
public abstract class Item
{
    // Mandatory for every item
    [field: SerializeField]
    public string ID { get; protected set; }
    [field: SerializeField]
    public ItemTypes ItemType { get; protected set; }
    [field: SerializeField]
    public int ItemSubTypeHash { get; protected set; }
    [field: SerializeField]
    public ItemMainStatTypes MainStatType { get; protected set; }
    [field: SerializeField]
    public int MainStatValue { get; protected set; }
    // Instanced
    [field: SerializeField]
    public bool IsInstanceItem { get; protected set; }

    [field: SerializeField]
    public string VariantID { get; protected set; }

    public void AttachVariantID(string variantID)
    {
        if (IsInstanceItem == true)
        {
            if (Guid.TryParse(variantID, out var checkGuid))
            {
                VariantID = variantID;
            }
            else
            {
                VariantID = null;
                Debug.LogError("Item VariantID is not a valid GUID.");
            }
        }
        else
        {
            Debug.LogError("Can not attach VariantID to a non-instance item.");
        }
    }

    protected abstract void LoadFromManifest();

    public ItemManifest GetManifest()
    {
        return Manifests.GetByObject(this);
    }

    public static ItemTypes GetInternalItemType(Type item)
    {
        ItemTypes internalItemType = ItemTypes.Undefined;
        if (Enum.IsDefined(typeof(ItemTypes), item.Name))
        {
            internalItemType = (ItemTypes)Enum.Parse(typeof(ItemTypes), item.Name);
        }
        return internalItemType;
    }

    public static ItemTypes GetInternalItemType(object item)
    {
        return GetInternalItemType(item.GetType());
    }
}