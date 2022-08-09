using System;
using System.Collections.Generic;
using UnityEngine;
using Cliche.System;

public partial class AdventureHandler
{
    /// <summary>
    /// Starts the specified timed adventure
    /// </summary>
    /// <param name="adventureID"></param>
    public void StartAdventure(string adventureID)
    {
        var player = GameObject.Find("Player");
        if (AvailableAdventures.Contains(adventureID))
        {
            AdventureManifest selectedAdventure = Manifests.GetByID<AdventureManifest>(adventureID);
            if (selectedAdventure != null)
            {
                var statsHandler = player.GetComponent<StatsHandler>();
                // FIXME: Possible issue with very high Intelligence stats; negative overall time
                var adventureTimeReductionPercent = Manifests.GetByID<IntervalValueModifier>("Intelligence").GetAmount(statsHandler.Intelligence.Value);
                var adventureTimeReductionAmount = (selectedAdventure.BaseLength * adventureTimeReductionPercent);
                var adventureLength = (selectedAdventure.BaseLength - adventureTimeReductionAmount);
                Debug.Log($"Adventure ({adventureID}) time reduction amount : {adventureTimeReductionAmount} seconds (from {selectedAdventure.BaseLength} seconds, {adventureTimeReductionPercent*100}%), new length : {adventureLength} seconds.");
                var adventureFinishTime = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(adventureLength)).ToUnixTimeSeconds();
                //
                TransferAdventureToActiveQueue(selectedAdventure.ID, adventureFinishTime);
            }
        }
    }
}