using System;
using UnityEngine;
using System.Collections;

public class Global
{
	// -------------------------------------------
	// -------------------------------------------
	// -------------------------------------------
	// CONSTANTS
	// -------------------------------------------
	// -------------------------------------------
	// -------------------------------------------
	public const bool DEBUG_MODE 				= true;
	
	public const int TYPE_PROTAGONIST 			= 0;
	public const int TYPE_ENEMY	    			= 1;
	
	public const String CAMERA_TAG	    		= "MainCamera";
	public const String PLAYER_TAG	    		= "Player";
	public const String ENEMY_TAG	    		= "Enemy";
	public const String WALL_TAG	    		= "Wall";
	
	public const String NAME_LEVEL	    		= "GameLevel";
	
	// CONSTANTS DIRECTIONS
	public const int DIRECTION_LEFT  			= 1;
	public const int DIRECTION_RIGHT 			= 2;
	public const int DIRECTION_UP    			= 100;
	public const int DIRECTION_DOWN  			= 200;			
	public const int DIRECTION_NONE  			= -1;	
	
	// -------------------------------------------
	// -------------------------------------------
	// -------------------------------------------
	// VARIABLES
	// -------------------------------------------
	// -------------------------------------------
	// -------------------------------------------
	
	private static int m_level = 1;
    public static int Level
    {
        get { return m_level; }
        set { m_level = value; }
    }

	private static int m_idGenerator = 0;
    public static int IdGenerator
    {
        get { return m_idGenerator; }
        set { m_idGenerator = value; }
    }
	
	// -------------------------------------------
	// -------------------------------------------
	// -------------------------------------------
	// FUNCTIONS
	// -------------------------------------------
	// -------------------------------------------
	// -------------------------------------------
	
	// -------------------------------------------
	/* 
	 * Will move the target
	 */
	public static void MoveTransform (Transform target, float speed) 
	{
		float moveForward = speed * Time.smoothDeltaTime;
		target.transform.Translate(Vector3.forward * moveForward);
	}

	// -------------------------------------------
	/* 
	 * Will move the position by the rotation
	 */
	public static Vector3 MoveToPosition (Transform target, float speed) 
	{
		float yaw = target.eulerAngles.y * Mathf.Deg2Rad;
		return (new Vector3(target.position.x + (1*(float)Mathf.Cos(yaw))
		                    , target.position.y
		                    , target.position.z + (1*(float)Mathf.Sin(yaw))));
	}
	
	// -------------------------------------------
	/* 
	 * Will move the target
	 */
	public static void MoveToPosition (Transform target, Vector3 newPosition) 
	{
		target.transform.position = newPosition;
	}		
	
	// -------------------------------------------
	/* 
	 * Move with control keys		
	 */
	public static void MoveAroundOriented(Transform target, float speedMovement, float speedRotation) 
	{
		float moveForward = speedMovement * Time.smoothDeltaTime * Input.GetAxis("Vertical");
		float moveLeft = speedMovement * Time.smoothDeltaTime * Input.GetAxis("Horizontal");
		float rotate = speedRotation * Time.smoothDeltaTime * Input.GetAxis("Horizontal");
		
		target.transform.Translate(Vector3.forward * moveForward);
		target.transform.Translate(Vector3.right * moveLeft);
		// transform.Translate(Vector3.up * rotate);		
	}

	// -------------------------------------------
	/* 
	 * Move with control keys		
	 */
	public static void MoveAroundKeys(Transform target, float speedMovement) 
	{
		float moveUpDown = speedMovement * Input.GetAxis("Vertical");
		float moveLeftRight = speedMovement * Input.GetAxis("Horizontal");			
		Vector3 newPosition = new Vector3(target.position.x, target.position.y, target.position.z);
		newPosition.x += moveLeftRight * Time.smoothDeltaTime;
		newPosition.z += moveUpDown * Time.smoothDeltaTime;
		target.transform.position = newPosition;
	}
	
