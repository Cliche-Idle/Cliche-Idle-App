using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public string ID = Guid.NewGuid().ToString();
    public string InternalID;
    public string InternalVariantID;
    public string FriendlyName;
    public string Description;
    public bool CurrentlyInUse;
    public string ClassType;
    public string StatType;
    public string StatValue;
    public int SellPrice;
    public Sprite Icon;
}
