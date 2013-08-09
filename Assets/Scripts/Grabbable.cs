using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour
{
	[HideInInspector]
	public Transform transformParent;	
	public GameObject characterObject;
	public Collider characterCollider;
	
	private const float throwForce = 12.5f;
	
	void Start()
	{
		transformParent = characterObject.transform.parent;
	}
	
	public void OnGrab()
	{
		SuperFreezeAxis f = characterObject.GetComponent<SuperFreezeAxis>();
		f.enabled = false;
	}
	
	public void Throw(Vector3 dir, ImpactEffect.Effect effect)
	{
		characterObject.rigidbody.isKinematic = false;
		dir = dir.normalized * throwForce * 0.9f;
		dir += throwForce * Vector3.up * 0.1f;
		characterObject.rigidbody.velocity = dir;
		
		ImpactEffect e = characterObject.GetComponent<ImpactEffect>();
		e.effect = effect;
		e.thrown = true;
		
		SuperFreezeAxis f = characterObject.GetComponent<SuperFreezeAxis>();
		f.enabled = true;
	}
}
