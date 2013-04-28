using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy :  Actor
{
	public const bool DEBUG_ENEMY  			= false;
	
	// ----------------------------------------------
	// CONSTANTS
	// ----------------------------------------------
	public const int STATE_NULL    					= -1;
	public const int STATE_IDLE    					= 0;
	public const int STATE_WAYPOINTS				= 1;
	public const int STATE_WAYPOINTS_PATHFINDING	= 2;
	public const int STATE_FOLLOW_PLAYER    		= 3;
	public const int STATE_FOLLOW_PLAYER_PATHFINDING= 4;
	public const int STATE_SHOOT_PLAYER				= 5;
	public const int STATE_DIE    					= 6;
	public const int STATE_END    					= 7;	
	
	public const float SHOOT_TIMEOUT = 0.5f;
	public const float SHOOT_SPEED 	 = 20.0f;
	
	public const int LIFE_ENEMY 	 = 100;	
	public const int DAMAGE_ENEMY  	 = 5;	
	
	// ----------------------------------------------
	// VARIABLE MEMBERS
	// ----------------------------------------------	
	private GameObject m_world;
	
	// WAYPOINTS
	private int m_currentWaypoint;
	private List<Vector3> m_waypoints = new List<Vector3>();
	private bool m_directionWaypoints;
	private GameObject m_goal;	
	private Vector2 m_lastCell;

	// VIEW LINE RENDER
	private int lengthOfLineRenderer = 4;	
	private Color c1 = Color.yellow;
	private Color c2 = Color.red;
	private LineRenderer m_lineRenderer;	

	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public Enemy() 
	{
		// INIT WAYPOINT STRUCT
		m_currentWaypoint = 0;
		m_directionWaypoints = true;
		m_waypoints = new List<Vector3>();
		m_lastCell = new Vector2();
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
		ChangeState(STATE_WAYPOINTS);
						
		// INIT GAME OBJECT
		Character = this.gameObject.gameObject;		
		Type = Global.TYPE_ENEMY;
		YawAdditional = 90;
		
		// GET REFERENCE TO WORLD SCRIPT
		m_world = GameObject.FindGameObjectWithTag(Global.CAMERA_TAG);
		if (m_world.GetComponent<World>()==null)
		{
			m_world = null;
		}
		
		// SET THE PROTAGONIST GOAL
		GameObject protagonistModel = GameObject.FindGameObjectWithTag(Global.PLAYER_TAG);	
		if (protagonistModel!=null)
		{
			Goal = protagonistModel;
		}
		
		// READ WAYPOINTS
		if (m_waypointsWorld!=null)
		{
			for (int i=0;i<m_waypointsWorld.Length;i++)
			{		
				GameObject sWay = m_waypointsWorld[i];			
				m_waypoints.Add(new Vector3(sWay.transform.position.x, 0, sWay.transform.position.z) );
				Destroy(sWay);
			}
		}
			
		// WAYPOINTS
		// AddWaypoint(new Vector3(2*2f, 0, 2*2f));
		// AddWaypoint(new Vector3(2*2f, 0, 4*2f));
		// AddWaypoint(new Vector3(9*2f, 0, 4*2f));
		// AddWaypoint(new Vector3(2*2f, 0, 4*2f));
		
		// ADD A LINE
		m_lineRenderer = gameObject.AddComponent<LineRenderer>();
		m_lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		m_lineRenderer.SetColors(c1, c2);
		m_lineRenderer.SetWidth(0.2F, 0.2F);
		m_lineRenderer.SetVertexCount(lengthOfLineRenderer);		
	}
	
	
		
	// -------------------------------------------
	/* 
	 * Init		
	 */
	public void Init() 
	{
	}	
	
	// -------------------------------------------
	/* 
	 * Destroy		
	 */
	void Destroy () 
	{			
		base.Destroy();
		if (m_world!=null) m_world.BroadcastMessage("EnemyDeath");
		
		if (DEBUG_ENEMY) Debug.Log("Enemy::Destroy");
	}

	// ----------------------------------------------
	// GETTERS/SETTERS
	// ----------------------------------------------	
	public GameObject Goal
    {
        get { return m_goal; }
        set { m_goal = value; }
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
	 * Apply damage		
	 */
	public override void ApplyDamage(float damage) 
	{					
		base.ApplyDamage(damage);
		
		if (Life<=0)
		{
			ChangeState(STATE_DIE);
		}
		else
		{
			ChangeState(STATE_FOLLOW_PLAYER);
		}			
	}
	
	// -------------------------------------------
	/* 
	 * Test if the element has finished
	 */
	public bool HasFinished() 
	{
		return (m_state == STATE_END);
	}
	
	// -------------------------------------------
	/* 
	 * Read waypoints
	 */
	public void AddWaypoint(Vector3 pos) 
	{
		m_waypoints.Add(new Vector3(pos.x, 0, pos.z) );
	}
	
	// -------------------------------------------
	/* 
	 * Will draw the vision of the enemy
	 */
	public RaycastHit UpdateVision(Vector3 goalPosition, float viewDistance, float angleView, bool render) 
	{
		Vector3 originLine = new Vector3(Position.x, 
		                                 Position.y, 
		                                 Position.z);		
		
		if (render)
		{
			// DRAW LINE 1		
			m_lineRenderer.SetPosition(0, originLine);
			Vector3 destinationLine1 = new Vector3(Position.x+(viewDistance*Mathf.Cos((Yaw+angleView) * Mathf.Deg2Rad)), 
			                                      Position.y+1, 
			                                      Position.z+(viewDistance*Mathf.Sin((Yaw+angleView) * Mathf.Deg2Rad)));		
			m_lineRenderer.SetPosition(1, destinationLine1);			
			
			// DRAW LINE 2
			// m_lineRenderer.SetPosition(2, originLine);
			Vector3 destinationLine2 = new Vector3(Position.x+(viewDistance*Mathf.Cos((Yaw-angleView) * Mathf.Deg2Rad)), 
			                                      Position.y+1, 
			                                      Position.z+(viewDistance*Mathf.Sin((Yaw-angleView) * Mathf.Deg2Rad)));		
			m_lineRenderer.SetPosition(2, destinationLine2);									
			m_lineRenderer.SetPosition(3, originLine);
		}
		
		
		// LOGIC OF DETECTION OF PLAYER
		if ((goalPosition.x!=-1)&&(goalPosition.y!=-1)&&(goalPosition.z!=-1))
		{
			if (Global.IsInsideCone(this, goalPosition, (float)viewDistance, (float)angleView))
			{
				Ray ray = new Ray();
				ray.origin = Position;
				Vector3 fwd = new Vector3( goalPosition.x - Position.x, 0, goalPosition.z - Position.z);
				fwd.Normalize(); 
				ray.direction = fwd;
				// ray.origin = Position - (1*fwd);
				Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
				RaycastHit hitCollision = new RaycastHit();
				
				if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
				{
					return (hitCollision);
				}
				else
				{
					return (new RaycastHit());
				}
			}
		}
		return (new RaycastHit());
	}

	// -------------------------------------------
	/* 
	 * Will update the waypoint to go
	 */
	public void UpdateWaypoints() 
	{	
		if (m_waypoints.Count>0)
		{			
			if (m_directionWaypoints)
			{
				m_currentWaypoint++;
			}
			else
			{
				m_currentWaypoint--;
			}
			if (m_currentWaypoint<0) m_currentWaypoint = m_waypoints.Count - 1;
			m_currentWaypoint=m_currentWaypoint%m_waypoints.Count;		
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
		Collider sCollision;
		Vector3 sWay;
		String tagCollided;
		RaycastHit hitCollision;
		
		base.Update();		
	
		// EVAL STATE
		switch (m_state)
		{
			////////////////////////////////
			case STATE_IDLE:
				if (m_iterator == 1)
				{
					UpdateWaypoints();
					if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_IDLE::m_currentWaypoint="+m_currentWaypoint);
				}				
			
				// ROTATE TO THE NEXT WAYPOINT
				if (m_waypoints.Count>0)
				{	
					sWay = m_waypoints[m_currentWaypoint];
					Global.LogicAlineation(this, new Vector2(sWay.x,sWay.z), 0, m_speedRotation);
					if (Global.IsInsideCone(this, new Vector3(sWay.x, Position.y, sWay.z), 100f, 5))
					{
						ChangeState(STATE_WAYPOINTS);
					}
				
				}
			
				// PLAYER DETECTED
				if (Goal != null)
				{
					hitCollision= UpdateVision(Goal.transform.position, m_viewDistance, m_angleView, true);
					if (hitCollision.collider!=null)
					{
						if ( hitCollision.collider.tag == Global.PLAYER_TAG)	
						{
							if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_IDLE::FOLLOW THE PLAYER!");
							ChangeState(STATE_FOLLOW_PLAYER);
						}
					}
				}
				else
				{
					hitCollision = UpdateVision(new Vector3(-1,-1,-1), m_viewDistance, m_angleView, true);
				}			
				break;

			////////////////////////////////
			case STATE_WAYPOINTS:
				if (m_iterator==1)
				{
					if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_WAYPOINTS::m_waypoints="+m_waypoints.Count);
				}				
				if (m_waypoints.Count>0)
				{	
					sWay = m_waypoints[m_currentWaypoint];
					Global.LogicAlineation(this, new Vector2(sWay.x,sWay.z), m_speedMovement, m_speedRotation);
					if (Vector3.Distance(new Vector3(Position.x, 0, Position.z), new Vector3(sWay.x, 0, sWay.z))<(m_speedMovement/2))
					{					
						// UpdateWaypoints();
						ChangeState(STATE_IDLE);
					}
					// CHECK WAYPOINT FOR COLLISION WALL
					hitCollision = UpdateVision(sWay, Mathf.Infinity, m_angleView, false);				
					if (hitCollision.collider != null)
					{
						if (hitCollision.collider.tag == Global.WALL_TAG)						
						{
							if (Vector3.Distance(Position, hitCollision.point)<(Vector3.Distance(Position, new Vector3(sWay.x, Position.y, sWay.z))))
						    {
								ChangeState(STATE_WAYPOINTS_PATHFINDING);
							}
						}
					}
				}
			
				// PLAYER DETECTED
				if (Goal != null)
				{
					hitCollision = UpdateVision(Goal.transform.position, m_viewDistance, m_angleView, true);
					if (hitCollision.collider!=null)
					{
						if ( hitCollision.collider.tag ==Global.PLAYER_TAG)	
						{
							if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_WAYPOINTS::FOLLOW THE PLAYER!");
							ChangeState(STATE_FOLLOW_PLAYER);
						}
					}
				}
				else					
				{
					UpdateVision(new Vector3(-1,-1,-1), m_viewDistance, m_angleView, true);
				}
				break;

			////////////////////////////////
			case STATE_WAYPOINTS_PATHFINDING:	
				if (m_iterator==1)
				{
					if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_WAYPOINTS::m_waypoints="+m_waypoints.Count);
				}				
				if (m_waypoints.Count>0)
				{	
					sWay = m_waypoints[m_currentWaypoint];
					int oriX = (int)(((Position.x-PathFinding.xIni)/PathFinding.CellWidth));
					int oriY = (int)(((Position.z-PathFinding.zIni)/PathFinding.CellHeight));
					int desX = (int)(((sWay.x-PathFinding.xIni)/PathFinding.CellWidth));
					int desY = (int)(((sWay.z-PathFinding.zIni)/PathFinding.CellHeight));
					if ((PathFinding.GetCellContent(oriX,oriY)==PathFinding.CELL_COLLISION)
				    	||
				    	(PathFinding.GetCellContent(desX,desY)==PathFinding.CELL_COLLISION))				    
					{
						ChangeState(STATE_WAYPOINTS);
						return;
					}
					else
					{
						Vector2 output=PathFinding.SearchAStar(	oriX, oriY, desX, desY);
						output.x = (output.x * PathFinding.CellWidth) + (PathFinding.CellWidth/2) + PathFinding.xIni;
						output.y = (output.y * PathFinding.CellHeight) + (PathFinding.CellHeight/2) + PathFinding.zIni;
						Debug.DrawRay(sWay, Vector3.up, Color.yellow);
						Debug.DrawLine(Position, new Vector3(output.x, Position.y, output.y), Color.white);
						while (!Global.IsInsideCone(this, new Vector3(output.x, Position.y, output.y), 100f, 5))
						{
							Global.LogicAlineation(this, new Vector2(output.x,output.y), 0, m_speedRotation);
						}
						Global.LogicAlineation(this, new Vector2(output.x,output.y), m_speedMovement, m_speedRotation);
						if (Vector3.Distance(new Vector3(Position.x, 0, Position.z), new Vector3(sWay.x, 0, sWay.z))<(m_speedMovement/2))
						{	
							// UpdateWaypoints();
							ChangeState(STATE_IDLE);
						}
					}
				}
			
				// PLAYER DETECTED
				if (Goal != null)
				{
					hitCollision = UpdateVision(Goal.transform.position, m_viewDistance, m_angleView, true);
					if (hitCollision.collider!=null)
					{			
						if ( hitCollision.collider.tag ==Global.PLAYER_TAG)	
						{
							if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_WAYPOINTS::FOLLOW THE PLAYER!");
							ChangeState(STATE_FOLLOW_PLAYER);
						}
					}
				}
				else					
				{
					UpdateVision(new Vector3(-1,-1,-1), m_viewDistance, m_angleView, true);
				}				
				break;

			////////////////////////////////
			case STATE_FOLLOW_PLAYER:	
				if (m_iterator==1)
				{
					if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_FOLLOW_PLAYER!!!!!!!!!!!!!");
				}				
			
				// TIMEOUT TO SHOOT
				if (m_world!=null)
				{
					m_timeAcum += Time.deltaTime;
					if (m_timeAcum>SHOOT_TIMEOUT)
					{
						m_timeAcum = 0;
						m_world.BroadcastMessage("AskNewShoot", new ShootParameters(Character, Position, Global.TYPE_ENEMY, SHOOT_SPEED, DAMAGE_ENEMY));
					}
				}
			
				// ALIGN WITH THE PLAYER
				if (Goal != null)
				{
					Global.LogicAlineation(this, new Vector2(Goal.transform.position.x, Goal.transform.position.z), m_speedMovement, m_speedRotation);				
					if (Vector3.Distance(Goal.transform.position, Position)> (5*m_viewDistance)/4)					
					{
						ChangeState(STATE_WAYPOINTS);
					}
				
					// PLAYER LOST DETECTION
					hitCollision = UpdateVision(Goal.transform.position, m_viewDistance, m_angleView, true);
					if (hitCollision.collider!=null)
					{			
						if ( hitCollision.collider.tag == Global.WALL_TAG)	
						{
							if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_FOLLOW_PLAYER::PLAYER LOST, GO BACK WAYPOINTS!");
							ChangeState(STATE_FOLLOW_PLAYER_PATHFINDING);
						}
					}
				
					// PLAYER IN RANGE OF SHOOT
					if (Vector3.Distance(Position, Goal.transform.position) < (2*m_viewDistance)/4)
					{
						ChangeState(STATE_SHOOT_PLAYER);
					}
				}
				else
				{
					UpdateVision(new Vector3(-1,-1,-1), m_viewDistance, m_angleView, true);
					ChangeState(STATE_WAYPOINTS);
				}
				break;
			
			////////////////////////////////
			case STATE_FOLLOW_PLAYER_PATHFINDING:	
				if (m_iterator==1)
				{
					if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_FOLLOW_PLAYER_PATHFINDING!!!!!!!!!!!!!");
				}				
			
				// ALIGN WITH THE PLAYER
				if (Goal != null)
				{
					int oriX=(int)(((Position.x-PathFinding.xIni)/PathFinding.CellWidth));
					int oriY=(int)(((Position.z-PathFinding.zIni)/PathFinding.CellHeight));
					if (PathFinding.GetCellContent(oriX,oriY)==PathFinding.CELL_COLLISION)
					{
						ChangeState(STATE_WAYPOINTS);
						return;
					}
					else
					{
						int desX=(int)(((Goal.transform.position.x-PathFinding.xIni)/PathFinding.CellWidth));
						int desY=(int)(((Goal.transform.position.z-PathFinding.zIni)/PathFinding.CellHeight));				
						Vector2 output=PathFinding.SearchAStar(oriX, oriY, desX, desY);
						if ((output.x == -1f)&&(output.y == -1f))
						{
							if (PathFinding.GetCellContent(desX+1,desY)==PathFinding.CELL_EMPTY)
							{
								desX = desX+1;
								desY = desY;				
							}
							else
							if (PathFinding.GetCellContent(desX-1,desY)==PathFinding.CELL_EMPTY)
							{
								desX = desX-1;
								desY = desY;				
							}
							else
							if (PathFinding.GetCellContent(desX,desY+1)==PathFinding.CELL_EMPTY)
							{
								desX = desX;
								desY = desY+1;				
							}
							else
							if (PathFinding.GetCellContent(desX,desY-1)==PathFinding.CELL_EMPTY)
							{
								desX = desX;
								desY = desY-1;				
							}	
							else
							{
								desX = (int)m_lastCell.x;
								desY = (int)m_lastCell.y;
							}
							output = PathFinding.SearchAStar(oriX, oriY, desX, desY);				
						}
						else
						{
							m_lastCell.x = desX;
							m_lastCell.y = desY;
						}
					
						output.x = (output.x * PathFinding.CellWidth) + (PathFinding.CellWidth/2) + PathFinding.xIni;
						output.y = (output.y * PathFinding.CellHeight) + (PathFinding.CellHeight/2) + PathFinding.zIni;
						Global.LogicAlineation(this, new Vector2(output.x, output.y), m_speedMovement, m_speedRotation);
						if (Vector3.Distance(Goal.transform.position, Position)>(5*m_viewDistance)/4)
						{
							ChangeState(STATE_WAYPOINTS);
							return;
						}					
					}
				}
									
				// LINE CLEAR TO PLAYER
				hitCollision = UpdateVision(Goal.transform.position, m_viewDistance, m_angleView, true);
				if (hitCollision.collider!=null)
				{
					if (hitCollision.collider.tag == Global.PLAYER_TAG)
					{
						if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_FOLLOW_PLAYER::PLAYER LOST, GO BACK WAYPOINTS!");
						ChangeState(STATE_FOLLOW_PLAYER);
					}
				}
				break;			
			
			////////////////////////////////
			case STATE_SHOOT_PLAYER:
				// TIMEOUT TO SHOOT
				if (m_world!=null)
				{
					m_timeAcum += Time.deltaTime;
					if (m_timeAcum>SHOOT_TIMEOUT)
					{
						m_timeAcum = 0;
						if (DEBUG_ENEMY) Debug.Log("Enemy::Update::STATE_SHOOT_PLAYER::NEW SHOOT! Position_A="+Character.transform.position.ToString());
						m_world.BroadcastMessage("AskNewShoot", new ShootParameters(Character,  Position, Global.TYPE_ENEMY, SHOOT_SPEED, DAMAGE_ENEMY));
					}
				}

				// GO IDLE
				if (Goal != null)
				{
					Global.LogicAlineation(this, new Vector2(Goal.transform.position.x, Goal.transform.position.z), 0f, m_speedRotation);				
					hitCollision = UpdateVision(Goal.transform.position, m_viewDistance, m_angleView, true);
					float distPlayer = Vector3.Distance(Position, Goal.transform.position);
					if (distPlayer> (5*m_viewDistance)/4)
					{
						ChangeState(STATE_WAYPOINTS);
					}
					else
					{
						if (distPlayer> (3*m_viewDistance)/4)
						{
							ChangeState(STATE_FOLLOW_PLAYER);
						}
					}
				}
				break;

			//////////////////////////////
			case STATE_DIE:
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

