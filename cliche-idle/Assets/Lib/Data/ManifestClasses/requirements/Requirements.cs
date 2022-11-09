using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Requirements")]
public class Requirements : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }
    
    /// <summary>
    /// The list of races that fulfill the requirement.
    /// </summary>
    [field: SerializeField]
    public List<Race> Race { get; private set; }

    /// <summary>
    /// The list of player class names that fulfill the requirement.
    /// </summary>
    [field: SerializeField]
    public List<string> ClassSpecName { get; private set; }

    /// <summary>
    /// The player level that fulfills the requirement.
    /// </summary>
    [field: SerializeField]
    public int Level { get; private set; }

    /// <summary>
    /// Checks whether or not the player is fulfilled all described requirements.
    /// </summary>
    /// <returns></returns>
    public bool IsFulfilled()
    {
        var PlayerCharacter = GameObject.Find("Player").GetComponent<CharacterHandler>();
        string userRace = PlayerCharacter.Race.ToString();
        string userClassSpecName = PlayerCharacter.ClassSpecName;
        var PlayerProgression = GameObject.Find("Player").GetComponent<ProgressionHandler>();
        int userLevel = PlayerProgression.Level;

        if(Race.Find(race => race.ID == userRace) != null || Race.Count == 0)
        {
            if(ClassSpecName.Contains(userClassSpecName) || ClassSpecName.Count == 0)
            {
                if(Level <= userLevel || Level == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}