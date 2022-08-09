using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Items/Armour")]
public class ArmourManifest : ItemManifest
{
    /// <summary>
    /// The item's subtype (ItemType specific.)
    /// </summary>
    [field: Header("Type")]
    [field: SerializeField]
    public ArmourType SubType { get; private set; }

    public override int SubTypeHash {
        get {
            return (int)SubType;
        }
    }

    protected override void OnValidate() {
        ItemType = ItemTypes.Armour;
        IsInstanceItem = true;
        MainStatType = ItemMainStatTypes.Defence;
    }
}