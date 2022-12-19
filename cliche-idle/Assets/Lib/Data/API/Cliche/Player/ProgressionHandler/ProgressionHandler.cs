using UnityEngine;
using Cliche.System;

public class ProgressionHandler : MonoBehaviour
{
    //[field: SerializeField]
    public IncreaseOnlyIntProperty Experience;

    public int DisplayLevel
    {
        get
        {
            return Level+1;
        }
    }

    public string DisplayLevelUI
    {
        get
        {
            return DisplayLevel.ToString();
        }
    }

    public int Level
    {
        get 
        {
            float levelXpScalar = Manifests.GetByID<SingleValueModifier>("XpLevelScalar").Value;
            int level = Mathf.FloorToInt(levelXpScalar * Mathf.Sqrt(Experience.Value));
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
        if (level >= 1)
        {
            float levelXpScalar = Manifests.GetByID<SingleValueModifier>("XpLevelScalar").Value;
            levelXP = Mathf.CeilToInt(Mathf.Pow((level / levelXpScalar), 2));
        }
        return levelXP;
    }

    public (int, int) GetLevelXpBands(int level)
    {
        var low = GetLevelXpFloor(level);
        var high = GetLevelXpFloor(level + 1);
        var range = new RangeInt(low, high);
        return (low, high);
    }
}
