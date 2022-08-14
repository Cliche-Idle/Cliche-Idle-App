using System;
using System.Collections.Generic;
using UnityEngine;
using Cliche.System;
using Cliche.Activities;

public partial class AdventureHandler
{
    // TODO: refactor this out to a modifier
    private int _DefencePenaltyDivisionScaler = 4;

    public PostActivityReport FinishAdventure(string adventureID)
    {
        // Check if the given adventure is in the queue AND finished
        var activity = AdventureQueue.Find(adventure => adventure.ID == adventureID && adventure.Finished == true);
        if (activity != null)
        {
            AdventureManifest selectedAdventure = Manifests.GetByID<AdventureManifest>(adventureID);
            var statsHandler = gameObject.GetComponent<StatsHandler>();
            var progressionHandler = gameObject.GetComponent<ProgressionHandler>();
            // * Determine if the adventure ended with a success or not
            var successChance = UnityEngine.Random.Range(1, 101);
            bool success = (successChance <= selectedAdventure.BaseChance);

            var damageTaken = GetDamageTaken(selectedAdventure);

            var rewardXP = GetExperienceReward(selectedAdventure, success);

            var rewardGold = GetGoldReward(selectedAdventure, success);

            var lootStream = selectedAdventure.Rewards.GetNewLootStream(selectedAdventure.MinRewardCount, selectedAdventure.MaxRewardCount);
            GrantItemRewards(lootStream);

            // * Grant rewards 
            progressionHandler.Experience.Grant(rewardXP);
            gameObject.GetComponent<CurrencyHandler>().Gold.Grant(rewardGold);
            statsHandler.Health.Take(damageTaken);

            // Generate activity end report
            var activityEndReport = new PostActivityReport(damageTaken, 0, rewardXP, rewardGold, lootStream);
            Debug.Log($"Adventure end report ({adventureID}, success: {success}): -{damageTaken} HP, +{rewardXP} XP, +{rewardGold} Gold.");

            // Remove adventure from the list
            AdventureQueue.Remove(activity);
            // Refill adventure list (also triggers update event)
            RefillAvailableList();
            return activityEndReport;
        }
        else
        {
            Debug.Log($"Can not finish adventure ({adventureID}), it is either not in queue or not finished yet.");
        }
        return null;
    }

    private float GetAdventureChanceScalar(AdventureManifest selectedAdventure)
    {
        var baseChancePercent = (selectedAdventure.BaseChance / 100);
        // Inverts the chance percentage: 90% -> 10% OR 10% -> 90%
        var chanceScalar = ((1 - baseChancePercent));
        return chanceScalar;
    }

    private int GetDamageTaken(AdventureManifest selectedAdventure)
    {
        var gearDefencePerLevel = Manifests.GetByID<IntervalValueModifier>("GearDefencePerLevel");
        var statsHandler = gameObject.GetComponent<StatsHandler>();
        var progressionHandler = gameObject.GetComponent<ProgressionHandler>();
        // Calculate damage
        var dexterityDamageReductionAmount = Manifests.GetByID<IntervalValueModifier>("Dexterity").GetAmount(statsHandler.Dexterity.Value);
        // Adventure defence level is user level + 1, so they are always below cap
        var activityScaledDefence = gearDefencePerLevel.GetAmount(progressionHandler.Level + 1);
        var playerDefenceLowBound = gearDefencePerLevel.GetAmount(progressionHandler.Level);
        // Check the difference between user defence and activity defence. For every point difference, one percent penalty is given
        var defencePenaltyPercent = 1 - (((activityScaledDefence - statsHandler.Defence) / (activityScaledDefence - playerDefenceLowBound)) / 100);
        // Scale max damage taken to the player level + 10
        var levelScaledMaxDamageTaken = (progressionHandler.Level + 10);
        // Penalty damage for being under the activity defence level
        var defencePenaltyAmount = ((levelScaledMaxDamageTaken / _DefencePenaltyDivisionScaler) * defencePenaltyPercent);
        // Add max level scaled damage and defence penalty, then subtract dexterity bonus
        var maxDamageTaken = Mathf.FloorToInt((levelScaledMaxDamageTaken + defencePenaltyAmount) - dexterityDamageReductionAmount);

        // * Final damage taken:
        var damageTaken = UnityEngine.Random.Range(0, maxDamageTaken);
        return damageTaken;
    }

