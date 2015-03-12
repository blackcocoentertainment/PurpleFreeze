using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public GameObject targetObject;
	public float playerPossibleOffset = 0.25f;

	public float areaTop;
	public float areaBottom;
	public float targetHorizontalLimit = 6.0f;
	public float possibleCameraSize = 5.0f;

	private Transform _transform;
	private float _maxDistanceOffset;
	private float _bottomMinDistanceOffset;
	private Vector2 currentResolution_;

	void Awake()
	{
		CalculatePossibleCameraSize();
	}

	// Use this for initialization
	void Start () 
	{
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float targetObjectY = targetObject.transform.position.y;
		float cameraCurrentY = _transform.position.y;
		float difference = 0.0f;
		bool inside = false;

		areaTop    = cameraCurrentY ;
		areaBottom = cameraCurrentY - _maxDistanceOffset;

		// Check if player is inside!
		if(targetObjectY > areaTop)
		{
			// Going more than camera; add difference to camera now!
			// So don't change diffrence value!
			difference = targetObjectY - cameraCurrentY;
			inside = false;
		}
		else if(targetObjectY < areaBottom)
		{
			// Set camera position as player's;
			// We change it's sign as -; because we add diffence value to the camera position
			// on the formula below!
			difference = -Mathf.Abs(targetObjectY - areaBottom);
			inside = false;
		}

		if(inside == false)
		{
			// Put target object inside that area!
			Vector3 newCameraPosition = _transform.position;
			newCameraPosition.y += difference;
			_transform.position = newCameraPosition;
		}

		ControlResolution();
	}

	private void ControlResolution()
	{
		// Does resolution changed?
		if(currentResolution_.x != Screen.width || currentResolution_.y != Screen.height)
		{
			CalculatePossibleCameraSize();
			Debug.Log("Res changed");
		}
	}

	private void CalculatePossibleCameraSize()
	{
		currentResolution_ = new Vector2(Screen.width, Screen.height);
		// Set camera size
		float possibleCameraSize = (targetHorizontalLimit * currentResolution_.y) / currentResolution_.x;
		Camera.main.orthographicSize = possibleCameraSize / 2.0f;
		_maxDistanceOffset = Camera.main.orthographicSize * (1.0f - playerPossibleOffset);
	}

	// For debug purposes to see area limit inside camera! Line can be seen on 
	// Scene, on Unity editor!
	void OnDrawGizmos()
	{
		Gizmos.DrawLine(new Vector3(0, areaBottom), new Vector3(0, areaTop));
	}
}
