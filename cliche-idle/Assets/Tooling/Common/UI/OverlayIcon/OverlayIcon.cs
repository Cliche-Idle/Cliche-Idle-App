using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cliche.UIElements
{
    public class OverlayIcon : VisualElement
    {
        public string ReferenceID { get; set; }

        public new class UxmlFactory : UxmlFactory<OverlayIcon, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription _referenceID = new UxmlStringAttributeDescription { name = "referenceID", defaultValue = "" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                OverlayIcon oi = ((OverlayIcon)ve);
                oi.ReferenceID = _referenceID.GetValueFromBag(bag, cc);
            }
        }

        public OverlayIcon()
        {
            style.backgroundImage = Resources.Load<Sprite>("icons/placeholder_250x250_cross2").texture;
            style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            style.height = 100;
            style.width = 100;
        }

        public OverlayIcon(Sprite iconSprite) : this()
        {
            style.backgroundImage = iconSprite.texture;
        }

        // TODO: return overlay
        public void AddOverlay(string overlayID, OverlayAlignment position, Sprite icon)
        {
            if (icon != null)
            {
                if (FindChild(overlayID) == null)
                {
                    // Add the new image to the overlay stack
                    AddNewIconOverlayElement(overlayID, position, icon);
                }
                else
                {
                    throw new Exception($"Overlay ID \"{overlayID}\" is already in use.");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(icon));
            }
        }

        public void AddOverlay(string overlayID, OverlayAlignment position, string text)
        {
            if (FindChild(overlayID) == null)
            {
                // Add the new label to the overlay stack
                AddNewTextOverlayElement(overlayID, position, text);
            }
            else
            {
                throw new Exception($"Overlay ID \"{overlayID}\" is already in use.");
            }
        }

        /// <summary>
        /// Returns the specified overlay's <see cref="VisualElement"/> (Either <see cref="VisualElement"/> or <see cref="Label"/>, manual conversion required).
        /// </summary>
        /// <param name="overlayID"></param>
        /// <returns></returns>
        public VisualElement GetOverlay(string overlayID)
        {
            return this.Q<VisualElement>(overlayID);
        }

        /// <summary>
        /// Clears and removes all overlay layers.
        /// </summary>
        public void ClearOverlays()
        {
            Clear();
        }

        /// <summary>
        /// Removes the overlay with the specified ID.
        /// </summary>
        /// <param name="overlayID"></param>
        public void RemoveOverlay(string overlayID)
        {
            var overlay = FindChild($"{overlayID}_container");
            if (overlay != null)
            {
                Remove(overlay);
            }
        }

        /// <summary>
        /// Sets the visibility of the specified Overlay.
        /// If <paramref name="visible"/> is not given, it inverts the overlay's current visibility setting; otherwise sets the visibility to the given value.
        /// </summary>
        /// <param name="overlayID"></param>
        public void ToggleOverlay(string overlayID)
        {
            var overlay = FindChild($"{overlayID}_container");
            if (overlay != null)
            {
                if (overlay.style.visibility == Visibility.Visible)
                {
                    overlay.style.visibility = Visibility.Hidden;
                }
                else
                {
                    overlay.style.visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Sets the visibility of the specified Overlay.
        /// </summary>
        /// <param name="overlayID"></param>
        /// <param name="visible"></param>
        public void ToggleOverlay(string overlayID, bool visible)
        {
            var overlay = FindChild($"{overlayID}_container");
            if (overlay != null)
            {
                if (visible == true)
                {
                    overlay.style.visibility = Visibility.Visible;
                }
                else
                {
                    overlay.style.visibility = Visibility.Hidden;
                }
            }
        }


        /// <summary>
        /// Finds and returns the VisualElement with the given name in this object's child list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private VisualElement FindChild(string name)
        {
            var child = this.Q<VisualElement>(name);
            if (child == null)
            {
                // Give proper heads up
                //throw new NullReferenceException($"No Overlay with the name \"{name}\" could be found.");
            }
            return child;
        }

        private void AddNewIconOverlayElement(string overlayID, OverlayAlignment position, Sprite icon)
        {
            OverlayAlignmentStyle alignment = GetOverlayAlignmentStyle(position);
            // Calculate icon size in percent
            float IconWidth = (icon.rect.size.x / style.backgroundImage.value.texture.width) * 100;
            float IconHeight = (icon.rect.size.y / style.backgroundImage.value.texture.height) * 100;

            if (position == OverlayAlignment.Fill)
            {
                IconWidth = 100;
                IconHeight = 100;
            }

            VisualElement iconOverlayContainer = new VisualElement
            {
                name = $"{overlayID}_container",
                style = {
                    position = Position.Absolute,
                    alignItems = alignment.horizontal,
                    justifyContent = alignment.vertical,
                    width = Length.Percent(100),
                    height = Length.Percent(100),
                }
            };
            VisualElement iconOverlay = new VisualElement
            {
                name = overlayID,
                style = {
                    backgroundImage = icon.texture,
                    position = Position.Absolute,
                    width = Length.Percent(IconWidth),
                    height = Length.Percent(IconHeight),
                }
            };
            iconOverlayContainer.Add(iconOverlay);
            Add(iconOverlayContainer);

            //Debug.Log($"Adding new overlay icon {layer.ID}. Size: {scaledIconSize.x}w, {scaledIconSize.y}h.");
        }

        private void AddNewTextOverlayElement(string overlayID, OverlayAlignment position, string text)
        {
            OverlayAlignmentStyle alignment = GetOverlayAlignmentStyle(position);
            VisualElement textOverlayContainer = new VisualElement
            {
                name = $"{overlayID}_container",
                style = {
                    position = Position.Absolute,
                    alignItems = alignment.horizontal,
                    justifyContent = alignment.vertical,
                    width = Length.Percent(100),
                    height = Length.Percent(100),
                }
            };
            Label textOverlay = new Label
            {
                name = overlayID,
                style = {
                    position = Position.Absolute,
                    unityTextAlign = TextAnchor.MiddleLeft
                },
                text = text
            };
            textOverlayContainer.Add(textOverlay);
            Add(textOverlayContainer);

            //Debug.Log($"Adding new overlay text {layer.ID}. Text: '{layer.Text}', fontSize: {layer.TextSize}px.");
        }

        /// <summary>
        /// Gets the alignment of the overlay element in Style format.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private OverlayAlignmentStyle GetOverlayAlignmentStyle(OverlayAlignment position)
        {
            // Default configuration is Top left, which is also used for Fill
            // (Index 1 and 5)
            OverlayAlignmentStyle alignment = new OverlayAlignmentStyle();
            switch (position)
            {
                case OverlayAlignment.Center:
                    // Center
                    alignment.vertical = Justify.Center;
                    alignment.horizontal = Align.Center;
                    break;
                case OverlayAlignment.Fill:
                case OverlayAlignment.TopLeft:
                    // Top left and fill
                    alignment.vertical = Justify.FlexStart;
                    alignment.horizontal = Align.FlexStart;
                    break;
                case OverlayAlignment.TopRight:
                    // Top right
                    alignment.vertical = Justify.FlexStart;
                    alignment.horizontal = Align.FlexEnd;
                    break;
                case OverlayAlignment.BottomLeft:
                    // Bottom left
                    alignment.vertical = Justify.FlexEnd;
                    alignment.horizontal = Align.FlexStart;
                    break;
                case OverlayAlignment.BottomRight:
                    // Bottom right
                    alignment.vertical = Justify.FlexEnd;
                    alignment.horizontal = Align.FlexEnd;
                    break;
            }
            return alignment;
        }

        private class OverlayAlignmentStyle
        {
            public Justify vertical;
            public Align horizontal;
        }
    }

    /// <summary>
    /// Alignment of overlay layers for <see cref="OverlayIcon"/>.
    /// </summary>
    public enum OverlayAlignment
    {
        /// <summary>
        /// Aligns the overlay layer into the center of the container.
        /// </summary>
        Center = 0,
        /// <summary>
        /// Aligns the overlay layer to always cover the entire area of the container. Will stretch sprites. Only works with sprites.
        /// </summary>
        Fill = 1,
        /// <summary>
        /// Aligns the overlay layer to the top left corner of the container.
        /// </summary>
        TopLeft = 2,
        /// <summary>
        /// Aligns the overlay layer to the top right corner of the container.
        /// </summary>
        TopRight = 3,
        /// <summary>
        /// Aligns the overlay layer to the bottom left corner of the container.
        /// </summary>
        BottomLeft = 4,
        /// <summary>
        /// Aligns the overlay layer to the bottom left corner of the container.
        /// </summary>
        BottomRight = 5,
    }
}