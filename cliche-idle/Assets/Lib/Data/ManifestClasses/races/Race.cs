using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Player race")]
public class Race : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    public string Name {
        get {
            return name;
        }
    }

    [SerializeField]
    [TextArea(3,10)]
    private string _description;
    public string Description {
        get {
            return _description;
        }
    }

    [SerializeField]
    private int _Dexterity;
    public int Dexterity {
        get {
            return _Dexterity;
        }
    }

    [SerializeField]
    private int _vitality;
    public int Vitality {
        get {
            return _vitality;
        }
    }

    [SerializeField]
    private int _intelligence;
    public int Intelligence {
        get {
            return _intelligence;
        }
    }

    [SerializeField]
    private int _strength;
    public int Strength {
        get {
            return _strength;
        }
    }
}