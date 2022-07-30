using System;
using Firebase.Firestore;

namespace Cliche.UserManagement
{
    /// <summary>
    /// Contains the list of currencies the user can hold.
    /// </summary>
    [Serializable]
    [FirestoreData]
    public class CurrencyBag
    {
        /// <summary>
        /// The Gold help by the user.
        /// </summary>
        [FirestoreProperty]
        public double gold { get; set; }
    }   
}