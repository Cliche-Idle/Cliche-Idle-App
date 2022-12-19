using System;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;
using Cliche.Idle;

public class PurchasePopup : PopUp
{
    public Item WindowItem;

    // TODO: rework this entire popup

    protected override void OnEnterFocus()
    {
        LoadPopUpContents();
        Player.CurrencyBag.Gold.OnValueChange += OnGoldChange;
        OnGoldChange(Player.CurrencyBag.Gold.Value);
    }

    protected override void OnLeaveFocus()
    {
        Player.CurrencyBag.Gold.OnValueChange -= OnGoldChange;
    }

    private void LoadPopUpContents()
    {
        if (WindowItem != null)
        {
            ItemManifest manifest = WindowItem.GetManifest();
            var nameString = manifest.Name;
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
            GetViewContainer().Q<Label>("PlayerCoin").text = $"{Player.CurrencyBag.Gold.Value}";
            //ItemCount
            var invItem = Player.Inventory.Consumables.Items.Find(item => item.ID == WindowItem.ID);
            var invItemCount = 0;
            if (invItem != null)
            {
                invItemCount = invItem.Quantity;
            }
            GetViewContainer().Q<Label>("ItemCount").text = $"{invItemCount}";
            GetViewContainer().Q<Button>("UseBtn").clicked += UseItem;
        }
    }

    private void OnGoldChange(int val)
    {
        var manifest = WindowItem.GetManifest();
        GetViewContainer().Q<Label>("PlayerCoin").text = $"{Player.CurrencyBag.Gold.Value}";
        if (manifest.Price > Player.CurrencyBag.Gold.Value)
        {
            GetViewContainer().Q<Button>("UseBtn").SetEnabled(false);
            GetViewContainer().Q<Label>("PlayerCoin").style.color = Color.red;
        }
    }

    private void UseItem()
    {
        Merchant.PurchaseItem((Consumable)WindowItem);
        GetViewContainer().Q<Label>("ItemCount").text = $"{Convert.ToInt32(GetViewContainer().Q<Label>("ItemCount").text) + 1}";
    }
}