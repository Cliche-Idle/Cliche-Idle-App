using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class BottomNavigation : UIScript
{

    // Start is called before the first frame update
    void Start()
    {
        DisplayView();
    }

    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        GetViewContainer().Q<Button>("AdventureBtn").clicked += AdventureMenuOpen;
        GetViewContainer().Q<Button>("InventoryBtn").clicked += InventoryMenuOpen;
        GetViewContainer().Q<Button>("TavernBtn").clicked += TavernMenuOpen;
        GetViewContainer().Q<Button>("OptionsBtn").clicked += SettingsMenuOpen;
    }

    private void AdventureMenuOpen()
    {
        Navigator.SwitchToView("AdventureBoard");
    }

    private void InventoryMenuOpen()
    {
        Navigator.SwitchToView("CS_SecondarySwitchMenu");
        Navigator.SwitchToView("CS_InventoryEquippedItems");
        Navigator.SwitchToView("CS_InventoryManagement");
    }

    private void TavernMenuOpen()
    {
        Navigator.SwitchToView("Tavern");
    }

    private void SettingsMenuOpen()
    {
        Navigator.SwitchToView("GameSettingsMenu");
    }
}