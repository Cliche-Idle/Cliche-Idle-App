using System;
using System.Collections.Generic;
using UnityEngine;
using Cliche.UserManagement;

namespace Cliche.GameModes
{
    public class Adventures
    {
        public static readonly string ACTIVITY_ADVENTURE_ASSET_COLLECTION_PATH = "Activities/Adventures";

        /// <summary>
        /// Gets the given Adventure's base data.
        /// </summary>
        /// <returns></returns>
        public static Adventure GetByID(string activityID)
        {
            Adventure queryAdventure = Resources.Load<Adventure>($"{ACTIVITY_ADVENTURE_ASSET_COLLECTION_PATH}/{activityID}");
            #if UNITY_EDITOR
            if (queryAdventure == null)
            {
                Debug.LogError($"<color=red>Adventure data at {ACTIVITY_ADVENTURE_ASSET_COLLECTION_PATH}/{activityID} could not be found.<color=red>");
            }
            #endif
            return queryAdventure;
        }

        /// <summary>
        /// Gets a list of adventures that meet the specified requirements.
        /// </summary>
        /// <param name="adventureCount">The number of adventures to return. If there aren't enough adventures meeting the given criteria, it can return a lower count.</param>
        /// <param name="user">The user object, used to check if they fulfill the activities requirements.</param>
        /// <returns></returns>
        public static List<Adventure> GetNewAdventureList(int adventureCount, User user)
        {
            Adventure[] allAdventures = Resources.LoadAll<Adventure>($"{ACTIVITY_ADVENTURE_ASSET_COLLECTION_PATH}");
            List<Adventure> availableAdventures = new List<Adventure>();
            foreach (var item in allAdventures)
            {
                if (item.Requirements == null || item.Requirements.IsFulfilled(user))
                {
                    availableAdventures.Add(item);
                }
            }
            List<Adventure> adventureList = new List<Adventure>();
            while (true)
            {
                var adventureIndex = UnityEngine.Random.Range(0, availableAdventures.Count-1);
                if (adventureList.Contains(availableAdventures[adventureIndex]) == false)
                {
                    adventureList.Add(availableAdventures[adventureIndex]);
                    availableAdventures.RemoveAt(adventureIndex);
                }
                if (adventureList.Count == adventureCount || availableAdventures.Count == 0)
                {
                    break;
                }
            }
            return adventureList;
        }
    }
}