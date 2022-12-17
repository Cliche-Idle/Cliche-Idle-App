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
    /// <exception cref="Exception">Thrown when the given <paramref name="amount"/> is larger than <see cref="Value"/></exception>
    public virtual void Take (int amount)
    {
        if (CanTakeValue(amount))
        {
            Value -= Mathf.Abs(amount);
            InvokeValueChangeEvent();
        }
        else
        {
            throw new Exception($"Can not take value ({amount}) from property ({GetType().Name}); it is larger the available property value {Value}.");
        }
    }

    public static AdjustableIntProperty operator ++(AdjustableIntProperty aip)
    {
        aip.Grant(1);
        return aip;
    }

    public static AdjustableIntProperty operator +(AdjustableIntProperty aip, int val)
    {
        aip.Grant(val);
        return aip;
    }

    public static AdjustableIntProperty operator --(AdjustableIntProperty aip)
    {
        aip.Take(1);
        return aip;
    }

    public static AdjustableIntProperty operator -(AdjustableIntProperty aip, int val)
    {
        aip.Take(val);
        return aip;
    }

    /// <summary>
    /// Checks if <see cref="Value"/> can cover the removal of the specified amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public virtual bool CanTakeValue(int amount)
    {
        return (Value >= Mathf.Abs(amount));
    }

    protected void InvokeValueChangeEvent()
    {
        if (OnValueChange != null)
        {
            OnValueChange.Invoke(Value);
        }
    }
}