using System;
using UnityEngine;
using System.Collections;

public class Actor : StateManager 
{
	// ----------------------------------------------
	// CONSTANTS
	// ----------------------------------------------
	
	// ----------------------------------------------
	// VARIABLE MEMBERS
	// ----------------------------------------------	
	public GameObject m_character;
	public int m_id = -1;
	public int m_type = -1;
	public int m_life = 100;
	public float m_yaw = 0;
	public float m_yawAdditional = 0;
	public float m_angleView = 45f;
	public float m_viewDistance = 20f;
	public float m_speedMovement = 5.0f;
	public float m_speedRotation = 0.05f;
	public float m_gravity = 20.0F;
		
	private Vector3 m_position;	
	private Vector3 m_lastPosition;
	
	private Vector3 m_moveDirection = Vector3.zero;
	
	public GameObject[] m_waypointsWorld;	
	protected Collider m_collisionObject;	
	
	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public Actor() 
	{
		m_position = new Vector3(0f,0f,0f);
		m_lastPosition = new Vector3(0f,0f,0f);
	}
	
	// ----------------------------------------------
	// INIT/DESTROY
	// ----------------------------------------------	
	
	// -------------------------------------------
	/* 
	 * Start		
	 */
	public void Start () 
	{			
	}
	
	// -------------------------------------------
	/* 
	 * Destroy
	 */
	public void Destroy () 
	{
		DestroyObject(Character);		
	}


	// ----------------------------------------------
	// GETTERS/SETTERS
	// ----------------------------------------------	
	public GameObject Character
    {
        get { return m_character; }
		set {
			m_character = value;
			m_position = new Vector3(m_character.transform.position.x,
									m_character.transform.position.y,
									m_character.transform.position.z);
			m_id = Global.IdGenerator;
			Global.IdGenerator = Global.IdGenerator + 1;				
		}
    }
	public int Id
    {
        get { return m_id; }
        set { m_id = value; }
    }
	public int Type
    {
        get { return m_type; }
        set { 
			m_type = value; 
		}
    }					
	public int Life
    {
        get { return m_life; }
        set { m_life = value; }
    }
	public float Yaw
    {
        get { return m_yaw; }
        set 
		{ 
			m_yaw = value; 				
			m_character.transform.eulerAngles = new Vector3(0f, -m_yaw + m_yawAdditional, 0f); 
		}
    }
	public float YawAdditional
    {
        get { return m_yawAdditional; }
        set { m_yawAdditional = value; }
    }
	public Vector3 Position
    {
        get { return m_position; }
        set 
		{ 
			m_lastPosition.x = m_position.x;
			m_lastPosition.y = m_position.y;
			m_lastPosition.z = m_position.z;
			
			m_position = value; 
			
			m_character.transform.position = m_position;
		}
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
	public virtual void OnTriggerEnter(Collider other)	 
	{
		// Debug.Log("Actor.OnTriggerEnter="+other.tag);
		m_collisionObject = other;
	}		
		
	// -------------------------------------------
	/* 
	 * Apply damage
	 */
	public virtual void ApplyDamage(float damage) 
	{		
		m_life -= (int)damage;
	}		

	// -------------------------------------------
	/* 
	 * Recover the previous position
	 */
	public void RecoverPreviousPosition() 
	{
		Position = m_lastPosition;
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
		
		// Apply gravity
		if (m_character!=null)
		{
			CharacterController controller = m_character.GetComponent<CharacterController>();
			if (controller!=null)
			{
				m_moveDirection.y -= m_gravity * Time.deltaTime;
				controller.Move(m_moveDirection * Time.deltaTime);		
				Position = m_character.transform.position;
			}
		}
	}
}
