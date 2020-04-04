using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.Device.Display
{
    /// <summary>
    /// Rotation enumeration
    /// </summary>
    public static class Surface {
        public static int ROTATION_0 = 0;
        public static int ROTATION_90 = 1;
        public static int ROTATION_180 = 2;
        public static int ROTATION_270 = 3;
    }
    /// <summary>
    /// https://docs.microsoft.com/dual-screen/android/api-reference/display-mask
    /// </summary>
    public class DisplayMask : IDisposable
    {
        private readonly AndroidJavaObject _displayMask;

        private DisplayMask(AndroidJavaObject displayMask)
        {
            _displayMask = displayMask;
        }

        /// <summary>
        /// Creates the display mask according to config_mainBuiltInDisplayMaskRect.
        /// </summary>
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
        /// <summary>
        /// Creates the display mask according to config_mainBuiltInDisplayMaskRectApproximation, which is the closest rectangle-base approximation of the mask.
        /// </summary>
        public static DisplayMask FromResourcesRectApproximation()
        {
            Debug.Log("DisplayMask.FromResourcesRectApproximation: ");

            var mask = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext");

                using (var dm = new AndroidJavaClass("com.microsoft.device.display.DisplayMask"))
                {
                    return dm.CallStatic<AndroidJavaObject>("fromResourcesRectApproximation", context);
                }
            });

            return new DisplayMask(mask);
        }
        /// <summary>
        /// Returns a list of Rects, each of which is the bounding rectangle for a non-functional area on the display.
        /// </summary>
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

        public RectInt[] GetBoundingRectsForRotation(int rotation)
        {
            var jrects = _displayMask.Call<AndroidJavaObject>("getBoundingRectsForRotation", rotation);
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
        /// <summary>
        /// Returns the bounding region of the mask
        /// </summary>
        public RectInt[] GetBounds()
        {
            var jrects = _displayMask.Call<AndroidJavaObject>("getBounds");
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
}
