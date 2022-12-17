using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cliche.System;
using System.Linq;

public partial class Merchant : MonoBehaviour
{
    public InventoryHandler inventoryHandler;
    public CurrencyHandler currencyHandler;

    private void Start()
    {
        var offerings = DailyOfferings.GetDailyOfferings().Select(item => item.ID).ToList();
        Debug.Log(String.Join(", ", offerings));
        PurchaseItem(DailyOfferings.GetDailyOfferings()[0]);
    }

    /// <summary>
    /// Purchases an item from the list of <see cref="DailyOfferings.GetDailyOfferings"/>. The item is then added to the player's inventory, and its total price is taken.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void PurchaseItem(Consumable item)
    {
        var offerings = DailyOfferings.GetDailyOfferings();
        if (offerings.Any(offering => offering.ID == item.ID))
        {
            var itemPrice = item.GetManifest().Price;
            if (currencyHandler.Gold.CanTakeValue(itemPrice))
            {
                inventoryHandler.GiveItem(item);
                currencyHandler.Gold -= itemPrice;
            }
            else
            {
                throw new Exception($"Player does not have enough currency ({currencyHandler.Gold.Value}) to cover this purchase (-{itemPrice}).");
            }
        }
        else
        {
            throw new Exception($"The given item ({item.ID}) is not in the offerings list.");
        }
    }

    /// <summary>
    /// Sells any item. The item is then removed from the player's inventory, and its total price is credited.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="consumableQuantity"></param>
    public void SellItem(Item item, int consumableQuantity = 1)
    {
        var itemPrice = item.GetManifest().Price;
        if (item.ItemType == ItemTypes.Consumable)
        {
            inventoryHandler.RemoveItem(item, consumableQuantity);
            itemPrice *= consumableQuantity;
        }
        else
        {
            inventoryHandler.RemoveItem(item);
        }
        currencyHandler.Gold.Take(itemPrice);
    }
}
