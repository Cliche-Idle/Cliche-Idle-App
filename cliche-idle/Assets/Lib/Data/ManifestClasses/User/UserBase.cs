using System;
using Firebase.Firestore;
using UnityEngine;

namespace Cliche.UserManagement
{
    [FirestoreData]
    [Serializable]
    public class User
    {
        [FirestoreProperty]
        [field: SerializeField]
        public AccountData account { get; set; }

        [FirestoreProperty]
        [field: SerializeField]
        public CharacterData character { get; set; }
    }
}