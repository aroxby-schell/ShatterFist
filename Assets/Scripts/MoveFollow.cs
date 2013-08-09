using UnityEngine;
using System.Collections;

public class MoveFollow : MonoBehaviour
{
	public Transform target;
	
	private Vector3 moveScale = Vector3.forward;
	private Vector3 lastPos;
	
	void Start()
	{
		lastPos = target.position;
	}

	void Update()
	{
		Vector3 newPos = target.position;
		Vector3 dist = newPos - lastPos;
		dist.Scale(moveScale);
		transform.Translate(dist, Space.World);
		lastPos = newPos;
	}
}
