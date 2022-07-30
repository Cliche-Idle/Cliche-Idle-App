using System;
using System.Collections.Generic;
using Firebase.Firestore;

namespace Cliche.UserManagement
{
    [Serializable]
    [FirestoreData]
    public class InventoryBag
    {
        [FirestoreProperty]
        public List<WeaponsPocketItem> weapons { get; set; }

        [FirestoreProperty]
        public List<ArmourPocketItem> armour { get; set; }

        [FirestoreProperty]
        public List<ItemsPocketItem> items { get; set; }
    }   
}