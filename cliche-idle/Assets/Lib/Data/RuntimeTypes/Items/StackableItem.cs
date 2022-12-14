using System;
using UnityEngine;
using Cliche.System;

[Serializable]
public abstract class StackableItem : Item
{
    [field: SerializeField]
    public int Quantity { get; private set; } = 0;

    public int MaxStackSize { get; protected set; }

    public StackableItem (string id) : base(id)
    {

    }

    /// <summary>
    /// 
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
    /// 
    /// </summary>
    /// <param name="quantity"></param>
    public void Grant(int quantity)
    {
        Quantity += Math.Abs(quantity);
    }

    /// <summary>
    /// 
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