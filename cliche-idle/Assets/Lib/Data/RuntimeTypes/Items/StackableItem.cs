using System;
using UnityEngine;
using Cliche.System;

[Serializable]
public abstract class StackableItem : Item
{
    [field: SerializeField]
    public int Quantity { get; private set; }

    public int MaxStackSize { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public void Use()
    {
        ConsumableManifest manifest = Manifests.GetByID<ConsumableManifest>(ID);
        manifest.Use();
        Take(1);
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