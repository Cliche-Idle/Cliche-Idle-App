using UnityEngine;

public class CurrencyHandler : MonoBehaviour
{
    /// <summary>
    /// The main earnable player currency.
    /// </summary>
    public AdjustableIntProperty Gold;

    /// <summary>
    /// Used to grant immediate access to dungeons.
    /// </summary>
    public AdjustableIntProperty DungeonTickets;
}