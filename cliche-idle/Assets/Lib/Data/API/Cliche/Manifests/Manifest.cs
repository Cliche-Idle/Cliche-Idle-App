using System;
using UnityEngine;

namespace Cliche.System
{
    public partial class Manifests
    {      
        /// <summary>
        /// Gets a manifest of type <paramref name="T"/> by its ID, if it exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="manifestID"></param>
        /// <returns></returns>
        public static T GetByID<T>(string manifestID) where T : ScriptableObject
        {
            string typeLocationPath = ManifestPaths[typeof(T)];
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

        /// <summary>
        /// Gets the generic <see cref="ItemManifest"/> of any <see cref="Item"/> based on their intrinsic <see cref="ItemTypes"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ItemManifest GetByObject(Item item)
        {
            string typeLocationPath = ManifestPaths[ItemManifestTypes[item.ItemType]];
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