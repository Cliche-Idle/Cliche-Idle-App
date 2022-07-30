using System;
using Firebase.Firestore;

namespace Cliche.UserManagement
{
    [FirestoreData]
    public class ArmourPocket
    {

    }

    [Serializable]
    [FirestoreData]
    public class ArmourPocketItem
    {
        [FirestoreProperty]
        public string variantID { get; set; }

        [FirestoreProperty]
        public string type { get; set; }

        [FirestoreProperty]
        public bool inUse { get; set; }

        [FirestoreProperty]
        public double defense { get; set; }

        [FirestoreProperty]
        public double price { get; set; }
    }   
}