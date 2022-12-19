using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cliche.System;
using UIViews;
using Cliche.UIElements;

public class StatsView : UIScript
{
    private IntSelector _strSelector;
    private IntSelector _dexSelector;
    private IntSelector _intSelector;
    private IntSelector _vitSelector;

    public StatsHandler Stats;

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.height = Length.Percent(90);
        //
        _strSelector = GetViewContainer().Q<IntSelector>("StrSelector");
        GetViewContainer().Q<Label>("StrText").text = Manifests.GetByID<IntervalValueModifier>("Strength").GameDescription;
        Stats.Strength.OnValueChange += OnStatsChange;
        _strSelector.lowValue = Stats.Strength.MinimumValue;
        _strSelector.OnValueIncrease += () => { Stats.Strength.Grant(1); };
        _strSelector.OnValueDecrease += () => { Stats.Strength.Take(1); };
        //
        _dexSelector = GetViewContainer().Q<IntSelector>("DexSelector");
        GetViewContainer().Q<Label>("DexText").text = Manifests.GetByID<IntervalValueModifier>("Dexterity").GameDescription;
        Stats.Dexterity.OnValueChange += OnStatsChange;
        _dexSelector.lowValue = Stats.Dexterity.MinimumValue;
        _dexSelector.OnValueIncrease += () => { Stats.Dexterity.Grant(1); };
        _dexSelector.OnValueDecrease += () => { Stats.Dexterity.Take(1); };
        //
        _intSelector = GetViewContainer().Q<IntSelector>("IntSelector");
        GetViewContainer().Q<Label>("IntText").text = Manifests.GetByID<IntervalValueModifier>("Intelligence").GameDescription;
        Stats.Intelligence.OnValueChange += OnStatsChange;
        _intSelector.lowValue = Stats.Intelligence.MinimumValue;
        _intSelector.OnValueIncrease += () => { Stats.Intelligence.Grant(1); };
        _intSelector.OnValueDecrease += () => { Stats.Intelligence.Take(1); };
        //
        _vitSelector = GetViewContainer().Q<IntSelector>("VitSelector");
        GetViewContainer().Q<Label>("VitText").text = Manifests.GetByID<IntervalValueModifier>("Vitality").GameDescription;
        Stats.Vitality.OnValueChange += OnStatsChange;
        _vitSelector.lowValue = Stats.Vitality.MinimumValue;
        _vitSelector.OnValueIncrease += () => { Stats.Vitality.Grant(1); };
        _vitSelector.OnValueDecrease += () => { Stats.Vitality.Take(1); };

        OnStatsChange(0);
    }

    protected override void OnLeaveFocus()
    {
        Stats.Strength.OnValueChange -= OnStatsChange;
        Stats.Dexterity.OnValueChange -= OnStatsChange;
        Stats.Intelligence.OnValueChange -= OnStatsChange;
        Stats.Vitality.OnValueChange -= OnStatsChange;
    }

    private void OnStatsChange(int val)
    {
        _strSelector.value = Stats.Strength.Value;
        _strSelector.highValue = Stats.Strength.Value + Stats.GetFreeStatPoints();

        _dexSelector.value = Stats.Dexterity.Value;
        _dexSelector.highValue = Stats.Dexterity.Value + Stats.GetFreeStatPoints();

        _intSelector.value = Stats.Intelligence.Value;
        _intSelector.highValue = Stats.Intelligence.Value + Stats.GetFreeStatPoints();

        _vitSelector.value = Stats.Vitality.Value;
        _vitSelector.highValue = Stats.Vitality.Value + Stats.GetFreeStatPoints();

        GetViewContainer().Q<Label>("PointsNum").text = Stats.GetFreeStatPoints().ToString();
    }
}
