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

    [field: SerializeField]
    [field: TextArea(3,10)]
    public string InternalDescription { get; private set; }

    [field: SerializeField]
    [field: TextArea(3,10)]
    public string GameDescription { get; private set; }

    [field: SerializeField]
    public float Value { get; private set; }

    [field: SerializeField]
    public bool IsPercentValue { get; private set; }

    public float Apply(float intervalValue, float value)
    {
        if (IsPercentValue)
        {
            value *= Value;
        }
        else
        {
            value += Value;
        }
        return value;
    }

    public float ApplyFloored(float intervalValue, float value)
    {
        if (IsPercentValue)
        {
            value = Mathf.FloorToInt(value * Value);
        }
        else
        {
            value = Mathf.FloorToInt(value + Value);
        }
        return value;
    }
}
