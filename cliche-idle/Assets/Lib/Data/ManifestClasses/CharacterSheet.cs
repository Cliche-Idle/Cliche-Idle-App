using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class CharacterSheet
{
    /// <summary>
    /// The character's in-game display name.
    /// </summary>
    public string Name = "";
    /// <summary>
    /// The character's race.
    /// </summary>
    public Races Race = Races.Human;

    /// <summary>
    /// The character's body type.
    /// </summary>
    public PlayerBodyTypes BodyType = PlayerBodyTypes.Feminine;
    
    /// <summary>
    /// The character's skin color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 SkinColor = new Color32(255, 209, 174, 255);

    public PlayerHairStyles HairStyle = PlayerHairStyles.Medium;
    /// <summary>
    /// The character's hair color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 HairColor = new Color32(150, 75, 0, 255);

    public PlayerBeardStyles BeardStyle = PlayerBeardStyles.None;
    /// <summary>
    /// The character's beard color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 BeardColor = new Color32(150, 75, 0, 255);
    
    public PlayerBrowStyles BrowStyle = PlayerBrowStyles.Default;

    /// <summary>
    /// The character's primary (iris) eye color. This is applied over the base sprite as a tint.
    /// </summary>
    public Color32 EyeColorPrimary = new Color32(0, 125, 125, 255);
}