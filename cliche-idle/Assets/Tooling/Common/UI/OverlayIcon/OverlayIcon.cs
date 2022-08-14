using System;

namespace UnityEngine.UIElements
{
    public class OverlayIcon : VisualElement
    {
        public string reference_item_ID;

        public new class UxmlFactory : UxmlFactory<OverlayIcon, VisualElement.UxmlTraits> {}

        public OverlayIcon()
        {
            reference_item_ID = "";
        }

        public OverlayIcon(string referenceItemID, Sprite iconSprite, float width, float height)
        {
            reference_item_ID = referenceItemID;
            name = $"{reference_item_ID}";
            style.height = height;
            style.width = width;
            style.backgroundImage = iconSprite.texture;
        }
    
        public OverlayIcon(string referenceItemID, Sprite iconSprite) : this(referenceItemID, iconSprite, (float)iconSprite.rect.width, (float)iconSprite.rect.height) 
        {

        }

        // * PUBLIC FUNCTIONS * //

        public void AddOverlay(string overlayID, OverlayAlignment position, Sprite icon)
        {
            if (icon != null)
            {
                int overlayPositionIndex = Convert.ToInt32(Enum.Parse(typeof(OverlayAlignment), position.ToString()));
                if (FindChild(overlayID) == null)
                {
                    // Add the new image to the overlay stack
                    AddNewIconOverlayElement(overlayID, overlayPositionIndex, icon);
                }
                else
                {
                    throw new ArgumentException($"Overlay ID {overlayID} is already in use.", nameof(overlayID));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(icon));
            }
        }

        public void AddOverlay(string overlayID, OverlayAlignment position, string text, Font font, float textSize, Color color)
        {
            int overlayPositionIndex = Convert.ToInt32(Enum.Parse(typeof(OverlayAlignment), position.ToString()));
            if (FindChild(overlayID) == null)
            {
                // Add the new label to the overlay stack
                AddNewTextOverlayElement(overlayID, overlayPositionIndex, text, font, textSize, color);
            }
            else
            {
                throw new ArgumentException($"Overlay ID {overlayID} is already in use.", nameof(overlayID));
            }
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
        /// Sets the visibility of the overlay with the specific ID.
        /// If `visible` is not given, it inverts the overlay's current visibility setting; otherwise sets the visibility to the given value.
        /// </summary>
        /// <param name="overlayID"></param>
        /// <param name="visible"></param>
        public void ToggleOverlay(string overlayID, bool? visible = null)
        {
            var overlay = FindChild(overlayID);
            if (overlay != null)
            {
                if (visible == null)
                {
                    overlay.style.visibility = (overlay.style.visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible );
                }
                else
                {
                    overlay.style.visibility = (visible == true ? Visibility.Hidden : Visibility.Visible );
                }
            }
        }

        // INTERNAL ----------------------------------- <summary>

        /// <summary>
        /// Finds and returns the VisualElement with the given name in this object's child list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private VisualElement FindChild(string name)
        {
            var childOverlays = Children();
            foreach (var child in childOverlays)
            {
                if (child.name == name)
                {
                    return child;
                }
            }
            return null;
        }

        private void AddNewIconOverlayElement(string overlayID, int position, Sprite icon)
        {
            // ? Might not be useful anymore; but keep an eye out on the scalar system
            //var scaledIconSize = ScaleIconSize(layer.Icon.rect.size);
            OverlayAlignmentStyle alignment = GetOverlayAlignment(position);
            // Calculate icon size in percent
            float IconWidth = (icon.rect.size.x / style.backgroundImage.value.texture.width) * 100;
            float IconHeight = (icon.rect.size.y / style.backgroundImage.value.texture.height) * 100;
            // Override icon size for FILL option
            if (((OverlayAlignment)position).ToString() == "Fill")
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

        private void AddNewTextOverlayElement(string overlayID, int position, string text, Font font, float textSize, Color color)
        {
            OverlayAlignmentStyle alignment = GetOverlayAlignment(position);
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
            Label textOverlay = new Label
            {
                name = overlayID,
                style = { 
                    position = Position.Absolute,
                    fontSize = textSize,
                    unityFont = font,
                    color = color,
                    unityTextAlign = TextAnchor.MiddleLeft
                },
                text = text
            };
            iconOverlayContainer.Add(textOverlay);
            Add(iconOverlayContainer);

            //Debug.Log($"Adding new overlay text {layer.ID}. Text: '{layer.Text}', fontSize: {layer.TextSize}px.");
        }

        /// <summary>
        /// Calculates the coodinates on the base image, at which the Top-Left corner of the overlay image will be drawn.
        /// This poit will be used as the origin point for rendering the rest of the image.
        /// </summary>
        /// <param name="positionIndex">The position index of the overlay image.</param>
        /// <returns></returns>
        private OverlayAlignmentStyle GetOverlayAlignment(int positionIndex)
        {
            // Default configuration is Top left, which is also used for Fill
            // (Index 1 and 5)
            OverlayAlignmentStyle alignment = new OverlayAlignmentStyle();
            switch (positionIndex)
            {
                case 0:
                    // Center
                    alignment.vertical = Justify.Center;
                    alignment.horizontal = Align.Center;
                    break;
                case 2:
                    // Top left
                    alignment.vertical = Justify.FlexStart;
                    alignment.horizontal = Align.FlexStart;
                    break;
                case 3:
                    // Top right
                    alignment.vertical = Justify.FlexEnd;
                    alignment.horizontal = Align.FlexStart;
                    break;
                case 4:
                    // Bottom left
                    alignment.vertical = Justify.FlexStart;
                    alignment.horizontal = Align.FlexEnd;
                    break;
                case 5:
                    // Bottom right
                    alignment.vertical = Justify.FlexEnd;
                    alignment.horizontal = Align.FlexEnd;
                    break;
                default:
                    alignment.vertical = Justify.FlexStart;
                    alignment.horizontal = Align.FlexStart;
                    break;
            }
            return alignment;
        }

        // * CLASSES * //

        private class OverlayAlignmentStyle
        {
            public Justify vertical;
            public Align horizontal;
        }
    }

    /// <summary>
    /// Alignment of overlay layers
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