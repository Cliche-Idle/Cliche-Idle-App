using System;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;

public class SecondaryViewSwitch : UIScript
{

    protected override void OnEnterFocus()
    {
        GetViewContainer().Q<Button>("SwitchInv").clicked += InventoryManagementOpen;
        GetViewContainer().Q<Button>("SwitchStat").clicked += StatsManagementOpen;
    }

    private void StatsManagementOpen()
    {
        Navigator.ShowView("CS_StatsManagement");
        Navigator.ClearContainer("CMCC_LowerContainer");
    }

    private void InventoryManagementOpen()
    {
        Navigator.ShowView("CharacterManagementScreen");
        Navigator.ShowView("CS_InventoryEquippedItems");
        Navigator.ShowView("CS_InventoryManagement");
    }
}
