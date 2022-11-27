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
    private Label _raceDescriptionText;

    private void Start()
    {
        ShowView();
    }

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.width = Length.Percent(100);
        GetViewContainer().style.height = Length.Percent(100);
        
        var iconSize = 400;
        
        var raceManifests = Resources.LoadAll<Race>(Manifests.Paths[typeof(Race)]);
        var racesContainer = Navigator.Target.rootVisualElement.Q<VisualElement>("RaceContainer");
        _raceDescriptionText = Navigator.Target.rootVisualElement.Q<Label>("RaceText");
        foreach (var raceManifest in raceManifests)
        {
            var raceSelectButton = new Button()
            {
                name = raceManifest.Name,
                text = "",
                style = {
                    height = iconSize,
                    width = iconSize,
                    backgroundImage = raceManifest.Icon.texture
                }
            };
            // TODO: selection visual feedback
            raceSelectButton.clicked += () => {
                UpdatePlayerRace(raceManifest.Name);
                _raceDescriptionText.text = raceManifest.Description;
            };
            racesContainer.Add(raceSelectButton);
        }
        var continueButton = Navigator.Target.rootVisualElement.Q<Button>("ContinueButton");
        continueButton.clicked += () => { 
            Navigator.ShowView("CustomisationScreen");
            HideView();
        };
        _raceDescriptionText.text = raceManifests[0].Description;
    }

    private void UpdatePlayerRace(string raceID)
    {
        GameObject.Find("CharacterCustomisation").GetComponent<CustomisationWindow>().PlayerRace = GetEnumValueFromString<Races>(raceID);
    }

    private T GetEnumValueFromString<T>(string enumName) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), enumName);
    }
}
