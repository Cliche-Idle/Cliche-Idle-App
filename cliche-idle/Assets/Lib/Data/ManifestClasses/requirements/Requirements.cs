using System.Collections.Generic;
using UnityEngine;
using Cliche.UserManagement;

[CreateAssetMenu(menuName = "GameData/Requirements")]
public class Requirements : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }
    
    public List<string> Race;

    public List<string> ClassSpecName;

    public int Level;

    public bool IsFulfilled(User user)
    {
        string userRace = user.character.general.race;
        string userClassSpecName = user.character.general.classSpecName;
        int userLevel = user.character.general.Level;

        if(Race.Contains(userRace) || Race.Count == 0)
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