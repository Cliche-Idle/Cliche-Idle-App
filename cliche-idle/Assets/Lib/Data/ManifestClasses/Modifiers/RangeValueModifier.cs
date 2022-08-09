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
    private float _upperBound;
    public float UpperBound {
        get {
            return _upperBound;
        }
    }

    [SerializeField]
    private float _lowerBound;
    public float LowerBound {
        get {
            return _lowerBound;
        }
    }
}
