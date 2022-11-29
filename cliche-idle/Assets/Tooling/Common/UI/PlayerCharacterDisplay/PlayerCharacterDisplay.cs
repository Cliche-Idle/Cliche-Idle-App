using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UIViews;
using Cliche.System;
using Cliche.UIElements;

class PlayerCharacterDisplay : VisualElement
{
    private CharacterSheet _playerVisuals = new CharacterSheet();

    //
    public CharacterSheet CharacterSheet
    {
        get { return _playerVisuals; }
        set { 
            _playerVisuals = value; 
            Refresh(); 
        }
    }

    private VisualElement bodyContainer;

    public Races Race
    {
        get { return _playerVisuals.Race; }
        set { 
            _playerVisuals.Race = value;
            SetBodySprites();
        }
    }

    public PlayerBodyTypes BodyType
    {
        get { return _playerVisuals.BodyType; }
        set { 
            _playerVisuals.BodyType = value;
            SetBodySprites();
        }
    }

    public Color32 SkinColor
    {
        get { return _playerVisuals.SkinColor; }
        set { 
            _playerVisuals.SkinColor = value;
            bodyContainer.style.unityBackgroundImageTintColor = (Color)value;
        }
    }

    private VisualElement hair;

    public PlayerHairStyles HairStyle
    {
        get { return _playerVisuals.HairStyle; }
        set { 
            _playerVisuals.HairStyle = value;
            SetHairSprite();
        }
    }

    public Color32 HairColor
    {
        get { return _playerVisuals.HairColor; }
        set { 
            _playerVisuals.HairColor = value;
            hair.style.unityBackgroundImageTintColor = (Color)value;
            brow.style.unityBackgroundImageTintColor = (Color)value;
        }          
    }

    private VisualElement beard;

    public PlayerBeardStyles BeardStyle
    {
        get { return _playerVisuals.BeardStyle; }
        set { 
            _playerVisuals.BeardStyle = value;
            SetBeardSprite();
        }
    }
  
    public Color32 BeardColor
    {
        get { return _playerVisuals.BeardColor; }
        set { 
            _playerVisuals.BeardColor = value;
            beard.style.unityBackgroundImageTintColor = (Color)value;
        }
    }

    private VisualElement brow;

    public PlayerBrowStyles BrowStyle
    {
        get { return _playerVisuals.BrowStyle; }
        set { 
            _playerVisuals.BrowStyle = value;
            SetBrowSprite();
        }
    }

    private VisualElement eyePrimary;

    public Color32 EyeColorPrimary
    {
        get { return _playerVisuals.EyeColorPrimary; }
        set { 
            _playerVisuals.EyeColorPrimary = value;
            eyePrimary.style.unityBackgroundImageTintColor = (Color)value;
        }
    }

    private VisualElement underwear;

    private VisualElement faceContainer;

    //
    private static Dictionary<string, string> _characterAssetPaths = new Dictionary<string, string>()
    {
        // Character body
        { "body", "characterSprites/base/body" },
        { "eye", "characterSprites/base/eye" },
        { "brow", "characterSprites/base/brow" },
        { "beard", "characterSprites/base/beard" },
        { "hair", "characterSprites/base/hair" },
        // Character outfits
        { "outfit", "characterSprites/outfit" },
    };

    private static Dictionary<Races, float> _racialHeightOffsets = new Dictionary<Races, float>()
    {
        { Races.Human, 0 },
        { Races.Elf, 0 },
        { Races.Orc, 0 },
        // This is sketchy as fuck
        { Races.Dwarf, 20.85f },
    };

    public new class UxmlFactory : UxmlFactory<PlayerCharacterDisplay, UxmlTraits> { }

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

    public PlayerCharacterDisplay()
    {
        GenerateStructure();
    }

    private void Refresh()
    {
        Clear();
        GenerateStructure();
    }

    private void GenerateStructure()
    {
        bodyContainer = new VisualElement()
        {
            name = "bodyContainer",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                unityBackgroundImageTintColor = (Color)_playerVisuals.SkinColor,
                unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };

        faceContainer = GenerateFaceStructure();

        bodyContainer.Add(GenerateUnderwearStructure());
        bodyContainer.Add(faceContainer);

        SetBeardSprite();
        SetBrowSprite();
        SetHairSprite();

        SetBodySprites();

        Add(bodyContainer);
    }

    private VisualElement GenerateFaceStructure()
    {
        VisualElement faceContainer = new VisualElement()
        {
            name = "faceContainer",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
            }
        };

        Sprite eyeSprite = GetBaseCharacterSprite("eye", "big_iris");
        eyePrimary = new VisualElement()
        {
            name = "eyePrimary",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                backgroundImage = eyeSprite.texture,
                unityBackgroundImageTintColor = (Color)_playerVisuals.EyeColorPrimary,
                unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };

        Sprite eyeSecondarySprite = GetBaseCharacterSprite("eye", "big_sclera");
        VisualElement eyeSecondary = new VisualElement()
        {
            name = "eyeSecondary",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                backgroundImage = eyeSecondarySprite.texture,
                unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };

        brow = new VisualElement()
        {
            name = "brow",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                unityBackgroundImageTintColor = (Color)_playerVisuals.HairColor,
                unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };

