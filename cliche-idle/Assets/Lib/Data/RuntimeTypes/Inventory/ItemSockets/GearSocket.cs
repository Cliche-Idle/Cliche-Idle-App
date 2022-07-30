using System;
using UnityEngine;

[Serializable]
public class GearSocket<TItem> where TItem : Item
{
    /// <summary>
    /// Identifies the given Socket.
    /// </summary>
    public string ID { get; private set; }

    /// <summary>
    /// The main Type this socket accepts.
    /// </summary>
    public Type AcceptType { 
        get {
            return typeof(TItem);
        }
    }

    /// <summary>
    /// The hash of the SubType this socket accepts.
    /// </summary>
    public int AcceptSubTypeHash { get; private set; }

    /// <summary>
    /// The currently equipped item in this socket.
    /// </summary>
    [field: SerializeField]
    public TItem EquippedItem { get; private set; }

    /// <summary>
    /// Event that fires when a valid equip finishes via <paramref name="SetSocketItem" />.
    /// </summary>
    public event EventHandler<TItem> OnEquip;

    /// <summary>
    /// Creates a new socket that accepts any SubType.
    /// </summary>
    /// <param name="socketID"></param>
    public GearSocket(string socketID)
    {
        ID = socketID;
        AcceptSubTypeHash = (-1);
    }

    /// <summary>
    /// Creates a new socket that only accepts the given SubType.
    /// </summary>
    /// <param name="socketID"></param>
    /// <param name="subTypeHash"></param>
    public GearSocket(string socketID, int subTypeHash)
    {
        ID = socketID;
        AcceptSubTypeHash = subTypeHash;
    }

    /// <summary>
    /// Sets the <paramref name="EquippedItem" /> to the specified item, if the socket accepts it.
    /// </summary>
    /// <param name="item"></param>
    public void SetSocketItem(TItem item)
    {
        if ((AcceptSubTypeHash == -1) || (item.ItemSubTypeHash == AcceptSubTypeHash))
        {
            EquippedItem = item;
            if (OnEquip != null)
            {
                OnEquip.Invoke(this, item);
            }
        }
    }
}