	// -------------------------------------------
	/* 
	 * The object look at the mouse
	 */
	public static void LookAtMouse (Transform target) 
	{
		Vector3 centerscreen;
		Vector3 rotator;
		float speed = 10;
				
		// target.rigidbody.velocity.x = Input.GetAxis("Horizontal") * speed;
		// target.rigidbody.velocity.z = Input.GetAxis("Vertical") * speed;
		centerscreen = new Vector3((float)(Screen.width * 0.5), (float)0.0, (float)(Screen.height * 0.5));		
		rotator = Input.mousePosition;
		rotator.z = rotator.y;
		rotator.y = 0;
		Vector3 inputRotation = rotator-centerscreen;
		target.transform.rotation = Quaternion.LookRotation(inputRotation);
	}
	
	// -------------------------------------------
	/* 
	 * Will rotate the origin object to the target object
	 */
	public static void RotateTo (Transform origin, Transform target, bool xDisable, bool yDisabled, bool zDisabled)
	{
		Vector3 direction =  target.transform.position - origin.transform.position;
		if (xDisable) direction.x = 0;
		if (yDisabled) direction.y = 0;
		if (zDisabled) direction.z = 0;
		origin.transform.rotation = Quaternion.Slerp (origin.transform.rotation, Quaternion.LookRotation(direction), 3 * Time.deltaTime);
	}
	
	
	// ---------------------------------------------------
	/**
	 @brief Creation of test plane to know which direction to follow
	 */
	public static float AskDirectionPoint( Vector2 pos, float yaw, Vector2 objetive)
	{
		// Create Plane
		Vector3 p1=new Vector3(pos.x,0.0f,pos.y);
		Vector3 p2=new Vector3((float)(pos.x+Mathf.Cos(yaw)),0.0f,(float)(pos.y+Mathf.Sin(yaw)));
		Vector3 p3=new Vector3(pos.x,1.0f,pos.y);
		
		Debug.DrawLine(new Vector3(pos.x, 1, pos.y), new Vector3(p2.x, 1, p2.z), Color.red);
		Debug.DrawLine(new Vector3(pos.x, 1, pos.y), new Vector3(p3.x, 2, p3.z), Color.blue);
				
		Vector3 p = new Vector3(0.0f,0.0f,0.0f);
		Vector3 q = new Vector3(0.0f,0.0f,0.0f);
		Vector3 r = new Vector3(0.0f,0.0f,0.0f);	
		
		p = p2 - p1;
		q = p3 - p1;			
		
		r.x=(p.y*q.z)-(p.z*q.y); 
		r.y=(p.z*q.x)-(p.x*q.z);  
		r.z=(p.x*q.y)-(p.y*q.x); 
			
		float moduloR = r.magnitude;			
		if (moduloR==0)
		{
			r.x=0.0f;
			r.y=0.0f;
			r.z=0.0f;
		}
		else
		{
			r.x=r.x/moduloR;
			r.y=r.y/moduloR;
			r.z=r.z/moduloR;		
		}	
		float d = -((r.x*p1.x)+(r.y*p1.y)+(r.z*p1.z));			
	
		// Check if point objective is in one side or another of the planeppos si centro del plano
		return (((objetive.x*r.x) + (objetive.y*r.z))+d);
	}
	
	
	// ---------------------------------------------------
	/**
	 @brief Get distance from plane to point
	 */
	public static float GetDistancePlanePoint(Vector2 pos, float yaw, Vector2 objetive)
	{
		// Create Plane
		Vector3 p1=new Vector3(pos.x,0.0f,pos.y);
		Vector3 p2=new Vector3((float)(pos.x+Mathf.Cos(yaw+(Mathf.PI/2))),0.0f, (float)(pos.y+Mathf.Sin(yaw+(Mathf.PI/2))));
		Vector3 p3=new Vector3(pos.x,1.0f,pos.y);

		Vector3 p = new Vector3(0.0f,0.0f,0.0f);
		Vector3 q = new Vector3(0.0f,0.0f,0.0f);
		Vector3 r = new Vector3(0.0f,0.0f,0.0f);	

		p = p2 - p1;
		q = p3 - p1;

		r.x=(p.y*q.z)-(p.z*q.y); 
		r.y=(p.z*q.x)-(p.x*q.z);  
		r.z=(p.x*q.y)-(p.y*q.x); 

		float moduloR = r.magnitude;
		if (moduloR==0)
		{
			r.x=0.0f;
			r.y=0.0f;
			r.z=0.0f;
		}
		else
		{
			r.x=r.x/moduloR;
			r.y=r.y/moduloR;
			r.z=r.z/moduloR;		
		}	

		float d = -((r.x*p1.x)+(r.y*p1.y)+(r.z*p1.z));

		return Mathf.Abs((((objetive.x*r.x) + (objetive.y*r.z))+d));
	}

	
	
