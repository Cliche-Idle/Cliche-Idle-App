using System;
using UnityEngine;

[System.Serializable]
public class AdjustableProperty
{
    [field: SerializeField]
    public int Value { get; private set; }

    public void Grant (int amount)
    {
        Value += Mathf.Abs(amount);
    }

    public void Take (int amount)
    {
        if (Value >= Mathf.Abs(amount))
        {
            Value -= Mathf.Abs(amount);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be larger than or equal to the Value.");
        }
    }
}