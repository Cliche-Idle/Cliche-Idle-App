using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StackedItemBag
{
    [SerializeField]
    protected Dictionary<string, int> _itemList;

    public Dictionary<string, int> Items {
        get {
            return _itemList;
        }
    }

    public void Add(Item item)
    {
        Add(item.ID, 1);
    }

    public void Add(string itemID, int quantity)
    {
        if (_itemList.ContainsKey(itemID))
        {
            _itemList[itemID] += Math.Abs(quantity);
        }
        else
        {
            _itemList.Add(itemID, quantity);
        }
    }

    public void Remove(Item item)
    {
        Remove(item.ID, 1);
    }

    public void Remove(string itemID, int quantity)
    {
        if (_itemList.ContainsKey(itemID))
        {
            _itemList[itemID] -= Math.Abs(quantity);
            if (_itemList[itemID] <= 0)
            {
                _itemList.Remove(itemID);
            }
        }
    }
}
