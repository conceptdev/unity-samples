using Microsoft.Device.Display;
using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnGUI()
    {
        const float ROW_HEIGHT = 50.0f;
        const float COL_WIDTH = 610.0f;
        const float HEAD_INDENT = 410.0f;

        //Changes both color and font size
        GUIStyle localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.black;
        localStyle.fontSize = 45;

#if UNITY_EDITOR
        //Hardcode the seam to specific width (2784x1800)
        if (Screen.width == 2784)
        {
            GUI.backgroundColor = Color.gray;
            var r = new Rect(x: 1350, y: 0, width: 84, height: 1800);
            GUI.Box(r,"");
        }
#endif

        GUI.Label(new Rect(2.0f, 2.0f, 200, 20), "Unity screen orientation:", localStyle);
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 1, 400, 20), "-DeviceHelper-", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 2, 200, 20), "IsDualScreenDevice:", localStyle);
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 3, 200, 20), "-DisplayMask-", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 4, 200, 20), "DisplayMask rect:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 5, 200, 20), "ResourcesRectApprox:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 6, 200, 20), "BoundingRectsForRot:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 7, 200, 20), "GetBoundsRegionBounds:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 8, 200, 20), "IsAppSpanned:", localStyle);
        GUI.Label(new Rect(HEAD_INDENT, ROW_HEIGHT * 9, 200, 20), "-ScreenHelper-", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 10, 200, 20), "IsDualMode:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 11, 200, 20), "IsDeviceSurfaceDuo:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 12, 200, 20), "GetCurrentRotation:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 13, 200, 20), "GetHinge:", localStyle);
        GUI.Label(new Rect(2.0f, ROW_HEIGHT * 14, 200, 20), "GetScreenRectangles:", localStyle);

        

        localStyle.normal.textColor = Color.blue;
        GUI.Label(new Rect(COL_WIDTH, 2.0f, 400, 20), Screen.orientation.ToString(), localStyle);
        GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 2, 400, 20), ScreenHelper.IsDualScreenDevice().ToString(), localStyle);
        

        if (DeviceHelper.IsDualScreenDevice())
        {
            Debug.Log("IsDualScreenDevice: true");

            try
            {
                var displayMask = DisplayMask.FromResourcesRect();

                Debug.Log("We have a DisplayMask");
                // draw the DisplayMask rectangle, but with a 25 pixel bleed so you can see on the device
                foreach (var rect in displayMask.GetBoundingRects())
                {
                    Debug.Log("DisplayMask Rect: " + rect);
                    GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 4, 400, 20), rect.ToString(), localStyle);

                    var o = Screen.orientation;
                    if (o == ScreenOrientation.LandscapeLeft || o == ScreenOrientation.LandscapeRight)
                    {   // wide
                        var r = new Rect(x: rect.x - 25, y: rect.y, width: rect.width + 50, height: rect.height);
                        GUI.Box(r, "HV");
                    }
                    else
                    {   // portrait - tall
                        var r = new Rect(x: rect.y, y: rect.x - 25, width: rect.height, height: rect.width + 50);
                        GUI.Box(r, "Hinge horizontal");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("DisplayMask.FromResourcesRect: " + e);
            }

            try
            {
                var displayMask = DisplayMask.FromResourcesRectApproximation();
                foreach (var rect in displayMask.GetBoundingRects())
                {
                    Debug.Log("ResourcesRectApprox Rect: " + rect);
                    GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 5, 400, 20), rect.ToString(), localStyle);
                }


                int orientation = Surface.ROTATION_0; // Portrait
                switch (Screen.orientation) { 
                case ScreenOrientation.LandscapeLeft:
                    orientation = Surface.ROTATION_90; break;
                case ScreenOrientation.LandscapeRight:
                    orientation = Surface.ROTATION_270; break;
                case ScreenOrientation.PortraitUpsideDown:
                    orientation = Surface.ROTATION_180; break;
                }
                foreach (var rect in displayMask.GetBoundingRectsForRotation(orientation))
                {
                    Debug.Log("BoundingRectsForRot Rect: " + rect);
                    GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 6, 400, 20), rect.ToString(), localStyle);
                }

                var regionBounds = displayMask.GetBoundsRegionBounds();
                Debug.Log("GetBoundsRegionBounds: " + regionBounds);
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 7, 400, 20), regionBounds.ToString(), localStyle);
                
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("DisplayMask stuff: " + e);
            }
            try
            {
                var isSpanned = DisplayMask.IsAppSpanned();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 8, 400, 20), isSpanned.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("DisplayMask.IsAppSpanned: " + e);
            }

            try {
                var isDualMode = ScreenHelper.IsDualMode();

                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 10, 400, 20), isDualMode.ToString(), localStyle);

            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.IsDualMode: " + e);
            }

            try
            {
                var isSurfaceDuo = ScreenHelper.IsDeviceSurfaceDuo();

                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 11, 400, 20), isSurfaceDuo.ToString(), localStyle);

            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.IsDeviceSurfaceDuo: " + e);
            }

            try
            {
                var currentRotation = ScreenHelper.GetCurrentRotation();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 12, 400, 20), currentRotation.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.GetCurrentRotation: " + e);
            }

            try
            {
                var hinge = ScreenHelper.GetHinge();
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 13, 400, 20), hinge.ToString(), localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.GetHinge: " + e);
            }

            try
            {
                var rects = ScreenHelper.GetScreenRectangles();
                var rectString = "";
                foreach (var rect in rects) {
                    rectString += rect.ToString() + "," + Environment.NewLine;
                }
                GUI.Label(new Rect(COL_WIDTH, ROW_HEIGHT * 14, 1000, 20 * 2), rectString, localStyle);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("ScreenHelper.GetHinge: " + e);
            }
        }
#if UNITY_EDITOR
        else
        {
            GUI.Label(new Rect(2.0f, ROW_HEIGHT * 16, 400, 20), "(most dual-screen attributes have no value in editor)", localStyle);
        }
#endif
    }
}
