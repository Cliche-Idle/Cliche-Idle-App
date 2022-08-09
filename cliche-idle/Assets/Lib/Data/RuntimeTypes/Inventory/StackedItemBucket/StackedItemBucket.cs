using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StackedItemBucket<TItem> where TItem : StackableItem
{
    [SerializeField]
    protected List<TItem> _itemList;

    public List<TItem> Items {
        get {
            return _itemList;
        }
    }

    public void Add(TItem item)
    {
        if (item.Quantity <= 0)
        {
            Add(item, 1);
        }
        else
        {
            Add(item, item.Quantity);
        }
    }

    public void Add(TItem item, int quantity)
    {
        var itemIndex = _itemList.FindIndex(element => element.ID == item.ID);
        if (itemIndex != -1)
        {
            _itemList[itemIndex].Grant(quantity);
        }
        else
        {
            if (item.Quantity <= 0)
            {
                item.Grant(quantity);
            }
            _itemList.Add(item);
        }
    }

    public void RemoveStack(TItem item)
    {
        var itemIndex = _itemList.FindIndex(element => element.ID == item.ID);
        if (itemIndex != -1)
        {
            _itemList.RemoveAt(itemIndex);
        }
    }

    public void Remove(TItem item, int quantity)
    {
        var itemIndex = _itemList.FindIndex(element => element.ID == item.ID);
        if (itemIndex != -1)
        {
            _itemList[itemIndex].Take(quantity);
        }
        if (_itemList[itemIndex].Quantity <= 0)
        {
            _itemList.RemoveAt(itemIndex);
        }
    }
}
