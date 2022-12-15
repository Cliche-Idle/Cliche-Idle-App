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
        var deleteIcon = InventoryCategoriesContainer.Q<OverlayIcon>(iconReferenceString);
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
        var itemIcons = InventoryCategoriesContainer.Query<OverlayIcon>().Build();
        foreach (var itemIcon in itemIcons)
        {
            var referenceStatValue = 0;
            if (item != null)
            {
                referenceStatValue = item.MainStatValue;
            }
            Item refItem = null;
            var ids = itemIcon.name.Split("__");
            switch (InventoryType)
            {
                case ItemTypes.Weapon:
                    refItem = Inventory.Weapons.Items.Find(_item => _item.ID == ids[0] && _item.VariantID == ids[1]);
                    break;
                case ItemTypes.Armour:
                    refItem = Inventory.Armour.Items.Find(_item => _item.ID == ids[0] && _item.VariantID == ids[1]);
                    break;
            }
            var overlay = itemIcon.GetOverlay("StatDiffIndicator");
            if (referenceStatValue < refItem.MainStatValue)
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
                ItemManifest manifest = item.GetManifest();
                OverlayIcon itemIcon = new OverlayIcon(manifest.Icon)
                {
                    name = itemID,
                    ReferenceID = item.ID,
                    style = {
                    width = 150,
                    height = 150,
                    marginLeft = 10,
                    marginRight = 10,
                    marginTop = 10,
                    marginBottom = 10
                }
                };
                itemIcon.AddOverlay("StatDiffIndicator", OverlayAlignment.BottomRight, Resources.Load<Sprite>("OverlayIcons/GreenUpArrow"));
                itemIcon.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                    var icon = (OverlayIcon)evt.currentTarget;
                    if (icon.ReferenceID != null)
                    {
                        evt.PreventDefault();
                        evt.StopImmediatePropagation();
                        GameObject.Find("UI_PopUp").GetComponent<UseWindow>().WindowItem = item;
                        Navigator.ShowView("UseItemPopUp");
                    }
                });
                categoryContainer.Q("Items").Add(itemIcon);
            }
        }
    }
}