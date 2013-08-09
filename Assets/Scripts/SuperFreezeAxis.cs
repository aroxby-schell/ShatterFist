using UnityEngine;
using System.Collections;

//HACK: THIS SCRIPT IS A HACK!

public class SuperFreezeAxis : MonoBehaviour
{
	private float x = 0f;
	
	void Start()
	{
		x = transform.position.x;
	}

	void Update()
	{
		Vector3 pos = transform.position;
		pos.x = x;
		transform.position = pos;
	}
}
