using Microsoft.Device.Display;
using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnGUI()
    {
        //Changes both color and font size
        GUIStyle localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.blue;
        localStyle.fontSize = 50;

        var text = "Dual screen device? " + ScreenHelper.IsDualScreenDevice();
        GUI.Label(new Rect(2.0f, 2.0f, 400, 20), text, localStyle);


        var o = Screen.orientation;
        Debug.Log("Screen.orientation: " + o);
        
        GUI.Label(new Rect(2.0f, 100.0f, 400, 20), "Screen orientation: "+o.ToString(), localStyle);

        if (Screen.width == 2784)
        {
            GUI.backgroundColor = Color.gray;
            var r = new Rect(x: 1350, y: 0, width: 84, height: 1800);
            GUI.Box(r,"");
        }
        if (ScreenHelper.IsDualScreenDevice())
        {
            Debug.Log("Is DualScreen");

            try
            {
                var displayMask = DisplayMask.FromResourcesRect();

                Debug.Log("We have a DisplayMask");

                foreach (var rect in displayMask.GetBoundingRects())
                {
                    Debug.Log("DisplayMask Rect: " + rect);
                    Console.WriteLine("DisplayMask Rect: " + rect);
                    GUI.Label(new Rect(2.0f, 50.0f, 400, 20), "DisplayMask Rect: " + rect, localStyle);

                    GUI.backgroundColor = Color.green;

                    if (o == ScreenOrientation.LandscapeLeft || o == ScreenOrientation.LandscapeRight)
                    {   // wide
                        var r = new Rect(x: rect.x - 25, y: rect.y, width: rect.width + 50, height: rect.height);
                        GUI.Box(r, "Ah");
                    }
                    else
                    {   // portrait - tall
                        var r = new Rect(x: rect.y, y: rect.x - 25, width: rect.height, height: rect.width + 50);
                        GUI.Box(r, "Ahhhhhhhhhhhhhhhhhhhh");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
