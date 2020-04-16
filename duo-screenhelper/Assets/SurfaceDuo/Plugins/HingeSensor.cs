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
            var sensor = OnPlayer.Run(p =>
            {
                var context = p.GetStatic<AndroidJavaObject>("currentActivity");

                var plugin = new AndroidJavaClass("com.microsoft.device.dualscreen.unity.HingeAngleSensor")
                        .CallStatic<AndroidJavaObject>("getInstance", context);

                if (plugin != null) { 
                    plugin.Call("setupSensor");
                    return new HingeSensor(plugin);
                }
                else
                {
                    return null;
                }
            });
                
            return sensor;
#else   
            return null;
#endif
        }

        /// <summary>
        /// Get the angle between the two screens
        /// </summary>
        /// <returns>0 to 360 (closed to fully opened), or -1 if error</returns>
        public float GetHingeAngle() {
#if UNITY_ANDROID
            if (plugin != null)
            {
                //float[] valueArray = plugin.Call<float[]>("getSensorValues", sensorName);
                float angle = plugin.Call<float>("getHingeAngle");
                return angle;
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
                plugin.Call("dispose");
                plugin = null;
            }
        }
        public void Dispose()
        {
            if (plugin != null)
            {
                plugin.Call("dispose");
                plugin = null;
            }
        }

 
    }

}