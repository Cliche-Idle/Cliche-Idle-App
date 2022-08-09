using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Activities/Adventure")]
public class AdventureManifest : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    /// <summary>
    /// The in-game name of the Adventure.
    /// </summary>
    [Header("General")]
    public string Title;

    /// <summary>
    /// The general flavour text that is displayed before the adventure starts.
    /// </summary>
    [TextArea(3,10)]
    public string Description;

    /// <summary>
    /// The post-completion flavour text that is displayed if the Adventure is successfully completed.
    /// </summary>
    [TextArea(3,10)]
    public string PostCompleteDescriptionSuccess;
    
    /// <summary>
    /// The post-completion flavour text that is displayed if the Adventure is failed.
    /// </summary>
    [TextArea(3,10)]
    public string PostCompleteDescriptionFail;
    
    /// <summary>
    /// The requirements after which this Adventure is available to the player.
    /// </summary>
    public Requirements Requirements;

    /// <summary>
    /// The base length of this adventure, in seconds.
    /// </summary>
    [Header("Adventure data")]
    public float BaseLength;

    /// <summary>
    /// The base chance of this adventure succeeding.
    /// </summary>
    public float BaseChance;

    /// <summary>
    /// The Adventure's loot table.
    /// </summary>
    [Header("Rewards settings")]
    public LootTable Rewards;

    /// <summary>
    /// The minimum amount of rewards this Adveture grants.
    /// </summary>
    public int MinRewardCount;

    /// <summary>
    /// The maximum amount of rewards this Adveture grants.
    /// </summary>
    public int MaxRewardCount;
}