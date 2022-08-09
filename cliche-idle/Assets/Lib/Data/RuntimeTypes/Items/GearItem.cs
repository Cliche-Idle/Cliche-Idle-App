using System;
using UnityEngine;
using Cliche.System;

public abstract class GearItem : Item
{
    

    public abstract ItemManifest Manifest { get; protected set; }
}