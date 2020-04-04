using System;
using UnityEngine;

namespace Microsoft.Device.Display
{
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
    }
}
