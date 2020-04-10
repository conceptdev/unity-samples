using UnityEngine;

public class PlayerControls : MonoBehaviour {

	public KeyCode moveUp = KeyCode.W;
	public KeyCode moveDown = KeyCode.S;
	public float speed = 10.0f; // HARDCODED
	public int player = 1; 

	private float boundY = 2.5f; // HARDCODED
	private float boundHeight = 5.0f; // HARDCODED
	private Rigidbody2D rb2d;
	private Vector3 position;
	private float height;
	private float middleX;
	private int lastScreenWidth = 0;

	private float lastYPosition = 0;

	// Use this for initialization
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lastScreenWidth != Screen.width)
		{	// detect rotation
			lastScreenWidth = Screen.width;
			height = (float)Screen.height;
			middleX = (float)Screen.width / 2.0f;
		}

		var pos = transform.position;

		// touch controls on mobile, at least two (one for each paddle)
		if (Input.touchCount > 0)
		{
			foreach (var touch in Input.touches)
			{
				// Move the paddle if the screen has the finger moving.
				if (touch.phase == TouchPhase.Moved)
				{
					Vector2 tpos = touch.position;
					
					var tposYpercent = (1 - (tpos.y / height)); // % from bottom up, converted to top-down
					var worldY = boundHeight * tposYpercent;
					var worldYadjusted = boundY - worldY;

					Debug.Log($"TOUCH touch-pos.y: {tpos.y}  tposYpercent:{tposYpercent}  worldY:{worldY}  worldYadjusted:{worldYadjusted}");

					if (tpos.x > middleX && player == 2) // pos.x > 0)
					{// right side of screen
						position = new Vector3(pos.x, worldYadjusted, 0.0f);
						transform.position = position;
						
					} 
					else if (tpos.x < middleX && player == 1) // pos.x < 0)
					{// left side of screen
						position = new Vector3(pos.x, worldYadjusted, 0.0f);
						transform.position = position;
					}
					lastYPosition = worldYadjusted;
				}
			}
		}
		else 
		{	// no touches, might as well check the keyboard
			// legacy, for testing in Unity
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

			if (pos.y > boundY)
			{
				pos.y = boundY;
			}
			else if (pos.y < -boundY)
			{
				pos.y = -boundY;
			}
			transform.position = pos;
		}
	}
}
