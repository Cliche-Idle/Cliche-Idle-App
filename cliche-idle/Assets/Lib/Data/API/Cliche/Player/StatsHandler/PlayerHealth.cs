using System;
using UnityEngine;
using Cliche.System;

[System.Serializable]
public class PlayerHealth : AdjustableIntProperty
{
    /// <summary>
    /// Event that fires when the player's health reaches 0.
    /// </summary>
    public Action OnPlayerDeath;

    /// <summary>
    /// Gets the maximum health the player can have.
    /// </summary>
    public int Max
    {
        get {
            int baseHealth = 100;
            int level = GameObject.Find("Player").GetComponent<ProgressionHandler>().Level;
            int levelBonus = Manifests.GetByID<IntervalValueModifier>("HealthBonusPerLevel").GetAmountFloored(level);
            int vitalityBonus = Manifests.GetByID<IntervalValueModifier>("Vitality").GetAmountFloored(GameObject.Find("Player").GetComponent<StatsHandler>().Vitality.Value);
            return (baseHealth + levelBonus + vitalityBonus);
        }
    }

    /// <summary>
    /// Grants the specified amount of health points to the player.
    /// </summary>
    /// <param name="amount"></param>
    public override void Grant (int amount)
    {
        Value += Mathf.Abs(amount);
        int maxHealth = Max;
        if (Value > maxHealth)
        {
            Value = maxHealth;
        }
        InvokeValueChangeEvent();
    }

    /// <summary>
    /// Removes the specified amount of health points to the player. If the health reaches zero, triggers the <see cref="OnPlayerDeath"/> event.
    /// </summary>
    /// <param name="amount"></param>
    public override void Take (int amount)
    {
        Value -= Mathf.Abs(amount);
        // Backfill if negative
        if (Value < 0)
        {
            Value = 0;
            InvokePlayerDeathEvent();
        }
        InvokeValueChangeEvent();
    }

    private void InvokePlayerDeathEvent()
    {
        if (OnPlayerDeath != null)
        {
            OnPlayerDeath.Invoke();
        }
    }
}