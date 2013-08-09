using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public GameObject template;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.End) || Utils.GetJoyStickButtonDown(6))
		{
			GameObject.Instantiate(template);
		}
	}
}
