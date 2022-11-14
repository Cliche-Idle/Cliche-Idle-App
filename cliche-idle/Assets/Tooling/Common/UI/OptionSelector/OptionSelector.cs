using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionSelector : VisualElement
{
    private List<string> _options;
    public List<string> Options
    {
        get
        {
            return _options;
        }
        set
        {
            _options = value;
            if (_options != null)
            {
                if (_options.Count > 0)
                {
                    UpdateSelection(0);
                }
            }
        }
    }

    /// <summary>
    /// The UXML attribute that stores the options. Only used if the base values are predefined in the inspector.
    /// </summary>
    private string optionsString { get; set; }

    public string SelectedOption { get; private set; }

    private int _selectedIndex = 0;
    public int SelectedIndex
    {
        get { 
            return _selectedIndex; 
        }
        set { 
            if (_options != null)
            {
                if (value < Options.Count)
                {
                    UpdateSelection(value);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("SelectedIndex", "Value must be non-negative and less than the size of the Options collection.");
                }
            }
        }
    }

    public Action<object, string> SelectionChange;

    private Label _optionLabel;

    public new class UxmlFactory : UxmlFactory<OptionSelector, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription _optionsString = new UxmlStringAttributeDescription { name = "options-string", defaultValue = "No option" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            OptionSelector os = ((OptionSelector)ve);
            os.optionsString = _optionsString.GetValueFromBag(bag, cc);
            var options = os.optionsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (options != null && options.Length > 0)
            {
                os._optionLabel.text = options[0].Trim();
            }
            os.GenerateOptionsList();
        }
    }

    public OptionSelector()
    {
        GenerateStructure();
    }

    public OptionSelector(List<string> options)
    {
        Options = options;
        GenerateStructure();
    }

    private void GenerateOptionsList()
    {
        if (optionsString != null)
        {
            var options = optionsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (options.Length > 0)
            {
                Options = new List<string>();
                foreach (var option in options)
                {
                    Options.Add(option.Trim());
                }
            }
        }
    }

    private void GenerateStructure()
    {
        name = "OptionSelector";
        style.flexDirection = FlexDirection.Row;
        style.alignItems = Align.Stretch;
        style.justifyContent = Justify.SpaceBetween;

        var leftScrollButton = new Button()
        {
            text = "<",
            name = "optionLeftScroll",
            style =
            {
                width = 150,
                height = Length.Percent(100),
                fontSize = Length.Percent(100)
            }
        };
        leftScrollButton.clicked += () => { UpdateSelection(GetPreviousOption()); };

        _optionLabel = new Label()
        {
            name = "optionLabel",
            text = TryGetFirstOption(),
            style =
            {
                fontSize = Length.Percent(85),
                unityTextAlign = TextAnchor.MiddleCenter,
            }
        };

        var rightScrollButton = new Button()
        {
            text = ">",
            name = "optionRightScroll",
            style =
            {
                width = 150,
                height = Length.Percent(100),
                fontSize = Length.Percent(100)
            },
        };
        rightScrollButton.clicked += () => { UpdateSelection(GetNextOption()); };

        Add(leftScrollButton);
        Add(_optionLabel);
        Add(rightScrollButton);
    }

    private void UpdateSelection(int optionIndex)
    {
        if (Options != null)
        {
            _selectedIndex = optionIndex;
            SelectedOption = Options[optionIndex];
            _optionLabel.text = SelectedOption;
            if (SelectionChange != null)
            {
                SelectionChange.Invoke(this, SelectedOption);
            }
        }
    }

    private int GetNextOption()
    {
        int index = 0;
        if (Options != null)
        {
            if (SelectedIndex + 1 < Options.Count)
            {
                index = SelectedIndex + 1;
            }
        }
        return index;
    }

    private int GetPreviousOption()
    {
        int index = 0;
        if (Options != null)
        {
            if (SelectedIndex - 1 >= 0)
            {
                index = SelectedIndex - 1;
            }
            else
            {
                index = Options.Count - 1;
            }
        }
        return index;
    }

    private string TryGetFirstOption()
    {
        string text = "No options";
        if (Options != null && Options.Count > 0)
        {
            text = Options[0];
        }
        return text;
    }
}
