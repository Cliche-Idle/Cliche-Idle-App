using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cliche.UIElements;
using UIViews;
using Cliche.Idle;

public class MarketView : UIScript
{
    private Label _itemInstructions;
    private readonly string _buyIstructions = "Tap on an item to see its details.";
    private readonly string _sellIstructions = "Tap on an item to add or remove it from the sell basket.";
    private MarketMode _mode = MarketMode.buy;

    private ScrollView _itemContainer;
    private VisualElement _transactionWindow;
    private Label _transactionValue;
    private Button _completeTransaction;


    private Button _buyButton;
    private Button _sellButton;

    private Label _goldCounter;
    private ScrollView _inventoryScroll;

    public VisualTreeAsset InventoryCategoryUXML;
    public PurchasePopup popup;

    private enum MarketMode
    {
        buy,
        sell,
    }

    protected override void OnEnterFocus()
    {
        var context = GetViewContainer();
        context.style.height = Length.Percent(100f);

        _mode = MarketMode.buy;
        _itemInstructions = context.Q<Label>("ContainerInstructions");

        _itemContainer = context.Q<ScrollView>("SellBasketScroll");

        _transactionWindow = context.Q<VisualElement>("TransactionWindow");
        _transactionValue = context.Q<Label>("TransactionTotal");
        _completeTransaction = context.Q<Button>("CompleteTransaction");
        _completeTransaction.clicked += _completeTransaction_onClick;

        _buyButton = context.Q<Button>("ToggleBuy");
        _buyButton.clicked += () =>
        {
            _mode = MarketMode.buy;
            UpdateModeUI();
        };
        _sellButton = context.Q<Button>("ToggleSell");
        _sellButton.clicked += () =>
        {
            _mode = MarketMode.sell;
            UpdateModeUI();
        };

        _goldCounter = context.Q<Label>("GoldAmount");
        Player.CurrencyBag.Gold.OnValueChange += UpdateGoldCounter;
        UpdateGoldCounter(Player.CurrencyBag.Gold.Value);

        _inventoryScroll = context.Q<ScrollView>("InventoryScroll");

        UpdateModeUI();
    }

    private void _completeTransaction_onClick()
    {
        var sellItems = _itemContainer.Children();
        foreach (var item in sellItems)
        {
            var sellItem = ((ItemDisplay)item).DisplayItem;
            if (sellItem.ItemType == ItemTypes.Consumable)
            {
                // Only sell x amount
                Merchant.SellItem(sellItem);
            }
            else
            {
                Merchant.SellItem(sellItem);
            }
        }
        _itemContainer.Clear();
        _transactionValue.text = "0";
    }

    protected override void OnLeaveFocus()
    {
        Player.CurrencyBag.Gold.OnValueChange -= UpdateGoldCounter;
    }

    private void UpdateGoldCounter(int val)
    {
        _goldCounter.text = val.ToString();
    }

    private void UpdateModeUI()
    {
        _itemContainer.Clear();
        RenderInventoryContents();
        if (_mode == MarketMode.buy)
        {
            _buyButton.SetEnabled(false);
            _sellButton.SetEnabled(true);
            //
            _itemInstructions.text = _buyIstructions;
            _transactionWindow.style.display = DisplayStyle.None;
            _inventoryScroll.SetEnabled(false);
            //
            var marketOfferings = Merchant.DailyOfferings.GetDailyOfferings();
            foreach (var offer in marketOfferings)
            {
                var itemDisplay = new ItemDisplay(offer)
                {
                    name = offer.ID,
                    style = {
                        width = Length.Percent(100f),
                        backgroundColor = (Color)new Color32(41, 39, 33, 255),
                        height = 150,
                        marginTop = 10,
                        marginBottom = 10
                    }
                };
                itemDisplay.RegisterCallback<ClickEvent>(evt => {
                    evt.PreventDefault();
                    evt.StopImmediatePropagation();
                    var item = ((ItemDisplay)evt.currentTarget).DisplayItem;
                    popup.WindowItem = item;
                    popup.ShowView();
                });
                _itemContainer.Add(itemDisplay);
            }
            
        }
        else
        {
            _buyButton.SetEnabled(true);
            _sellButton.SetEnabled(false);
            //
            _transactionValue.text = "0";
            _itemInstructions.text = _sellIstructions;
            _transactionWindow.style.display = DisplayStyle.Flex;
            _inventoryScroll.SetEnabled(true);
            //
            var inventoryItems = _inventoryScroll.contentContainer.Query<ItemDisplay>().Build().ToList();
            foreach (var inventoryItem in inventoryItems)
            {
                inventoryItem.RegisterCallback<ClickEvent>(MoveItemToSellBasket);
            }
            //
        }
    }

