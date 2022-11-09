using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cliche.Activities;

namespace Assets.Lib.Data.API.Cliche.Activities
{
    public abstract class Activity : MonoBehaviour
    {
        public abstract bool IsCompleted();

        public bool IsInProgress { get; protected set; } = false;

        public abstract void StartActivity();

        public abstract PostActivityReport CompleteActivity();
    }
}