	// ---------------------------------------------------
	/**
	 @brief Rotate the movie clip to align to goal position
	 */
	public static void LogicAlineation(Actor target, Vector2 goal, float speedMovement, float speedRotation)
	{
		float yaw = target.Yaw * Mathf.Deg2Rad;
		Vector2 pos = new Vector2(target.Position.x, target.Position.z);

		Vector2 v1=new Vector2((float)Mathf.Cos(yaw),(float)Mathf.Sin(yaw));
		Vector2 v2=new Vector2(goal.x-pos.x,goal.y-pos.y);
		
		float moduloV2=v2.magnitude;	
		if (moduloV2==0)
		{
			v2.x=0.0f;
			v2.y=0.0f;
		}
		else
		{
			v2.x=v2.x/moduloV2;
			v2.y=v2.y/moduloV2;
		}
		float angulo=(v1.x*v2.x)+(v1.y*v2.y);			
	
		float increment = speedRotation;			
		if (angulo>0.9)	increment=(1-angulo);
	
		// ASK DIRECTION OF THE ROTATION TO REACH THE GOAL
		float directionLeft=Global.AskDirectionPoint(pos, yaw, goal);
		float yawGoal=yaw;
		if (directionLeft>0)
		{
			yawGoal+=increment;
		}
		else
		{
			yawGoal-=increment;
		}
		Vector2 vf = new Vector2((float)Mathf.Cos(yawGoal),(float)Mathf.Sin(yawGoal));
		vf.Normalize();
		Debug.DrawLine(new Vector3(pos.x, 1, pos.y), new Vector3(pos.x + vf.x, 1, pos.y + vf.y), Color.yellow);			
		
		// MOVE AND ROTATE
		yawGoal = yawGoal * Mathf.Rad2Deg;
		target.Position = new Vector3(pos.x + (vf.x * speedMovement * Time.deltaTime)
		                              , target.Position.y
		                              , pos.y + (vf.y * speedMovement * Time.deltaTime));			
		target.Yaw = yawGoal;	
	}
	
	
	// ---------------------------------------------------
	/**
	 @brief isInsideCone: Will test if the game player is inside the cone of vision
	 */
	public static bool IsInsideCone(Actor target, Vector3 objective, float rangeDetection, float angleDetection)
	{		
		if (Vector3.Distance(new Vector3(target.Position.x, 0, target.Position.z),
		                     new Vector3(objective.x, 0, objective.z))
		                     <rangeDetection)
		{
			float yaw = target.Yaw * Mathf.Deg2Rad;
			Vector2 pos = new Vector2(target.Position.x, target.Position.z);
			
			Vector2 v1=new Vector2((float)Mathf.Cos(yaw),(float)Mathf.Sin(yaw));			
			Vector2 v2=new Vector2(objective.x-pos.x,objective.z-pos.y);			
			
			// Angle detection
			float moduloV2=v2.magnitude;	
			if (moduloV2==0)
			{
				v2.x=0.0f;
				v2.y=0.0f;
			}
			else
			{
				v2.x=v2.x/moduloV2;
				v2.y=v2.y/moduloV2;
			}
			float angleCreated=(v1.x*v2.x)+(v1.y*v2.y);			
			float angleResult = Mathf.Cos(angleDetection * Mathf.Deg2Rad);			
				
			if (angleCreated>angleResult)
			{
				return (true);
			}
			else
			{
				return (false);
			}
		}
		else
		{
			return (false);
		}
	}
	
