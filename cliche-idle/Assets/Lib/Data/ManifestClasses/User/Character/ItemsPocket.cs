using System;
using Firebase.Firestore;

namespace Cliche.UserManagement
{
    [FirestoreData]
    public class ItemsPocket
    {

    }

    [Serializable]
    [FirestoreData]
    public class ItemsPocketItem
    {
        [FirestoreProperty]
        public string id { get; set; }

        [FirestoreProperty]
        public double quantity { get; set; }
    }   
}