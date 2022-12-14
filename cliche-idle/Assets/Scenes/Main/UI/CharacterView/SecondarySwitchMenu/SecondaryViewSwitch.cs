using System;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;

public class SecondaryViewSwitch : UIScript
{

    protected override void OnEnterFocus()
    {
        GetViewContainer().Q<Button>("SwitchInv").clicked += SwitchToInventoryMenu;
        GetViewContainer().Q<Button>("SwitchStat").clicked += SwitchToStatsMenu;
    }

    private void SwitchToStatsMenu()
    {
        Navigator.ShowView("CS_StatsManagement");
        Navigator.HideView("CS_CharacterSheet");
        Navigator.HideView("CS_InventoryEquippedItems");
        HideView();
        ShowView();
    }

    private void SwitchToInventoryMenu()
    {
        Navigator.HideView("CS_StatsManagement");
        Navigator.ShowView("CS_CharacterSheet");
        Navigator.ShowView("CS_InventoryEquippedItems");
        HideView();
        ShowView();
    }
}
