using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Player race")]
public class Race : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    /// <summary>
    /// The in-game display name of the given race.
    /// </summary>
    public string Name {
        get {
            return name;
        }
    }

    /// <summary>
    /// The in-game icon of the given race.
    /// </summary>
    [field: SerializeField]
    public Sprite Icon { get; private set; }

    /// <summary>
    /// The in-game description of the given race.
    /// </summary>
    [field: TextArea(3,10)]
    [field: SerializeField]
    public string Description { get; private set; }

    /// <summary>
    /// The starting base racial stat value.
    /// </summary>
    [field: SerializeField]
    public int Dexterity { get; private set; }

    /// <summary>
    /// The starting base racial stat value.
    /// </summary>
    [field: SerializeField]
    public int Vitality { get; private set; }

    /// <summary>
    /// The starting base racial stat value.
    /// </summary>
    [field: SerializeField]
    public int Intelligence { get; private set; }

    /// <summary>
    /// The starting base racial stat value.
    /// </summary>
    [field: SerializeField]
    public int Strength { get; private set; }
}