	// ---------------------------------------------------
	/**
	 @brief Will make a collision matrix through testing the collision with rays
	 */
	public static void MakeMatrix(bool render, int[] matrix, float xIni, float zIni, float widthCell, float heightCell, int rows, int cols)
	{		
		for (int i = 0;i<cols;i++)
		{
			for (int j = 0; j<rows; j++)
			{
				float x = xIni + (widthCell * i);
				float z = zIni + (heightCell * j);
				Vector3 basePoint = new Vector3(x, -100, z);
				Vector3 upperPoint = new Vector3(x, 5, z);				
				if (render)
				{
					Debug.DrawLine(basePoint, upperPoint, Color.red);	
				}
				else
				{
					Ray ray = new Ray();
					ray.origin = basePoint;
					ray.direction = Vector3.up;
					
					// POINT 1
					// Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
					RaycastHit hitCollision = new RaycastHit();					
					if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
					{
						if (hitCollision.collider.tag == WALL_TAG)
						{
							matrix[(j*cols)+i]=1;
						}
					}					
					
					// POINT 2
					// Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
					hitCollision = new RaycastHit();					
					ray.origin = new Vector3(basePoint.x + widthCell, basePoint.y, basePoint.z + heightCell);					
					if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
					{
						if (hitCollision.collider.tag == WALL_TAG)
						{
							matrix[(j*cols)+i]=1;
						}
					}					
					
					// POINT 3
					// Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
					hitCollision = new RaycastHit();					
					ray.origin = new Vector3(basePoint.x, basePoint.y, basePoint.z + heightCell);					
					if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
					{
						if (hitCollision.collider.tag == WALL_TAG)
						{
							matrix[(j*cols)+i]=1;
						}
					}					

					// POINT 4
					// Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
					hitCollision = new RaycastHit();					
					ray.origin = new Vector3(basePoint.x + widthCell, basePoint.y, basePoint.z);					
					if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
					{
						if (hitCollision.collider.tag == WALL_TAG)
						{
							matrix[(j*cols)+i]=1;
						}
					}					

					// POINT 2 (NEGATIVE)
					// Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
					hitCollision = new RaycastHit();
					ray.origin = new Vector3(basePoint.x - widthCell, basePoint.y, basePoint.z - heightCell);					
					if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
					{
						if (hitCollision.collider.tag == WALL_TAG)
						{
							matrix[(j*cols)+i]=1;
						}
					}					
					
					// POINT 3 (NEGATIVE)
					// Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
					hitCollision = new RaycastHit();					
					ray.origin = new Vector3(basePoint.x, basePoint.y, basePoint.z - heightCell);					
					if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
					{
						if (hitCollision.collider.tag == WALL_TAG)
						{
							matrix[(j*cols)+i]=1;
						}
					}					

					// POINT 4 (NEGATIVE)
					// Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
					hitCollision = new RaycastHit();					
					ray.origin = new Vector3(basePoint.x - widthCell, basePoint.y, basePoint.z);					
					if (Physics.Raycast(ray, out hitCollision, Mathf.Infinity))
					{
						if (hitCollision.collider.tag == WALL_TAG)
						{
							matrix[(j*cols)+i]=1;
						}
					}										
				}				
			}
		}
	}
	
}

