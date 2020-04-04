using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.Device.Display
{
    internal class OnPlayer
    {
        public static T Run<T>(Func<AndroidJavaClass, T> runner)
        {
            if (runner == null)
                throw new ArgumentNullException(nameof(runner));

            using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                return runner(player);
            }
        }
    }
}
