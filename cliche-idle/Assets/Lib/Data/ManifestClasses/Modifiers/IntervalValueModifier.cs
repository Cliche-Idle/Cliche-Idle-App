using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Modifiers/Interval value modifier")]
public class IntervalValueModifier : ScriptableObject
{
    public string ID {
        get
        {
            return name;
        }
    }

    [field: SerializeField]
    [field: TextArea(3,10)]
    public string InternalDescription { get; private set; }

    [field: SerializeField]
    [field: TextArea(3,10)]
    public string GameDescription { get; private set; }

    [field: SerializeField]
    public float Interval { get; private set; }

    [field: SerializeField]
    public float Value { get; private set; }

    [field: SerializeField]
    public bool IsPercentValue { get; private set; }

    public float Apply(float intervalValue, float value)
    {
        if (IsPercentValue)
        {
            value *= GetAmount(value);
        }
        else
        {
            value += GetAmount(value);
        }
        return value;
    }

    public float ApplyFloored(float intervalValue, float value)
    {
        if (IsPercentValue)
        {
            value *= GetAmountFloored(value);
        }
        else
        {
            value += GetAmountFloored(value);
        }
        return value;
    }

    public float GetAmount(float intervalValue)
    {
        return (intervalValue / Interval) * Value;
    }

    public int GetAmountFloored(float intervalValue)
    {
        return Mathf.FloorToInt(GetAmount(intervalValue));
    }
}
