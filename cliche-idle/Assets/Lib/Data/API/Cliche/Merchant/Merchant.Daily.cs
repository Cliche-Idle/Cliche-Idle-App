using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cliche.System;
using System.Linq;

public partial class Merchant : MonoBehaviour
{
    public class DailyOfferings
    {
        /// <summary>
        /// Gets a seed that changes daily.
        /// </summary>
        private static int _dailySeed
        {
            get
            {
                var currDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0, 0, DateTimeKind.Utc);
                var epoch = DateTime.UnixEpoch;
                return (int)(currDate - epoch).TotalDays;
            }
        }

        /// <summary>
        /// Gets a seed that changes weekly.
        /// </summary>
        private static int _weeklySeed
        {
            get
            {
                // TODO: Day calculation apparently refreshes wrong, as sunday is 0
                var currDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, (DateTime.UtcNow.Day - (int)DateTime.UtcNow.DayOfWeek), 0, 0, 0, 0, DateTimeKind.Utc);
                var epoch = DateTime.UnixEpoch;
                return (int)(currDate - epoch).TotalDays;
            }
        }

        /// <summary>
        /// Gets the list of items on offer for the current day.
        /// </summary>
        /// <returns></returns>
        public static List<Consumable> GetDailyOfferings()
        {
            return GetOfferingsList(_dailySeed);
        }

        /// <summary>
        /// Gets a random list of items on offer for the given seed.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        private static List<Consumable> GetOfferingsList(int seed)
        {
            System.Random random = new System.Random(seed);
            // Load in store manifest
            var storeData = Manifests.GetByID<MerchantStore>("merchantInventory");

            var itemList = storeData.Consumables.Select(item => item.ID).ToList();

            var offeringsList = new List<Consumable>();

            for (int i = 0; (i < storeData.MaxAvailableItems || i < itemList.Count); i++)
            {
                var itemIndex = random.Next(0, itemList.Count);
                var itemID = itemList[itemIndex];
                offeringsList.Add(new Consumable(itemID, 1));
                itemList.RemoveAt(itemIndex);
            }

            return offeringsList;
        }
    }
}
