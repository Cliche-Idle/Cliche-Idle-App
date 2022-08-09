using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Cliche.Activities;
using Cliche.System;

public class DirtyDebugTestScript : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        //Authentication.OnLogin += AuthTestDelegate;
        //Authentication.AttemptSessionRestoreAfterRestart();

        var statsBag = Player.GetComponent<StatsHandler>();
        Debug.Log($"Free stat points: {statsBag.GetFreeStatPoints()}");

        var adventureHandler = Player.GetComponent<AdventureHandler>();
        adventureHandler.RefillAvailableList();

        adventureHandler.StartAdventure("basicTimed_Dev1_short");

        /*
        var inventoryBag = Player.GetComponent<InventoryHandler>();
        Weapon testWeapon = new Weapon("advStartBlade", 12);
        inventoryBag.Weapons.Add(testWeapon);

        Debug.Log("Attack before equip: " + statsBag.Attack);
        testWeapon = (inventoryBag.Weapons.Items)[0];
        Debug.Log("Weapon: " + testWeapon.ID + "_" + testWeapon.VariantID);
        inventoryBag.Weapons.Equip(testWeapon);
        Debug.Log("Attack after equip: " + statsBag.Attack);

        inventoryBag.Weapons.Remove(testWeapon);
        */
    }
}
