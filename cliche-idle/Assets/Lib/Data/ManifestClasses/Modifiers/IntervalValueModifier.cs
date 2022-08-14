using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Modifiers/Interval value setting")]
public class IntervalValueModifier : ScriptableObject
{
    public string ID {
        get
        {
            return name;
        }
    }

    [SerializeField]
    [TextArea(3,10)]
    private string _internalDescription;
    public string InternalDescription {
        get {
            return _internalDescription;
        }
    }

    [SerializeField]
    [TextArea(3,10)]
    private string _gameDescription;
    public string GameDescription {
        get {
            return _gameDescription;
        }
    }

    [SerializeField]
    private float _interval;
    public float Interval {
        get {
            return _interval;
        }
    }

    [SerializeField]
    private float _value;
    public float Value {
        get {
            return _value;
        }
    }
    
    [SerializeField]
    private bool _isPercentValue;
    public bool IsPercentValue {
        get {
            return _isPercentValue;
        }
    }

    public void Apply(float intervalValue, ref float value)
    {
        if (IsPercentValue)
        {
            value *= GetAmount(value);
        }
        else
        {
            value += GetAmount(value);
        }
    }

    public void ApplyFloored(float intervalValue, ref float value)
    {
        if (IsPercentValue)
        {
            value *= GetAmountFloored(value);
        }
        else
        {
            value += GetAmountFloored(value);
        }
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
