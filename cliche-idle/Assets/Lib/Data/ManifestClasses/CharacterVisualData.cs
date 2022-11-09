using System;
using UnityEngine;

[Serializable]
public class CharacterVisualData
{
    public string race = "human";
    public string gender = "female";
    public string hairStyle;
    public Color32 hairColour = Color.black;
    public string beardStyle;
    public Color32 beardColour = Color.black;
    public string eyebrowStyle;
    public Color32 eyebrowColour = Color.black;
    public Color32 eyeColourPrimary = Color.black;
    public Color32 eyeColourSecondary = Color.white;
    public Color32 skinColour = Color.clear;
}