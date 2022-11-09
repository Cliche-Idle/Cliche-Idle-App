using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Modifiers/Range value modifier")]
public class RangeValueModifier : ScriptableObject
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
    public float UpperBound { get; private set; }

    [field: SerializeField]
    public float LowerBound { get; private set; }
}
