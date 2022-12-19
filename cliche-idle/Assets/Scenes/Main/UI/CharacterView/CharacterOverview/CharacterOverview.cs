using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UIViews;
using Cliche.UIElements;

public class CharacterOverview : UIScript
{
    public CharacterHandler Character;
    public StatsHandler Stats;
    public CurrencyHandler Currency;

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.height = Length.Percent(60);
        GetViewContainer().Q<Label>("CharacterName").text = Character.CharacterSheet.Name;
        GetViewContainer().Q<Label>("Race").text = Character.CharacterSheet.Race.ToString();
        GetViewContainer().Q<CharacterDisplay>().CharacterSheet = Character.CharacterSheet;
        //
        GetViewContainer().Q<Label>("AtkValue").text = Stats.Attack.ToString();
        GetViewContainer().Q<Label>("DefValue").text = Stats.Defence.ToString();
        //
        GetViewContainer().Q<Label>("StrValue").text = Stats.Strength.Value.ToString();
        GetViewContainer().Q<Label>("DexValue").text = Stats.Dexterity.Value.ToString();
        GetViewContainer().Q<Label>("IntValue").text = Stats.Intelligence.Value.ToString();
        GetViewContainer().Q<Label>("VitValue").text = Stats.Vitality.Value.ToString();
        //
        GetViewContainer().Q<Label>("GoldValue").text = Currency.Gold.Value.ToString();
        GetViewContainer().Q<Label>("TicketValue").text = Currency.DungeonTickets.Value.ToString();
        //
        GetViewContainer().Q<Button>("StatsBtn").clicked += () =>
        {
            HideView();
            Navigator.ShowView("CS_StatsManagement");
            Navigator.HideView("CS_InventoryEquippedItems");
        };
    }
}
