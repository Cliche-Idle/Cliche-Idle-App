using System.Collections.Generic;

namespace Cliche.Activities
{
    public class PostActivityReport
    {
        public int DamageTaken { get; private set; }

        public int DamageDealt { get; private set; }

        public int ExperienceGained { get; private set; }

        public int GoldGained { get; private set; }
    
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