    private void MoveItemToSellBasket(ClickEvent evt)
    {
        evt.PreventDefault();
        evt.StopImmediatePropagation();
        var itemDisplay = ((ItemDisplay)evt.currentTarget);
        itemDisplay.IconOnly = true;
        itemDisplay.style.width = 150;
        itemDisplay.RemoveFromHierarchy();
        _itemContainer.Add(itemDisplay);
        //
        var manifest = itemDisplay.DisplayItem.GetManifest();
        int transSum = System.Convert.ToInt32(_transactionValue.text);
        _transactionValue.text = $"{transSum + manifest.Price}";
        //
        itemDisplay.UnregisterCallback<ClickEvent>(MoveItemToSellBasket);
        itemDisplay.RegisterCallback<ClickEvent>(MoveItemFromSellBasket);
    }

    private void MoveItemFromSellBasket(ClickEvent evt)
    {
        evt.PreventDefault();
        evt.StopImmediatePropagation();
        var itemDisplay = ((ItemDisplay)evt.currentTarget);
        itemDisplay.IconOnly = false;
        itemDisplay.style.width = Length.Percent(100f);
        itemDisplay.RemoveFromHierarchy();
        // Move it back to the correct category
        var itemSubTypeName = "";
        switch (itemDisplay.DisplayItem.ItemType)
        {
            case ItemTypes.Weapon:
                itemSubTypeName = Enum.GetName(typeof(WeaponType), itemDisplay.DisplayItem.ItemSubTypeHash);
                break;
            case ItemTypes.Armour:
                itemSubTypeName = Enum.GetName(typeof(ArmourType), itemDisplay.DisplayItem.ItemSubTypeHash);
                break;
            case ItemTypes.Consumable:
                itemSubTypeName = Enum.GetName(typeof(ConsumableType), itemDisplay.DisplayItem.ItemSubTypeHash);
                break;
        }
        string categoryContainerID = $"{itemSubTypeName}CategoryContainer";
        var categoryContainer = _inventoryScroll.Q(categoryContainerID);
        var categoryCountLabel = categoryContainer.Q<Label>("CategoryCount");
        categoryContainer.Add(itemDisplay);
        //
        //
        var manifest = itemDisplay.DisplayItem.GetManifest();
        int transSum = System.Convert.ToInt32(_transactionValue.text);
        _transactionValue.text = $"{transSum - manifest.Price}";
        //
        //
        itemDisplay.UnregisterCallback<ClickEvent>(MoveItemFromSellBasket);
        itemDisplay.RegisterCallback<ClickEvent>(MoveItemToSellBasket);
    }

    private void RenderInventoryContents()
    {
        _inventoryScroll.contentContainer.Clear();
        RenderDisplayCategories<WeaponType>();
        RenderCategoryContents<WeaponType>(Player.Inventory.Weapons.Items);
        RenderDisplayCategories<ArmourType>();
        RenderCategoryContents<ArmourType>(Player.Inventory.Armour.Items);
        RenderDisplayCategories<ConsumableType>();
        RenderCategoryContents<ConsumableType>(Player.Inventory.Consumables.Items);
    }

    private void RenderDisplayCategories<T>() where T : Enum
    {
        foreach (var categoryName in Enum.GetNames(typeof(T)))
        {
            if (categoryName != "Any")
            {
                InventoryCategoryUXML.CloneTree(_inventoryScroll.contentContainer);
                VisualElement container = _inventoryScroll.Query("InventoryCategory").Build().Last();
                container.name = $"{categoryName}CategoryContainer";
                container.Q<Label>("CategoryTag").text = categoryName;
            }
        }
    }

    private void RenderCategoryContents<T>(IEnumerable<Item> items) where T : Enum
    {
        foreach (var item in items)
        {
            if (Player.Inventory.IsEquipped(item) != true)
            {
                var itemDisplay = new ItemDisplay(item)
                {
                    name = item.ID,
                    style = {
                        width = Length.Percent(100f),
                        backgroundColor = (Color)new Color32(41, 39, 33, 255),
                        height = 150,
                        marginTop = 10,
                        marginBottom = 10
                    }
                };
                var manifest = item.GetManifest();
                itemDisplay.Icon.AddOverlay("PriceOverlay_text", OverlayAlignment.BottomLeft, $"{manifest.Price}");
                itemDisplay.Icon.AddOverlay("PriceOverlay_icon", OverlayAlignment.BottomRight, Resources.Load<Sprite>("StatIcons/coin"));
                //
                var itemSubTypeName = Enum.GetName(typeof(T), item.ItemSubTypeHash);
                string categoryContainerID = $"{itemSubTypeName}CategoryContainer";
                var categoryContainer = _inventoryScroll.Q(categoryContainerID);
                var categoryCountLabel = categoryContainer.Q<Label>("CategoryCount");
                categoryCountLabel.text = $"{Convert.ToInt32(categoryCountLabel.text) + 1}";
                //
                categoryContainer.Add(itemDisplay);
            }
        }
    }
}
