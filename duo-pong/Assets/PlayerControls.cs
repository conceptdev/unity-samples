using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

	public KeyCode moveUp = KeyCode.W;
	public KeyCode moveDown = KeyCode.S;
	public float speed = 10.0f;
	public float boundY = 2.5f;
	private Rigidbody2D rb2d;
	
	private int lastScreenWidth = 0;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		
		height = (float)Screen.height;
		middleX = (float)Screen.width / 2.0f;

		bottomleft = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, Screen.width, 0), Camera.MonoOrStereoscopicEye.Mono);
		Debug.LogError($"Start bottomleft: {bottomleft}");
	}

	private Vector3 position;
	
	private float height;
	private float middleX;
	private Vector3 bottomleft;

	// Update is called once per frame
	void Update ()
	{
		if (lastScreenWidth != Screen.width)
		{
			lastScreenWidth = Screen.width;
			height = (float)Screen.height;
			middleX = (float)Screen.width / 2.0f;
			bottomleft = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, Screen.width, 0), Camera.MonoOrStereoscopicEye.Mono);
			Debug.LogError($"Update bottomleft: {bottomleft}");
		}
		
		
		var vel = rb2d.velocity;
		if (Input.GetKey(moveUp))
		{
			vel.y = speed;
		}
		else if (Input.GetKey(moveDown))
		{
			vel.y = -speed;
		}
		else if (!Input.anyKey)
		{
			vel.y = 0;
		}
		rb2d.velocity = vel;

		var pos = transform.position;
		pos = RestrictBounds(pos);
		transform.position = pos;

		//Debug.Log($"AFTER KEY paddle-pos: {pos.x},{pos.y}");

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			// Move the paddle if the screen has the finger moving.
			if (touch.phase == TouchPhase.Moved)
			{
				Vector2 tpos = touch.position;
				//Debug.Log($"TOUCH touch-pos: {tpos.x},{tpos.y}    paddle-pos: {pos.x},{pos.y}");
				// 874/1800 = .48
				// 5*.48 = 2.42
				// 2.5 - 2.42 = 0.08
				var tposYpercent = (1 - (tpos.y / height)); // % from bottom up, converted to top-down
				var worldY = 5.0f * tposYpercent;
				var worldYadjusted = 2.5f - worldY;

				Debug.Log($"TOUCH touch-pos.y: {tpos.y}  tposYpercent:{tposYpercent}  worldY:{worldY}  worldYadjusted:{worldYadjusted}   bottomleft:{bottomleft.x},{bottomleft.y}  Screen.width:{Screen.width}");

				if (tpos.x > middleX && pos.x > 0)
				{
					//tpos.x = (tpos.x - width) / width;
					//tpos.y = (tpos.y - height) / height;
					position = new Vector3(pos.x, worldYadjusted, 0.0f);
					//position = RestrictBounds(position);
					transform.position = position;
				}
				else if (tpos.x < middleX && pos.x < 0)
				{
					//tpos.y = (tpos.y - height) / height;
					//position = new Vector3(pos.x, tpos.y, 0.0f);
					position = new Vector3(pos.x, worldYadjusted, 0.0f);
					//position = RestrictBounds(position);
					transform.position = position;
				}

				//vel.y = speed;
			}
		}
		//else
		//{
		//	// no touch
		//	vel.y = 0;
		//}
		//rb2d.velocity = vel;

		Vector3 RestrictBounds(Vector3 proposedPos)
		{
			if (proposedPos.y > boundY)
			{
				proposedPos.y = boundY;
			}
			else if (proposedPos.y < -boundY)
			{
				proposedPos.y = -boundY;
			}

			return proposedPos;
		}
	}
}
