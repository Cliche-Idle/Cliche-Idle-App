using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UIViews;

public class PlayerBar : UIScript
{
    private StatsHandler PlayerStats;
    private ProgressionHandler PlayerProgression;
    private ProgressBar HPBar;
    private ProgressBar XPBar;
    private Label LevelText;

    // Start is called before the first frame update
    void Start()
    {
        DisplayView();
    }

    protected override void UIUpdate()
    {
        // Update HP bar
        HPBar.value = PlayerStats.Health.Value;
        HPBar.title = PlayerStats.Health.Value.ToString();
        // Update XP bar, current level progress
        int xpHigh = PlayerProgression.GetLevelXpFloor(PlayerProgression.Level + 1);
        int xpLow = PlayerProgression.GetLevelXpFloor(PlayerProgression.Level);
        XPBar.highValue = (xpHigh - xpLow);
        XPBar.value = (PlayerProgression.Experience.Value - xpLow);
        XPBar.title = (PlayerProgression.Experience.Value - xpLow).ToString();
        // Display current level
        LevelText.text = $"Level {PlayerProgression.Level.ToString()}";
    }

    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        PlayerStats = GameObject.Find("Player").GetComponent<StatsHandler>();
        PlayerProgression = GameObject.Find("Player").GetComponent<ProgressionHandler>();
        // Get Element references
        HPBar = GetViewContainer().Q<ProgressBar>("HpBar");
        XPBar = GetViewContainer().Q<ProgressBar>("ExpBar");
        LevelText = GetViewContainer().Q<Label>("PlayerLvl");
        // Set non changing values
        GetViewContainer().Q<Label>("PlayerName").text = GameObject.Find("Player").GetComponent<CharacterHandler>().Name;
        GetViewContainer().Q<Label>("PlayerInfo").text = GameObject.Find("Player").GetComponent<CharacterHandler>().Race.ToString();
        GetViewContainer().Q<Label>("PlayerInfo").text += " " + GameObject.Find("Player").GetComponent<CharacterHandler>().ClassSpecName;
    }
}