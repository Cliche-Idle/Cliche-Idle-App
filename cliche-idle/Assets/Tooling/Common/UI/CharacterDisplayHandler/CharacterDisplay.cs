using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    // Renderers
    public SpriteRenderer hair;
    public SpriteRenderer beard;
    public SpriteRenderer eyebrow;
    public SpriteRenderer eyePrimary;
    public SpriteRenderer eyeSecondary;
    public SpriteRenderer skin;
    public SpriteRenderer underwear;

    // Visual data
    public CharacterVisualData CharData = new CharacterVisualData();

    // Start is called before the first frame update
    void Start()
    {
        // TODO: generate UI structure on startup
        GenerateCharacterDisplayStructure();

        // TODO: query and load in asset lists
        CharData.gender = "female";

        // TODO: auto pull character data if available?
        // Auto override for race specific styles
    }

    private void GenerateCharacterDisplayStructure()
    {
        GameObject hair_obj = new GameObject("hair");
        hair_obj.AddComponent<SpriteRenderer>();
        hair_obj.transform.parent = gameObject.transform;
        hair = hair_obj.GetComponent<SpriteRenderer>();

        GameObject beard_obj = new GameObject("beard");
        beard_obj.AddComponent<SpriteRenderer>();
        beard_obj.transform.parent = gameObject.transform;
        beard = beard_obj.GetComponent<SpriteRenderer>();

        GameObject eyebrow_obj = new GameObject("eyebrow");
        eyebrow_obj.AddComponent<SpriteRenderer>();
        eyebrow_obj.transform.parent = gameObject.transform;
        eyebrow = eyebrow_obj.GetComponent<SpriteRenderer>();

        GameObject eyePrimary_obj = new GameObject("eyePrimary");
        eyePrimary_obj.AddComponent<SpriteRenderer>();
        eyePrimary_obj.transform.parent = gameObject.transform;
        eyePrimary = eyePrimary_obj.GetComponent<SpriteRenderer>();

        GameObject eyeSecondary_obj = new GameObject("eyeSecondary");
        eyeSecondary_obj.AddComponent<SpriteRenderer>();
        eyeSecondary_obj.transform.parent = gameObject.transform;
        eyeSecondary = eyeSecondary_obj.GetComponent<SpriteRenderer>();

        GameObject skin_obj = new GameObject("skin");
        skin_obj.AddComponent<SpriteRenderer>();
        skin_obj.transform.parent = gameObject.transform;
        skin = skin_obj.GetComponent<SpriteRenderer>();

        GameObject underwear_obj = new GameObject("underwear");
        underwear_obj.AddComponent<SpriteRenderer>();
        underwear_obj.transform.parent = gameObject.transform;
        underwear = underwear_obj.GetComponent<SpriteRenderer>();
    }

    private Sprite LoadSpriteWithOverride(string styleName, string overrideName="")
    {
        // ! This assumes the assets used for this can be uniquely identified among all other assets by their name.
        Sprite sprite = null;
        if (overrideName.Length > 0)
        {
            sprite = Resources.Load<Sprite>($"{overrideName}_{styleName}");
        }
        if (sprite == null)
        {
            // Fall back to default:
            sprite = Resources.Load<Sprite>($"{styleName}");
        }
        return sprite;
    }

    public string race {
        get 
        {
            return CharData.race;
        }
        set
        {
            CharData.race = value;
            skin.sprite = LoadSpriteWithOverride($"{value}_{CharData.gender}");
        }
    }

    public string gender {
        get 
        {
            return CharData.gender;
        }
        set
        {
            CharData.gender = value;
            skin.sprite = LoadSpriteWithOverride($"{CharData.race}_{value}");
        }
    }

    public string hairStyle {
        get 
        {
            return CharData.hairStyle;
        }
        set
        {
            CharData.hairStyle = value;
            hair.sprite = LoadSpriteWithOverride(value, CharData.race);
        }
    }

    public Color hairColour {
        get 
        {
            return CharData.hairColour;
        }
        set
        {
            CharData.hairColour = value;
            hair.color = value;
        }
    }

    public string beardStyle;

    public Color beardColour {
        get 
        {
            return CharData.beardColour;
        }
        set
        {
            CharData.beardColour = value;
            beard.color = value;
        }
    }
    
    public string eyebrowStyle;
    public Color eyebrowColour;
    public Color eyeColourPrimary;
    public Color eyeColourSecondary;
    public Color skinColour;
}
