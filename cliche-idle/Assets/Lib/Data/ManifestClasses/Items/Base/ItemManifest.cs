using UnityEngine;

/// <summary>
/// Provides the base for all item manifests.
/// </summary>
public abstract class ItemManifest : ScriptableObject
{
    /// <summary>
    /// The ID of this item. (Filename).
    /// </summary>
    public string ID {
        get {
            return name;
        }
    }

    /// <summary>
    /// The item's base type.
    /// </summary>
    public ItemTypes ItemType { get; protected set; }

    /// <summary>
    /// Indicates if this item's individual instances an have different stat values than the base.
    /// </summary>
    public bool IsInstanceItem { get; protected set; }

    /// <summary>
    /// The item's main stat type.
    /// </summary>
    public ItemMainStatTypes MainStatType { get; protected set; }

    /// <summary>
    /// The item's subtype in integer format.
    /// </summary>
    public abstract int SubTypeHash { get; }

    /// <summary>
    /// The main base stat value this item provides.
    /// </summary>
    [field: Header("Base data")]
    [field: SerializeField]
    public int MainStatValue { get; protected set; }


    /// <summary>
    /// The item's in-game icon.
    /// </summary>
    [field: Header("Manifest data")]
    [field: SerializeField]
    public Sprite Icon { get; protected set; }

    /// <summary>
    /// The item's in-game name (player-friendly).
    /// </summary>
    [field: SerializeField]
    public string Name { get; protected set; }

    /// <summary>
    /// The item's in-game description.
    /// </summary>
    [field: SerializeField]
    [field: TextArea(3,10)]
    public string Description { get; protected set; }

    /// <summary>
    /// The base sell-price of this item.
    /// </summary>
    [field: SerializeField]
    public int Price { get; protected set; }

    /// <summary>
    /// Use to set the item Default values for certain attributes.
    /// </summary>
    protected abstract void OnValidate();
}