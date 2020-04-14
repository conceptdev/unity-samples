using System;
using UnityEngine;

namespace Microsoft.Device.Display
{
    public static class DeviceHelper
    {
        /// <summary>
        /// Determine whether your app is running on a dual-screen device. 
        /// You should perform this check before you call APIs from the Surface Duo SDK
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/dual-screen/android/sample-code/is-dual-screen-device
        /// </remarks>
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
