using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Cliche.Activities;
using Cliche.System;

public class MockInventoryManager : MonoBehaviour
{
    public GameObject Player;
    public UIDocument docu;
    public VisualElement m_Root;
    public int iconCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get Player instance
        Player = GameObject.Find("Player");
        //

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

        docu = GetComponent<UIDocument>();
        m_Root = docu.rootVisualElement.Q<VisualElement>("Container");
        for (int i = 0; i < iconCount; i++)
        {
            OverlayIcon testIcon = new OverlayIcon("testItem", Resources.Load<Sprite>("icons/placeholder_250x250_cross"), 100, 100);

            testIcon.AddOverlay("testLayerTransparent-Center", OverlayAlignment.Center, Resources.Load<Sprite>("icons/placeholder_overlay_75_opacity_50x50"));
            testIcon.AddOverlay("testLayerTransparent-TopLeft", OverlayAlignment.TopLeft, Resources.Load<Sprite>("icons/placeholder_overlay_75_opacity_50x50"));
            testIcon.AddOverlay("testLayerOpaque-TopRight", OverlayAlignment.TopRight, Resources.Load<Sprite>("icons/placeholder_overlay_50x50"));
            testIcon.AddOverlay("testLayerOpaque-BottomLeft", OverlayAlignment.BottomLeft, Resources.Load<Sprite>("icons/placeholder_overlay_50x50"));
            testIcon.AddOverlay("testLayerTransparent-BottomRight", OverlayAlignment.BottomRight, Resources.Load<Sprite>("icons/placeholder_overlay_75_opacity_50x50"));

            m_Root.Add(testIcon);
        }

    }
}
