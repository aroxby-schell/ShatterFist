using UnityEngine;
using System.Collections;

public class RenderPulse : MonoBehaviour
{
	public bool singleUse = false;
	public bool useStartOrientation = true;
	
	//This confusing system of bools helps keep this script backwards compatible with objects in the scene that are giving me trouble
	public bool doubleDelay = false;
	public bool quardupleDelay = false;
	
	private const float delay = 0.25f;
	private float offTime = 0f;
	private Quaternion startRotation;
	
	void Start()
	{
		if(offTime==0f) offTime = float.PositiveInfinity;
		startRotation = transform.rotation;
		
		if(renderer.enabled) Pulse();
	}
	
	public void Pulse()
	{
		if(useStartOrientation) transform.rotation = startRotation;
		
		renderer.enabled = true;
		
		offTime = delay;
		if(doubleDelay) offTime = 2*delay;
		if(quardupleDelay) offTime= 4*delay;
		offTime += Time.timeSinceLevelLoad;
		
		this.enabled = true;
	}
	
	void Update()
	{
		if(Time.timeSinceLevelLoad>=offTime)
		{
			renderer.enabled = false;
			if(singleUse) Destroy(gameObject);
			this.enabled = false;
		}
	}
}
