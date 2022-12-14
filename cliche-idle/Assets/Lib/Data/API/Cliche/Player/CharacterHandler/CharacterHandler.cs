using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{    
    /// <summary>
    /// Currently unused, akin to player titles.
    /// </summary>
    [field: SerializeField]
    [Obsolete]
    public string ClassSpecName { get; private set; }

    /// <summary>
    /// Contains information about the player character's appearance.
    /// </summary>
    public CharacterSheet CharacterSheet;
}
