using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorPicker : VisualElement
{
    /// <summary>
    /// The list of options the selector can choose from.
    /// </summary>
    public Color32 SelectedColor
    {
        get
        {
            return new Color32((byte)_rSlider.value, (byte)_gSlider.value, (byte)_bSlider.value, 255);
        }
        set
        {
            _rSlider.value = value.r;
            _gSlider.value = value.g;
            _bSlider.value = value.b;
            colorValue = value;
        }
    }
    /// <summary>
    /// Event that occurs when the selected colour changes.
    /// </summary>
    public Action<object, Color32> ValueChange;

    /// <summary>
    /// The label that displays the selected option.
    /// </summary>
    private SliderInt _rSlider;
    private SliderInt _gSlider;
    private SliderInt _bSlider;

    private Color32 colorValue { get; set; }

    public new class UxmlFactory : UxmlFactory<ColorPicker, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlColorAttributeDescription _colorString = new UxmlColorAttributeDescription { name = "color-value", defaultValue = Color.black };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ColorPicker os = ((ColorPicker)ve);
            os.colorValue = (Color32)_colorString.GetValueFromBag(bag, cc);
        }
    }

    public ColorPicker()
    {
        GenerateStructure();
    }

    private void GenerateStructure()
    {
        name = "ColorPicker";
        style.flexDirection = FlexDirection.Column;
        style.alignItems = Align.Stretch;
        style.justifyContent = Justify.SpaceBetween;

        _rSlider = new SliderInt()
        {
            name = "redSlider",
            value = colorValue.r,
            highValue = 255,
            lowValue = 0,
        };
        _rSlider.RegisterValueChangedCallback((val) => { 
            UpdateSelection(); 
        });
        styleSheets.Add(Resources.Load<StyleSheet>($"Styles/SliderRestyle"));

        _gSlider = new SliderInt()
        {
            name = "greenSlider",
            value = colorValue.g,
            highValue = 255,
            lowValue = 0,
            
        };
        _gSlider.RegisterValueChangedCallback((val) => {
            UpdateSelection();
        });

        _bSlider = new SliderInt()
        {
            name = "blueSlider",
            value = colorValue.b,
            highValue = 255,
            lowValue = 0,
        };
        _bSlider.RegisterValueChangedCallback((val) => {
            UpdateSelection();
        });

        Add(_rSlider);
        Add(_gSlider);
        Add(_bSlider);
    }

    private void UpdateSelection()
    {
        if (ValueChange != null)
        {
            ValueChange.Invoke(this, SelectedColor);
        }
    }
}
