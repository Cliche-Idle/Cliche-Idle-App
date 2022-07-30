using Firebase.Firestore;

namespace Cliche.UserManagement
{
    /// <summary>
    /// This is used to store personally identifiable account data, such as username, date of registration and time of last sync.
    /// </summary>
    [FirestoreData]
    public class IdentifiableAccountData
    {
        /// <summary>
        /// The UNIX timestamp of when the account was created.
        /// </summary>
        [FirestoreProperty]
        public double registeredOn { get; set; }
        /// <summary>
        /// The UNIX timestamp of the last sync (write) of the account. This is mainly for character info.
        /// </summary>
        [FirestoreProperty]
        public double lastSync { get; set; }
        /// <summary>
        /// The unique account username of the user.
        /// </summary>
        [FirestoreProperty]
        public string userName { get; set; }
    }   
}