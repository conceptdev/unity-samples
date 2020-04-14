using UnityEngine;
using Microsoft.Device.Display;
public class GameManager : MonoBehaviour {

	public static int PlayerScore1 = 0;
	public static int PlayerScore2 = 0;
	public GUISkin layout;

	private float buttonY = 35;
	private float buttonX = -100;
	const float BUTTON_WIDTH = 200;
	private float messagePlayer1 = -150, messagePlayer2 = -150;
	private int lastScreenWidth = 0;
	private bool isDualScreenDevice = false;

	GameObject theBall;

	// Use this for initialization
	void Start () {
		theBall = GameObject.FindGameObjectWithTag ("Ball");
		Debug.LogWarning("GameManager.Start Screen.width:" + Screen.width);
		lastScreenWidth = Screen.width;

		isDualScreenDevice = ScreenHelper.IsDeviceSurfaceDuo();
		Debug.LogWarning("ScreenHelper.IsDeviceSurfaceDuo:" + isDualScreenDevice);

		if (isDualScreenDevice)
		{	// duo reports rotation strangely...
			if (Screen.orientation == ScreenOrientation.LandscapeLeft)
			{
				Screen.orientation = ScreenOrientation.LandscapeRight;
			}
			else if (Screen.orientation == ScreenOrientation.LandscapeRight)
			{
				Screen.orientation = ScreenOrientation.LandscapeLeft;
			}
			else
			{	// TODO: consider forcing landscape ....
				Screen.orientation = ScreenOrientation.AutoRotation;
			}
		}
	}

	public static void Score(string wallID) {
		if (wallID == "RightWall") {
			PlayerScore1++;
		} else {
			PlayerScore2++;
		}
	}

	void OnGUI() {

#if UNITY_EDITOR
		//Hardcode the seam to specific width (2784x1800)
		if (Screen.width == 2784)
		{
			GUI.backgroundColor = Color.gray;
			var r = new Rect(x: 1350, y: 0, width: 84, height: 1800);
			GUI.Box(r, "");
		}
		else if (Screen.height == 2784)
		{
			GUI.backgroundColor = Color.gray;
			var r = new Rect(x: 0, y: 1350, width: 1800, height: 84);
			GUI.Box(r, "");
		}
#endif


		if (isDualScreenDevice && lastScreenWidth != Screen.width)
		{	// dual-screen and has been spanned or rotated...
			var isSpanned = ScreenHelper.IsDualMode();
			lastScreenWidth = Screen.width;
			if (isSpanned)
			{	// move button messages around seam
				buttonX = 300;
				buttonY = 20;
				messagePlayer1 = -900;
				messagePlayer2 = 300;
			}
			else
			{	// revert to defaults (centered)
				buttonX = -60;
				buttonY = 35;
				messagePlayer1 = -150;
				messagePlayer2 = -150;
			}
		}

		GUI.skin = layout;
		GUI.Label (new Rect (Screen.width / 2 - 230 - 12, 20, 100, 100), "" + PlayerScore1);
		GUI.Label (new Rect (Screen.width / 2 + 200 + 12, 20, 100, 100), "" + PlayerScore2);

		if (GUI.Button (new Rect (Screen.width / 2 + buttonX, buttonY, BUTTON_WIDTH, 53), "RESTART")) { // move away from seam on Duo
			PlayerScore1 = 0;
			PlayerScore2 = 0;
			theBall.SendMessage ("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
		}

		if (PlayerScore1 == 10) {
			GUI.Label (new Rect (Screen.width / 2 + messagePlayer1, 200, 2000, 1000), "PLAYER ONE WINS");
			theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
		} else if (PlayerScore2 == 10) {
			GUI.Label (new Rect (Screen.width / 2 + messagePlayer2, 200, 2000, 1000), "PLAYER TWO WINS");
			theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
		}
	}
}
