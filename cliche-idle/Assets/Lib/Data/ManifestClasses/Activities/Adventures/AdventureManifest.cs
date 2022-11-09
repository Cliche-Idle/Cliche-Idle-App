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
    /// The in-game name of the adventure.
    /// </summary>
    [field: Header("General")]
    [field:SerializeField]
    public string Title { get; private set; }

    /// <summary>
    /// The in-game icon of the adventure.
    /// </summary>
    [field: SerializeField]
    public Sprite Icon { get; private set; }

    /// <summary>
    /// The general flavour text that is displayed before the adventure starts.
    /// </summary>
    [field: TextArea(3,10)]
    [field: SerializeField]
    public string Description { get; private set; }

    /// <summary>
    /// The post-completion flavour text that is displayed if the adventure is successfully completed.
    /// </summary>
    [field: TextArea(3,10)]
    [field: SerializeField]
    public string PostCompleteDescriptionSuccess { get; private set; }

    /// <summary>
    /// The post-completion flavour text that is displayed if the adventure is failed.
    /// </summary>
    [field: TextArea(3,10)]
    [field: SerializeField]
    public string PostCompleteDescriptionFail { get; private set; }

    /// <summary>
    /// The requirements after which this adventure is available to the player.
    /// </summary>
    [field: SerializeField]
    public Requirements Requirements { get; private set; }

    /// <summary>
    /// The base length of this adventure, in seconds.
    /// </summary>
    [field: Header("Adventure data")]
    [field: SerializeField]
    public float BaseLength { get; private set; } = 0;

    /// <summary>
    /// The base chance of this adventure succeeding.
    /// </summary>
    [field: SerializeField]
    public float BaseChance { get; private set; } = 100;

    /// <summary>
    /// The Adventure's loot table.
    /// </summary>
    [field: Header("Rewards settings")]
    [field: SerializeField]
    public LootTable Rewards { get; private set; }

    /// <summary>
    /// The minimum amount of rewards this Adveture grants.
    /// </summary>
    [field: SerializeField]
    public int MinRewardCount { get; private set; } = 0;

    /// <summary>
    /// The maximum amount of rewards this Adveture grants.
    /// </summary>
    [field: SerializeField]
    public int MaxRewardCount { get; private set; } = 0;
}