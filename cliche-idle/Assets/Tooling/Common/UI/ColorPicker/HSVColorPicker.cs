using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cliche.UIElements
{
    /// <summary>
    /// A 3 <see cref="FloatSlider"/> based Hue - Saturation - Luminescence colour picker.
    /// </summary>
    public class HSVColorPicker : VisualElement
    {
        /// <summary>
        /// The currntly selected colour.
        /// </summary>
        public Color SelectedColor { get; private set; }
        /// <summary>
        /// Event that occurs when the selected colour changes.
        /// </summary>
        public Action<Color> ValueChange;

        private FloatSlider _hue;
        private FloatSlider _saturation;
        private FloatSlider _luminescence;

        public new class UxmlFactory : UxmlFactory<HSVColorPicker, UxmlTraits> { }

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

        public HSVColorPicker()
        {
            GenerateStructure();
        }

        /// <summary>
        /// Sets the colour sliders and <see cref="SelectedColor"/> to the given colour.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            float hue = 0;
            float sat = 0;
            float lum = 0;
            Color.RGBToHSV(color, out hue, out sat, out lum);
            _hue.value = hue;
            _saturation.value = sat;
            _luminescence.value = lum;
        }

        private void GenerateStructure()
        {
            name = "HSVColorPicker";
            style.flexDirection = FlexDirection.Column;
            style.alignItems = Align.Stretch;
            style.justifyContent = Justify.SpaceAround;

            Sprite hueSprite = Resources.Load<Sprite>("HSVColorPicker_scales/hue");
            Sprite satSprite = Resources.Load<Sprite>("HSVColorPicker_scales/sat");
            Sprite lumSprite = Resources.Load<Sprite>("HSVColorPicker_scales/lum");

            _hue = new FloatSlider()
            {
                name = "hueSlider",
                value = 0.5f,
                lowValue = 0f,
                highValue = 1f,
                style =
            {
                height = Length.Percent(20),
                backgroundImage = hueSprite.texture,
                unityBackgroundScaleMode = ScaleMode.StretchToFill
            }
            };
            _hue.ValueChange += (val) => {
                UpdateSelection();
                _saturation.style.unityBackgroundImageTintColor = Color.HSVToRGB(val, 1, 1);
            };

            _saturation = new FloatSlider()
            {
                name = "satSlider",
                value = 1f,
                lowValue = 0f,
                highValue = 1f,
                style =
            {
                height = Length.Percent(20),
                // Background colour is set to white so it stays that way on the left side of the slider
                // where saturation would wash it out completely. The background image is tinted with the
                // hue colour, where white is overriden with the full colour, so this "fakes" the correct look.
                backgroundColor = Color.white,
                backgroundImage = satSprite.texture,
                unityBackgroundImageTintColor = Color.HSVToRGB(_hue.value, 1, 1),
                unityBackgroundScaleMode= ScaleMode.StretchToFill
            }
            };
            _saturation.ValueChange += (val) => {
                UpdateSelection();
            };

            _luminescence = new FloatSlider()
            {
                name = "lumSlider",
                value = 1f,
                lowValue = 0f,
                highValue = 1f,
                style =
            {
                height = Length.Percent(20),
                backgroundImage = lumSprite.texture,
                unityBackgroundScaleMode = ScaleMode.StretchToFill
            }
            };
            _luminescence.ValueChange += (val) => {
                UpdateSelection();
            };

            Add(_hue);
            Add(_saturation);
            Add(_luminescence);
            SelectedColor = Color.HSVToRGB(_hue.value, _saturation.value, _luminescence.value, false);
        }

        private void UpdateSelection()
        {
            SelectedColor = Color.HSVToRGB(_hue.value, _saturation.value, _luminescence.value, false);

            if (ValueChange != null)
            {
                ValueChange.Invoke(SelectedColor);
            }
        }
    }

}