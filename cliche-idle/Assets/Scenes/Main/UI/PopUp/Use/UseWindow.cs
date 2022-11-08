using System;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;

public class UseWindow : PopUp
{
    public Item WindowItem;
    private InventoryHandler Inventory;

    private void Start()
    {
        Inventory = GameObject.Find("Player").GetComponent<InventoryHandler>();
    }

    protected override void OnEnterFocus()
    {
        LoadPopUpContents();
    }

    // FIXME: Consumable title count is not updating
    // Apparently this seems to be working? Altough this script doesn't use the UI update loop

    // TODO: Visibly disable button if consumable runs out

    private void LoadPopUpContents()
    {
        if (WindowItem != null)
        {
            ItemManifest manifest = WindowItem.GetManifest();
            var nameString = manifest.Name;
            if (WindowItem.ItemType == ItemTypes.Consumable)
            {
                nameString += $"\n{((Consumable)WindowItem).Quantity}x";
                GetViewContainer().Q<Button>("UseBtn").text = "USE";
            }
            GetViewContainer().Q<VisualElement>("ItemIcon").style.backgroundImage = manifest.Icon.texture;
            GetViewContainer().Q<Label>("ItemName").text = nameString;
            GetViewContainer().Q<Label>("ItemDesc").text = manifest.Description;
            GetViewContainer().Q<Label>("ItemNum").text = $"{WindowItem.MainStatValue}";
            var statIcon = GetViewContainer().Q<VisualElement>("StatIcon");
            switch(WindowItem.MainStatType)
            {
                case ItemMainStatTypes.Attack:
                    statIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/attack").texture;
                    break;
                case ItemMainStatTypes.Defence:
                    statIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/defence").texture;
                    break;
                case ItemMainStatTypes.Heal:
                    statIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/heal").texture;
                    break;
            }
            GetViewContainer().Q<Label>("ItemCoin").text = $"{manifest.Price}";
            GetViewContainer().Q<Button>("UseBtn").clicked += UseItem;
        }
    }

    private void UseItem()
    {
        if (WindowItem.ItemType == ItemTypes.Armour)
        {
            Inventory.Armour.Equip(WindowItem.ID, WindowItem.VariantID);
        }
        else if (WindowItem.ItemType == ItemTypes.Weapon)
        {
            Inventory.Weapons.Equip(WindowItem.ID, WindowItem.VariantID);
        }
        else if (WindowItem.ItemType == ItemTypes.Consumable)
        {
            ((Consumable)WindowItem).Use();
            ItemManifest manifest = WindowItem.GetManifest();
            GetViewContainer().Q<Label>("ItemName").text = $"{manifest.Name}\n{((Consumable)WindowItem).Quantity}x";
            if (((Consumable)WindowItem).Quantity == 0)
            {
                Inventory.Consumables.RemoveStack(((Consumable)WindowItem));
            }
        }
    }
}