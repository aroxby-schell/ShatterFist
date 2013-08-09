using UnityEngine;
using System.Collections;

public class InputView
{	
	public Vector3 GetMoveSpeed()
	{
		//xbox l stick 8
		
		Vector3 speed = Vector3.zero;

		if(Input.GetKey(KeyCode.UpArrow)) speed += Vector3.forward;
		if(Input.GetKey(KeyCode.DownArrow)) speed += Vector3.back;
		if(Input.GetKey(KeyCode.LeftArrow)) speed += Vector3.left;
		if(Input.GetKey(KeyCode.RightArrow)) speed += Vector3.right;
		
		speed += Input.GetAxis("Move X") * Vector3.left;
		speed += Input.GetAxis("Move Y") * Vector3.forward;
		
		return speed;
	}
	
	public Vector3 GetDodgeDirection()
	{
		//xbox r stick 9
		
		Vector3 speed = Vector3.zero;
		
		if(Input.GetKey(KeyCode.W)) speed += Vector3.forward;
		if(Input.GetKey(KeyCode.S)) speed += Vector3.back;
		if(Input.GetKey(KeyCode.A)) speed += Vector3.left;
		if(Input.GetKey(KeyCode.D)) speed += Vector3.right;

		speed += Input.GetAxis("Dodge X") * Vector3.forward;
		speed += Input.GetAxis("Dodge Y") * Vector3.left;
		
		string s = Utils.FloatString(Input.GetAxis("Dodge X"));
		
		if(Input.GetKeyDown(KeyCode.LeftShift)) Debug.Log("dodge: " + s);
		
		return speed.normalized;
	}
	
	public bool GetPause()
	{
		//xbox start 7
		
		return Input.GetKeyDown(KeyCode.Escape) || Utils.GetJoyStickButtonDown(7);
	}
	
	public bool GetJump()
	{
		//xbox a 0
		
		return Input.GetKeyDown(KeyCode.Space) || Utils.GetJoyStickButtonDown(0);
	}
	
	public bool GetAttack()
	{
		//xbox x 2

		return Input.GetKeyDown(KeyCode.Q) || Utils.GetJoyStickButtonDown(2);
	}
	
	public bool GetGrab()
	{
		//xbox y 3
		
		return Input.GetKeyDown(KeyCode.R) || Utils.GetJoyStickButtonDown(3);
	}
	
	public bool GetBlock()
	{
		//xbox l shoulder 4
		
		return Input.GetKey(KeyCode.E) || Utils.GetJoyStickButton(4);
	}
	
	public bool GetTackle()
	{
		//xbox r shoulder 5
		
		return Input.GetKeyDown(KeyCode.Z) || Utils.GetJoyStickButtonDown(5);
	}
}
