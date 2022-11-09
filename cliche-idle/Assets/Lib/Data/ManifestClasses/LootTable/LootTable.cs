using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Activities/Loot table")]
public class LootTable : ScriptableObject
{
    public string ID {
        get {
            return name;
        }
    }

    /// <summary>
    /// The number of reward items contained within the loot table.
    /// </summary>
    public int MaxPossibleRewardCount {
        get {
            return (Weapons.Count + Armour.Count + Consumables.Count);
        }
    }

    /// <summary>
    /// The list of possible Weapon rewards.
    /// </summary>
    [field: SerializeField]
    public List<WeaponManifest> Weapons { get; private set; }

    /// <summary>
    /// The list of possible Armour rewards.
    /// </summary>
    [field: SerializeField]
    public List<ArmourManifest> Armour { get; private set; }

    /// <summary>
    /// The list of possible Consumable rewards.
    /// </summary>
    [field: SerializeField]
    public List<ConsumableManifest> Consumables { get; private set; }

    /// <summary>
    /// Returns a categorised list of item IDs from the table's contents, randomly chosen, between the specified counts. Duplicate items may appear.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public Dictionary<ItemTypes, List<string>> GetNewLootStream(int min, int max)
    {
        // Validate and set constrains
        if (min <= 0)
        {
            min = 1;
        }
        //
        var lootStream = new Dictionary<ItemTypes, List<string>>();

        int rewardCount = Random.Range(min, max+1);

        var rewardDistribution = new Dictionary<ItemTypes, int>();

        // Wish there was an easier way to do this dynamically
        if (Weapons != null && Weapons.Count != 0)
        {
            rewardDistribution.Add(ItemTypes.Weapon, 0);
        }
        if (Armour != null && Armour.Count != 0)
        {
            rewardDistribution.Add(ItemTypes.Armour, 0);
        }
        if (Consumables != null && Consumables.Count != 0)
        {
            rewardDistribution.Add(ItemTypes.Consumable, 0);
        }

        while (rewardCount > 0)
        {
            int lootCategoryID = Random.Range(0, rewardDistribution.Count);
            ItemTypes itemType = rewardDistribution.ElementAt(lootCategoryID).Key;
            int categoryRewardCount = Random.Range(1, rewardCount+1);
            rewardDistribution[itemType] += categoryRewardCount;
            rewardCount -= categoryRewardCount;
        }

        foreach (var rewardCategory in rewardDistribution)
        {
            lootStream.Add(rewardCategory.Key, new List<string>());
            for (int i = 1; i <= rewardCategory.Value; i++)
            {
                int rewardIndex = 0;
                switch(rewardCategory.Key)
                {
                    case ItemTypes.Weapon:
                        rewardIndex = Random.Range(0, Weapons.Count);
                        lootStream[ItemTypes.Weapon].Add(Weapons[rewardIndex].ID);
                        break;
                    case ItemTypes.Armour:
                        rewardIndex = Random.Range(0, Armour.Count);
                        lootStream[ItemTypes.Armour].Add(Armour[rewardIndex].ID);
                        break;
                    case ItemTypes.Consumable:
                        rewardIndex = Random.Range(0, Consumables.Count);
                        lootStream[ItemTypes.Consumable].Add(Consumables[rewardIndex].ID);
                        break;
                }
            }
        }
        return lootStream;
    }
}