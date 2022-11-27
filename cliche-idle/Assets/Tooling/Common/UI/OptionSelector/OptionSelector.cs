using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cliche.UIElements
{
    /// <summary>
    /// A selector that can contains and sidescroll through multiple options.
    /// </summary>
    public class OptionSelector : VisualElement
    {
        private string[] _options;
        /// <summary>
        /// The list of options the selector can choose from.
        /// </summary>
        public string[] Options
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
                    if (_options.Length > 0)
                    {
                        UpdateSelection(0);
                    }
                }
                else
                {
                    _selectedIndex = -1;
                    SelectedOption = null;
                }
            }
        }

        /*
        Apparently there is an implicit field conversion between UXML attributes and C# fields:
        UXML: "example-attribute"   =>   C# "exampleAttribute"
        If this naming is not followed properly, the UI builder will not make the proper connection and the value
        will reset everytime.
         */
        /// <summary>
        /// The UXML attribute that stores the options. Only used if the base values are predefined in the inspector.
        /// </summary>
        private string optionsString { get; set; }

        /// <summary>
        /// The currently selected string option. Defaults to null if <see cref="Options"/> is null.
        /// </summary>
        public string SelectedOption { get; private set; }

        private int _selectedIndex = -1;
        /// <summary>
        /// The currently selected option's index. Defaults to -1 if <see cref="Options"/> is null.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_options != null)
                {
                    if (value < Options.Length)
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

        /// <summary>
        /// Event that occurs when the selection changes.
        /// </summary>
        public Action<string> SelectionChange;

        /// <summary>
        /// The label that displays the selected option.
        /// </summary>
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
                os.GenerateOptionsList();
                os._optionLabel.text = os.TryGetFirstOption();
            }
        }

        public OptionSelector()
        {
            GenerateStructure();
        }

        public OptionSelector(string[] options)
        {
            _options = options;
            GenerateStructure();
        }

        /// <summary>
        /// Gets the string names from enum <typeparamref name="T"/>, and sets them as the available options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SetOptionsFromEnum<T>() where T : Enum
        {
            // Assign to public Options to trigger update
            _options = Enum.GetNames(typeof(T));
            UpdateSelection(0, false);
        }

        private void GenerateOptionsList()
        {
            if (optionsString != null)
            {
                var optionsFromStringAttribute = optionsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (optionsFromStringAttribute.Length > 0)
                {
                    for (int i = 0; i < optionsFromStringAttribute.Length; i++)
                    {
                        optionsFromStringAttribute[i] = optionsFromStringAttribute[i].Trim();
                    }
                    _options = optionsFromStringAttribute;
                }
            }
        }

        private void GenerateStructure()
        {
            name = "OptionSelector";
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Stretch;
            style.justifyContent = Justify.SpaceBetween;
            AddToClassList("options-selector");

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
            leftScrollButton.AddToClassList("optionScrollButton");
            leftScrollButton.clicked += () => { UpdateSelection(GetPreviousOptionIndex()); };

            _optionLabel = new Label()
            {
                name = "optionLabel",
                text = TryGetFirstOption(),
                style =
                {
                    fontSize = style.fontSize,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            _optionLabel.AddToClassList("optionLabel");

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
            rightScrollButton.AddToClassList("optionRightScroll");
            rightScrollButton.AddToClassList("optionScrollButton");
            rightScrollButton.clicked += () => { UpdateSelection(GetNextOptionIndex()); };

            Add(leftScrollButton);
            Add(_optionLabel);
            Add(rightScrollButton);
        }

        private void UpdateSelection(int optionIndex, bool triggerEvent = true)
        {
            if (_options != null)
            {
                _selectedIndex = optionIndex;
                SelectedOption = _options[optionIndex];
                _optionLabel.text = SelectedOption;
                if (SelectionChange != null && triggerEvent == true)
                {
                    SelectionChange.Invoke(SelectedOption);
                }
            }
        }

        private int GetNextOptionIndex()
        {
            int index = 0;
            if (_options != null)
            {
                if (SelectedIndex + 1 < _options.Length)
                {
                    index = SelectedIndex + 1;
                }
            }
            return index;
        }

        private int GetPreviousOptionIndex()
        {
            int index = _options.Length - 1;
            if (_options != null)
            {
                if (SelectedIndex - 1 >= 0)
                {
                    index = SelectedIndex - 1;
                }
            }
            return index;
        }

        /// <summary>
        /// Tries to get the first option from the array. If it's null or the array is empty, returns the string "No options".
        /// </summary>
        /// <returns></returns>
        private string TryGetFirstOption()
        {
            string text = "No options";
            if (_options != null && _options.Length > 0)
            {
                text = _options[0];
            }
            return text;
        }
    }

}