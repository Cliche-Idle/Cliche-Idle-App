using System;
using System.Collections;
using UnityEngine;
using Cliche.Activities;
using Cliche.System;

namespace Assets.Lib.Data.API.Cliche.Activities
{
    public class TimedActivity : Activity
    {
        public TimedActivity(string adventureID, double adventureEndTime)
        {
            ID = adventureID;
            EndTime = adventureEndTime;
            Finished = false;
        }

        /// <summary>
        /// The ID of the activity.
        /// </summary>
        [field: SerializeField]
        public string ID { get; private set; }

        /// <summary>
        /// The time (in UNIX format) when the adventure completes.
        /// </summary>
        [field: SerializeField]
        public double EndTime { get; private set; }

        /// <summary>
        /// Shorthand for checking if the adventure is finished.
        /// </summary>
        [field: SerializeField]
        public bool Finished { get; private set; }

        public override bool IsCompleted()
        {
            if (Finished == false)
            {
                var baseTime = DateTime.UnixEpoch;
                baseTime = baseTime.AddSeconds(EndTime);
                if (baseTime < DateTime.UtcNow)
                {
                    Finished = true;
                }
            }
            return Finished;
        }

        public override void StartActivity()
        {
            if (IsInProgress == false)
            {
                var player = GameObject.Find("Player");
                AdventureManifest selectedAdventure = Manifests.GetByID<AdventureManifest>(ID);
                if (selectedAdventure != null)
                {
                    var statsHandler = player.GetComponent<StatsHandler>();
                    // FIXME: Possible issue with very high Intelligence stats; negative overall time
                    var adventureTimeReductionPercent = Manifests.GetByID<IntervalValueModifier>("Intelligence").GetAmount(statsHandler.Intelligence.Value);
                    var adventureTimeReductionAmount = (selectedAdventure.BaseLength * adventureTimeReductionPercent);
                    var adventureLength = (selectedAdventure.BaseLength - adventureTimeReductionAmount);

                    Debug.Log($"Adventure ({ID}) time reduction amount : {adventureTimeReductionAmount} seconds (from {selectedAdventure.BaseLength} seconds, {adventureTimeReductionPercent * 100}%), new length : {adventureLength} seconds.");
                    
                    var adventureFinishTime = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(adventureLength)).ToUnixTimeSeconds();
                    
                    //
                    EndTime = adventureFinishTime;
                    IsInProgress = true;
                }
            }
        }

        public override PostActivityReport CompleteActivity()
        {
            throw new System.NotImplementedException();
        }
    }
}