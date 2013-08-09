using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
	public Transform body;
	
	public Animation leftArm;
	public Animation rightArm;
	
	public GameObject normalModel;
	public GameObject dodgeModel;
	
	private string punchName;
	
	private string startGrabName;
	private string stopGrabName;
	
	private string startBlockName;
	private string stopBlockName;
	
	private string uppercutName;
	private string haymakerName;
	
	private string startBackSlamName;
	private string stopBackSlamName;
	
	private bool useRight = true;
	private int playerLayer;
	
	private bool blockLeft;
	private bool blockRight;
	
	void Start()
	{
		playerLayer = LayerMask.NameToLayer("Player");
		
		punchName = "Punch";
		startGrabName = "StartGrab";
		stopGrabName = "StopGrab";
		startBlockName = "StartBlock";
		stopBlockName = "StopBlock";
		
		startBackSlamName = "StartBackSlam";
		stopBackSlamName = "StopBackSlam";
		
		uppercutName = "Uppercut";
		haymakerName = "Haymaker";
	}
	
	public void Face(Vector3 dir)
	{
		if(!Utils.IsZeroVector(dir)) body.forward = dir;
	}
	
	
	public void Attack(string name)
	{
		if(useRight && !blockRight)
		{
			rightArm.Rewind();
			rightArm.Play(name);
		}
		else if(!blockLeft)
		{
			leftArm.Rewind();
			leftArm.Play(name);
		}
		else if(!blockRight)	//HACK: grumble, grumble, too many hax!
		{
			rightArm.Rewind();
			rightArm.Play(name);
		}
		useRight = !useRight;
	}
	
	public void SetDodgeModel()
	{
		SetRenderer(dodgeModel, true);
		SetRenderer(normalModel, false);
	}

	public void SetNormalModel()
	{		
		SetRenderer(normalModel, true);
		SetRenderer(dodgeModel, false);
	}
	
	public void LastFistGrabbed()
	{
		if(useRight)
		{
			leftArm.Play(startGrabName);
			blockLeft = true;
		}
		else
		{
			rightArm.Play(startGrabName);
			blockRight = true;
		}
	}
	
	//Assumes only one locked at a time
	public void ReleaseFist(PlayerController.DropDirection dir)
	{
		if(blockRight)
		{
			blockRight = false;
			if(dir==PlayerController.DropDirection.Front) rightArm.Play(stopGrabName);
			else if(dir==PlayerController.DropDirection.Back) rightArm.Play(stopBackSlamName);
			rightArm.PlayQueued(punchName);
		}
		else
		{
			blockLeft = false;
			if(dir==PlayerController.DropDirection.Front) leftArm.Play(stopGrabName);
			else if(dir==PlayerController.DropDirection.Back) leftArm.Play(stopBackSlamName);
			leftArm.PlayQueued(punchName);
		}
	}
	
	public void StartBlock()
	{
		rightArm.Play(startBlockName);
		leftArm.Play(startBlockName);
	}
	
	public void StopBlock()
	{
		rightArm.Play(stopBlockName);
		leftArm.Play(stopBlockName);
	}
	
	public void Punch()
	{
		Attack(punchName);
	}
	
	public void Uppercut()
	{
		Attack(uppercutName);
	}
	
	public void Haymaker()
	{
		Attack(haymakerName);
	}
	
	public void BackSlam()
	{
		if(blockRight)
		{
			rightArm.Play(startBackSlamName);
		}
		else
		{
			leftArm.Play(startBackSlamName);
		}
	}
	
	private void SetRenderer(GameObject tree, bool on)
	{
		foreach(Renderer r in tree.GetComponentsInChildren<Renderer>(true))
		{
			if(r.gameObject.layer==playerLayer) r.enabled = on;
		}
	}
}
