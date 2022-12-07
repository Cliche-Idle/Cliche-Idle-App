using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;
using Cliche.System;
using Cliche.UIElements;

public class CustomisationWindow : UIScript
{
    public CharacterHandler CharacterHandler;

    private enum CharacterCreatorOptionsMode
    {
        body,
        eyes,
        hair,
        beard
    }

    private CharacterDisplay _characterDisplay;

    private OptionSelector _optionSelector;

    private HSVColorPicker _colorPicker;

    private CharacterCreatorOptionsMode _pickingMode;

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.width = Length.Percent(100);
        GetViewContainer().style.height = Length.Percent(100);

        _characterDisplay = Navigator.Target.rootVisualElement.Q<CharacterDisplay>("CharacterDisplay");
        _characterDisplay.Race = CharacterHandler.CharacterSheet.Race;

        var bodyButton = Navigator.Target.rootVisualElement.Q<Button>("BodyOption");
        bodyButton.clicked += BodyTabOpen;

        var eyesButton = Navigator.Target.rootVisualElement.Q<Button>("EyesOption");
        eyesButton.clicked += EyesTabOpen;

        var hairButton = Navigator.Target.rootVisualElement.Q<Button>("HairOption");
        hairButton.clicked += HairTabOpen;

        var beardButton = Navigator.Target.rootVisualElement.Q<Button>("BeardOption");
        beardButton.clicked += BeardTabOpen;

        _optionSelector = Navigator.Target.rootVisualElement.Q<OptionSelector>("MainOptionsSelector");
        _optionSelector.SelectionChange += (option) => {
            switch (_pickingMode)
            {
                case CharacterCreatorOptionsMode.body:
                    _characterDisplay.BodyType = GetEnumValueFromString<PlayerBodyTypes>(option);
                    break;
                case CharacterCreatorOptionsMode.hair:
                    _characterDisplay.HairStyle = GetEnumValueFromString<PlayerHairStyles>(option);
                    break;
                case CharacterCreatorOptionsMode.beard:
                    _characterDisplay.BeardStyle = GetEnumValueFromString<PlayerBeardStyles>(option);
                    break;
            }
        };


        _colorPicker = Navigator.Target.rootVisualElement.Q<HSVColorPicker>("MainColorSelector");
        _colorPicker.ValueChange += (color) => {
            switch (_pickingMode)
            {
                case CharacterCreatorOptionsMode.body:
                    _characterDisplay.SkinColor = color;
                    break;
                case CharacterCreatorOptionsMode.eyes:
                    _characterDisplay.EyeColorPrimary = color;
                    break;
                case CharacterCreatorOptionsMode.hair:
                    _characterDisplay.HairColor = color;
                    break;
                case CharacterCreatorOptionsMode.beard:
                    _characterDisplay.BeardColor = color;
                    break;
            }
        };
        SetRandomStartingTintColors();
        BodyTabOpen();

        var continueButton = Navigator.Target.rootVisualElement.Q<Button>("Create");
        continueButton.clicked += () => {
            CharacterHandler.CharacterSheet = _characterDisplay.CharacterSheet;
            Navigator.ShowView("ConfirmCharacterPopup");
            HideView();
        };

        var backButton = Navigator.Target.rootVisualElement.Q<Button>("Back");
        backButton.clicked += () => {
            Navigator.ShowView("RaceSelect");
            HideView();
        };
    }

    private void SetRandomStartingTintColors()
    {
        // Set skin colour bases
        Color[] baseSkinTones = new Color[3]
        {
            new Color32(255, 209, 174, 255), // light
            new Color32(141, 85, 36, 255), // dark
            new Color32(224, 172, 105, 255) // middle
        };
        var baseSkinTone = baseSkinTones[UnityEngine.Random.Range(0, 3)];
        _characterDisplay.SkinColor = baseSkinTone;

        // Set hair colour bases
        Color[] baseHairColors = new Color[3]
        {
            new Color32(250, 240, 190, 255), // blonde
            new Color32(150, 75, 0, 255), // brown
            new Color32(0, 28, 28, 255) // dark cyan
        };
        var baseHairColor = baseHairColors[UnityEngine.Random.Range(0, 3)];
        _characterDisplay.HairColor = baseHairColor;
        _characterDisplay.BeardColor = baseHairColor;

        // Set eye colour bases
        Color[] baseEyeColors = new Color[3]
        {
            Color.black,
            new Color32(0, 125, 125, 255),
            Color.cyan
        };
        var baseEyeColor = baseEyeColors[UnityEngine.Random.Range(0, 3)];
        _characterDisplay.EyeColorPrimary = baseEyeColor;
    }

    private void BodyTabOpen()
    {
        _optionSelector.visible = true;
        _pickingMode = CharacterCreatorOptionsMode.body;

        _colorPicker.color = (_characterDisplay.SkinColor);
        _optionSelector.SetOptionsFromEnum<PlayerBodyTypes>();
        _optionSelector.SelectedIndex = (int)_characterDisplay.BodyType;
    }

    private void EyesTabOpen()
    {
        _optionSelector.visible = false;
        _pickingMode = CharacterCreatorOptionsMode.eyes;

        _colorPicker.color = (_characterDisplay.EyeColorPrimary);
    }

    private void HairTabOpen()
    {
        _optionSelector.visible = true;
        _pickingMode = CharacterCreatorOptionsMode.hair;

        _colorPicker.color = (_characterDisplay.HairColor);
        _optionSelector.SetOptionsFromEnum<PlayerHairStyles>();
        _optionSelector.SelectedIndex = (int)_characterDisplay.HairStyle;
    }

    private void BeardTabOpen()
    {
        _optionSelector.visible = true;
        _pickingMode = CharacterCreatorOptionsMode.beard;

        _colorPicker.color = (_characterDisplay.BeardColor);
        _optionSelector.SetOptionsFromEnum<PlayerBeardStyles>();
        _optionSelector.SelectedIndex = (int)_characterDisplay.BeardStyle;
    }

    private T GetEnumValueFromString<T>(string enumName) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), enumName);
    }
}
