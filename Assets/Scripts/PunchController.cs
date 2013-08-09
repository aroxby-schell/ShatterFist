using UnityEngine;
using System.Collections;

public class PunchController : MonoBehaviour
{
	public PlayerController pc;
	
	void FixedUpdate()
	{
		if(pc) collider.enabled = animation.isPlaying || pc.fistOverride;
		else collider.enabled = animation.isPlaying;
	}
}
