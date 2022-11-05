using System;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;

public class SecondaryViewSwitch : UIScript
{

    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        GetViewContainer().Q<Button>("SwitchInv").clicked += InventoryManagementOpen;
        GetViewContainer().Q<Button>("SwitchStat").clicked += StatsManagementOpen;
    }

    private void StatsManagementOpen()
    {
        Navigator.SwitchToView("CS_StatsManagement");
        Navigator.ClearUpViewContainer("CMCC_LowerContainer");
    }

    private void InventoryManagementOpen()
    {
        Navigator.SwitchToView("CharacterManagementScreen");
        Navigator.SwitchToView("CS_InventoryEquippedItems");
        Navigator.SwitchToView("CS_InventoryManagement");
    }
}
