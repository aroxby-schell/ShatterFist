using UnityEngine;
using System.Collections;

public class ImpactEffect : MonoBehaviour
{
	public RenderPulse explosion;
	public GroundSensor frontFeet, backFeet;
	
	public enum Effect
	{
		None, Explode, Bounce
	}
	
	[HideInInspector]
	public bool thrown = false;
	[HideInInspector]
	public Effect effect;
	
	private Transform player;
	
	void Start()
	{
		player = PlayerController.singleton.transform;
	}
	
	private void Explode()
	{
		if(!rigidbody.isKinematic) rigidbody.velocity = Vector3.zero;
		explosion.transform.parent = null;
		explosion.Pulse();
		thrown = false;
		Destroy(gameObject);
	}
	
	void OnCollisionEnter(Collision c)
	{
		if(!thrown) return;
		
		//print(gameObject.name + " impact: " + c.gameObject.name + ":" + c.gameObject.tag + ">" + c.contacts[0].point);
		
		if(c.gameObject.tag=="Ground")
		{
			if(effect==Effect.Explode)
			{
				Explode();
			}
			else if(effect==Effect.Bounce)
			{
				rigidbody.velocity = Physics.gravity * -1f;
				effect = Effect.None;
			}
			else
			{
				if(frontFeet.GetCurrentTag()=="Ground" && backFeet.GetCurrentTag()=="Ground")
				{					
					rigidbody.velocity = Vector3.zero;
					rigidbody.isKinematic = true;
					
					//these next bits rotate to face the player, only around Y
					float yaw = Quaternion.LookRotation(player.position - transform.position).eulerAngles.y;				
					//roudns yaw to the nearest 180 degrees
					yaw /= 180f;
					yaw = Mathf.Floor(yaw+0.5f)*180f;
					
					transform.rotation = Quaternion.Euler(yaw * Vector3.up);
					
					thrown = false;
				}
			}
		}
		else if(c.gameObject.tag=="Enemy" && effect==Effect.Explode)
		{
			Explode();
			c.gameObject.SendMessage("Explode");
		}
	}
}
