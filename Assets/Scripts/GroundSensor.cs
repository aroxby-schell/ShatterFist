using UnityEngine;
using System.Collections;

public class GroundSensor : MonoBehaviour
{
	private string filter = "Ground";
	private Collider current = null;
	public bool debug = false;
	
	public string GetCurrentTag()
	{
		if(current) return current.tag;
		return "";
	}
	
	void OnTriggerEnter(Collider c)
	{
		if(c.tag==filter)
		{
			if(debug) print("Now Using: " + c.name);
			current = c;
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if(c==current)
		{
			if(debug) print("Not Using: " + c.name);
			current = null;
		}
	}
}
