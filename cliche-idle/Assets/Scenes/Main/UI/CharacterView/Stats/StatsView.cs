using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cliche.System;

public class StatsView : UIScript
{
    private StatsHandler Stats;
    private VisualElement StrengthContainer;
    private VisualElement DexterityContainer;
    private VisualElement IntelligenceContainer;
    private VisualElement VitalityContainer;

    protected override void UIUpdate()
    {
        StrengthContainer.Q<Label>("NumberStat").text = $"{Stats.Strength.Value}";
        DexterityContainer.Q<Label>("NumberStat").text = $"{Stats.Dexterity.Value}";
        IntelligenceContainer.Q<Label>("NumberStat").text = $"{Stats.Intelligence.Value}";
        VitalityContainer.Q<Label>("NumberStat").text = $"{Stats.Vitality.Value}";
        GetViewContainer().Q<Label>("PointsNum").text = $"{Stats.GetFreeStatPoints()}";
    }

    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        Stats = GameObject.Find("Player").GetComponent<StatsHandler>();
        //
        StrengthContainer = GetViewContainer().Q<VisualElement>("StrRow");
        GetViewContainer().Q<Label>("StrText").text = Manifests.GetByID<IntervalValueModifier>("Strength").GameDescription;
        StrengthContainer.Q<Button>("AddStat").clicked += () => { Stats.Strength.Grant(1); };
        StrengthContainer.Q<Button>("MinusStat").clicked += () => { Stats.Strength.Take(1); };
        //
        DexterityContainer = GetViewContainer().Q<VisualElement>("DexRow");
        GetViewContainer().Q<Label>("DexText").text = Manifests.GetByID<IntervalValueModifier>("Dexterity").GameDescription;
        DexterityContainer.Q<Button>("AddStat").clicked += () => { Stats.Dexterity.Grant(1); };
        DexterityContainer.Q<Button>("MinusStat").clicked += () => { Stats.Dexterity.Take(1); };
        //
        IntelligenceContainer = GetViewContainer().Q<VisualElement>("IntRow");
        GetViewContainer().Q<Label>("IntText").text = Manifests.GetByID<IntervalValueModifier>("Intelligence").GameDescription;
        IntelligenceContainer.Q<Button>("AddStat").clicked += () => { Stats.Intelligence.Grant(1); };
        IntelligenceContainer.Q<Button>("MinusStat").clicked += () => { Stats.Intelligence.Take(1); };
        //
        VitalityContainer = GetViewContainer().Q<VisualElement>("VitRow");
        GetViewContainer().Q<Label>("VitText").text = Manifests.GetByID<IntervalValueModifier>("Vitality").GameDescription;
        VitalityContainer.Q<Button>("AddStat").clicked += () => { Stats.Vitality.Grant(1); };
        VitalityContainer.Q<Button>("MinusStat").clicked += () => { Stats.Vitality.Take(1); };
    }
}
