using System;
using System.Collections.Generic;
using Firebase.Firestore;

namespace Cliche.UserManagement
{

    /// <summary>
    /// Contains the list of activities that are available; or are in-progress for the user.
    /// </summary>
    [Serializable]
    [FirestoreData]
    public class ActivityBag
    {
        /// <summary>
        /// Contains the list of available activities in the user's feed.
        /// </summary>
        [FirestoreProperty]
        public List<ActivityBagAvailableItem> available { get; set; }
        /// <summary>
        /// Contains the list of currencty active activities.
        /// </summary>
        [FirestoreProperty]
        public List<ActivityBagQueueItem> queue { get; set; }
    }

    [Serializable]
    [FirestoreData]
    public class ActivityBagAvailableItem
    {
        /// <summary>
        /// The ID of the activity.
        /// </summary>
        [FirestoreProperty]
        public string id { get; set; }
    }

    [Serializable]
    [FirestoreData]
    public class ActivityBagQueueItem
    {
        /// <summary>
        /// The ID of the activity.
        /// </summary>
        [FirestoreProperty]
        public string id { get; set; }
        /// <summary>
        /// Contains activity instance specific data.
        /// </summary>
        [FirestoreProperty]
        public ActivityBagQueueItem_VolatileData vol { get; set;  }
    }

    /// <summary>
    /// Contains activity instance specific data.
    /// </summary>
    [Serializable]
    [FirestoreData]
    public class ActivityBagQueueItem_VolatileData
    {
        /// <summary>
        /// The UNIX timestamp of the activity's expiration (time of completion).
        /// </summary>
        [FirestoreProperty]
        public double end { get; set; }
    }   
}