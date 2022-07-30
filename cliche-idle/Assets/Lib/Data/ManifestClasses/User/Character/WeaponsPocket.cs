using System;
using Firebase.Firestore;

namespace Cliche.UserManagement
{

    [FirestoreData]
    public class WeaponsPocket
    {

    }

    [Serializable]
    [FirestoreData]
    public class WeaponsPocketItem
    {
        [FirestoreProperty]
        public string variantID { get; set; }

        [FirestoreProperty]
        public string type { get; set; }

        [FirestoreProperty]
        public bool inUse { get; set; }

        [FirestoreProperty]
        public double attack { get; set; }

        [FirestoreProperty]
        public double price { get; set; }
    }   
}