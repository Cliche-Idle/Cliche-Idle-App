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
            return manifest;
        }
    }
}