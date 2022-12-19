using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cliche.Idle
{
    public static class Player
    {
        public static void Init()
        {
            var playerGameObject = GameObject.Find("Player");
            Character = playerGameObject.GetComponent<CharacterHandler>();
            Stats = playerGameObject.GetComponent<StatsHandler>();
            CurrencyBag = playerGameObject.GetComponent<CurrencyHandler>();
            Inventory = playerGameObject.GetComponent<InventoryHandler>();
            Leveling = playerGameObject.GetComponent<ProgressionHandler>();
        }

        /// <summary>
        /// Contains the basic character data, such as the <see cref="CharacterSheet"/>.
        /// </summary>
        public static CharacterHandler Character { get; private set; }

        /// <summary>
        /// Contains all of the player stats and tools to manage them.
        /// </summary>
        public static StatsHandler Stats { get; private set; }

        /// <summary>
        /// Contains the list of all currencies.
        /// </summary>
        public static CurrencyHandler CurrencyBag { get; private set; }

        /// <summary>
        /// Handles the player's inventory.
        /// </summary>
        public static InventoryHandler Inventory { get; private set; }

        /// <summary>
        /// Handles player progression and tools to manage it.
        /// </summary>
        public static ProgressionHandler Leveling { get; private set; }
    }
}
