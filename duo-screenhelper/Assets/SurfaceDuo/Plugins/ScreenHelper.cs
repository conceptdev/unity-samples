using System;
using UnityEngine;

namespace Microsoft.Device.Display
{
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

        public static int GetCurrentRotation()
        {
            var rotation = OnPlayer.Run(p =>
            {
                var activity = p.GetStatic<AndroidJavaObject>("currentActivity");

                using (var dm = new AndroidJavaClass("com.microsoft.device.dualscreen.layout.ScreenHelper"))
                {
                    /*
                     ScreenHelper.IsDeviceSurfaceDuo: UnityEngine.AndroidJavaException: 
                     java.lang.NoSuchMethodError: no static method with name='isDeviceSurfaceDuo' 
                     signature='(Lcom.unity3d.player.UnityPlayerActivity;)Ljava/lang/Object;' 
                     in class Lcom.microsoft.device.dualscreen.layout.ScreenHelper;
                     */
                    return dm.CallStatic<int>("getCurrentRotation", activity);
                }
            });
            return rotation;
        }
    }
}
