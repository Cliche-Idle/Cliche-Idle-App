using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UIViews;

public class PlayerBar : UIScript
{
    public PlayerHealth PlayerHealth;
    public ProgressionHandler PlayerProgression;
    public CharacterHandler CharacterHandler;

    private ProgressBar HPBar;
    private ProgressBar XPBar;
    private Label LevelText;

    // Start is called before the first frame update
    void Start()
    {
        ShowView();
    }

    protected override void UIUpdate()
    {
        // Update HP bar
        HPBar.value = PlayerHealth.Value;
        HPBar.title = PlayerHealth.Value.ToString();
        // Update XP bar, current level progress
        int xpHigh = PlayerProgression.GetLevelXpFloor(PlayerProgression.Level + 1);
        int xpLow = PlayerProgression.GetLevelXpFloor(PlayerProgression.Level);
        XPBar.highValue = (xpHigh - xpLow);
        XPBar.value = (PlayerProgression.Experience.Value - xpLow);
        XPBar.title = (PlayerProgression.Experience.Value - xpLow).ToString();
        // Display current level
        LevelText.text = $"Level {PlayerProgression.Level}";
    }

    protected override void OnEnterFocus()
    {
        // FIXME: HP max is not set or updated
        HPBar = GetViewContainer().Q<ProgressBar>("HpBar");
        HPBar.highValue = PlayerHealth.Max;
        XPBar = GetViewContainer().Q<ProgressBar>("ExpBar");
        LevelText = GetViewContainer().Q<Label>("PlayerLvl");
        // Set non changing values
        GetViewContainer().Q<Label>("PlayerName").text = CharacterHandler.CharacterSheet.Name;
        GetViewContainer().Q<Label>("PlayerInfo").text = CharacterHandler.CharacterSheet.Race.ToString();
        GetViewContainer().Q<Label>("PlayerInfo").text += " " + CharacterHandler.ClassSpecName;
    }
}