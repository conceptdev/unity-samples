using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static int PlayerScore1 = 0;
	public static int PlayerScore2 = 0;
	public GUISkin layout;

	private bool isDuoWide = false;
	private float buttonY = 35;
	private float buttonX = -60;
	private float messagePlayer1 = -150, messagePlayer2 = -150;
	private int lastScreenWidth = 0;

	GameObject theBall;

	// Use this for initialization
	void Start () {
		theBall = GameObject.FindGameObjectWithTag ("Ball");
		Debug.LogWarning("GameManager.Start Screen.width:" + Screen.width);
		lastScreenWidth = Screen.width;
	}

	public static void Score(string wallID) {
		if (wallID == "RightWall") {
			PlayerScore1++;
		} else {
			PlayerScore2++;
		}
	}

	void OnGUI() {
		if (lastScreenWidth != Screen.width)
		{
			lastScreenWidth = Screen.width;
			if (lastScreenWidth == 2784)
			{	// move button messages around seam
				isDuoWide = true;
				buttonX = 300;
				buttonY = 20;
				messagePlayer1 = -900;
				messagePlayer2 = 300;
			}
			else
			{	// revert to defaults (centered)
				isDuoWide = true;
				buttonX = 35;
				buttonY = 60;
				messagePlayer1 = -150;
				messagePlayer2 = -150;
			}
		}

		GUI.skin = layout;
		GUI.Label (new Rect (Screen.width / 2 - 150 - 12, 20, 100, 100), "" + PlayerScore1);
		GUI.Label (new Rect (Screen.width / 2 + 150 + 12, 20, 100, 100), "" + PlayerScore2);

		if (GUI.Button (new Rect (Screen.width / 2 + buttonX, buttonY, 120, 53), "RESTART")) { // move away from seam on Duo
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
