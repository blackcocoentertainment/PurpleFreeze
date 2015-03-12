using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	public bool isSwipeBegin = false;
	public bool gotASwipe = false;

	public float JumpAngle = 0.0f;
	public float JumpLength = 0.0f;
	public float JumpSpeed = 2.0f;
	public int GroundStatus = 0;
	public int WallStatus = 0;
	public float vertialMovement = 1.28f;
	public float horizontalMovement = 0.64f;

	public Vector3 SwipeBeginPosition;
	public Vector3 SwipeEndPosition;
	private Transform _transform;
	private Transform _GC_BC;
	private Transform _GC_BL;
	private Transform _GC_BR;
	private Transform _WC_L;
	private Transform _WC_R;
	private Animator _playerAnimator;
	private ObjectMovement _movementScript;

	// Use this for initialization
	void Start () 
	{
		_playerAnimator = gameObject.GetComponent<Animator>();
		_movementScript = gameObject.GetComponent<ObjectMovement>();
		_transform = gameObject.transform;

		// Setup groundcheck objects!
		_GC_BC = _transform.FindChild("GC_BC");
		_GC_BL = _transform.FindChild("GC_BL");
		_GC_BR = _transform.FindChild("GC_BR");
		_WC_L  = _transform.FindChild("WC_L");
		_WC_R  = _transform.FindChild("WC_R");
	}

	void FixedUpdate()
	{
		// Check ground first!
		MovementColliderControl();
		// Only add force if on surface!
		if(GroundStatus != 0)
		{
			if(gotASwipe == true)
			{
				// If not moving!
				if(_movementScript.IsMoving == false)
				{
					// Check angle! Act accordingly!
					if(JumpAngle > -22.5f && JumpAngle < 22.5f)
					{
						// MoveRight! Only if not already on top right!
						if(WallStatus != 2)
						{
							// Let the sun shine!!! Let the sun shine, in!!!
							_movementScript.TargetDestination = new Vector2(_transform.position.x + horizontalMovement, _transform.position.y);
						}
					}
					else if((JumpAngle < -157.5f && JumpAngle > -180.0f) || JumpAngle > 157.5f)
					{
						// Move left!
						if(WallStatus != 1)
						{
							_movementScript.TargetDestination = new Vector2(_transform.position.x - horizontalMovement, _transform.position.y);
						}
					}
					else if(JumpAngle > 67.5f && JumpAngle < 112.5f)
					{
						// Up!
						_movementScript.TargetDestination = new Vector2(_transform.position.x, _transform.position.y + vertialMovement);
						//_playerAnimator.SetTrigger("Jump_UP");
					}
					else if(JumpAngle > -112.5f && JumpAngle < -67.5f)
					{
						// Down
						_movementScript.TargetDestination = new Vector2(_transform.position.x, _transform.position.y - vertialMovement);
					}
					// Diagonal controls for now! Upper right or left!
					else if(JumpAngle > 22.5f && JumpAngle < 67.5f)
					{
						if(WallStatus != 2)
						{
							_movementScript.TargetDestination = new Vector2(_transform.position.x + horizontalMovement, _transform.position.y + vertialMovement);
						}
					}
					else if(JumpAngle > 112.5f && JumpAngle < 157.5f)
					{
						if(WallStatus != 1)
						{
							_movementScript.TargetDestination = new Vector2(_transform.position.x - horizontalMovement, _transform.position.y + vertialMovement);
						}
					}
				}

				// We used that swipe, make it false no matter what!
				gotASwipe = false;
			}
		}
		else
		{
			// Not grounded! So if "not moving"; we must go down beatch!
			if(_movementScript.IsMoving == false)
			{
				// Yep, not moving! Go down for 1 space!
				_movementScript.TargetDestination = new Vector2(_transform.position.x, _transform.position.y - vertialMovement);
			}

			gotASwipe = false;
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Sidewall")
		{
			// Hitted the wall!
		}
		else if(collider.gameObject.tag == "Platform")
		{
			// Above or below! If below; we can stand on it!
			//Debug.Log ("OnCollisionEnter2D");
			//_rigidbody2D.velocity = new Vector2(0,0);
		}
	}
	

	// Update is called once per frame
	void Update () 
	{
		// Mouse events are only valid for standalone and web. We'll use Input.touch for
		// mobile! There is not much need for isSwipeBegin controls but good to add them now
		// because we may add more control for player later!
#if UNITY_STANDALONE
		if(Input.GetMouseButtonDown(0) && isSwipeBegin == false)
		{
#elif UNITY_ANDROID  || UNITY_IOS
		if(Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			
#endif
			// Starting to swipe
			isSwipeBegin = true;
			gotASwipe = false;
			// Get mouse position for begin!
			Vector3 startPosition; 
			// We'll have a switch on startPosition init too because for touch events;
			// we can't use mousePosition.y (We can actually but not very "efficient".
			// So for mobile, we'll use touch event position and etc!
#if UNITY_STANDALONE
			startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
#elif UNITY_ANDROID  || UNITY_IOS
			startPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
#endif
			// Translate mouse position as world coordinates!
			SwipeBeginPosition = startPosition;//Camera.main.ScreenToWorldPoint(startPosition);
		}
#if UNITY_STANDALONE
		else if(Input.GetMouseButtonUp(0) && isSwipeBegin == true)
#elif UNITY_ANDROID || UNITY_IOS
		else if(Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
#endif
		{
			// Now it means that up!
			isSwipeBegin = false; // Finished!
			// No need to get for "end". We get it on update all the time!
			gotASwipe = true;

			JumpAngle = Mathf.Atan2(SwipeEndPosition.y - SwipeBeginPosition.y, SwipeEndPosition.x - SwipeBeginPosition.x) * Mathf.Rad2Deg;
			JumpLength = Vector2.Distance(Camera.main.ScreenToWorldPoint(SwipeBeginPosition), Camera.main.ScreenToWorldPoint(SwipeEndPosition));
		}

		if(isSwipeBegin == true)
		{
			Vector3 mousePosition;
#if UNITY_STANDALONE
			mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
#elif UNITY_ANDROID || UNITY_IOS
			if(Input.touchCount != 0)
			{
				mousePosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);
			}
			else
			{
				mousePosition = new Vector3();
			}
#endif
			// Translate mouse position as world coordinates!
			SwipeEndPosition = mousePosition;//Camera.main.ScreenToWorldPoint(mousePosition);
		}

		//if(gotASwipe == true)
		{
			// Draw 2DDebugRay!
			Vector2 lineStart = Camera.main.ScreenToWorldPoint(SwipeBeginPosition);
			Vector2 lineEnd = Camera.main.ScreenToWorldPoint(SwipeEndPosition);
			Debug.DrawLine(new Vector3(lineStart.x, lineStart.y, 0),
			               new Vector3(lineEnd.x, lineEnd.y, 0));
		}
	}

	private void MovementColliderControl()
	{
		int newStatus = 0;
		// Check grounds first!
		Collider2D col = Physics2D.OverlapPoint(_GC_BL.position);
		if(col != null)
		{
			if(col.gameObject.tag.Equals("Platform"))
			{
				newStatus |= 2;
			}
		}

		col = Physics2D.OverlapPoint(_GC_BC.position);
		if(col != null)
		{
			if(col.gameObject.tag.Equals("Platform"))
			{
				newStatus |= 1;
			}
		}

		col = Physics2D.OverlapPoint(_GC_BR.position);
		if(col != null)
		{
			if(col.gameObject.tag.Equals("Platform"))
			{
				newStatus |= 4;
			}
		}

		// Set groundstatus!
		GroundStatus = newStatus;
		newStatus = 0;
	
		// Now for wall controllers!
		col = Physics2D.OverlapPoint(_WC_L.position);
		if(col != null)
		{
			if(col.gameObject.tag.Equals("Wall"))
			{
				newStatus |= 1;
			}
		}

		col = Physics2D.OverlapPoint(_WC_R.position);
		if(col != null)
		{
			if(col.gameObject.tag.Equals("Wall"))
			{
				newStatus |= 2;
			}
		}
		// Set wall status!
		WallStatus = newStatus;
	}
}
