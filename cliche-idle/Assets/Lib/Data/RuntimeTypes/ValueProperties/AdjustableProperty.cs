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
        Value -= Mathf.Abs(amount);
        // Backfill if negative
        if (Value < 0)
        {
            Value = 0;
        }
    }
}