using System;
using UnityEngine;

namespace Microsoft.Device.Display
{
    public static class DeviceHelper
    {
        /// <summary>2784 - width when game spanned over double-portrait (wide)</summary>
        public const int SURFACEDUO_SPANNEDWIDTH = 2784;
        /// <summary>2784 - height when game spanned over double-landscape (tall)</summary>
        public const int SURFACEDUO_SPANNEDHEIGHT = 2784;
        /// <summary>1350 - single screen width in portrait (or the height in landscape)</summary>
        public const int SURFACEDUO_SCREENWIDTH = 1350;
        /// <summary>1800 - single screen height in portrait (or the width in landscape)</summary>
        public const int SURFACEDUO_SCREENHEIGHT = 1800;
        /// <summary>84</summary>
        public const int SURFACEDUO_HINGEWIDTH = 84;

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
