using UnityEngine;

public abstract class ConsumableManifest : ItemManifest
{
    /// <summary>
    /// The item's main stat type.
    /// </summary>
    [field: SerializeField]
    // ! This is an intentional override, as this is normally hidden in the inspector and set from OnValidate()
    // ! but for consumables it makes sense for this to be public.
    new public ItemMainStatTypes MainStatType { get; protected set; }

    /// <summary>
    /// The item's subtype (ItemType specific.)
    /// </summary>
    [field: Header("Type")]
    [field: SerializeField]
    public ConsumableType SubType { get; private set; }

    /// <summary>
    /// The item's subtype in integer format.
    /// </summary>
    public override int SubTypeHash {
        get {
            return (int)SubType;
        }
    }

    /// <summary>
    /// The maximum amount of items of this type in a stack.
    /// </summary>
    [field: SerializeField]
    public int MaxStackSize { get; private set; }

    /// <summary>
    /// Applies the consumables's effect(s). This is individually set for every consumable subtype.
    /// </summary>
    public abstract void Use();
}