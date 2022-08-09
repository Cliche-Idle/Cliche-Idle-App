using System;
using UnityEngine;
using Cliche.System;

[System.Serializable]
public class PlayerHealth : MonoBehaviour
{
    [field: SerializeField]
    public int Value { get; private set; }

    public int Max
    {
        get {
            int level = gameObject.GetComponent<ProgressionHandler>().Level;
            int levelBonus = Manifests.GetByID<IntervalValueModifier>("HealthBonusPerLevel").GetAmountFloored(level);
            int vitalityBonus = Manifests.GetByID<IntervalValueModifier>("Vitality").GetAmountFloored(gameObject.GetComponent<StatsHandler>().Vitality.Value);
            return (levelBonus + vitalityBonus);
        }
    }

    public void Grant (int amount)
    {
        Value += Mathf.Abs(amount);
        int maxHealth = Max;
        if (Value > maxHealth)
        {
            Value = maxHealth;
        }
    }

    public void Take (int amount)
    {
        Value -= Mathf.Abs(amount);
        // Backfill if negative
        if (Value < 0)
        {
            Value = 0;
        }
    }
}