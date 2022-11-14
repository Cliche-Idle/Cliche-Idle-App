using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;
using Cliche.System;

public class CharacterDisplay : MonoBehaviour
{
    public ViewNavigator Navigator;

    // Visual data
    public CharacterVisualData PlayerCharacterData = new CharacterVisualData();

    public VisualTreeAsset PlayerCharacterDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: generate UI structure on startup
        //GenerateCharacterDisplayStructure();

        // TODO: query and load in asset lists

        // TODO: auto pull character data if available?

        // Auto override for race specific styles

        var nameField = Navigator.Target.rootVisualElement.Q<TextField>("Name");
        nameField.RegisterValueChangedCallback(fieldVal => UpdatePlayerName(fieldVal.newValue));

        var raceManifests = Resources.LoadAll<Race>(Manifests.Paths[typeof(Race)]);
        var racesContainer = Navigator.Target.rootVisualElement.Q<VisualElement>("RaceContainer");
        foreach (var raceManifest in raceManifests)
        {
            var raceSelectButton = new Button()
            {
                name = raceManifest.Name,
                text = "",
                style = {
                    height = 150,
                    width = 150,
                    backgroundImage = raceManifest.Icon.texture
                }
            };
            raceSelectButton.clicked += () => { 
                UpdatePlayerRace(raceManifest.Name);
                Navigator.Target.rootVisualElement.Q<Label>("RaceText").text = raceManifest.Description;
            };
            racesContainer.Add(raceSelectButton);
        }

        var genderSelector = Navigator.Target.rootVisualElement.Q<OptionSelector>("GenderSelector");
        genderSelector.SetOptionsFromEnum<PlayerBodyTypes>();
        genderSelector.SelectionChange += (sender, selection) => { UpdatePlayerGender(selection); };
        
        // This is horrible, change it to auto load races and assign this automatically
        //Navigator.Target.rootVisualElement.Q<Button>("OrcBtn").clicked += () => { UpdatePlayerRace("Orc"); };
        //Navigator.Target.rootVisualElement.Q<Button>("HumanBtn").clicked += () => { UpdatePlayerRace("Human"); };
        //Navigator.Target.rootVisualElement.Q<Button>("ElfBtn").clicked += () => { UpdatePlayerRace("Elf"); };
        //Navigator.Target.rootVisualElement.Q<Button>("DwarfBtn").clicked += () => { UpdatePlayerRace("Dwarf"); };
    }

    private void UpdatePlayerName(string name)
    {
        PlayerCharacterData.Name = name;
    }

    private void UpdatePlayerRace(string raceID)
    {
        PlayerCharacterData.Race = GetEnumValueFromString<Races>(raceID);
    }

    private void UpdatePlayerGender(string genderID)
    {
        PlayerCharacterData.BodyType = GetEnumValueFromString<PlayerBodyTypes>(genderID);
    }

    private T GetEnumValueFromString<T>(string enumName) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), enumName);
    }

    /// <summary>
    /// Gets the specified default character sprite. To get the alternat version, set <paramref name="styleName"/> to the override identifier. If no override version is found, returns default.
    /// </summary>
    /// <param name="styleName">The default, generic identifier of the sprite.</param>
    /// <param name="overrideName">The optional, override variant identifier of the sprite.</param>
    /// <returns></returns>
    private Sprite GetCharacterSprite(string styleName, string overrideName="")
    {
        // This assumes the assets used for this can be uniquely identified among all other assets by their name.
        Sprite sprite = null;
        if (overrideName.Length > 0)
        {
            sprite = Resources.Load<Sprite>($"{overrideName}_{styleName}");
        }
        if (sprite == null)
        {
            // If no override was found, load the default one
            sprite = Resources.Load<Sprite>($"{styleName}");
        }
        return sprite;
    }
}