        beard = new VisualElement()
        {
            name = "beard",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                unityBackgroundImageTintColor = (Color)_playerVisuals.BeardColor,
                unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };

        hair = new VisualElement()
        {
            name = "hair",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                unityBackgroundImageTintColor = (Color)_playerVisuals.HairColor,
                unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };

        // Constructs face container
        faceContainer.Add(eyeSecondary);
        faceContainer.Add(eyePrimary);
        faceContainer.Add(brow);
        faceContainer.Add(beard);
        faceContainer.Add(hair);

        return faceContainer;
    }

    private VisualElement GenerateUnderwearStructure()
    {
        VisualElement outfitContainer = new VisualElement()
        {
            name = "outfitContainer",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
            }
        };

        underwear = new VisualElement()
        {
            name = "underwear",
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                unityBackgroundScaleMode = ScaleMode.ScaleToFit
            }
        };

        outfitContainer.Add(underwear);

        return outfitContainer;
    }

    private void SetBrowSprite()
    {
        if (_playerVisuals.Race != Races.Orc)
        {
            brow.style.backgroundImage = GetBaseCharacterSprite("brow", _playerVisuals.BrowStyle).texture;
        }
        else
        {
            brow.style.backgroundImage = null;
        }
    }

    private void SetHairSprite()
    {
        if (_playerVisuals.HairStyle != PlayerHairStyles.None)
        {
            hair.style.backgroundImage = GetBaseCharacterSprite("hair", _playerVisuals.HairStyle, _playerVisuals.Race).texture;
        }
        else
        {
            hair.style.backgroundImage = null;
        }
    }

    private void SetBeardSprite()
    {
        if (_playerVisuals.BeardStyle != PlayerBeardStyles.None)
        {
            beard.style.backgroundImage = GetBaseCharacterSprite("beard", _playerVisuals.BeardStyle).texture;
        }
        else
        {
            beard.style.backgroundImage = null;
        }
    }

    private void SetBodySprites()
    {
        bodyContainer.style.backgroundImage = GetBaseCharacterSprite("body", _playerVisuals.BodyType, _playerVisuals.Race).texture;
        underwear.style.backgroundImage = GetCharacterSprite("outfit", _playerVisuals.BodyType, _playerVisuals.Race, "underwear", "chest").texture;
        if (_playerVisuals.Race != Races.Orc)
        {
            brow.style.backgroundImage = GetBaseCharacterSprite("brow", _playerVisuals.BrowStyle).texture;
        }
        else
        {
            brow.style.backgroundImage = null;
        }
        faceContainer.style.top = Length.Percent(_racialHeightOffsets[_playerVisuals.Race]);
        faceContainer.style.bottom = Length.Percent(-_racialHeightOffsets[_playerVisuals.Race]);
    }

    // These are mostly for the future when the outfit system is decided

    private Sprite GetCharacterSprite(string type, string styleName, Enum race = null, params string[] overrides)
    {
        string cleanStyleName = styleName.Trim().ToLower();
        Sprite characterAsset = null;
        if (overrides.Length != 0)
        {
            string cleanOverrideNames = String.Join("/", overrides.Select( element => element.Trim().ToLower()));
            string assetPath = "";
            if (race != null)
            {
                string raceString = race.ToString().ToLower();
                assetPath = $"{_characterAssetPaths[type]}/{cleanOverrideNames}/{raceString}/{cleanStyleName}";
                characterAsset = Resources.Load<Sprite>(assetPath);
            }

            if (characterAsset == null)
            {
                // If no override was found, fall back to the default one
                assetPath = $"{_characterAssetPaths[type]}/{cleanOverrideNames}/{cleanStyleName}";
                characterAsset = Resources.Load<Sprite>(assetPath);
            }
        }
        
        if (characterAsset == null)
        {
            // If no override was found, fall back to the default one
            string assetPath = $"{_characterAssetPaths[type]}/{cleanStyleName}";
            characterAsset = Resources.Load<Sprite>(assetPath);
        }
        return characterAsset;
    }

    private Sprite GetCharacterSprite(string type, Enum styleName, Enum race = null, params object[] overrides)
    {
        string styleNameString = styleName.ToString();
        string[] overrideStrings = Array.ConvertAll(overrides, element => element.ToString());
        return GetCharacterSprite(type, styleNameString, race, overrideStrings);
    }

    private Sprite GetBaseCharacterSprite(string type, string styleName, Enum race = null)
    {
        string cleanStyleName = styleName.Trim().ToLower();
        Sprite characterAsset = null;
        if (race != null)
        {
            string cleanOverrideName = race.ToString().ToLower();
            string assetPath = $"{_characterAssetPaths[type]}/{cleanOverrideName}/{cleanStyleName}";
            characterAsset = Resources.Load<Sprite>(assetPath);
        }

        if (characterAsset == null)
        {
            // If no override was found, fall back to the default one
            string assetPath = $"{_characterAssetPaths[type]}/{cleanStyleName}";
            characterAsset = Resources.Load<Sprite>(assetPath);
        }
        return characterAsset;
    }

    private Sprite GetBaseCharacterSprite(string type, Enum styleName, Enum race = null)
    {
        string cleanStyleName = styleName.ToString().Trim().ToLower();
        return GetBaseCharacterSprite(type, cleanStyleName, race);
    }
}
