using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;
using Cliche.UIElements;

public class InventoryView : UIScript
{
    private InventoryHandler Inventory;
    private StatsHandler Stats;
    private CurrencyHandler Currencies;
    public VisualTreeAsset InventoryCategoryUXML;
    private VisualElement InventoryCategoriesContainer;
    private EquipmentSocket _equipmentSocket;

    public ItemTypes InventoryType = ItemTypes.Weapon;
    public int SubTypeHash = -1;

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.height = Length.Percent(100f);
        Inventory = GameObject.Find("Player").GetComponent<InventoryHandler>();
        Stats = GameObject.Find("Player").GetComponent<StatsHandler>();
        Currencies = GameObject.Find("Player").GetComponent<CurrencyHandler>();
        InventoryCategoriesContainer = GetViewContainer().Q("InventoryCategoriesContainer");
        _equipmentSocket = GetViewContainer().Q<EquipmentSocket>();
        //GetViewContainer().Q<Label>("GoldAmount").text = $"{Currencies.Gold.Value}";
        RenderInventoryContents();
    }

    protected override void OnLeaveFocus()
    {
        switch (InventoryType)
        {
            case ItemTypes.Weapon:
                var wepSocket = Inventory.Weapons.Sockets.Find(socket => socket.AcceptSubTypeHash == SubTypeHash);
                wepSocket.OnEquip -= UpdateIconIndicators;
                break;
            case ItemTypes.Armour:
                var armSocket = Inventory.Armour.Sockets.Find(socket => socket.AcceptSubTypeHash == SubTypeHash);
                armSocket.OnEquip -= UpdateIconIndicators;
                break;
        }
    }

    private void RenderInventoryContents()
    {
        switch(InventoryType)
        {
            case ItemTypes.Weapon:
                RenderDisplayCategories<WeaponType>();
                RenderCategoryContents<WeaponType>(Inventory.Weapons.Items);
                var wepSocket = Inventory.Weapons.Sockets.Find(socket => socket.AcceptSubTypeHash == SubTypeHash);
                _equipmentSocket.BindToSocket(wepSocket);
                wepSocket.OnEquip += UpdateIconIndicators;
                UpdateIconIndicators(null, wepSocket.EquippedItem);
                break;
            case ItemTypes.Armour:
                RenderDisplayCategories<ArmourType>();
                RenderCategoryContents<ArmourType>(Inventory.Armour.Items);
                var armSocket = Inventory.Armour.Sockets.Find(socket => socket.AcceptSubTypeHash == SubTypeHash);
                _equipmentSocket.BindToSocket(armSocket);
                armSocket.OnEquip += UpdateIconIndicators;
                UpdateIconIndicators(null, armSocket.EquippedItem);
                break;
            case ItemTypes.Consumable:
                RenderDisplayCategories<ConsumableType>();
                RenderCategoryContents<ConsumableType>(Inventory.Consumables.Items);
                break;
        }        
    }

    private void RemoveIcon(string iconReferenceString)
    {
        var deleteIcon = InventoryCategoriesContainer.Q<ItemDisplay>(iconReferenceString);
        if (deleteIcon != null)
        {
            InventoryCategoriesContainer.Remove(deleteIcon);
        }
    }

    private void RenderDisplayCategories<T>() where T : Enum
    {
        if (SubTypeHash != -1)
        {
            RenderCategory(Enum.GetName(typeof(T), SubTypeHash));
        }
        else
        {
            foreach (var categoryName in Enum.GetNames(typeof(T)))
            {
                if (categoryName != "Any")
                {
                    RenderCategory(categoryName);
                }
            }
        }
    }

    private void RenderCategory(string categoryName)
    {
        InventoryCategoryUXML.CloneTree(InventoryCategoriesContainer);
        VisualElement container = InventoryCategoriesContainer.Query("InventoryCategory").Build().Last();
        container.name = $"{categoryName}CategoryContainer";
        container.Q<Label>("CategoryTag").text = categoryName;
    }

    private void UpdateIconIndicators(object sender, Item item)
    {
        var displayItems = InventoryCategoriesContainer.Query<ItemDisplay>().Build();
        foreach (var displayItem in displayItems)
        {
            var currentlyEquippedItemStatValue = 0;
            if (item != null)
            {
                currentlyEquippedItemStatValue = item.MainStatValue;
            }
            var overlay = displayItem.Icon.GetOverlay("StatDiffIndicator");
            if (currentlyEquippedItemStatValue < displayItem.DisplayItem.MainStatValue)
            {
                overlay.style.backgroundImage = Resources.Load<Sprite>("OverlayIcons/GreenUpArrow").texture;
            }
            else
            {
                overlay.style.backgroundImage = Resources.Load<Sprite>("OverlayIcons/RedDownArrow").texture;
            }
        }
    }

    private void RenderCategoryContents<T>(IEnumerable<Item> contents) where T : Enum
    {
        foreach (var item in contents)
        {
            if (item.ItemSubTypeHash == SubTypeHash || SubTypeHash == -1)
            {
                var itemSubTypeName = Enum.GetName(typeof(T), item.ItemSubTypeHash);
                string categoryContainerID = $"{itemSubTypeName}CategoryContainer";
                var categoryContainer = InventoryCategoriesContainer.Q(categoryContainerID);
                var categoryCountLabel = categoryContainer.Q<Label>("CategoryCount");
                categoryCountLabel.text = $"{Convert.ToInt32(categoryCountLabel.text) + 1}";
                string itemID = $"{item.ID}";

                if (item.IsInstanceItem)
                {
                    itemID += $"__{item.VariantID}";
                }

                ItemDisplay itemDisplay = new ItemDisplay(item)
                {
                    name = itemID,
                    style = {
                    width = Length.Percent(100f),
                    backgroundColor = (Color)new Color32(41, 39, 33, 255),
                    height = 150,
                    marginTop = 10,
                    marginBottom = 10
                }
                };
                itemDisplay.Icon.AddOverlay("StatDiffIndicator", OverlayAlignment.BottomRight, Resources.Load<Sprite>("OverlayIcons/GreenUpArrow"));
                itemDisplay.RegisterCallback<ClickEvent>(OpenItemDetailsPopup);
                categoryContainer.Q("Items").Add(itemDisplay);
            }
        }
    }

    private void OpenItemDetailsPopup(ClickEvent evt)
    {
        evt.PreventDefault();
        evt.StopImmediatePropagation();
        var itemDisplay = (ItemDisplay)evt.currentTarget;
        GameObject.Find("UI_PopUp").GetComponent<UseWindow>().WindowItem = itemDisplay.DisplayItem;
        Navigator.ShowView("UseItemPopUp");
    }
}