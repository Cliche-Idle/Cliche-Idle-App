using UnityEngine;
using Cliche.System;

public class ProgressionHandler : MonoBehaviour
{
    //[field: SerializeField]
    public IncreaseOnlyProperty Experience;

    public int Level
    {
        get 
        {
            float levelXpScalar = Manifests.GetByID<SingleValueModifier>("XpLevelScalar").Value;
            int level = Mathf.FloorToInt(levelXpScalar * Mathf.Sqrt(Experience.Value));
            if (level == 0)
            {
                level = 1;
            }
            return level;
        }
    }

    public int GetXpToNextLevel()
    {
        float levelXpScalar = Manifests.GetByID<SingleValueModifier>("XpLevelScalar").Value;
        int currentLevel = Level;
        int nextLevelXP = Mathf.CeilToInt(Mathf.Pow((currentLevel+1 / levelXpScalar), 2));
        int difference = nextLevelXP - Experience.Value;
        return difference;
    }

    public int GetLevelXpFloor(int level)
    {
        int levelXP = 0;
        if (level > 1)
        {
            float levelXpScalar = Manifests.GetByID<SingleValueModifier>("XpLevelScalar").Value;
            levelXP = Mathf.CeilToInt(Mathf.Pow((level / levelXpScalar), 2));
        }
        return levelXP;
    }
}
