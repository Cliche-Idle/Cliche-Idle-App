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
        if (AvailableAdventures.Contains(adventureID))
        {
            AdventureManifest selectedAdventure = Manifests.GetByID<AdventureManifest>(adventureID);
            if (selectedAdventure != null)
            {
                int intelligenceStatValue = gameObject.GetComponent<StatsHandler>().Intelligence.Value;
                var adventureTimeReductionPercent = Manifests.GetByID<IntervalValueModifier>("Intelligence").GetAmount(intelligenceStatValue);
                // Restrict time reduction to maximum half of the base adventure time
                var adventureTimeReductionAmount = ((selectedAdventure.BaseLength / 2) * adventureTimeReductionPercent);
                var adventureLength = (selectedAdventure.BaseLength - adventureTimeReductionAmount);
                // Convert the length from seconds to an unix timestamp when the adventure ends
                var adventureFinishTime = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(adventureLength)).ToUnixTimeSeconds();
                //
                TransferAdventureToActiveQueue(selectedAdventure.ID, adventureFinishTime);
            }
        }
    }
}