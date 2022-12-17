using System;
using UnityEngine;
using Cliche.System;

[Serializable]
public abstract class StackableItem : Item
{
    /// <summary>
    /// The amount of items of this type in this stack.
    /// </summary>
    [field: SerializeField]
    public int Quantity { get; private set; } = 0;

    /// <summary>
    /// The maximum amount of items in this stack.
    /// </summary>
    [Obsolete]
    public int MaxStackSize { get; protected set; }

    public StackableItem (string id) : base(id)
    {

    }

    /// <summary>
    /// Calls the item's <see cref="ConsumableManifest.Use"/> and removes a single item from the stack. 
    /// </summary>
    public void Use()
    {
        if (Quantity > 0)
        {
            ConsumableManifest manifest = Manifests.GetByID<ConsumableManifest>(ID);
            manifest.Use();
            Take(1);
        }
    }

    /// <summary>
    /// Adds the specified amount to <see cref="Quantity"/>.
    /// </summary>
    /// <param name="quantity"></param>
    public void Grant(int quantity)
    {
        Quantity += Math.Abs(quantity);
    }

    /// <summary>
    /// Removes the specified amount from <see cref="Quantity"/>. If the amount is larger than <see cref="Quantity"/>, removes as much as it can, without going below zero. 
    /// </summary>
    /// <param name="quantity"></param>
    public void Take(int quantity)
    {
        Quantity -= Math.Abs(quantity);
        if (Quantity < 0)
        {
            Quantity = 0;
        }
    }
}