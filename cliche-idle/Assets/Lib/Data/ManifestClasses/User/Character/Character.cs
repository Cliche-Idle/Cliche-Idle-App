using System;
using Firebase.Firestore;

namespace Cliche.UserManagement
{
    [Serializable]
    [FirestoreData]
    public class CharacterData
    {
        [FirestoreProperty]
        public GeneralData general { get; set; }

        [FirestoreProperty]
        public CurrencyBag currencies { get; set; }

        [FirestoreProperty]
        public InventoryBag inventory { get; set; }

        [FirestoreProperty]
        public ActivityBag activities { get; set; }

        [FirestoreProperty]
        public CoreStats stats { get; set; }
    }   
}