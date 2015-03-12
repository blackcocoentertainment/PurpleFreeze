using UnityEngine;
using System.Collections;

public class ObjectMovement : MonoBehaviour 
{
	public Vector2 TargetDestination { get; set; }
	public bool IsMoving { get; set; }
	public float MovementTime = 0.2f;

	private Transform _parentTransform;

	// Use this for initialization
	void Start () 
	{
		_parentTransform = gameObject.transform;
		// Init values!
		TargetDestination = _parentTransform.position;
		IsMoving = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if((TargetDestination.x != _parentTransform.position.x) || (TargetDestination.y != _parentTransform.position.y))
		{
			if(IsMoving == false)
			{
				// Not started to move yet! Let's move iet move it!
				iTween.MoveTo(gameObject, new Vector3(TargetDestination.x, TargetDestination.y, _parentTransform.position.z), MovementTime);
			}

			IsMoving = true;
		}
		else
		{
			IsMoving = false;
		}
	}
}
