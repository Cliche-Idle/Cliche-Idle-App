using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Store/Merchant store")]
public class MerchantStore : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    /// <summary>
    /// The maximum number of items the merchant can offer at a time. Depending on the amount of items actually in the list, the daily offerings might be less.
    /// </summary>
    [field: SerializeField]
    public int MaxAvailableItems { get; private set; }

    /// <summary>
    /// The list of possible items the merchant can sell.
    /// </summary>
    [field: SerializeField]
    public List<ConsumableManifest> Consumables { get; private set; }
}