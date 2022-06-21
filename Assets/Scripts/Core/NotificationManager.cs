using System;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

namespace ProjectU
{
    public class NotificationManger
    {
        public static readonly Dictionary<string, Action<object>> DeathCallbacks = new Dictionary<string, Action<object>>();
        public static void CallOnDeath(GameObject actor)
        {
            var tags = actor._GetTags();

            foreach (var tag in tags)
            {
                if (DeathCallbacks.ContainsKey(tag))
                    DeathCallbacks[tag]?.Invoke(actor);
            }
        }
    }
}