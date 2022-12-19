using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UIViews;

public class BottomNavigation : UIScript
{
    protected override void OnEnterFocus()
    {
        GetViewContainer().Q<Button>("AdventureBtn").clicked += AdventureMenuOpen;
        GetViewContainer().Q<Button>("InventoryBtn").clicked += InventoryMenuOpen;
        GetViewContainer().Q<Button>("MarketBtn").clicked += TavernMenuOpen;
        GetViewContainer().Q<Button>("OptionsBtn").clicked += SettingsMenuOpen;
    }

    private void AdventureMenuOpen()
    {
        Navigator.ShowView("MainPlayerStatusBar");
        Navigator.ClearContainer("BaseContentContainer");
        Navigator.ShowView("AdventureBoardMenu");
    }

    private void InventoryMenuOpen()
    {
        Navigator.ShowView("MainPlayerStatusBar");
        Navigator.ClearContainer("BaseContentContainer");
        Navigator.ShowView("CS_CharacterSheet"); // CS_InventoryManagement
        Navigator.ShowView("CS_InventoryEquippedItems");
        //Navigator.ShowView("CS_SecondarySwitchMenu");
    }

    private void TavernMenuOpen()
    {
        Navigator.ClearContainer("BaseContentContainer");
        Navigator.ShowView("MarketView");
        Navigator.HideView("MainPlayerStatusBar");
    }

    private void SettingsMenuOpen()
    {
        Navigator.ClearContainer("BaseContentContainer");
        Navigator.ShowView("GameSettingsMenu");
    }
}