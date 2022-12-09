using System;
using UnityEngine;

/// <summary>
/// An integer property thats value can be increased or descreased (but never go below 0), and can be subscribed to.
/// </summary>
[System.Serializable]
public class AdjustableIntProperty
{
    /// <summary>
    /// The property's value.
    /// </summary>
    [field: SerializeField]
    public int Value { get; protected set; }

    /// <summary>
    /// Called when the property's value changes.
    /// </summary>
    public Action<int> OnValueChange;

    /// <summary>
    /// Increases the property's value by the given amount. Always adds an absolute value.
    /// </summary>
    /// <param name="amount"></param>
    public virtual void Grant (int amount)
    {
        Value += Mathf.Abs(amount);
        InvokeValueChangeEvent();
    }

    /// <summary>
    /// Decreases the property's value by the given amount. Always takes an absolute value. Can not go below 0.
    /// </summary>
    /// <param name="amount"></param>
    public virtual void Take (int amount)
    {
        Value -= Mathf.Abs(amount);
        // Backfill if negative
        if (Value < 0)
        {
            Value = 0;
        }
        InvokeValueChangeEvent();
    }

    protected void InvokeValueChangeEvent()
    {
        if (OnValueChange != null)
        {
            OnValueChange.Invoke(Value);
        }
    }
}