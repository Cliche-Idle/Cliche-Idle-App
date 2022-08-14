using System;
using UnityEngine;

namespace Cliche.System
{
    public partial class Manifests
    {        
        public static T GetByID<T>(string manifestID) where T : ScriptableObject
        {
            string typeLocationPath = Paths[typeof(T)];
            T manifest = Resources.Load<T>($"{typeLocationPath}/{manifestID}");
            if (manifest == null)
            {
                manifest = Resources.Load<T>($"{typeLocationPath}/{manifestID}/{manifestID}");
            }
            if (manifest == null)
            {
                Debug.LogError($"Manifest<{typeof(T)}> could not be found at neither path:\nRESOURCES://{typeLocationPath}/{manifestID} \nRESOURCES://{typeLocationPath}/{manifestID}/{manifestID}.");
            }
            return manifest;
        }

        public static ItemManifest GetByObject(Item item)
        {
            string typeLocationPath = Paths[ItemManifestTypes[item.ItemType]];
            Type manifestType = ItemManifestTypes[item.ItemType];
            ItemManifest manifest = (ItemManifest)Resources.Load($"{typeLocationPath}/{item.ID}", manifestType);
            if (manifest == null)
            {
                manifest = (ItemManifest)Resources.Load($"{typeLocationPath}/{item.ID}/{item.ID}", manifestType);
            }
            if (manifest == null)
            {
                Debug.LogError($"Item manifest<{manifestType}> could not be found at neither path:\nRESOURCES://{typeLocationPath}/{item.ID} \nRESOURCES://{typeLocationPath}/{item.ID}/{item.ID}.");
            }
            return manifest;
        }
    }
}