using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Android sensors for Unity
/// https://unitylist.com/p/j7h/Unity-Android-Sensors
/// https://github.com/mmeiburg/unityAndroidSensors
/// Inspired by
/// https://unity.com/how-to/architect-game-code-scriptable-objects
/// https://github.com/roboryantron/Unite2017/tree/master/Assets/Code
/// </summary>

namespace Microsoft.Device.Display
{
    public class HingeSensor : IDisposable
    {
        AndroidJavaObject plugin;
        const string sensorName = "hingeangle"; // 'hingeangle' is a static identifier in the java plugin

        /// <summary>
        /// Only get an object when the plugin is created
        /// </summary>
        HingeSensor(AndroidJavaObject sensorPlugin)
        {
            plugin = sensorPlugin;
        }
        /// <summary>
        /// Create an object to read the hinge sensor
        /// </summary>
        public static HingeSensor Start()
        {
#if UNITY_ANDROID
            var plugin = new AndroidJavaClass("jp.kshoji.unity.sensor.UnitySensorPlugin").CallStatic<AndroidJavaObject>("getInstance");
            if (plugin != null)
            {// will be in AndroidX in future
             // https://developer.android.com/reference/android/hardware/Sensor#TYPE_HINGE_ANGLE
             // https://developer.android.com/reference/android/hardware/Sensor#STRING_TYPE_HINGE_ANGLE
                plugin.Call("startSensorListening", sensorName);
                var hs = new HingeSensor(plugin);
                return hs;
            }
#endif   
            return null;
        }

        /// <summary>
        /// Get the angle between the two screens
        /// </summary>
        /// <returns>0 to 360 (closed to fully opened), or -1 if error</returns>
        public float GetHingeAngle() {
#if UNITY_ANDROID
            if (plugin != null)
            {
                float[] valueArray = plugin.Call<float[]>("getSensorValues", sensorName);
                return valueArray[0];
            }
            else return -1;
#else
            return -1;
#endif
        }

        public void StopSensing()
        { 
            if (plugin != null)
            {
                plugin.Call("terminate");
                plugin = null;
            }
        }
        public void Dispose()
        {
            if (plugin != null)
            {
                plugin.Call("terminate");
                plugin = null;
            }
        }

 
    }

}