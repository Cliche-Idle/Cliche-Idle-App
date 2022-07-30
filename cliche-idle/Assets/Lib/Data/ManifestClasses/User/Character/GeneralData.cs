using Firebase.Firestore;
using Cliche.System;
using UnityEngine;

namespace Cliche.UserManagement
{
    [FirestoreData]
    public class GeneralData
    {
        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public int health { get; set; }

        public int MaxHealth
        {
            // TODO: restructure for access to stats and then code
            get {
                return -1;
            }
        }

        [FirestoreProperty]
        public string race { get; set; }

        [FirestoreProperty]
        public int xp { get; set; }

        public int Level
        {
            get {
                float levelXpScalar = Manifests.GetByID<SingleValueModifier>("XpLevelScalar").Value;
                int level = Mathf.FloorToInt(levelXpScalar * Mathf.Sqrt(xp));
                return level;
            }
        }

        public int XpToNextLevel
        {
            get {
                float levelXpScalar = Manifests.GetByID<SingleValueModifier>("XpLevelScalar").Value;
                int currentLevel = Level;
                int nextLevelXP = Mathf.CeilToInt(Mathf.Pow((currentLevel+1 / levelXpScalar), 2));
                int difference = nextLevelXP - xp;
                return difference;
            }
        }

        [FirestoreProperty]
        public string classSpecName { get; set; }

        // TODO: add visual description storage
    }
}