    private int GetExperienceReward(AdventureManifest selectedAdventure, bool success)
    {
        var progressionHandler = gameObject.GetComponent<ProgressionHandler>();
        //
        var chanceScalar = GetAdventureChanceScalar(selectedAdventure);
        //
        var currentLevelXpBracket = progressionHandler.GetLevelXpFloor(progressionHandler.Level);
        var nextLevelXpBracket = progressionHandler.GetLevelXpFloor(progressionHandler.Level+1);
        // Gets whole level XP bracket
        var xpBracket = nextLevelXpBracket - currentLevelXpBracket;
        // Scales up the max XP by the chance scalar, so lower chance yields more max XP
        // TODO: refactor this out to a modifier
        var baseMaxRewardXP = (xpBracket / (8 - (4 * chanceScalar)));
        // TODO: use above modifier but low bound
        // Scales up the min XP by the chance scalar, so lower chance yields more min XP
        var baseMinRewardXP = (baseMaxRewardXP / (4 - (4 * chanceScalar)));
        // * Final XP given:
        var rewardXP = 10 + Mathf.CeilToInt(UnityEngine.Random.Range(baseMinRewardXP, baseMaxRewardXP));
        // Grants minimum XP if failed
        if (!success)
        {
            rewardXP = Mathf.CeilToInt(baseMinRewardXP);
        }
        return rewardXP;
    }

    private int GetGoldReward(AdventureManifest selectedAdventure, bool success)
    {
        var progressionHandler = gameObject.GetComponent<ProgressionHandler>();
        //
        var chanceScalar = GetAdventureChanceScalar(selectedAdventure);
        // TODO: refactor this out to a modifier
        var maxBaseGold = (10 + progressionHandler.Level);
        // TODO: refactor this out to a modifier
        var chanceScaledGold = (2 * chanceScalar);
        // Get random amount + adventure difficulty multiplier
        var rewardGold = Mathf.CeilToInt(UnityEngine.Random.Range((maxBaseGold / 2), maxBaseGold) * (1 + chanceScaledGold));
        // Grants 50% gold if failed
        if (!success)
        {
            rewardGold = Mathf.CeilToInt(rewardGold * 0.5f);
        }
        return rewardGold;
    }

    private void GrantItemRewards(Dictionary<ItemTypes, List<string>> lootStream)
    {
        var progressionHandler = gameObject.GetComponent<ProgressionHandler>();
        var inventoryHandler = gameObject.GetComponent<InventoryHandler>();
        var gearDefencePerLevel = Manifests.GetByID<IntervalValueModifier>("GearDefencePerLevel");
        var gearAttackPerLevel = Manifests.GetByID<IntervalValueModifier>("GearAttackPerLevel");
        var gearStatVariance = Manifests.GetByID<RangeValueModifier>("GearStatVariance");
        // Get the current level progress in percentage
        var nextLevelXpBracket = progressionHandler.GetLevelXpFloor(progressionHandler.Level+1);
        var currentLevelProgressPercent = (float)progressionHandler.Experience.Value / (float)nextLevelXpBracket;
            
        var playerDefenceLowBound = gearDefencePerLevel.GetAmount(progressionHandler.Level);
        var gearDefence = playerDefenceLowBound + (gearDefencePerLevel.Value * currentLevelProgressPercent);
            
        var playerAttackLowBound = gearAttackPerLevel.GetAmount(progressionHandler.Level);
        var gearAttack = playerAttackLowBound + (gearAttackPerLevel.Value * currentLevelProgressPercent);

        foreach (var item in lootStream)
        {
            switch(item.Key)
            {
                case ItemTypes.Weapon:
                    foreach (var weaponID in item.Value)
                    {
                        var weaponManifest = Manifests.GetByID<WeaponManifest>(weaponID);
                        // Calculate variance
                        var variance = UnityEngine.Random.Range(-(gearAttackPerLevel.Value * gearStatVariance.LowerBound), (gearAttackPerLevel.Value * gearStatVariance.UpperBound));
                        // Add variance
                        var weaponAttack = Mathf.FloorToInt(weaponManifest.MainStatValue + (gearAttack + variance));
                        inventoryHandler.Weapons.Add(new Weapon(weaponID, weaponAttack));
                    }
                    break;
                case ItemTypes.Armour:
                    foreach (var armourID in item.Value)
                    {
                        var armourManifest = Manifests.GetByID<ArmourManifest>(armourID);
                        // Calculate variance
                        var variance = UnityEngine.Random.Range(-(gearDefencePerLevel.Value * gearStatVariance.LowerBound), (gearDefencePerLevel.Value * gearStatVariance.UpperBound));
                        // Add variance
                        var weaponAttack = Mathf.FloorToInt(armourManifest.MainStatValue + (gearDefence + variance));
                        inventoryHandler.Weapons.Add(new Weapon(armourID, weaponAttack));
                    }
                    break;
                case ItemTypes.Consumable:
                    foreach (var consumableID in item.Value)
                    {
                        inventoryHandler.Consumables.Add(new Consumable(consumableID));
                    }
                    break;
            }
        }
    }
}