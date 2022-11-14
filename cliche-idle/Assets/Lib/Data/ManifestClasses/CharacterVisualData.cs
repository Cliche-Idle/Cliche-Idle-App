using System;
using UnityEngine;

[Serializable]
public class CharacterVisualData
{
    public string Name = "";
    /// <summary>
    /// The player character's race.
    /// </summary>
    public Races Race = Races.Human;

    public PlayerBodyTypes BodyType = PlayerBodyTypes.Feminine;
    /// <summary>
    /// The player character's skin color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 SkinColor = Color.clear;

    public PlayerHairStyles HairStyle;
    /// <summary>
    /// The player character's hair color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 HairColor = Color.black;

    public PlayerBeardStyles BeardStyle;
    /// <summary>
    /// The player character's beard color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 BeardColor = Color.black;
    
    public PlayerBrowStyles BrowStyle;
    /// <summary>
    /// The player character's brow color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 BrowColor = Color.black;

    /// <summary>
    /// The player character's primary (iris) eye color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 EyeColorPrimary = Color.black;
    /// <summary>
    /// The player character's secondary (sclera) eye color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 EyeColorSecondary = Color.white;
}