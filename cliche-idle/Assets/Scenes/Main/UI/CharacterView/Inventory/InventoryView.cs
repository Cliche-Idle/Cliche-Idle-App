using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum InventoryPopUpMode
{
    Use = 1,
    Sell = 2
}

public class InventoryView : UIScript
{
    private InventoryHandler Inventory;
    private StatsHandler Stats;
    private CurrencyHandler Currencies;
    public VisualTreeAsset InventoryCategoryUXML;
    private VisualElement InventoryCategoriesContainer;
    public InventoryPopUpMode InventoryMode;

    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        Inventory = GameObject.Find("Player").GetComponent<InventoryHandler>();
        Stats = GameObject.Find("Player").GetComponent<StatsHandler>();
        Currencies = GameObject.Find("Player").GetComponent<CurrencyHandler>();
        InventoryCategoriesContainer = GetViewContainer().Q("InventoryCategoriesContainer");
        GetViewContainer().Q<Label>("GoldAmount").text = $"{Currencies.Gold.Value}";
        RenderInventoryContents();
    }

    protected override void OnLeaveFocus(object sender, EventArgs e)
    {

    }

    private void RenderInventoryContents()
    {
        RenderCategoryGroup<ArmourType>();
        RenderCategoryContents<ArmourType>(Inventory.Armour.Items);
        //
        RenderCategoryGroup<WeaponType>();
        RenderCategoryContents<WeaponType>(Inventory.Weapons.Items);
        //
        RenderCategoryGroup<ConsumableType>();
        RenderCategoryContents<ConsumableType>(Inventory.Consumables.Items);
    }

    private void RemoveIcon(string iconReferenceString)
    {
        var deleteIcon = InventoryCategoriesContainer.Q<OverlayIcon>(iconReferenceString);
        if (deleteIcon != null)
        {
            InventoryCategoriesContainer.Remove(deleteIcon);
        }
    }

    private void RenderCategoryGroup<T>() where T : Enum
    {
        foreach (var item in Enum.GetNames(typeof(T)))
        {
            if (item != "Any")
            {
                InventoryCategoryUXML.CloneTree(InventoryCategoriesContainer);
                VisualElement container = InventoryCategoriesContainer.Query("InventoryCategory").Build().Last();
                container.name = $"{item}CategoryContainer";
                ((Label)container.Q("CategoryTag")).text = item;
            }
        }
    }

    private void RenderCategoryContents<T>(IEnumerable<Item> contents) where T : Enum
    {
        foreach (var item in contents)
        {
            var itemSubTypeName = Enum.GetName(typeof(T), item.ItemSubTypeHash);
            string categoryContainerID = $"{itemSubTypeName}CategoryContainer";
            var categoryContainer = InventoryCategoriesContainer.Q(categoryContainerID);
            var categoryCountLabel = categoryContainer.Q<Label>("CategoryCount");
            categoryCountLabel.text = $"{Convert.ToInt32(categoryCountLabel.text)+1}";
            string itemID = $"{item.ID}";
            if (item.VariantID.Length != 0)
            {
                itemID += $"__{item.VariantID}";
            }
            ItemManifest manifest = item.GetManifest();
            OverlayIcon itemIcon = new OverlayIcon(itemID, manifest.Icon, 150, 150)
            { 
                style = {
                    marginLeft = 10,
                    marginRight = 10,
                    marginTop = 10,
                    marginBottom = 10
                } 
            };
            var refValue = 0;
            switch(item.MainStatType)
            {
                case ItemMainStatTypes.Attack:
                    refValue = Stats.Attack;
                    break;
                case ItemMainStatTypes.Defence:
                    refValue = Stats.Defence;
                    break;
            }
            if (refValue <= item.MainStatValue)
            {
                itemIcon.AddOverlay("UpArrow", OverlayAlignment.BottomRight, Resources.Load<Sprite>("OverlayIcons/GreenUpArrow"));
            }
            else
            {
                itemIcon.AddOverlay("DownArrow", OverlayAlignment.BottomRight, Resources.Load<Sprite>("OverlayIcons/RedDownArrow"));
            }
            itemIcon.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                var icon = (OverlayIcon)evt.currentTarget;
                if (icon.reference_item_ID != null)
                {
                    evt.PreventDefault();
                    evt.StopImmediatePropagation();
                    Debug.Log(icon.reference_item_ID);
                    Navigator.SwitchToView("PopUpWindowBase");
                    if (InventoryMode == InventoryPopUpMode.Use)
                    {
                        GameObject.Find("UI_PopUp").GetComponent<UseWindow>().WindowItem = item;
                        Navigator.SwitchToView("CS_UseItemPopUp");
                        
                    }
                    else if (InventoryMode == InventoryPopUpMode.Sell)
                    {
                        //GameObject.Find("UI_PopUp").GetComponent<UseWindow>().WindowItem = item;
                        //Navigator.SwitchToView("CS_UseItemPopUp");
                    }
                }
            });
            categoryContainer.Q("Items").Add(itemIcon);
        }
    }
}