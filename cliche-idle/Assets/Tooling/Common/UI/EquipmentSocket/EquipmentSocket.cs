using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Cliche.UIElements
{
    public class EquipmentSocket : VisualElement
    {
        private VisualElement _equippedItemIcon;
        private Label _equippedItemLabel;
        private Label _equippedItemStatLabel;
        private VisualElement _equippedItemStatIcon;

        private Action OnBindingChange;

        public Action<ClickEvent> OnClick;

        public Action OnSocketUpdate;
        

        public new class UxmlFactory : UxmlFactory<EquipmentSocket, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }

        public EquipmentSocket()
        {
            GenerateStructure();
        }

        public void BindToSocket<T>(GearSocket<T> socket) where T : Item
        {
            if (OnBindingChange != null)
            {
                OnBindingChange();
            }
            
            socket.OnEquip += Socket_OnEquip;

            OnBindingChange += () => {
                // Keep reference to the socket and unsubscribe when no longer needed.
                socket.OnEquip -= Socket_OnEquip;
                OnBindingChange = null;
            };
            

            switch (socket.AcceptItemType)
            {
                case ItemTypes.Weapon:
                    _equippedItemIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/weaponSocket").texture;
                    _equippedItemStatIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/attack").texture;
                    break;
                case ItemTypes.Armour:
                    switch ((ArmourType)socket.AcceptSubTypeHash)
                    {
                        case ArmourType.Helmet:
                            _equippedItemIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/helmetSocket").texture;
                            break;
                        case ArmourType.Chest:
                            _equippedItemIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/chestSocket").texture;
                            break;
                        case ArmourType.Leg:
                            _equippedItemIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/feetSocket").texture;
                            break;
                    }
                    _equippedItemStatIcon.style.backgroundImage = Resources.Load<Sprite>("StatIcons/defence").texture;
                    break;
            }

            Socket_OnEquip(null, socket.EquippedItem);
        }

        private void Socket_OnEquip(object sender, Item e)
        {
            if (e != null)
            {
                var itemManifest = e.GetManifest();
                _equippedItemIcon.style.backgroundImage = itemManifest.Icon.texture;
                _equippedItemLabel.text = itemManifest.Name;
                _equippedItemStatLabel.text = e.MainStatValue.ToString();
            }
        }

        private void GenerateStructure()
        {
            name = "EquipmentSocket";
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.SpaceBetween;
            //style.height = 100;
            AddToClassList("equipment-socket");

            RegisterCallback<ClickEvent>((evt) =>
            {
                if (OnClick != null)
                {
                    OnClick.Invoke(evt);
                }
            });

            RegisterCallback<DetachFromPanelEvent>((evt) =>
            {
                if (OnBindingChange != null)
                {
                    OnBindingChange.Invoke();
                }
            });

            _equippedItemIcon = new VisualElement()
            {
                name = "itemIcon",
                style =
                {
                    height = Length.Percent(90f),
                    width = Length.Percent(10f),
                    unityBackgroundScaleMode = ScaleMode.ScaleToFit,
                    backgroundImage = Resources.Load<Sprite>("StatIcons/placeholder").texture
                }
            };
            _equippedItemIcon.AddToClassList("itemIcon");

            _equippedItemLabel = new Label()
            {
                name = "itemLabel",
                text = "<Item name>",
                style =
                {
                    height = Length.Percent(90f),
                    fontSize = style.fontSize,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    width = Length.Percent(60f),
                }
            };
            _equippedItemLabel.AddToClassList("itemLabel");

            _equippedItemStatLabel = new Label()
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
            _equippedItemStatLabel.AddToClassList("itemStatLabel");

            _equippedItemStatIcon = new VisualElement()
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
            _equippedItemStatIcon.AddToClassList("itemStatIcon");


            Add(_equippedItemIcon);
            Add(_equippedItemLabel);
            Add(_equippedItemStatLabel);
            Add(_equippedItemStatIcon);
        }
    }
}
