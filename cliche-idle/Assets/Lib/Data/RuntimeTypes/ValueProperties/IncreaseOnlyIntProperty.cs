using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// An integer property thats value can only be increased, and can be subscribed to.
/// </summary>
[Serializable]
public class IncreaseOnlyIntProperty
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

    private void InvokeValueChangeEvent()
    {
        if (OnValueChange != null)
        {
            OnValueChange.Invoke(Value);
        }
    }
}