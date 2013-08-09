using UnityEngine;
using System.Collections;

public class PunchEffect : MonoBehaviour
{
	public PlayerController player;
	public RenderPulse effect;
	public Transform attachPoint;
	
	private int punchLayer;
	
	void Start()
	{
		punchLayer = LayerMask.NameToLayer("Punchable");
	}
	
	void OnTriggerEnter(Collider c)
	{
		if(c.gameObject.layer!=punchLayer) return;
		if(player.fistPunchMode==PlayerController.PunchMode.Attack) Punch(c);
		else if(player.fistPunchMode==PlayerController.PunchMode.Grab) Grab(c);
	}
	
	private void Punch(Collider c)
	{
		Vector3 pos = collider.ClosestPointOnBounds(c.transform.position);		
		Transform t = effect.transform;
		
		GameObject newEffect = (GameObject)Instantiate(effect.gameObject);
		newEffect.transform.position = new Vector3(t.position.x, t.position.y, pos.z);
		
		RenderPulse rp = newEffect.GetComponent<RenderPulse>();
		rp.singleUse = true;
		rp.Pulse();
	}
	
	private void Grab(Collider c)
	{
		Grabbable g = c.GetComponent<Grabbable>();
		if(!g) return;
		player.GrabEnemy(g, attachPoint);
	}
	
	public void DropBackward()
	{
		player.DropEnemy(PlayerController.DropDirection.Back);
	}
}
