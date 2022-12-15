using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Cliche.System;
using Cliche.Activities;
using UIViews;

public class AdventureDetailsPopup : PopUp
{
    public AdventureHandler Adventures;

    public VisualElement AdventureSlot;

    public Action OnAccept;

    protected override void OnEnterFocus()
    {
        var adventure = Manifests.GetByID<AdventureManifest>(AdventureSlot.name);
        ContentContainer.Q<Label>("RequestName").text = adventure.Title;
        ContentContainer.Q<Label>("RequestDescription").text = adventure.Description;
        ContentContainer.Q<Label>("RequestTimer").text = TimeSpan.FromSeconds(Math.Floor(adventure.BaseLength)).ToString();
        if (adventure.Rewards.Weapons.Count == 0)
        {
            ContentContainer.Q<VisualElement>("WeaponLoot").style.display = DisplayStyle.None;
        }
        if (adventure.Rewards.Armour.Count == 0)
        {
            ContentContainer.Q<VisualElement>("ArmourLoot").style.display = DisplayStyle.None;
        }
        if (adventure.Rewards.Consumables.Count == 0)
        {
            ContentContainer.Q<VisualElement>("ItemLoot").style.display = DisplayStyle.None;
        }

        ContentContainer.Q<Button>("StartAdventureButton").clicked += () =>
        {
            string adventureID = AdventureSlot.name;
            AdventureSlot.parent.Clear();
            Adventures.StartAdventure(adventureID);
            ClosePopUpWindow();
        };
    }

    protected override void OnLeaveFocus()
    {
        AdventureSlot = null;
    }
}
