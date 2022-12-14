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
        _itemList.Add(item);
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
            var item = _itemList.Find(element => element.ID == itemID && element.VariantID == variantID);
            if (item != null)
            {
                var socket = Sockets.Find(socket => socket.EquippedItem == item);
                if (socket == null)
                {
                    _itemList.Remove(item);
                }
                else
                {
                    throw new Exception($"Can't remove a currently equipped item from the inventory.");
                }
            }
            else
            {
                throw new KeyNotFoundException($"{item} is not present in the bucket.");
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
    /// <exception cref="KeyNotFoundException">Thrown then the specific item does not exist.</exception>
    public void Equip(string itemID, string variantID)
    {
        var item = _itemList.Find(element => element.ID == itemID && element.VariantID == variantID);
        if (item != null)
        {
            var socket = FindBestMatchingSocket(item);
            socket.SetSocketItem(item);
        }
        else
        {
            throw new KeyNotFoundException($"{item} is not present in the bucket.");
        }
    }

    /// <summary>
    /// Equips the specific item in the inventory to one of the available sockets.
    /// 
    /// If multiple matching sockets are found, this item will be equipped into the one that has the item with the lowest <paramref name="MainStatValue" />.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public void Equip(TItem item)
    {
        Equip(item.ID, item.VariantID);
    }

    /// <summary>
    /// Gets the best matching socket for a given item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private GearSocket<TItem> FindBestMatchingSocket(TItem item)
    {
        var compatibleSockets = Sockets.FindAll(socket => socket.IsItemCompatible(item));
        if (compatibleSockets.Count == 0)
        {
            throw new Exception($"No matching socket(s) found for item {item}.");
        }
        else
        {
            // Find empty socket and return it, if there is any
            var emptySocket = compatibleSockets.Find(socket => socket.EquippedItem == null);
            if (emptySocket != null)
            {
                return emptySocket;
            }
            else
            {
                // If there isn't an empty socket, grab the one that has the item with the lowest MainStatValue
                int lowestSocketValue = compatibleSockets.Min(socket => socket.EquippedItem.MainStatValue);
                var lowestValueCompatibleSocket = compatibleSockets.Find(socket => socket.EquippedItem.MainStatValue == lowestSocketValue);
                return lowestValueCompatibleSocket;
            }
        }
    }
}
