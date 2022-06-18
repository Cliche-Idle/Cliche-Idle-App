using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayIcon : VisualElement
{
    private readonly string reference_item_ID;
    // Do not expose this
    private List<OverlayLayer> OverlayLayers = new List<OverlayLayer>();

    public OverlayIcon(string referenceItemID, Sprite iconSprite, int width = 50, int height = 50)
    {
        reference_item_ID = referenceItemID;
        name = $"{reference_item_ID}";
        style.height = height;
        style.width = width;
        style.backgroundImage = iconSprite.texture;
        RegisterCallback<GeometryChangedEvent>(UpdateOverlay);
    }
    
    public OverlayIcon(string referenceItemID, Sprite iconSprite) : this(referenceItemID, iconSprite, (int)iconSprite.rect.width, (int)iconSprite.rect.height)
    {

    }

    public void AddOverlayLayer(string overlayID, int overlayPosition, Sprite overlayImage)
    {
        if (overlayImage != null)
        {
            if (overlayPosition > 4 || overlayPosition < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(overlayPosition), "overlayPosition must be an integer between 0 and 4 (inclusive).");
            }
            else if (OverlayLayers.FindIndex(layer => layer.ID == overlayID) == -1)
            {
                // Add the new image to the overlay stack
                OverlayLayer layer = new OverlayLayer(overlayID, overlayPosition, overlayImage);
                OverlayLayers.Add(layer);
                AddNewIconOverlayElement(layer);
            }
            else
            {
                throw new ArgumentException($"Overlay ID {overlayID} is already in use.", nameof(overlayID));
            }
        }
        else
        {
            throw new ArgumentNullException(nameof(overlayImage));
        }
    }

    /// <summary>
    /// Clears and removes all overlay layers.
    /// </summary>
    public void ClearOverlays()
    {
        OverlayLayers.Clear();
        Clear();
    }

    /// <summary>
    /// Removes the overlay with the specified ID.
    /// </summary>
    /// <param name="overlayID"></param>
    public void RemoveOverlayLayer(string overlayID)
    {
        OverlayLayers.RemoveAll(layer => layer.ID == overlayID);
        var childOverlays = Children();
        foreach (var overlay in childOverlays)
        {
            if (overlay.name == overlayID)
            {
                Remove(overlay);
                break;
            }
        }
    }

    public void ChangeOverlayLayerVisibility(string overlayID, bool? visible = null)
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="overlayID"></param>
    public T GetOverlayLayer<T>(string overlayID)
    {
        var childOverlay = FindChild(overlayID);
        return (T)(object)childOverlay;
    }

    // INTERNAL ----------------------------------- <summary>

    private VisualElement FindChild(string overlayID)
    {
        var childOverlays = Children();
        foreach (var overlay in childOverlays)
        {
            if (overlay.name == overlayID)
            {
                return overlay;
            }
        }
        return null;
    }

    private void AddNewIconOverlayElement(OverlayLayer layer)
    {
        var scaledIconSize = ScaleIconSize(layer.Icon.rect.size);
        Vector2 overlayOriginPoint = CalculateOverlayImageOriginPoint(layer.Position, scaledIconSize);
        VisualElement iconOverlay = new VisualElement
        {
            name = layer.ID,
            style = { 
                backgroundImage = layer.Icon.texture,
                position = Position.Absolute,
                left = Length.Percent((overlayOriginPoint.x / style.backgroundImage.value.texture.width) * 100),
                top = Length.Percent((overlayOriginPoint.y / style.backgroundImage.value.texture.height) * 100),
                width = scaledIconSize.x,
                height = scaledIconSize.y,
                //maxWidth = layer.Icon.rect.size.x,
                //maxHeight = layer.Icon.rect.size.y,
                //unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };
        Add(iconOverlay);
        Debug.Log(overlayOriginPoint);
        Debug.Log((overlayOriginPoint.x / style.backgroundImage.value.texture.width) * 100);
        Debug.Log((overlayOriginPoint.y / style.backgroundImage.value.texture.height) * 100);
        Debug.Log($"Adding new overlay icon {layer}. Size: {scaledIconSize.x}w, {scaledIconSize.y}h.");
    }

    private void AddNewTextOverlayElement(OverlayLayer layer)
    {
        //float scaleRatio = GetIconScalingRatio();
        var scaledIconSize = ScaleIconSize(layer.Icon.rect.size);
        Vector2 overlayOriginPoint = CalculateOverlayImageOriginPoint(layer.Position, scaledIconSize);
        Label textOverlay = new Label
        {
            name = layer.ID,
            style = { 
                position = Position.Absolute,
                fontSize = layer.TextSize,
                unityFont = layer.TextFont,
                color = layer.TextColor,
                unityTextAlign = TextAnchor.MiddleLeft
                //unityBackgroundScaleMode = ScaleMode.ScaleToFit
            },
            text = layer.Text
        };
        //textOverlay.MeasureTextSize()
        Add(textOverlay);
        //Debug.Log($"Adding new overlay icon {layer}. Size: {newSize.x}w, {newSize.y}h.");
    }

    private Vector2 GetIconScalingRatio(Vector2? referenceSize = null)
    {
        float mainContainerWidth = worldBound.size.x;
        float mainContainerHeight = worldBound.size.y;

        if (referenceSize != null)
        {
            mainContainerWidth = referenceSize.Value.x;
            mainContainerHeight = referenceSize.Value.y;
        }

        int baseIconSpriteWidth = style.backgroundImage.value.texture.width;
        int baseIconSpriteHeight = style.backgroundImage.value.texture.height;

        float widthRatio =  mainContainerWidth / baseIconSpriteWidth; 
        float heightRatio = mainContainerHeight / baseIconSpriteHeight;

        /*if (widthRatio < heightRatio)
        {
            return widthRatio;
        }
        else
        {
            return heightRatio;
        }*/
        return new Vector2(widthRatio, heightRatio);
    }

    private float ScaleSizeValue(float value, float currentScalar, float previousScalar)
    {
        double baseValue = value / previousScalar;
        float result = (float)(baseValue * currentScalar);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateOverlay(GeometryChangedEvent iconGeometry)
    {
        var childOverlays = (List<VisualElement>)Children();
        foreach (var childOverlay in childOverlays)
        {
            Vector2 scalingRatio = GetIconScalingRatio();

            //Vector2 overlayPosition = CalculateOverlayImageOriginPoint(layer.Position, newSize);

            /* Not sure where this is useful anymore
            if (childOverlay.resolvedStyle.backgroundImage != null)
            {
                childOverlay.style.backgroundImage = layer.Icon.texture;
            }
            else
            {
                ((Label)childOverlay).text = layer.Text;
                childOverlay.style.color = layer.TextColor;
                childOverlay.style.fontSize = layer.TextSize;
            }*/

            // Update styling:
            // Get previous scalars:
            float mainContainerWidth = iconGeometry.oldRect.size.x;
            float mainContainerHeight = iconGeometry.oldRect.size.y;

            int baseIconSpriteWidth = style.backgroundImage.value.texture.width;
            int baseIconSpriteHeight = style.backgroundImage.value.texture.height;

            float widthRatio =  mainContainerWidth / baseIconSpriteWidth; 
            float heightRatio = mainContainerHeight / baseIconSpriteHeight;
            Vector2 previousScalingRatio = GetIconScalingRatio(iconGeometry.oldRect.size);
            //
            Vector2 overlaySpriteSize = new Vector2(childOverlay.style.backgroundImage.value.texture.width, childOverlay.style.backgroundImage.value.texture.height);
            var newElementBaseSize = ScaleIconSize(overlaySpriteSize);
            childOverlay.style.width = newElementBaseSize.x;
            childOverlay.style.height = newElementBaseSize.y;
            //
            //childOverlay.style.left = ScaleSizeValue(childOverlay.resolvedStyle.left, scalingRatio.x, previousScalingRatio.x);
            //childOverlay.style.top = ScaleSizeValue(childOverlay.resolvedStyle.top, scalingRatio.y, previousScalingRatio.y);
            //Debug.Log(scalingRatio);
            //Debug.Log(ScaleSizeValue(childOverlay.resolvedStyle.left, scalingRatio.x, previousScalingRatio.x));
            //Debug.Log(ScaleSizeValue(childOverlay.resolvedStyle.top, scalingRatio.y, previousScalingRatio.y));
            //
            /*childOverlay.style.marginBottom = (childOverlay.resolvedStyle.marginBottom * scalingRatio.y);
            childOverlay.style.marginTop = (childOverlay.resolvedStyle.marginTop * scalingRatio.y);
            childOverlay.style.marginLeft = (childOverlay.resolvedStyle.marginLeft * scalingRatio.x);
            childOverlay.style.marginRight = (childOverlay.resolvedStyle.marginRight * scalingRatio.x);
            //
            childOverlay.style.paddingBottom = (childOverlay.resolvedStyle.paddingBottom * scalingRatio.y);
            childOverlay.style.paddingTop = (childOverlay.resolvedStyle.paddingTop * scalingRatio.y);
            childOverlay.style.paddingLeft = (childOverlay.resolvedStyle.paddingLeft * scalingRatio.x);
            childOverlay.style.paddingRight = (childOverlay.resolvedStyle.paddingRight * scalingRatio.x);*/
        }
    }

    /// <summary>
    /// Calculates the coodinates on the base image, at which the Top-Left corner of the overlay image will be drawn.
    /// This poit will be used as the origin point for rendering the rest of the image.
    /// </summary>
    /// <param name="positionIndex">The position index of the overlay image.</param>
    /// <param name="overlaySize">The Size of the overlay image.</param>
    /// <returns></returns>
    private Vector2 CalculateOverlayImageOriginPoint(int positionIndex, Vector2 overlaySize)
    {
        // Default configuration is Top left, which is also used for Fill
        // (Index 1 and 5)
        float x = 0;
        float y = 0;

        Debug.Log(worldBound);
        float width = worldBound.width;
        float height = worldBound.height;
        switch (positionIndex)
        {
            case 0:
                // Center
                x = ((width / 2) - (overlaySize.x / 2));
                y = ((height / 2) - (overlaySize.y / 2));
                break;
            case 2:
                // Top right
                x = (width - overlaySize.x);
                y = 0;
                break;
            case 3:
                // Bottom left
                x = 0;
                y = (height - overlaySize.y);
                break;
            case 4:
                // Bottom right
                x = (width - overlaySize.x);
                y = (height - overlaySize.y);
                break;
            default:
                x = 0;
                y = 0;
                break;
        }
        return new Vector2(x, y);
    }

    private Vector2 ScaleIconSize(Vector2 overlaySpriteSize)
    {
        // * This function scales the overlay image based on the ratio of the base sprite's real size to the icon container's size.
        // * A very big base sprite in a small container will scale the overlays to very small, which is unintended, but should be avoided.
        Vector2 scaledSize = overlaySpriteSize;
        Vector2 scalingRatio = GetIconScalingRatio();
        if (scalingRatio.x < 1 || scalingRatio.y < 1)
        {
            if (scalingRatio.x < scalingRatio.y)
            {
                scaledSize = overlaySpriteSize * scalingRatio.x;
            }
            else
            {
                scaledSize = overlaySpriteSize * scalingRatio.y;
            }
        }
        return scaledSize;
    }

    // TODO: make this funky thing work:
    private enum Overlay_Position_Friendly_Values
    {
        Center = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 3,
        BottomRight = 4,
        Fill = 5
    }

    private class OverlayLayer
    {
        public string ID = "";
        // Position for the overlay
        public int Position = 0;
        public Vector2 Offset = Vector2.zero;
        // Display types
        public Sprite Icon;
        public string Text;
        // Text specific parameters
        public Font TextFont;
        public Color TextColor;
        public int TextSize;
            
        /// <summary>
        /// Creates a new OverlayLayer that displays an image.
        /// </summary>
        /// <param name="overlayPosition"></param>
        /// <param name="overlayImage"></param>
        public OverlayLayer(string overlayID, int overlayPosition, Sprite overlayImage, Vector2? offset = null)
        {
            ID = overlayID;
            Icon = overlayImage;
            Position = overlayPosition;
            Offset = offset != null ? (Vector2)offset : Vector2.zero;
        }

        /// <summary>
        /// Creates a new OverlayLayer that displays a string.
        /// </summary>
        /// <param name="overlayPosition"></param>
        /// <param name="overlayText"></param>
        /// <param name="overlayTextFont"></param>
        /// <param name="overlayTextSize"></param>
        /// <param name="overlayTextColor"></param>
        public OverlayLayer(string overlayID, int overlayPosition, string overlayText, Font overlayTextFont, int overlayTextSize, Color? overlayTextColor = null, Vector2? offset = null)
        {
            ID = overlayID;
            Text = overlayText;
            Position = overlayPosition;
            Offset = offset != null ? (Vector2)offset : Vector2.zero;
            TextFont = overlayTextFont;
            TextSize = overlayTextSize;
            TextColor = overlayTextColor != null ? (Color)overlayTextColor : Color.black;
        }
    }
}
