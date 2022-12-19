using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UIViews;

public class PlayerBar : UIScript
{
    public StatsHandler Stats;
    public ProgressionHandler PlayerProgression;
    public CharacterHandler CharacterHandler;

    private ProgressBar HPBar;
    private ProgressBar XPBar;
    private Label LevelText;

    protected override void OnEnterFocus()
    {
        HPBar = GetViewContainer().Q<ProgressBar>("HpBar");
        Stats.Health.OnValueChange += UpdateHealthUI;
        UpdateHealthUI(Stats.Health.Value);

        XPBar = GetViewContainer().Q<ProgressBar>("ExpBar");
        LevelText = GetViewContainer().Q<Label>("PlayerLvl");
        PlayerProgression.Experience.OnValueChange += UpdateExperienceUI;
        UpdateExperienceUI(PlayerProgression.Experience.Value);

        // Set non changing values
        GetViewContainer().Q<Label>("PlayerName").text = CharacterHandler.CharacterSheet.Name;
        GetViewContainer().Q<Label>("PlayerInfo").text = CharacterHandler.CharacterSheet.Race.ToString();
        GetViewContainer().Q<Label>("PlayerInfo").text += " " + CharacterHandler.ClassSpecName;
    }

    protected override void OnLeaveFocus()
    {
        Stats.Health.OnValueChange -= UpdateHealthUI;
        PlayerProgression.Experience.OnValueChange -= UpdateExperienceUI;
    }

    private void UpdateHealthUI(int hp)
    {
        HPBar.highValue = Stats.Health.Max;
        HPBar.value = hp;
        HPBar.title = hp.ToString();
    }

    private void UpdateExperienceUI(int xp)
    {
        int playerLevel = PlayerProgression.Level;
        int levelXpCeiling = PlayerProgression.GetLevelXpFloor(playerLevel + 1);
        int levelXpFloor = PlayerProgression.GetLevelXpFloor(playerLevel);
        XPBar.highValue = (levelXpCeiling - levelXpFloor);
        XPBar.value = (xp - levelXpFloor);
        XPBar.title = (xp - levelXpFloor).ToString();
        // Display current level
        LevelText.text = $"Level {PlayerProgression.DisplayLevel}";
    }
}