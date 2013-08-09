using UnityEngine;
using System.Collections;

public class CreateWalls : MonoBehaviour
{	
	private const float targetScale = 100f;
	
	public PhysicMaterial physMat;
	
	public bool forward = true;
	public bool back = true;
	public bool left = true;
	public bool right = true;
	public bool up = true;
	public bool down = true;
	
	void Start()
	{
		Vector3 insideVolume = transform.lossyScale;
		
		GameObject wallContainer = new GameObject("Generated Walls");
		
		wallContainer.transform.parent = transform;
		wallContainer.transform.localPosition = Vector3.zero;
		wallContainer.transform.localRotation = Quaternion.identity;
		//HACK: prevents Ground's rigidbody attribs from affecting walls
		//Really they each should have their own rigidbody
		wallContainer.transform.parent = null;
		
		if(forward) CreateWall(insideVolume, wallContainer.transform, Vector3.forward, "forward");
		if(back) CreateWall(insideVolume, wallContainer.transform, Vector3.back, "back");
		if(left) CreateWall(insideVolume, wallContainer.transform, Vector3.left, "left");
		if(right) CreateWall(insideVolume, wallContainer.transform, Vector3.right, "right");
		if(up) CreateWall(insideVolume, wallContainer.transform, Vector3.up, "up");
		if(down) CreateWall(insideVolume, wallContainer.transform, Vector3.down, "down");
	}
	
	private void CreateWall(Vector3 insideVolume, Transform parent, Vector3 side, string wallName)
	{
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.name = wallName;
		wall.renderer.enabled = false;
		wall.transform.parent = parent;
		wall.transform.localPosition = Vector3.zero;
		wall.transform.localRotation = Quaternion.identity;
		//NOTE: Assumes wallContainer.lossyScale==Vector3.one
		wall.transform.localScale = Vector3.one * targetScale;
		
		//wall.transform.localPosition = side * targetScale * 0.5f + Vector3.Scale(insideVolume, side) * 0.5f;
		wall.transform.localPosition = (side * targetScale + Vector3.Scale(insideVolume, side)) * 0.5f;
		wall.collider.material = physMat;
	}
}
