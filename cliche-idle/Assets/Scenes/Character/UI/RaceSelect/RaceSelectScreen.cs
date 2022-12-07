using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;
using Cliche.System;
using Cliche.UIElements;

public class RaceSelectScreen : UIScript
{
    public CharacterHandler CharacterHandler;

    public float IconSize = 400;

    private Label _raceDescriptionText;
    private Label _raceStatsText;

    private List<Button> _raceButtons = new List<Button>();

    private Button _continueButton;

    private void Start()
    {
        ShowView();
    }

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.width = Length.Percent(100);
        GetViewContainer().style.height = Length.Percent(100);
        
        var raceManifests = Resources.LoadAll<Race>(Manifests.Paths[typeof(Race)]);
        var racesContainer = GetViewContainer().Q<VisualElement>("RaceContainer");
        _raceDescriptionText = GetViewContainer().Q<Label>("RaceText");
        _raceStatsText = GetViewContainer().Q<Label>("StatsText");

        foreach (var raceManifest in raceManifests)
        {
            var raceSelectButton = new Button()
            {
                name = raceManifest.Name,
                text = "",
                style = {
                    height = IconSize,
                    width = IconSize,
                    backgroundImage = raceManifest.Icon.texture
                }
            };
            raceSelectButton.clicked += () => {
                UpdatePlayerRace(raceManifest.Name);
                _raceDescriptionText.text = raceManifest.Description;
                _raceStatsText.text = $"Strenght: {raceManifest.Strength},   Vitality: {raceManifest.Vitality}\nDexterity: {raceManifest.Dexterity},   Intelligence: {raceManifest.Intelligence}";
                foreach (var button in _raceButtons)
                {
                    if (button.name != raceManifest.Name)
                    {
                        button.style.unityBackgroundImageTintColor = Color.grey;
                    }
                    else
                    {
                        button.style.unityBackgroundImageTintColor = Color.white;
                    }
                }
            };
            _raceButtons.Add(raceSelectButton);
            racesContainer.Add(raceSelectButton);
        }
        _continueButton = GetViewContainer().Q<Button>("ContinueButton");
        _continueButton.clicked += () => { 
            Navigator.ShowView("CustomisationScreen");
            HideView();
        };
        _continueButton.SetEnabled(false);
    }

    private void UpdatePlayerRace(string raceID)
    {
        CharacterHandler.CharacterSheet.Race = GetEnumValueFromString<Races>(raceID);
        _continueButton.SetEnabled(true);
    }

    private T GetEnumValueFromString<T>(string enumName) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), enumName);
    }
}
