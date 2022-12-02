using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{    
    /// <summary>
    /// The player character's name.
    /// </summary>
    [field: SerializeField]
    public string Name { get; private set; }

    /// <summary>
    /// The player character's race.
    /// </summary>
    [field: SerializeField]
    public Races Race { get; private set; }

    /// <summary>
    /// Currently unused, akin to player titles.
    /// </summary>
    [field: SerializeField]
    public string ClassSpecName { get; private set; }

    // TODO: build this out properly
    /// <summary>
    /// Contains information about the player character's appearance.
    /// </summary>
    public CharacterSheet CharacterVisuals;
}
