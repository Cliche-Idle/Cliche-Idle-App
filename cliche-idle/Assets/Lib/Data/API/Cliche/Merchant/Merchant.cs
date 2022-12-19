using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cliche.System;
using System.Linq;
using Cliche.Idle;

public static partial class Merchant
{
    /// <summary>
    /// Purchases an item from the list of <see cref="DailyOfferings.GetDailyOfferings"/>. The item is then added to the player's inventory, and its total price is taken.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public static void PurchaseItem(Consumable item)
    {
        var offerings = DailyOfferings.GetDailyOfferings();
        if (offerings.Any(offering => offering.ID == item.ID))
        {
            var itemPrice = item.GetManifest().Price;
            if (Player.CurrencyBag.Gold.CanTakeValue(itemPrice))
            {
                Player.Inventory.GiveItem(item);
                Player.CurrencyBag.Gold -= itemPrice;
            }
            else
            {
                throw new Exception($"Player does not have enough currency ({Player.CurrencyBag.Gold.Value}) to cover this purchase (-{itemPrice}).");
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
    public static void SellItem(Item item, int consumableQuantity = 1)
    {
        var itemPrice = item.GetManifest().Price;
        if (item.ItemType == ItemTypes.Consumable)
        {
            Player.Inventory.RemoveItem(item, consumableQuantity);
            itemPrice *= consumableQuantity;
        }
        else
        {
            Player.Inventory.RemoveItem(item);
        }
        Player.CurrencyBag.Gold.Grant(itemPrice);
    }
}
