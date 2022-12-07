using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Cliche.System;
using Cliche.Activities;
using UIViews;

public class AdventureFinishPopup : PopUp
{
    public AdventureHandler Adventures;

    public VisualElement AdventureSlot;

    protected override void OnEnterFocus()
    {
        string adventureID = AdventureSlot.name;
        AdventureSlot.Clear();
        AdventureSlot.name = "request";
        AdventureSlot.RemoveFromClassList("CompletedAdventure");
        AdventureSlot.RemoveFromClassList("ActiveAdventure");
        var adventure = Manifests.GetByID<AdventureManifest>(adventureID);

        PostActivityReport adventureReport = Adventures.FinishAdventure(adventureID);

        ContentContainer.Q<Label>("RequestName").text = adventure.Title;

        ContentContainer.Q<Label>("Status").text = adventureReport.Status.ToString();

        if (adventureReport.Status == ActivityStatus.Success)
        {
            ContentContainer.Q<Label>("RequestDescription").text = adventure.PostCompleteDescriptionSuccess;
            ContentContainer.Q<Label>("Status").style.color = (Color)new Color32(50, 205, 50, 255);
        }
        else
        {
            ContentContainer.Q<Label>("RequestDescription").text = adventure.PostCompleteDescriptionFail;
            ContentContainer.Q<Label>("Status").style.color = Color.red;
        }

        ContentContainer.Q<Label>("DamageTaken").text = $"-{adventureReport.DamageTaken}";
        ContentContainer.Q<Label>("Experience").text = $"+{adventureReport.ExperienceGained}";
        ContentContainer.Q<Label>("Gold").text = $"+{adventureReport.GoldGained}";

        ContentContainer.Q<Button>("ConfirmButton").clicked += () =>
        {
            ClosePopUpWindow();
        };
    }

    protected override void OnLeaveFocus()
    {
        AdventureSlot = null;
    }
}
