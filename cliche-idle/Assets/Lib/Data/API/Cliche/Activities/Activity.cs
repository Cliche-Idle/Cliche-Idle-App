using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cliche.Activities;

namespace Assets.Lib.Data.API.Cliche.Activities
{
    public abstract class Activity : MonoBehaviour
    {
        /// <summary>
        /// The total amount of damage taken during this activity.
        /// </summary>
        protected int _damageTaken { get; set; } = 0;

        /// <summary>
        /// The total amount of damage dealt during this activity.
        /// </summary>
        protected int _damageDealt { get; set; } = 0;

        /// <summary>
        /// The amount of Experience received.
        /// </summary>
        protected int _experienceGained { get; set; } = 0;

        /// <summary>
        /// The amount of Gold received.
        /// </summary>
        protected int _goldGained { get; set; } = 0;

        /// <summary>
        /// The list of items received. Only lists the item manifest IDs.
        /// </summary>
        protected Dictionary<ItemTypes, List<string>> _itemsReceived { get; set; }

        public abstract void StartActivity();

        public abstract PostActivityReport CompleteActivity();

        protected abstract void GrantActivityRewards();
    }
}