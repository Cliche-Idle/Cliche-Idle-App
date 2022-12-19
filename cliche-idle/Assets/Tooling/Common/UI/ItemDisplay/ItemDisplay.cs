using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Cliche.UIElements
{
    public class ItemDisplay : VisualElement
    {
        private Label _itemNameLabel;
        // TODO: optionally display quantity and price
        private Label _itemQuantity;
        private Label _itemPrice;
        private Label _itemStatLabel;
        private VisualElement _itemStatIcon;

        /// <summary>
        /// The displayed item's icon (<see cref="OverlayIcon"/>).
        /// </summary>
        public OverlayIcon Icon { get; private set; }
        private bool iconOnly { get; set; } = false;

        /// <summary>
        /// Gets or sets whether only the item's icon or other stats are displayed too.
        /// </summary>
        public bool IconOnly
        {
            get
            {
                return iconOnly;
            }

            set
            {
                iconOnly = value;
                UpdateItemDisplay();
            }
        }

        private Item _displayItem;
        /// <summary>
        /// Gets or sets the currenctly displayed item.
        /// </summary>
        public Item DisplayItem { 
            get 
            { 
                return _displayItem; 
            }

            set
            {
                _displayItem = value;
                UpdateItemDisplay();
            }
        }

        /// <summary>
        /// Gets the <see cref="ItemTypes"/> of the currenct disyplayed item.
        /// </summary>
        public ItemTypes DisplayItemType
        {
            get
            {
                return DisplayItem.ItemType;
            }
        }


        public new class UxmlFactory : UxmlFactory<ItemDisplay, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlBoolAttributeDescription _iconOnly = new UxmlBoolAttributeDescription() { name = "icon-only", defaultValue = false };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ItemDisplay id = ((ItemDisplay)ve);
                id.iconOnly = _iconOnly.GetValueFromBag(bag, cc);
            }
        }

        public ItemDisplay()
        {
            GenerateStructure();
        }

        public ItemDisplay(Item item) : this()
        {
            DisplayItem = item;
            UpdateItemDisplay();
        }

        public void DisplayManifestItem(ItemManifest manifest)
        {
            switch (manifest.ItemType)
            {
                case ItemTypes.Weapon:
                    DisplayItem = new Weapon(manifest.ID, manifest.MainStatValue);
                    break;
                case ItemTypes.Armour:
                    DisplayItem = new Armour(manifest.ID, manifest.MainStatValue);
                    break;
                case ItemTypes.Consumable:
                    DisplayItem = new Consumable(manifest.ID, 1);
                    break;
            }

        }

        private void UpdateItemDisplay()
        {
            if (DisplayItem != null)
            {
                var manifest = DisplayItem.GetManifest();
                if (iconOnly)
                {
                    _itemNameLabel.style.display = DisplayStyle.None;
                    _itemStatLabel.style.display = DisplayStyle.None;
                    _itemStatIcon.style.display = DisplayStyle.None;
                }
                else
                {
                    _itemNameLabel.style.display = DisplayStyle.Flex;
                    _itemStatLabel.style.display = DisplayStyle.Flex;
                    _itemStatIcon.style.display = DisplayStyle.Flex;
                    _itemNameLabel.text = manifest.Name;
                    switch (DisplayItem.ItemType)
                    {
                        case ItemTypes.Weapon:
                            _itemStatIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/attack").texture;
                            break;
                        case ItemTypes.Armour:
                            _itemStatIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/defence").texture;
                            break;
                        case ItemTypes.Consumable:
                            // TODO: add consumable stat icon selector here
                            _itemStatIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/heal").texture;
                            _itemNameLabel.text += $"\n{((Consumable)DisplayItem).Quantity}x";
                            break;
                    }

                    
                    _itemStatLabel.text = DisplayItem.MainStatValue.ToString();
                }

                if (manifest.Icon != null)
                {
                    Icon.style.backgroundImage = manifest.Icon.texture;
                }
            }
        }

        private void GenerateStructure()
        {
            name = "ItemDisplay";
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.SpaceBetween;
            style.paddingLeft = 15;
            style.paddingRight = 15;
            style.paddingTop = 10;
            style.paddingBottom = 10;
            AddToClassList("item-display");

            // Hacky fix to keep the Icon always rectangular
            RegisterCallback<GeometryChangedEvent>((evt) => {
                var height = Mathf.RoundToInt(evt.newRect.height * 0.8f);
                Icon.style.height = height;
                Icon.style.width = height;
            });

            Icon = new OverlayIcon()
            {
                name = "itemIcon",
            };
            Icon.AddToClassList("itemIcon");

            _itemNameLabel = new Label()
            {
                name = "itemLabel",
                text = "<Item name>",
                style =
                {
                    height = Length.Percent(90f),
                    fontSize = style.fontSize,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    width = Length.Percent(60f),
                    marginLeft = 10
                }
            };
            _itemNameLabel.AddToClassList("itemLabel");

            _itemStatLabel = new Label()
            {
                name = "itemStatLabel",
                text = "0",
                style =
                {
                    height = Length.Percent(90f),
                    fontSize = style.fontSize,
                    width = Length.Percent(20f),
                    unityTextAlign = TextAnchor.MiddleRight,
                }
            };
            _itemStatLabel.AddToClassList("itemStatLabel");

            _itemStatIcon = new VisualElement()
            {
                name = "itemStatIcon",
                style =
                {
                    height = Length.Percent(50f),
                    width = Length.Percent(6f),
                    unityBackgroundScaleMode = ScaleMode.ScaleToFit,
                    backgroundImage = Resources.Load<Sprite>("StatIcons/placeholder").texture
                }
            };
            _itemStatIcon.AddToClassList("itemStatIcon");


            Add(Icon);
            Add(_itemNameLabel);
            Add(_itemStatLabel);
            Add(_itemStatIcon);
        }
    }
}
