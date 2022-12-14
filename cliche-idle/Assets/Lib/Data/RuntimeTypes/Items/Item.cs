using System;
using UnityEngine;
using Cliche.System;

/// <summary>
/// Provides the base for every game item.
/// </summary>
[Serializable]
public abstract class Item
{
    // Mandatory for every item

    /// <summary>
    /// The Manifest ID of the item.
    /// </summary>
    [field: SerializeField]
    public string ID { get; protected set; }

    /// <summary>
    /// The base type of the item.
    /// </summary>
    [field: SerializeField]
    public ItemTypes ItemType { get; protected set; }

    /// <summary>
    /// The base type of the item, converted to int.
    /// </summary>
    [field: SerializeField]
    public int ItemSubTypeHash { get; protected set; }

    /// <summary>
    /// The item's base stat type.
    /// </summary>
    [field: SerializeField]
    public ItemMainStatTypes MainStatType { get; protected set; }

    /// <summary>
    /// The item's base stat value.
    /// </summary>
    [field: SerializeField]
    public int MainStatValue { get; protected set; }

    // Instanced
    
    /// <summary>
    /// Checks whether or not this item is uniquely instanced, or if its a generic.
    /// </summary>
    [field: SerializeField]
    public bool IsInstanceItem { get; protected set; }

    /// <summary>
    /// If this item is instanced, containes the instance's unique GUID.
    /// </summary>
    [field: SerializeField]
    public string VariantID { get; protected set; }

    public Item (string id)
    {
        ID = id;
        LoadFromManifest();
        if (IsInstanceItem == true)
        {
            VariantID = Guid.NewGuid().ToString();
        }
    }

    /// <summary>
    /// Loads the item's details from its Manifest.
    /// </summary>
    protected abstract void LoadFromManifest();

    /// <summary>
    /// Gets the item's Manifest.
    /// </summary>
    /// <returns></returns>
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