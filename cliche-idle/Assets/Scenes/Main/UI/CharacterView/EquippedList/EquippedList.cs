using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UIViews;

public class EquippedList : UIScript
{
    private InventoryHandler Inventory;
    private StatsHandler Stats;
    public VisualTreeAsset EquipmentSocketUXML;
    private VisualElement SocketsContainer;

    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        Inventory = GameObject.Find("Player").GetComponent<InventoryHandler>();
        Stats = GameObject.Find("Player").GetComponent<StatsHandler>();
        SocketsContainer = GetViewContainer().Q("SocketsContainer");
        DrawSocketEquipmentList(null, null);

    }

    protected override void OnLeaveFocus(object sender, EventArgs e)
    {
        UnSubscribeFromSocketEvents<Weapon>(Inventory.Weapons.Sockets);
        UnSubscribeFromSocketEvents<Armour>(Inventory.Armour.Sockets);
    }

    private void DrawSocketEquipmentList(object sender, Item item)
    {
        SocketsContainer.Clear();
        Navigator.ClearUpViewContainer("CMCC_LowerContainer");
        Navigator.SwitchToView("CS_InventoryManagement");
        ListSockets<Weapon>(Inventory.Weapons.Sockets);
        // FIXME: Don't subscribe to event here, do it in OnEnterFocus, as this will loop subscribe
        SubscribeToSocketEvents<Weapon>(Inventory.Weapons.Sockets);
        ListSockets<Armour>(Inventory.Armour.Sockets);
        SubscribeToSocketEvents<Armour>(Inventory.Armour.Sockets);
        GetViewContainer().Q<Label>("AtkNum").text = $"{Stats.Attack}";
        GetViewContainer().Q<Label>("DefNum").text = $"{Stats.Defence}";
    }

    private void SubscribeToSocketEvents<T>(IEnumerable<GearSocket<T>> sockets) where T : Item
    {
        foreach (var socket in sockets)
        {
            socket.OnEquip += DrawSocketEquipmentList;
        }
    }

    private void UnSubscribeFromSocketEvents<T>(IEnumerable<GearSocket<T>> sockets) where T : Item
    {
        foreach (var socket in sockets)
        {
            socket.OnEquip -= DrawSocketEquipmentList;
        }
    }

    private void ListSockets<T>(IEnumerable<GearSocket<T>> sockets) where T : Item
    {
        foreach (var socket in sockets)
        {
            EquipmentSocketUXML.CloneTree(SocketsContainer);
            VisualElement container = SocketsContainer.Query("Socket").Build().Last();
            container.name = $"{socket}Socket";
            var socketStatIcon = container.Q<VisualElement>("StatIcon");
            // Set Icons
            switch(socket.AcceptItemType)
            {
                case ItemTypes.Weapon:
                    socketStatIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/attack").texture;
                    break;
                case ItemTypes.Armour:
                    socketStatIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/defence").texture;
                    break;
            }
            if (socket.EquippedItem != null && socket.EquippedItem.ID != null)
            {
                container.Q<Label>("StatValue").text = socket.EquippedItem.MainStatValue.ToString();
                var manifest = socket.EquippedItem.GetManifest();
                container.Q<Label>("ItemName").text = manifest.Name;
                container.Q<VisualElement>("ItemIcon").style.backgroundImage = manifest.Icon.texture;
            }
        }
    }
}