using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorSelector : VisualElement
{
    /// <summary>
    /// The UXML attribute that stores the color mode.
    /// </summary>
    private string colorMode { get; set; } = "red";

    /// <summary>
    /// The currently selected full color.
    /// </summary>
    public Color32 SelectedColor { get; private set; }

    /// <summary>
    /// The current color value.
    /// </summary>
    private int _selectedIndex = 0;

    /// <summary>
    /// Event that occurs when the selection changes.
    /// </summary>
    public Action<object, Color32> SelectionChange;

    /// <summary>
    /// The label that displays the selected option.
    /// </summary>
    private VisualElement _colorDisplay;

    public new class UxmlFactory : UxmlFactory<ColorSelector, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription _optionsString = new UxmlStringAttributeDescription { name = "color-mode", defaultValue = "red" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ColorSelector os = ((ColorSelector)ve);
            os.colorMode = _optionsString.GetValueFromBag(bag, cc);
            os.UpdateSelection(0);
        }
    }

    public ColorSelector()
    {
        GenerateStructure();
    }

    public ColorSelector(string colorPickerMode)
    {
        colorMode = colorPickerMode;
        GenerateStructure();
    }

    private void GenerateStructure()
    {
        name = "OptionSelector";
        style.flexDirection = FlexDirection.Row;
        style.alignItems = Align.Center;
        style.justifyContent = Justify.SpaceBetween;

        var leftScrollButton = new Button()
        {
            text = "<",
            name = "optionLeftScroll",
            style =
            {
                width = Length.Percent(15),
                height = Length.Percent(100),
                fontSize = style.fontSize,
                unityTextAlign = TextAnchor.MiddleCenter,
            }
        };
        leftScrollButton.AddToClassList("optionLeftScroll");
        leftScrollButton.clicked += () => { UpdateSelection(GetPreviousOptionIndex()); };

        _colorDisplay = new VisualElement()
        {
            name = "optionDisplay",
            style =
            {
                width = Length.Percent(40),
                height = Length.Percent(95),
                backgroundColor = (Color)GetColor(255),
            }
        };
        _colorDisplay.AddToClassList("optionDisplay");

        var rightScrollButton = new Button()
        {
            text = ">",
            name = "optionRightScroll",
            style =
            {
                width = Length.Percent(15),
                height = Length.Percent(100),
                fontSize = style.fontSize,
                unityTextAlign = TextAnchor.MiddleCenter,
            },
        };
        leftScrollButton.AddToClassList("optionRightScroll");
        rightScrollButton.clicked += () => { UpdateSelection(GetNextOptionIndex()); };

        Add(leftScrollButton);
        Add(_colorDisplay);
        Add(rightScrollButton);
    }

    private void UpdateSelection(int optionIndex)
    {
        _selectedIndex = optionIndex;
        SelectedColor = GetColor(_selectedIndex);
        _colorDisplay.style.backgroundColor = (Color)SelectedColor;
        if (SelectionChange != null)
        {
            SelectionChange.Invoke(this, SelectedColor);
        }
    }

    private Color32 GetColor(int colorVal)
    {
        Color32 color = Color.white;
        switch (colorMode)
        {
            case "red":
                color = new Color32((byte)colorVal, 0, 0, 255);
                break;
            case "green":
                color = new Color32(0, (byte)colorVal, 0, 255);
                break;
            case "blue":
                color = new Color32(0, 0, (byte)colorVal, 255);
                break;
        }
        return color;
    }

    private int GetNextOptionIndex()
    {
        int index = 0;
        if (_selectedIndex + 15 <= 255)
        {
            index = _selectedIndex + 15;
        }
        return index;
    }

    private int GetPreviousOptionIndex()
    {
        int index = 255;
        if (_selectedIndex - 15 <= 0)
        {
            index = _selectedIndex - 15;
        }
        return index;
    }
}
