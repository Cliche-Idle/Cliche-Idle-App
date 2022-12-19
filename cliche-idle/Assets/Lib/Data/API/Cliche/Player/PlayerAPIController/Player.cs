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

        public static CharacterHandler Character { get; private set; }

        public static StatsHandler Stats { get; private set; }

        public static CurrencyHandler CurrencyBag { get; private set; }

        public static InventoryHandler Inventory { get; private set; }

        public static ProgressionHandler Leveling { get; private set; }
    }
}
