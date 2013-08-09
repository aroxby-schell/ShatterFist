using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public PlayerAnimationController animator;
	public bool artificialGravity = true;
	public GameObject pauseObject;
	
	//private readonly Vector3 moveVelocity = new Vector3(2f, 1f, 1f) * 5f;
	private readonly Vector3 moveVelocity = new Vector3(0f, 1f, 1f) * 5f;
	private readonly Vector3 dodgeVelocity = new Vector3(2f, 1f, 1f) * 10f;
	private const float jumpForce = 1000f;
	private const float gravityScale = 5f;
	
	//fixme: probably should just rotate the level in the scene
	private readonly Quaternion levelDirection = Quaternion.Euler(0f, 270f, 0f);
	private InputView input = new InputView();	
	private bool paused = false;
	private bool blocking = false;
	private float nextMoveTime = 0f;
	
	private Grabbable grabbedEnemy;
	
	public enum PunchMode
	{
		Drop, Attack, Grab
	}
	
	public enum DropDirection
	{
		Back, Front, Down, Up
	}
	
	private PunchMode punchMode = PunchMode.Attack;
	public PunchMode fistPunchMode { get { return punchMode; } }
	
	public bool gamePaused
	{
		get { return paused; }
		set { if(!paused) PauseGame(value); }
	}
	
	private bool tackling = false;
	private Vector3 tackleVec = Vector3.zero;
	
	private float dodgeStop = -1f;
	private float nextDodge = 0f;
	private Vector3 dodgeVec = Vector3.zero;
	
	public bool fistOverride { get { return tackling; } }
	
	private static bool created = false;
	private static PlayerController instance;
	public static PlayerController singleton { get { return instance; } }
	
	public bool onGround = true;
	
	void Awake()
	{
		if(created) Debug.LogError("Multiple Instanciation of PlayerController!");
		created = true;
		instance = this;
	}
	
	void FixedUpdate()
	{
		if(!artificialGravity) return;
		if(rigidbody.useGravity) rigidbody.AddForce((gravityScale-1f)*Physics.gravity, ForceMode.Acceleration);
		else rigidbody.AddForce(gravityScale*Physics.gravity, ForceMode.Acceleration);
	}
	
	void Update()
	{
		if(!paused)
		{
			Vector3 jumpGravity = rigidbody.velocity.y * Vector3.up;
			Vector3 moveVec = input.GetMoveSpeed();
			moveVec = levelDirection * moveVec;
			Vector3 newDodgeVec = input.GetDodgeDirection();
			
			//Grabbing an enemy has the highest priority, you can do anything else while doing that
			if(grabbedEnemy!=null)
			{
				if(input.GetGrab())
				{					
					if( animator.body.forward.z<0f ) moveVec.z *= -1f;
					
					if(moveVec.normalized.z<-0.5f) animator.BackSlam();
					else if(moveVec.normalized.x<-0.5f) DropEnemy(DropDirection.Up);
					else if(moveVec.normalized.x>0.5f) DropEnemy(DropDirection.Down);
					else DropEnemy(DropDirection.Front);
				}
				
				if(input.GetAttack())
				{
					punchMode = PunchMode.Attack;
					
					if(moveVec.normalized.x>0.5f) animator.Uppercut();
					else if(moveVec.normalized.x<-0.5f) animator.Haymaker();
					else
					{
						animator.Punch();
						if(moveVec.z>0f && animator.body.forward.z>0f) rigidbody.velocity += Vector3.Scale(moveVec, moveVelocity);
						if(moveVec.z<0f && animator.body.forward.z<0f) rigidbody.velocity += Vector3.Scale(moveVec, moveVelocity);
					}
				}
				
				if(input.GetJump() && onGround)
				{
					rigidbody.AddForce(jumpForce*Vector3.up, ForceMode.Acceleration);
					onGround = false;
				}
			}
			//Can't do anything else while dodging
			else if(dodgeStop>Time.timeSinceLevelLoad)
			{
				rigidbody.velocity = Vector3.Scale(dodgeVec, dodgeVelocity) + jumpGravity;
			}
			else if(Time.timeSinceLevelLoad>=nextDodge && !Utils.IsZeroVector(newDodgeVec))
			{
				animator.SetDodgeModel();
				dodgeStop = Time.timeSinceLevelLoad + 0.25f;
				nextDodge = dodgeStop + 0.5f;
				newDodgeVec.z = 0f;
				dodgeVec = levelDirection * newDodgeVec;
			}
			else
			{
				//fixme: not every frame dude...
				animator.SetNormalModel();
				animator.Face(moveVec.z * Vector3.forward);
				
				if(input.GetTackle())
				{
					tackleVec = Vector3.Scale(animator.body.forward, moveVelocity*4f);
					punchMode = PunchMode.Grab;
					tackling = true;
				}

				rigidbody.velocity = jumpGravity + tackleVec;
				
				if(input.GetAttack())
				{
					punchMode = PunchMode.Attack;
					
					if(moveVec.normalized.x>0.5f) animator.Uppercut();
					else if(moveVec.normalized.x<-0.5f) animator.Haymaker();
					else
					{
						animator.Punch();
						rigidbody.velocity += Vector3.Scale(moveVec, moveVelocity)*2f;
					}
					nextMoveTime = Time.timeSinceLevelLoad + 0.375f;
				}
				else if(Time.timeSinceLevelLoad>=nextMoveTime) rigidbody.velocity += Vector3.Scale(moveVec, moveVelocity);
				
				tackleVec = Vector3.MoveTowards(tackleVec, Vector3.zero, 1f);
				if(tackling && Utils.IsZeroVector(tackleVec) ) tackling = false;
			
				if(input.GetJump() && onGround)
				{
					rigidbody.AddForce(jumpForce*Vector3.up, ForceMode.Acceleration);
					onGround = false;
				}
				
				if(input.GetGrab())
				{
					punchMode = PunchMode.Grab;
					animator.Punch();
				}

				if(input.GetBlock())
				{
					if(!blocking) animator.StartBlock();
					blocking = true;
				}
				else
				{
					if(blocking) animator.StopBlock();
					blocking = false;
				}
				
			}//grab/dodge/move
		}//paused
		
		if(input.GetPause()) PauseGame(!paused);
	}
	
	public void DropEnemy(DropDirection dir)
	{
		grabbedEnemy.characterObject.transform.parent = grabbedEnemy.transformParent;
		grabbedEnemy.characterCollider.isTrigger = false;
		if(dir==DropDirection.Front) grabbedEnemy.Throw(animator.body.forward, ImpactEffect.Effect.None);
		else if(dir==DropDirection.Back) grabbedEnemy.Throw(-animator.body.forward, ImpactEffect.Effect.None);
		else if(dir==DropDirection.Down) grabbedEnemy.Throw(Vector3.down, ImpactEffect.Effect.Bounce);
		else if(dir==DropDirection.Up) grabbedEnemy.Throw(animator.body.forward, ImpactEffect.Effect.Explode);
		grabbedEnemy = null;
		punchMode = PunchMode.Drop;
		animator.ReleaseFist(dir);
	}
	
	public void GrabEnemy(Grabbable enemy, Transform fist)
	{
		if(grabbedEnemy!=null) return;
		grabbedEnemy = enemy;
		grabbedEnemy.characterCollider.isTrigger = true;
		grabbedEnemy.characterObject.rigidbody.isKinematic = true;
		grabbedEnemy.characterObject.transform.parent = fist;
		grabbedEnemy.OnGrab();
		animator.LastFistGrabbed();
	}
	
	void OnCollisionEnter(Collision c)
	{
		if(c.gameObject.tag=="Ground") onGround = true;
	}
	
	private void PauseGame(bool startPause)
	{
		if(startPause)
		{
			Time.timeScale = 0f;
			paused = true;
		}
		else
		{
			Time.timeScale = 1f;
			paused = false;
		}
		
		pauseObject.SetActive(paused);
	}
}
