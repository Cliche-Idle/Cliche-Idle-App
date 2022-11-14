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

        Debug.Log($"Gender scale max: 0 <-> {GetEnumRange<PlayerBodyTypes>()}");
        Debug.Log($"Gender string from index 0: {GetEnumStringFromIndex<PlayerBodyTypes>(0)}");

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
        genderSelector.Options = GetEnumNames<PlayerBodyTypes>();
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
        PlayerCharacterData.Race = (Races)Enum.Parse(typeof(Races), raceID);
    }

    private void UpdatePlayerGender(string genderID)
    {
        PlayerCharacterData.Bodytype = (PlayerBodyTypes)Enum.Parse(typeof(PlayerBodyTypes), genderID);
    }

    private List<string> GetEnumNames<T>() where T : Enum
    {
        var names = Enum.GetNames(typeof(T));
        return new List<string>(names);
    }

    private int GetEnumRange<T>() where T : Enum
    {
        // Inherently unsafe but this is based on the assumption that the character style enums are auto indexed
        // so they are always starting from 0 -> whatever.
        int enumMax = ((int[])Enum.GetValues(typeof(T))).Max();
        return enumMax;
    }

    private string GetEnumStringFromIndex<T>(int index) where T : Enum
    {
        string enumString = Enum.GetName(typeof(T), index);
        return enumString;
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
