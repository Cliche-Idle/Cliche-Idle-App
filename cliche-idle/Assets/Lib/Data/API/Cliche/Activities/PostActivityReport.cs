using System.Collections.Generic;

namespace Cliche.Activities
{
    /// <summary>
    /// Provides an overall summary of the result of an activity.
    /// </summary>
    public class PostActivityReport
    {
        /// <summary>
        /// The total amount of damage taken during this activity.
        /// </summary>
        public int DamageTaken { get; private set; }

        /// <summary>
        /// The total amount of damage dealt during this activity.
        /// </summary>
        public int DamageDealt { get; private set; }

        /// <summary>
        /// The amount of Experience received.
        /// </summary>
        public int ExperienceGained { get; private set; }

        /// <summary>
        /// The amount of Gold received.
        /// </summary>
        public int GoldGained { get; private set; }
    
        /// <summary>
        /// The list of items received. Only lists the item manifest IDs.
        /// </summary>
        public Dictionary<ItemTypes, List<string>> ItemsReceived  { get; private set; }
    
        public PostActivityReport(int damageTaken, int damageDealt, int xp, int gold, Dictionary<ItemTypes, List<string>> rewards)
        {
            DamageTaken = damageTaken;
            DamageDealt = damageDealt;
            ExperienceGained = xp;
            GoldGained = gold;
            ItemsReceived = rewards;
        }
    }   
}