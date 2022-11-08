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
        Navigator.HideView("CS_InventoryManagement");
    }

    private void SwitchToInventoryMenu()
    {
        Navigator.ShowView("CS_InventoryEquippedItems");
        Navigator.ShowView("CS_InventoryManagement");
    }
}
