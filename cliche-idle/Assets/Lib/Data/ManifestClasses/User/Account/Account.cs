using System;
using Firebase.Firestore;
using UnityEngine;

namespace Cliche.UserManagement
{
    /// <summary>
    /// Contains the user's global account specific data.
    /// </summary>
    [FirestoreData]
    [Serializable]
    public class AccountData
    {
        [FirestoreProperty]
        [field: SerializeField]
        public double createdOn { get; set; }

        [FirestoreProperty]
        [field: SerializeField]
        public double lastWrite { get; set; }

        [FirestoreProperty]
        [field: SerializeField]
        public string userName { get; set; }
    
        // TODO: switch this structure out to IdentifiableAccountData for easy data protection management

        [FirestoreProperty]
        public IdentifiableAccountData identifiable { get; set; }
    }   
}