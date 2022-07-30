using Firebase.Firestore;
using UnityEngine;
using System.Collections.Generic;

[FirestoreData]
public class CoreStats
{
    [FirestoreProperty]
    public double vitality { get; set; }
    [FirestoreProperty]
    public double dexterity { get; set; }
    [FirestoreProperty]
    public double strength { get; set; }
    [FirestoreProperty]
    public double intelligence { get; set; }
}