using System;
using UnityEngine;
using Cliche.System;

[System.Serializable]
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// Event that fires when the player's health reaches 0.
    /// </summary>
    public event EventHandler OnPlayerDeath;

    /// <summary>
    /// The current amount of health the player has.
    /// </summary>
    [field: SerializeField]
    public int Value { get; private set; }

    /// <summary>
    /// Gets the maximum health the player can have.
    /// </summary>
    public int Max
    {
        get {
            int level = gameObject.GetComponent<ProgressionHandler>().Level;
            int levelBonus = Manifests.GetByID<IntervalValueModifier>("HealthBonusPerLevel").GetAmountFloored(level);
            int vitalityBonus = Manifests.GetByID<IntervalValueModifier>("Vitality").GetAmountFloored(gameObject.GetComponent<StatsHandler>().Vitality.Value);
            return (levelBonus + vitalityBonus);
        }
    }

    /// <summary>
    /// Grants the specified amount of health points to the player.
    /// </summary>
    /// <param name="amount"></param>
    public void Grant (int amount)
    {
        Value += Mathf.Abs(amount);
        int maxHealth = Max;
        if (Value > maxHealth)
        {
            Value = maxHealth;
        }
    }

    /// <summary>
    /// Removes the specified amount of health points to the player. If the health reaches zero, triggers the OnPlayerDeath event.
    /// </summary>
    /// <param name="amount"></param>
    public void Take (int amount)
    {
        Value -= Mathf.Abs(amount);
        // Backfill if negative
        if (Value < 0)
        {
            Value = 0;
            if (OnPlayerDeath != null)
            {
                OnPlayerDeath.Invoke(this, null);
            }
        }
    }
}