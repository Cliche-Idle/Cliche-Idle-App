using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemBucket<TItem> where TItem : Item
{
    [SerializeField]
    protected List<TItem> _itemList = new List<TItem>();

    /// <summary>
    /// The Type this bucket accepts.
    /// </summary>
    public Type BucketType {
        get {
            return typeof(TItem);
        }
    }

    /// <summary>
    /// The list of items in this bucket.
    /// </summary>
    public List<TItem> Items {
        get {
            return _itemList;
        }
    }

    /// <summary>
    /// Contains the equipment sockets for this bucket, where items are equipped.
    /// </summary>
    [field: SerializeField]
    public List<GearSocket<TItem>> Sockets { get; private set; }

    /// <summary>
    /// Adds a new item to the inventory. Automatically attaches a unique variant ID.
    /// </summary>
    /// <param name="item"></param>
    public void Add(TItem item)
    {
        item.AttachVariantID(GetNewVariantID(item.ID));
        _itemList.Add(item);
    }

    /// <summary>
    /// Gets a new VariantID GUID unique amongst other items with the given itemID in the inventory.
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    protected string GetNewVariantID(string itemID)
    {
        string newGUID = Guid.NewGuid().ToString();
        while (true)
        {
            int index = _itemList.FindIndex(element => element.VariantID == newGUID);
            if (index == -1)
            {
                break;
            }
            else
            {
                newGUID = Guid.NewGuid().ToString();
            }
        }
        return newGUID;
    }

    /// <summary>
    /// Removes the specified item from the inventory, if it exists.
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="variantID"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Remove(string itemID, string variantID)
    {
        if (Guid.TryParse(variantID, out var checkGuid))
        {
            int itemIndex = _itemList.FindIndex(element => element.ID == itemID && element.VariantID == variantID);
            if (itemIndex != -1)
            {
                var item = _itemList[itemIndex];
                var socket = Sockets.Find(socket => socket.EquippedItem == item);
                if (socket == null)
                {
                    _itemList.RemoveAt(itemIndex);
                }
                else
                {
                    Debug.LogError($"Can not remove item {itemID}#{variantID}; item is currently equipped.");
                }
            }
            else
            {
                Debug.LogError($"Can not remove item {itemID}#{variantID}; item is not in inventory.");
            }
        }
        else
        {
            throw new ArgumentException("VariantID must be a valid GUID string.", nameof(variantID));
        }
    }

    /// <summary>
    /// Removes the specified item from the inventory, if it exists.
    /// </summary>
    /// <param name="item"></param>
    public void Remove(TItem item)
    {
        Remove(item.ID, item.VariantID);        
    }

    /// <summary>
    /// Checks whether an item exists in the inventory.
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="variantID"></param>
    /// <returns></returns>
    public bool Contains(string itemID, string variantID)
    {
        int itemIndex = _itemList.FindIndex(element => element.ID == itemID && element.VariantID == variantID);
        if (itemIndex == -1)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks whether an item exists in the inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(TItem item)
    {
        return Contains(item.ID, item.VariantID);
    }

    /// <summary>
    /// Equips the specific item in the inventory to one of the available sockets.
    /// 
    /// If multiple matching sockets are found, this item will be equipped into the one that has the item with the lowest <paramref name="MainStatValue" />.
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="variantID"></param>
    /// <exception cref="NullReferenceException">Thrown then the specific item does not exist.</exception>
    public void Equip(string itemID, string variantID)
    {
        var item = _itemList.Find(element => element.ID == itemID && element.VariantID == variantID);
        if (item != null)
        {
            var sockets = Sockets.FindAll(socket => socket.AcceptSubTypeHash == item.ItemSubTypeHash || socket.AcceptSubTypeHash == -1);
            //
            var socket = sockets.Find(socket => socket.EquippedItem.MainStatValue == (sockets.Min(_socket => _socket.EquippedItem.MainStatValue)));
            //
            if (sockets.Count == 1)
            {
                socket = sockets[0];
            }
            socket.SetSocketItem(item);
        }
        else
        {
            throw new NullReferenceException("No item with the specified ID and VariantID could be found.");
        }
    }

    /// <summary>
    /// Equips the specific item in the inventory to one of the available sockets.
    /// 
    /// If multiple matching sockets are found, this item will be equipped into the one that has the item with the lowest <paramref name="MainStatValue" />.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Equip(TItem item)
    {
        if (Guid.TryParse(item.VariantID, out var checkGuid))
        {
            Equip(item.ID, item.VariantID);
        }
        else
        {
            throw new ArgumentException("VariantID must be a valid GUID string.", nameof(item.VariantID));
        }
    }
}
