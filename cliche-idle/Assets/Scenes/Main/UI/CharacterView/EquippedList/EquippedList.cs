using System.Collections.Generic;
using UnityEngine.UIElements;
using UIViews;
using Cliche.UIElements;

public class EquippedList : UIScript
{
    private VisualElement SocketsContainer;

    public InventoryHandler Inventory;
    public StatsHandler Stats;
    public InventoryView InventoryView;

    protected override void OnEnterFocus()
    {
        SocketsContainer = GetViewContainer().Q("EquippedItemsView");   
        DrawSocketEquipmentList();
    }

    private void DrawSocketEquipmentList()
    {
        ListSockets(Inventory.Weapons.Sockets);
        ListSockets(Inventory.Armour.Sockets);
    }

    private void ListSockets<T>(IEnumerable<GearSocket<T>> sockets) where T : Item
    {
        foreach (var socket in sockets)
        {
            var equipmentSocket = new EquipmentSocket()
            {
                name = socket.ID
            };
            equipmentSocket.BindToSocket(socket);
            equipmentSocket.OnClick += (evt) =>
            {
                InventoryView.InventoryType = socket.AcceptItemType;
                InventoryView.SubTypeHash = socket.AcceptSubTypeHash;

                Navigator.ClearContainer(ContainerID);
                InventoryView.ShowView();
            };
            SocketsContainer.Add(equipmentSocket);
        }
    }
}