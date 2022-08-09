using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Modifiers/Single value modifier")]
public class SingleValueModifier : ScriptableObject
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
            value *= _value;
        }
        else
        {
            value += _value;
        }
    }

    public void ApplyFloored(float intervalValue, ref float value)
    {
        if (IsPercentValue)
        {
            value = Mathf.FloorToInt(value * _value);
        }
        else
        {
            value = Mathf.FloorToInt(value + _value);
        }
    }
}
