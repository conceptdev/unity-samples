using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
    /// Surface Duo DisplayMask helper. 
    /// Call <see cref="DeviceHelper.IsDualScreenDevice"/> before using methods on this class.
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/dual-screen/android/api-reference/display-mask
    /// </remarks>
    public class DisplayMask : IDisposable
    {
        /// <summary>
        /// com.microsoft.device.display.DisplayMask
        /// </summary>
        static string DISPLAYMASK_CLASSNAME = "com.microsoft.device.display.DisplayMask";

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

                using (var dm = new AndroidJavaClass(DISPLAYMASK_CLASSNAME))
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

                using (var dm = new AndroidJavaClass(DISPLAYMASK_CLASSNAME))
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
        /// <remarks>
        /// DisplayMask.getBounds returns an SKRegion which doesn't have a simple
        /// equivalent in C# (that I could think of). For Surface Duo the code just
        /// confirms it's a rectangle and calls getBounds() on the region.
        /// </remarks>
        public RectInt GetBoundsRegionBounds()
        {
            var jrects = _displayMask.Call<AndroidJavaObject>("getBounds");
            var isComplex = jrects.Call<bool>("isComplex"); // SKRegion
            var isEmpty = jrects.Call<bool>("isEmpty"); // SKRegion
            var isRect = jrects.Call<bool>("isRect"); // SKRegion

            Debug.LogWarning($"GetBounds isComplex:{isComplex} isEmpty:{isEmpty} isRect:{isRect}");

            if (isRect & !isEmpty & !isComplex)
            {
                var rect = jrects.Call<AndroidJavaObject>("getBounds");

                var left = rect.Get<int>("left");
                var top = rect.Get<int>("top");
                var width = rect.Call<int>("width");
                var height = rect.Call<int>("height");

                return new RectInt(xMin: left, yMin: top, width: width, height: height);
            }
            return new RectInt (0,0,0,0);
        }

        /// <summary>
        /// Whether the app is spanned across both screens
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/dual-screen/android/sample-code/is-app-spanned?tabs=java
        /// </remarks>
        public static bool IsAppSpanned()
        {
            var isSpanned = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext");
                using (var dm = new AndroidJavaClass(DISPLAYMASK_CLASSNAME))
                {
                    var displayMask = dm.CallStatic<AndroidJavaObject>("fromResourcesRectApproximation", context);
                    var region = displayMask.Call<AndroidJavaObject>("getBounds");
                    var boundings = displayMask.Call<AndroidJavaObject>("getBoundingRects");
                    var size = boundings.Call<int>("size");
                    if (size > 0)
                    {
                        var first = boundings.Call<AndroidJavaObject>("get", 0);
                        var rootView = p.GetStatic<AndroidJavaObject>("currentActivity")
                            .Call<AndroidJavaObject>("getWindow")
                            .Call<AndroidJavaObject>("getDecorView")
                            .Call<AndroidJavaObject>("getRootView");
                        var drawingRect = new AndroidJavaObject("android.graphics.Rect");
                        rootView.Call("getDrawingRect", drawingRect);
                        if (first.Call<bool>("intersect", drawingRect))
                        {
                            Debug.LogWarning($"Dual screen - intersect");
                            return true;
                        }
                        else
                        {
                            Debug.LogWarning($"Single screen - not intersect");
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            });
            return isSpanned;
        }

        public void Dispose()
        {
            _displayMask?.Dispose();
        }
    }
}
