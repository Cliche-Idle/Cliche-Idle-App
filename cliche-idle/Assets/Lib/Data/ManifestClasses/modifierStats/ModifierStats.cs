using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Items/Consumable modifier")]
public class ModifierStats : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    public int Health;
    
    public int MaxHealth;
}