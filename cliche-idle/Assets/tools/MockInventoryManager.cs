using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Cliche.UserManagement;
using Cliche.GameModes;
using Cliche.System;
using Firewrap;

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
        
        // Load player data from local save
        Player.GetComponent<SaveManager>().LoadUserState();
        //
        /*
        var currencyBag = Player.GetComponent<CurrencyHandler>();
        Debug.Log("Gold : " + currencyBag.Gold.Value);

        var statsBag = Player.GetComponent<StatsHandler>();
        statsBag.Intelligence.Take(3);
        Debug.Log("Attack before equip: " + statsBag.Attack);

        var inventoryBag = Player.GetComponent<InventoryHandler>();
        Weapon testWeapon = new Weapon("advStartBlade", 12);
        inventoryBag.Weapons.Add(testWeapon);
        testWeapon = new Weapon("advStartBlade", 15);
        inventoryBag.Weapons.Add(testWeapon);
        testWeapon = new Weapon("advStartBlade", 20);
        inventoryBag.Weapons.Add(testWeapon);

        testWeapon = ((List<Weapon>)inventoryBag.Weapons.Items)[0];
        Debug.Log("Weapon: " + testWeapon.ID + "_" + testWeapon.VariantID);
        inventoryBag.Weapons.Equip(testWeapon);
        Debug.Log("Attack after equip: " + statsBag.Attack);


        Player.GetComponent<SaveManager>().SaveUserState();
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

    public async void AuthTestDelegate(object sender, AuthEventArgs e) {
        if (e.User != null)
        {
            User testUser = await Users.GetUserData();
            Debug.Log(testUser.character.general.name);
            Debug.Log(testUser.character.general.Level);
            // * Adventure test
            Adventure testAdventure = Adventures.GetByID("basicTimed_Dev1_short");
            Debug.Log($"ID: {testAdventure.ID} : {testAdventure.Title}");
            List<Adventure> adventures = Adventures.GetNewAdventureList(3, testUser);
            foreach (var item in adventures)
            {
                Debug.Log($"Adventure list item: {item.ID}");
            }
            Debug.Log(Manifests.GetByID<IntervalValueModifier>("Vitality").ID);
            //
        }
        else
        {
            Debug.LogError(e.UserFriendlyMessage);
        }
    }
}
