using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    // TODO: build this out properly
    
    [field: SerializeField]
    public string Name { get; private set;}

    [field: SerializeField]
    public Races Race { get; private set;}

    [field: SerializeField]
    public string ClassSpecName { get; private set;}

    public CharacterVisualData CharacterVisual;
}
