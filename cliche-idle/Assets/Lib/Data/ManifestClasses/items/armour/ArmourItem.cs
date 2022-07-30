using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Items/Armour")]
public class ArmourItem : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }


    [field: Header("Type")]
    [field: SerializeField]
    public ArmourType SubType { get; private set; }
    public int SubTypeHash {
        get {
            return (int)SubType;
        }
    }
    public readonly ItemTypes ItemType = ItemTypes.Armour;

    public readonly bool IsInstanceItem = true;

    public readonly ItemMainStatTypes MainStatType = ItemMainStatTypes.Defence;

    [field: SerializeField]
    public int MainStatValue { get; private set; }


    [field: Header("Manifest data")]


    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    [field: TextArea(3,10)]
    public string Description { get; private set; }

    [field: SerializeField]
    public int Price { get; private set; }
}