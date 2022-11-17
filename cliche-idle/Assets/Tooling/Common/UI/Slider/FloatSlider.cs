using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cliche.UIElements
{
    /// <summary>
    /// Custom replacement for <see cref="Slider"/> that not only works but can also be formatted.
    /// </summary>
    public class FloatSlider : VisualElement
    {
        private float startingValue { get; set; } = 0f;
        private float min { get; set; } = 0f;
        private float max { get; set; } = 10f;

        private bool _isDragging = false;

        private int _capturedPointerId = 0;

        private float _value = 0f;

        /// <summary>
        /// The current slider value.
        /// </summary>
        public float value
        {
            get { return _value; }
            set
            {
                _value = value;
                SetDraggerPositonFromValue(value);
                UpdateValue();
            }
        }

        /// <summary>
        /// The slider's minimum value.
        /// </summary>
        public float lowValue
        {
            get { return min; }
            set
            {
                this.min = value;
                SetDraggerPositonFromValue(_value);
            }
        }

        /// <summary>
        /// The slider's maximum value.
        /// </summary>
        public float highValue
        {
            get { return max; }
            set
            {
                this.max = value;
                SetDraggerPositonFromValue(_value);
            }
        }


        /// <summary>
        /// Event that occurs when the slider's value changes.
        /// </summary>
        public Action<float> ValueChange;

        private VisualElement _dragger;

        public new class UxmlFactory : UxmlFactory<FloatSlider, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {

            UxmlFloatAttributeDescription _valueAttribute = new UxmlFloatAttributeDescription { name = "starting-value", defaultValue = 0 };
            UxmlFloatAttributeDescription _minAttribute = new UxmlFloatAttributeDescription { name = "min", defaultValue = 0 };
            UxmlFloatAttributeDescription _maxAttribute = new UxmlFloatAttributeDescription { name = "max", defaultValue = 10 };

            private FloatSlider _fs;

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                _fs = ((FloatSlider)ve);
                _fs.startingValue = _valueAttribute.GetValueFromBag(bag, cc);
                _fs.value = _fs.startingValue;
                _fs.min = _minAttribute.GetValueFromBag(bag, cc);
                _fs.max = _maxAttribute.GetValueFromBag(bag, cc);
            }
        }

        public FloatSlider()
        {
            GenerateStructure();
            if (float.IsNaN(resolvedStyle.width) == false && float.IsNaN(_dragger.resolvedStyle.width) == false)
            {
                SetDraggerPositonFromValue(value);
            }
            else
            {
                _dragger.RegisterCallback<GeometryChangedEvent>(PlaceInitialDragger);
            }
        }

        private void PlaceInitialDragger(GeometryChangedEvent evt)
        {
            _dragger.UnregisterCallback<GeometryChangedEvent>(PlaceInitialDragger);
            SetDraggerPositonFromValue(_value);
        }

        private void GenerateStructure()
        {
            name = "FloatSlider";
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;

            style.backgroundColor = Color.gray;
            style.width = Length.Percent(100);
            style.borderLeftWidth = 2;
            style.borderRightWidth = 2;
            style.borderTopWidth = 2;
            style.borderBottomWidth = 2;
            style.borderLeftColor = Color.black;
            style.borderRightColor = Color.black;
            style.borderTopColor = Color.black;
            style.borderBottomColor = Color.black;
            style.borderTopLeftRadius = 2;
            style.borderBottomLeftRadius = 2;
            style.borderTopRightRadius = 2;
            style.borderBottomRightRadius = 2;
            AddToClassList("floatSlider");

            _dragger = new VisualElement()
            {
                name = $"{name}__dragger",
                style =
            {
                height = Length.Percent(100),
                width = 30,
                backgroundColor = Color.white,
                borderLeftWidth = 2,
                borderRightWidth = 2,
                borderTopWidth = 2,
                borderBottomWidth = 2,
                borderLeftColor = Color.black,
                borderRightColor = Color.black,
                borderTopColor = Color.black,
                borderBottomColor = Color.black,
                borderTopLeftRadius = 2,
                borderBottomLeftRadius = 2,
                borderTopRightRadius = 2,
                borderBottomRightRadius = 2,
            }
            };
            _dragger.AddToClassList("floatSlider_dragger");
            RegisterCallback<PointerDownEvent>(e => {
                UpdateState(e.localPosition.x);
                // Capture pointer so PointerMoveEvent keeps updating even if it leaves the area when dragging the notch.
                // Realeases on PointerUpEvent
                if (!_isDragging)
                {
                    _capturedPointerId = e.pointerId;
                    PointerCaptureHelper.CapturePointer(this, _capturedPointerId);
                }
                _isDragging = true;
            });
            RegisterCallback<PointerUpEvent>(e => {
                _isDragging = false;
                PointerCaptureHelper.ReleasePointer(this, _capturedPointerId);
            });
            RegisterCallback<PointerMoveEvent>(e => {
                if (_isDragging)
                {
                    if (e.pointerId == _capturedPointerId)
                    {
                        UpdateState(e.localPosition.x);
                    }
                }
            });
            Add(_dragger);
        }

        private void UpdateState(float pointerPositionX)
        {
            SetDraggerPosition(pointerPositionX);
            value = GetValueFromPointerPosition(pointerPositionX);
            UpdateValue();
        }

        private float GetValueFromPointerPosition(float pointerPosX)
        {
            if (pointerPosX < 0)
            {
                pointerPosX = 0;
            }
            if (pointerPosX > resolvedStyle.width)
            {
                pointerPosX = resolvedStyle.width;
            }
            var xMax = resolvedStyle.width;
            var valueRange = max - min;
            var scaledVal = pointerPosX / xMax;
            var currVal = valueRange * scaledVal;
            return currVal;
        }

        private void SetDraggerPosition(float positionX)
        {
            var widthBuffer = (_dragger.resolvedStyle.width / 2);
            var draggerXVal = positionX - widthBuffer;
            if (draggerXVal < 0)
            {
                draggerXVal = 0;
            }
            if ((positionX + widthBuffer) > resolvedStyle.width)
            {
                draggerXVal = resolvedStyle.width - _dragger.resolvedStyle.width;
            }
            _dragger.transform.position = new Vector3(draggerXVal, _dragger.transform.position.y);
        }

        private void SetDraggerPositonFromValue(float val)
        {
            if (val < min)
            {
                val = min;
            }
            if (val > max)
            {
                val = max;
            }

            var valueRange = max - min;
            var xMax = resolvedStyle.width;

            var scaledVal = val / valueRange;
            var position = xMax * scaledVal;

            SetDraggerPosition(position);
        }

        private void UpdateValue()
        {
            if (ValueChange != null)
            {
                ValueChange.Invoke(value);
            }
        }
    }
}
