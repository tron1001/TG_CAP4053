using System;
using UnityEngine;
using System.Collections;

public class Shoot : Actor 
{
	public const bool DEBUG_SHOOT  			= false;
	
	// ----------------------------------------------
	// CONSTANTS
	// ----------------------------------------------
	public const int STATE_IDLE    	= 0;
	public const int STATE_MOVE    	= 1;
	public const int STATE_EXPLODE	= 2;
	public const int STATE_END		= 3;
		
	// ----------------------------------------------
	// VARIABLE MEMBERS
	// ----------------------------------------------	
	private GameObject m_world;
	private float m_speed;
	private float m_damage;
		
	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public Shoot()
	{
		ChangeState(STATE_IDLE);	
	}
	
	// ----------------------------------------------
	// INIT/DESTROY
	// ----------------------------------------------	
	
	// -------------------------------------------
	/* 
	 * Start		
	 */
	void Start () 
	{	
		if (DEBUG_SHOOT) Debug.Log("Shoot::Start!!!!!!");
		
		// GET REFERENCE TO WORLD SCRIPT
		m_world = GameObject.FindGameObjectWithTag(Global.CAMERA_TAG);	
		
		// INIT GAME OBJECT
		Character = this.gameObject.gameObject;		
		
		// MOVE
		ChangeState(STATE_MOVE);	
	}
	
	// -------------------------------------------
	/* 
	 * Destroy
	 */
	public void Destroy () 
	{
		base.Destroy();		
		if (DEBUG_SHOOT) Debug.Log("Shoot::Destroy!!!!!!");
	}

	// ----------------------------------------------
	// GETTERS/SETTERS
	// ----------------------------------------------	
	public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }
	public float Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

	// ----------------------------------------------
	// LISTENERS
	// ----------------------------------------------	
	

	// ----------------------------------------------
	// PRIVATE/PROTECTED FUNCTIONS
	// ----------------------------------------------	
	
	// ----------------------------------------------
	// PUBLIC FUNCTIONS
	// ----------------------------------------------	
	
	// -------------------------------------------
	/* 
	 * Check collision with object
	 */		
	public override void OnTriggerEnter(Collider other)	 
	{
		base.OnTriggerEnter(other);
						
		// ONLY IN MOVE STATE
		if (m_state != STATE_MOVE) return;
		
		if (DEBUG_SHOOT) Debug.Log("Shoot::OnTriggerEnter::IMPACTED ON SOMETHING("+other.gameObject.tag+")!!!");
		
		// CHECK COLLISION
		if (m_type == Global.TYPE_ENEMY)
		{
		 	if (other.gameObject.tag == Global.PLAYER_TAG)
			{
				if (DEBUG_SHOOT) Debug.Log("Shoot::OnTriggerEnter::PLAYER DAMAGED!!!");
				Protagonist sProtagonist = other.gameObject.GetComponent<Protagonist>();
				sProtagonist.ApplyDamage(m_damage);
				ChangeState(STATE_EXPLODE);
			}			
		}
		if (m_type == Global.TYPE_PROTAGONIST)
		{
			if (other.gameObject.tag == Global.ENEMY_TAG)
			{
				if (DEBUG_SHOOT) Debug.Log("Shoot::OnTriggerEnter::ENEMY DAMAGED!!!");
				Enemy sEnemy = other.gameObject.GetComponent<Enemy>();
				sEnemy.ApplyDamage(m_damage);
				ChangeState(STATE_EXPLODE);
			}
		}		
		if (other.gameObject.tag == Global.WALL_TAG)
		{
			ChangeState(STATE_EXPLODE);	
		}		
	}

	// ----------------------------------------------
	// UPDATE
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Update		
	 */
	public override void Update () 
	{
		base.Update();	
		
		switch (m_state)
		{
			//////////////////////////////
			case STATE_IDLE:	
				break;
			
			//////////////////////////////
			case STATE_MOVE:
				// MOVE
				Global.MoveTransform(Character.transform, m_speed);						

				// TIMEOUT
				m_timeAcum += Time.deltaTime;
				if (m_timeAcum>3)
				{
					ChangeState(STATE_END);					
				}				
				break;			

			//////////////////////////////
			case STATE_EXPLODE:	
				if (m_world != null) m_world.BroadcastMessage("AskNewExplosion",Character);
				ChangeState(STATE_END);
				break;

			//////////////////////////////
			case STATE_END:
				if (m_iterator==1)
				{
					Destroy();
				}
				break;
		}
	}
}