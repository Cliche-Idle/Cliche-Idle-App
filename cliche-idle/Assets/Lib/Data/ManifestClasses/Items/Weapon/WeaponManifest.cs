using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Items/Weapon")]
public class WeaponManifest : ItemManifest
{
    /// <summary>
    /// The item's subtype (ItemType specific.)
    /// </summary>
    [field: Header("Item type")]
    [field: SerializeField]
    public WeaponType SubType { get; private set; }
    
    public override int SubTypeHash {
        get {
            return (int)SubType;
        }
    }

    protected override void OnValidate() {
        ItemType = ItemTypes.Weapon;
        IsInstanceItem = true;
        MainStatType = ItemMainStatTypes.Attack;
    }
}