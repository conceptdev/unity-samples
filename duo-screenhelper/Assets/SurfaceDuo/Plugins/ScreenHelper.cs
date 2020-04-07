﻿using System;
using UnityEngine;

namespace Microsoft.Device.Display
{
    /// <summary>
    /// https://docs.microsoft.com/dual-screen/android/api-reference/dualscreen-layout#screenhelper
    /// </summary>
    public class ScreenHelper
    {
        /// <summary>
        /// com.microsoft.device.dualscreen.layout.ScreenHelper
        /// </summary>
        static string SCREENHELPER_CLASSNAME = "com.microsoft.device.dualscreen.layout.ScreenHelper";
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
        /// <summary>
        /// https://docs.microsoft.com/dual-screen/android/sample-code/is-app-spanned
        /// </summary>
        public static bool IsAppSpanned()
        {
            var displayMask = DisplayMask.FromResourcesRectApproximation();
#if !UNITY_EDITOR && UNITY_ANDROID
            
            throw new NotImplementedException("IsAppSpanned is not yet implemented");
            
            return false;
#else
            return false;
#endif
        }

        public static bool IsDeviceSurfaceDuo()
        {
            var isDuo = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");

                using (var dm = new AndroidJavaClass(SCREENHELPER_CLASSNAME))
                {
                    return dm.CallStatic<bool>("isDeviceSurfaceDuo", activity);
                }
            });
            return isDuo;
        }

        /// <summary>
        /// Returns the coordinates of the Hinge in a Rect object.
        /// </summary>
        public static RectInt GetHinge()
        {
            var hinge = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity");

                using (var dm = new AndroidJavaClass(SCREENHELPER_CLASSNAME))
                {
                    return dm.CallStatic<AndroidJavaObject>("getHinge", context);
                }
            });

            Debug.Log("hinge: " + hinge);
            if (hinge != null)
            {
                var left = hinge.Get<int>("left");
                var top = hinge.Get<int>("top");
                var width = hinge.Call<int>("width");
                var height = hinge.Call<int>("height");

                return new RectInt(left, top, width, height);
            }
            else return new RectInt (0,0,0,0); // TODO: return null??
        }

        /// <summary>
        /// Returns a boolean that indicates whether the application is in spanned mode or not
        /// </summary>
        public static bool IsDualMode()
        {
            var isDualMode = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");
                using (var sc = new AndroidJavaClass("com.microsoft.device.dualscreen.layout.ScreenHelper"))
                {
                    return sc.CallStatic<bool>("isDualMode", activity);
                }
            });
            return isDualMode;
        }

        /// <summary>
        /// Returns the rotation of the screen - Surface.ROTATION_0, Surface.ROTATION_90, Surface.ROTATION_180, Surface.ROTATION_270
        /// </summary>
        public static int GetCurrentRotation()
        {
            var rotation = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");

                using (var dm = new AndroidJavaClass("com.microsoft.device.dualscreen.layout.ScreenHelper"))
                {
                    return dm.CallStatic<int>("getCurrentRotation", activity);
                }
            });
            return rotation;
        }

        /// <summary>
        /// Returns a list of two elements that contain the coordinates of the screen rectangles
        /// </summary>
        public static RectInt[] GetScreenRectangles()
        {
            var jScreenRects = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity");

                using (var dm = new AndroidJavaClass(SCREENHELPER_CLASSNAME))
                {
                    return dm.CallStatic<AndroidJavaObject>("getScreenRectangles", context);
                }
            });

            var size = jScreenRects.Call<int>("size");
            if (size > 0)
            {
                var rectangles = new RectInt[size];
                for (var i = 0; i < size; i++)
                {
                    var jRect = jScreenRects.Call<AndroidJavaObject>("get", i);

                    var left = jRect.Get<int>("left");
                    var top = jRect.Get<int>("top");
                    var width = jRect.Call<int>("width");
                    var height = jRect.Call<int>("height");

                    rectangles[i] = new RectInt(left, top, width, height);
                }
                return rectangles;
            }
            else return new RectInt[0]; // TODO: return null??
        }
    }
}
