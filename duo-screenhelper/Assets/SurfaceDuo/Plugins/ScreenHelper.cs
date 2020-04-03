using System;
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

    public class DisplayMask : IDisposable
    {
        private readonly AndroidJavaObject _displayMask;

        private DisplayMask(AndroidJavaObject displayMask)
        {
            _displayMask = displayMask;
        }

        public static DisplayMask FromResourcesRect()
        {
            Debug.Log("DisplayMask.FromResourcesRect: ");

            var mask = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext");

                using (var dm = new AndroidJavaClass("com.microsoft.device.display.DisplayMask"))
                {
                    return dm.CallStatic<AndroidJavaObject>("fromResourcesRect", context);
                }
            });

            return new DisplayMask(mask);
        }

        public RectInt[] GetBoundingRects()
        {
            var jrects = _displayMask.Call<AndroidJavaObject>("getBoundingRects");
            var size = jrects.Call<int>("size");

            Debug.Log("BoundingRects size: " + size);

            var rects = new RectInt[size];

            for (int i = 0; i < size; i++)
            {
                var jrect = jrects.Call<AndroidJavaObject>("get", i);

                var left = jrect.Get<int>("left");
                var top = jrect.Get<int>("top");
                var width = jrect.Call<int>("width");
                var height = jrect.Call<int>("height");

                rects[i] = new RectInt(xMin: left, yMin: top, width: width, height: height);
            }

            return rects;
        }

        public void Dispose()
        {
            _displayMask?.Dispose();
        }
    }

    public class ScreenHelper
    {
        public static bool IsDualScreenDevice()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            try
            {
                return OnPlayer.Run(p => p
                    .GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext")
                    .Call<AndroidJavaObject>("getPackageManager")
                    .Call<bool>("hasSystemFeature", "com.microsoft.device.display.displaymask"));
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }
    }
}
