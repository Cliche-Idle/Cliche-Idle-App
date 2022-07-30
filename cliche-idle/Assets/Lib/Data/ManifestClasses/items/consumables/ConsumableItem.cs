using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Items/Consumable")]
public class ConsumableItem : ScriptableObject
{
    public string ID
    {
        get
        {
            return name;
        }
    }

    [SerializeField]
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
    }

    [SerializeField]
    [TextArea(3, 10)]
    private string _description;
    public string Description
    {
        get
        {
            return _description;
        }
    }

    [SerializeField]
    private int _price;
    public int Price
    {
        get
        {
            return _price;
        }
    }

    [SerializeField]
    private ModifierStats _stats;
    public ModifierStats Stats
    {
        get
        {
            return _stats;
        }
    }
    /*
    
    [SerializeField]
    public IConsumableFunction ItemFunction { get; private set; }
    
    */
}