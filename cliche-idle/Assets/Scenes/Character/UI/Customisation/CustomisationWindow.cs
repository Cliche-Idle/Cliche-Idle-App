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
    public CharacterVisualData PlayerCharacterData = new CharacterVisualData();

    private enum CharacterCreatorOptionsMode
    {
        body,
        eyes,
        hair,
        beard
    }

    private OptionSelector optionSelector;

    private HSVColorPicker colorPicker;

    private CharacterCreatorOptionsMode pickingMode;

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.width = Length.Percent(100);
        GetViewContainer().style.height = Length.Percent(100);

        var bodyButton = Navigator.Target.rootVisualElement.Q<Button>("BodyOption");
        bodyButton.clicked += BodyTabOpen;

        var eyesButton = Navigator.Target.rootVisualElement.Q<Button>("EyesOption");
        eyesButton.clicked += EyesTabOpen;

        var hairButton = Navigator.Target.rootVisualElement.Q<Button>("HairOption");
        hairButton.clicked += HairTabOpen;

        var beardButton = Navigator.Target.rootVisualElement.Q<Button>("BeardOption");
        beardButton.clicked += BeardTabOpen;

        optionSelector = Navigator.Target.rootVisualElement.Q<OptionSelector>("MainOptionsSelector");
        optionSelector.SelectionChange += (option) => {
            switch (pickingMode)
            {
                case CharacterCreatorOptionsMode.body:
                    PlayerCharacterData.BodyType = GetEnumValueFromString<PlayerBodyTypes>(option);
                    break;
                case CharacterCreatorOptionsMode.eyes:

                    break;
                case CharacterCreatorOptionsMode.hair:
                    PlayerCharacterData.HairStyle = GetEnumValueFromString<PlayerHairStyles>(option);
                    break;
                case CharacterCreatorOptionsMode.beard:
                    PlayerCharacterData.BeardStyle = GetEnumValueFromString<PlayerBeardStyles>(option);
                    break;
            }
        };


        colorPicker = Navigator.Target.rootVisualElement.Q<HSVColorPicker>("MainColorSelector");
        colorPicker.ValueChange += (color) => {
            switch (pickingMode)
            {
                case CharacterCreatorOptionsMode.body:
                    PlayerCharacterData.SkinColor = color;
                    break;
                case CharacterCreatorOptionsMode.eyes:
                    PlayerCharacterData.EyeColorPrimary = color;
                    break;
                case CharacterCreatorOptionsMode.hair:
                    PlayerCharacterData.HairColor = color;
                    break;
                case CharacterCreatorOptionsMode.beard:
                    PlayerCharacterData.BeardColor = color;
                    break;
            }
        };
        SetRandomStartingTintColors();
        BodyTabOpen();

        var continueButton = Navigator.Target.rootVisualElement.Q<Button>("Create");
        continueButton.clicked += () => {  };

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
            new Color32(255, 209, 174, 255), // white
            new Color32(141, 85, 36, 255), // dark
            new Color32(224, 172, 105, 255) // middle
        };
        var baseSkinTone = baseSkinTones[UnityEngine.Random.Range(0, 3)];
        PlayerCharacterData.SkinColor = baseSkinTone;

        // Set hair colour bases
        Color[] baseHairColors = new Color[3]
        {
            new Color32(250, 240, 190, 255), // blonde
            new Color32(150, 75, 0, 255), // brown
            new Color32(0, 28, 28, 255) // dark cyan
        };
        var baseHairColor = baseHairColors[UnityEngine.Random.Range(0, 3)];
        PlayerCharacterData.HairColor = baseHairColor;
        PlayerCharacterData.BeardColor = baseHairColor;

        // Set eye colour bases
        Color[] baseEyeColors = new Color[3]
        {
            Color.black,
            new Color32(0, 125, 125, 255),
            Color.cyan
        };
        var baseEyeColor = baseEyeColors[UnityEngine.Random.Range(0, 3)];
        PlayerCharacterData.EyeColorPrimary = baseEyeColor;
    }

    private void BodyTabOpen()
    {
        optionSelector.visible = true;
        pickingMode = CharacterCreatorOptionsMode.body;

        colorPicker.color = (PlayerCharacterData.SkinColor);
        optionSelector.SetOptionsFromEnum<PlayerBodyTypes>();
        optionSelector.SelectedIndex = (int)PlayerCharacterData.BodyType;
    }

    private void EyesTabOpen()
    {
        optionSelector.visible = false;
        pickingMode = CharacterCreatorOptionsMode.eyes;

        colorPicker.color = (PlayerCharacterData.EyeColorPrimary);
    }

    private void HairTabOpen()
    {
        optionSelector.visible = true;
        pickingMode = CharacterCreatorOptionsMode.hair;

        colorPicker.color = (PlayerCharacterData.HairColor);
        optionSelector.SetOptionsFromEnum<PlayerHairStyles>();
        optionSelector.SelectedIndex = (int)PlayerCharacterData.HairStyle;
    }

    private void BeardTabOpen()
    {
        optionSelector.visible = true;
        pickingMode = CharacterCreatorOptionsMode.beard;

        colorPicker.color = (PlayerCharacterData.BeardColor);
        optionSelector.SetOptionsFromEnum<PlayerBeardStyles>();
        optionSelector.SelectedIndex = (int)PlayerCharacterData.BeardStyle;
    }

    private T GetEnumValueFromString<T>(string enumName) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), enumName);
    }
}
