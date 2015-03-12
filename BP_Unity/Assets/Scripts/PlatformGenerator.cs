using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameObject))]
//[ExecuteInEditMode]
public class PlatformGenerator : MonoBehaviour 
{
	public GameObject prefabLeft;
	public GameObject prefabMiddle;
	public GameObject prefabRight;

	public float pixelToUnits = 100.0f;
	public float prefabWidth = 0.32f; // As game world size!
	public int platformLength = 3;
	public int tileCount = 0;

	public bool horizontal = true;

	public Vector3 boxSize;
	private BoxCollider2D myBoxCollider;

	void OnDrawGizmos()
	{
		Vector3 boxSize = GetComponent<BoxCollider2D> ().size;
		boxSize.z = 1;
		Gizmos.color = new Color (1.0f, 0.0f, 0.0f, 0.3f);
		Gizmos.DrawCube (transform.position, boxSize);
	}

	void Awake()
	{
		// We'll have total of platformLength objects; 1 left, 1 right and (platformLength - 2) middle!
		// So, colliders size should be corrected with that too!
		myBoxCollider = GetComponent<BoxCollider2D> ();
		boxSize = myBoxCollider.size;
		// Calculate needed "middle" count for both sides!
		// Value should be odd for best results.
		int sideCount = (int)(((float)(platformLength - 2) / 2.0f));
		float currentX = 0.0f;
		float prefabOffset = 0.0f;
		// Put middle one by hand here; not inside loop!
		if (platformLength > 2) 
		{
			// Center it!
			AddTile(prefabMiddle, new Vector3(0,0,0));
			// We have at least one block in middle. So we must offset others as full width!
			prefabOffset = prefabWidth;
		}
		else
		{
			// We don't have any blocks in middle. So our offset for this algorithm to work
			// properly is width / 2.0f. Because there will not be any tile at 0,0,0!
			prefabOffset = prefabWidth / 2.0f;
		}

		if(horizontal == true)
		{
			// Put middle and start moving right...
			for (int i = 0; i < sideCount; i++) 
			{
				// Increase currentX as half of width!
				currentX = currentX + prefabOffset;
				AddTile(prefabMiddle, new Vector3(currentX, 0, 0));
			}
			// Now; put right! We've updated index "before" putting for middle. So now, increase X as needed!
			AddTile(prefabRight, new Vector3(currentX + prefabOffset, 0, 0));
			// Same algorithm now for "left" Zero currentX
			currentX = 0.0f;
			// We've put middle already; skip it!
			for (int i = 0; i < sideCount; i++) 
			{
				// Increase currentX as half of width!
				currentX = currentX - prefabOffset;
				AddTile(prefabMiddle, new Vector3(currentX, 0, 0));
			}
			// Left...
			AddTile(prefabLeft, new Vector3(currentX - prefabOffset, 0, 0));
		}
		else
		{
			// Vertical alignment! Use currentX as Y value; that's all..
			// Put middle and start moving "down"...
			for (int i = 0; i < sideCount; i++) 
			{
				// Increase currentX as half of width!
				currentX = currentX + prefabOffset;
				AddTile(prefabMiddle, new Vector3(0, currentX, 0));
			}
			// Now; put right! We've updated index "before" putting for middle. So now, increase X as needed!
			AddTile(prefabRight, new Vector3(0, currentX + prefabOffset, 0));
			// Same algorithm now for "up" Zero currentX
			currentX = 0.0f;
			// We've put middle already; skip it!
			for (int i = 0; i < sideCount; i++) 
			{
				// Increase currentX as half of width!
				currentX = currentX - prefabOffset;
				AddTile(prefabMiddle, new Vector3(0, currentX, 0));
			}
			// Up...
			AddTile(prefabLeft, new Vector3(0, currentX - prefabOffset, 0));
		}
		// Since we're done; correct collider size!
		ControlTileCount();
	}

	void AddTile(GameObject in_prefab, Vector3 in_prefabPosition)
	{
		GameObject newTileLeft = (GameObject)Instantiate(in_prefab, new Vector3(0, 0, 0), transform.rotation);
		newTileLeft.transform.parent = transform;
		newTileLeft.transform.localPosition = in_prefabPosition;
		// Increase tile count since we've updated it!
		tileCount++;
	}

	void ControlTileCount()
	{
		// Check if desired and used are equal!
		if(horizontal == true)
		{
			float correctLength = tileCount * prefabWidth;
			boxSize.x = correctLength;

			myBoxCollider.size = boxSize;
		}
		else
		{
			float correctLength = tileCount * prefabWidth;
			boxSize.y = correctLength;
		}

		myBoxCollider.size = boxSize;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
}
