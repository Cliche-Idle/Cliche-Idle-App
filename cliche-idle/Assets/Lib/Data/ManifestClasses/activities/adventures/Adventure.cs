using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Activities/Adventure")]
public class Adventure : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    [Header("General")]
    public string Title;

    [TextArea(3,10)]
    public string Description;

    public Requirements Requirements;

    [Header("Adventure data")]
    public float BaseLength;
    public float BaseChance;

    [Header("Rewards settings")]
    public LootTable Rewards;

    public bool CompositeLoot;

    public int MinRewardCount;

    public int MaxRewardCount;
}