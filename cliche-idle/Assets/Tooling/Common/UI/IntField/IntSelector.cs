using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cliche.UIElements
{
    /// <summary>
    /// A integer selector that can step through a range.
    /// </summary>
    public class IntSelector : VisualElement
    {
        private int startingValue { get; set; } = 0;
        private int min { get; set; } = 0;
        private int max { get; set; } = 10;

        private int _value = 0;

        /// <summary>
        /// The current selected value.
        /// </summary>
        public int value
        {
            get { return _value; }
            set
            {
                _value = value;
                KeepValueInConstraints();
                UpdateValue();
            }
        }

        /// <summary>
        /// The field's minimum value.
        /// </summary>
        public int lowValue
        {
            get { return min; }
            set
            {
                this.min = value;
                KeepValueInConstraints();
            }
        }

        /// <summary>
        /// The field's maximum value.
        /// </summary>
        public int highValue
        {
            get { return max; }
            set
            {
                this.max = value;
                KeepValueInConstraints();
            }
        }

        /// <summary>
        /// Event that occurs when the field's value changes.
        /// </summary>
        public Action<int> OnValueChange;

        /// <summary>
        /// Event that occurs when the field's value increases.
        /// </summary>
        public Action OnValueIncrease;

        /// <summary>
        /// Event that occurs when the field's value decreases.
        /// </summary>
        public Action OnValueDecrease;

        /// <summary>
        /// The label that displays the selected option.
        /// </summary>
        private Label _valueLabel;

        public new class UxmlFactory : UxmlFactory<IntSelector, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {

            UxmlIntAttributeDescription _valueAttribute = new UxmlIntAttributeDescription { name = "starting-value", defaultValue = 0 };
            UxmlIntAttributeDescription _minAttribute = new UxmlIntAttributeDescription { name = "min", defaultValue = 0 };
            UxmlIntAttributeDescription _maxAttribute = new UxmlIntAttributeDescription { name = "max", defaultValue = 10 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var _is = ((IntSelector)ve);
                _is.startingValue = _valueAttribute.GetValueFromBag(bag, cc);
                _is.value = _is.startingValue;
                _is.min = _minAttribute.GetValueFromBag(bag, cc);
                _is.max = _maxAttribute.GetValueFromBag(bag, cc);
            }
        }

        public IntSelector()
        {
            GenerateStructure();
        }

        private void KeepValueInConstraints()
        {
            if (value < min)
            {
                _value = min;
            }
            if (value > max)
            {
                _value = max;
            }
        }

        private void GenerateStructure()
        {
            name = "IntSelector";
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Stretch;
            style.justifyContent = Justify.SpaceBetween;
            AddToClassList("int-selector");

            var valueDecreaseButton = new Button()
            {
                text = "-",
                name = "valueDecrease",
                style =
                {
                    width = Length.Percent(15),
                    height = Length.Percent(100),
                    fontSize = style.fontSize,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            valueDecreaseButton.AddToClassList("valueDecrease");
            valueDecreaseButton.AddToClassList("valueChangeButton");
            valueDecreaseButton.clicked += () => {
                if (_value - 1 >= lowValue)
                {
                    _value--;
                    UpdateValue();
                    if (OnValueDecrease != null)
                    {
                        OnValueDecrease.Invoke();
                    }
                }
            };

            _valueLabel = new Label()
            {
                name = "valueLabel",
                text = min.ToString(),
                style =
                {
                    fontSize = style.fontSize,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            _valueLabel.AddToClassList("valueLabel");

            var valueIncreaseButton = new Button()
            {
                text = "+",
                name = "valueIncrease",
                style =
                {
                    width = Length.Percent(15),
                    height = Length.Percent(100),
                    fontSize = style.fontSize,
                    unityTextAlign = TextAnchor.MiddleCenter,
                },
            };
            valueIncreaseButton.AddToClassList("valueIncrease");
            valueIncreaseButton.AddToClassList("valueChangeButton");
            valueIncreaseButton.clicked += () => {
                if (_value + 1 <= highValue)
                {
                    _value++;
                    UpdateValue();
                    if (OnValueIncrease != null)
                    {
                        OnValueIncrease.Invoke();
                    }
                }
            };

            Add(valueDecreaseButton);
            Add(_valueLabel);
            Add(valueIncreaseButton);
        }

        private void UpdateValue()
        {
            _valueLabel.text = _value.ToString();
            if (OnValueChange != null)
            {
                OnValueChange.Invoke(value);
            }
        }
    }

}