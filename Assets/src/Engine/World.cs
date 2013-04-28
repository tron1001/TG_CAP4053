using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : StateManager 
{
	// ----------------------------------------------
	// CONSTANTS
	// ----------------------------------------------
	public const int STATE_NULL  	  	= -1;
	public const int STATE_LOAD  	  	= 0;
	public const int STATE_PRESENTATION = 1;
	public const int STATE_RUN 		   	= 2;
	public const int STATE_WIN 		   	= 3;
	public const int STATE_LOSE		   	= 4;
	public const int STATE_PAUSE	   	= 5;
	
	// ----------------------------------------------
	// VARIABLE MEMBERS
	// ----------------------------------------------	
	public GameObject shootProtagonist = null;
	public GameObject shootEnemy = null;
	public GameObject shootExplosion = null;
	public GameObject screenInterface = null;	
	public GameObject screenWin = null;
	public GameObject screenLose = null;
	
	public AudioClip gameMelody = null;
	public AudioClip gameOver = null;
	public AudioClip gameShoot = null;
	public AudioClip gameScream = null;
	
	private cCamera m_camera;
	private GameObject m_protagonist;
	private List<GameObject> m_enemies;	
	
	private GameObject m_interface;
	private GameObject m_screenWin;
	private GameObject m_screenLose;

	// MATRIX COLLISI0N PATHFINDING
	public float m_xIniMatrix = -100f;
	public float m_zIniMatrix = -100f;
	public float m_cellWidth = 2f;
	public float m_cellHeight = 2f;
	public int m_rows = 100;
	public int m_cols = 100;
	
	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public World() 
	{	
		ChangeState(STATE_NULL);
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
		ChangeState(STATE_LOAD);
	}
	
	// -------------------------------------------
	/* 
	 * Destroy		
	 */
	void Destroy () 
	{			
	}

	// ----------------------------------------------
	// GETTERS/SETTERS
	// ----------------------------------------------	
	public Protagonist MyProtagonist
    {
        get { return m_protagonist.GetComponent<Protagonist>(); }        
    }
	
	// ----------------------------------------------
	// LISTENERS
	// ----------------------------------------------	
	

	// ----------------------------------------------
	// PRIVATE/PROTECTED FUNCTIONS
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * The player has been killed and the game finish
	 */
	public void PlayerDeath() 
	{
		ChangeState(STATE_LOSE);
	}
				
	// -------------------------------------------
	/* 
	 * And enemy disappears
	 */
	public void EnemyDeath() 
	{
		for (int i=0;i<m_enemies.Count;i++)
		{
			Enemy sEnemy = m_enemies[i].GetComponent<Enemy>();
			if (sEnemy.State==Enemy.STATE_END)				
			{
				m_enemies.RemoveAt(i);
				Destroy(sEnemy);
				i--;

				// PLAY SCREAM
				if (gameScream!=null) audio.PlayOneShot(gameScream);
			}
		}
		if (m_enemies.Count==0)
		{
			ChangeState(STATE_WIN);
		}
	}
	
	// ----------------------------------------------
	// PUBLIC FUNCTIONS
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Ask for a new shoot
	 */
	public void AskNewShoot(ShootParameters newData) 
	{
		if ((shootProtagonist!=null)&&(shootEnemy!=null))
		{
			GameObject gShoot = (GameObject)Instantiate(((newData.type==Global.TYPE_PROTAGONIST)?shootProtagonist:shootEnemy)
														, new Vector3(newData.position.x, newData.position.y+1f, newData.position.z)
			            								, newData.origin.transform.rotation);
			Shoot sShoot = gShoot.GetComponent<Shoot>();
			sShoot.Type = newData.type;
			sShoot.Speed = newData.speed;
			sShoot.Damage = newData.damage;
			
			// PLAY SHOOT
			if (m_state == STATE_RUN)
			{
				if (gameShoot!=null) audio.PlayOneShot(gameShoot);
			}
		}
	}
	
	// -------------------------------------------
	/* 
	 * Ask for a new shoot explosion
	 */
	public void AskNewExplosion(GameObject origin) 
	{	
		if (shootExplosion!=null)
		{
			Instantiate(shootExplosion, origin.transform.position, origin.transform.rotation);
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
			///////////////////////////
			case STATE_LOAD:
				// INIT PLAYER
				m_protagonist = GameObject.FindGameObjectWithTag(Global.PLAYER_TAG);
	
				// INIT THE ENEMIES OF THE LEVEL
				GameObject[] enemiesList = GameObject.FindGameObjectsWithTag(Global.ENEMY_TAG);
				m_enemies = new List<GameObject>();
				Debug.Log("TOTAL NUMBER OF ENEMIES="+enemiesList.Length);
				for (int i=0;i<enemiesList.Length;i++)
				{
					m_enemies.Add(enemiesList[i]);				
				}				
			
				// INTERFACE
				if (screenInterface!=null)
				{
					m_interface = (GameObject)Instantiate(screenInterface);				 
				}
				

				// INIT CAMERA
				m_camera = new cCamera();
				m_camera.MyCamera = camera;
				m_camera.Goal = m_protagonist.transform;			
			
				// REDO ALL THE PATHFINDING
				PathFinding.RemakePathfinding();
				
				// PLAY SOUND
				if (gameMelody!=null) audio.PlayOneShot(gameMelody);			
			                  
				// STATE PRESENTATION LEVEL
				ChangeState(STATE_PRESENTATION);
				break;
			
			///////////////////////////
			case STATE_PRESENTATION:
				ChangeState(STATE_RUN);
				break;
			
			///////////////////////////
			case STATE_RUN:			
				// RENDER DEBUG OF PATHFINDING
				PathFinding.RenderDebug();
			
				// UPDATE CAMERA
				m_camera.Update();				
				break;
			
			///////////////////////////
			case STATE_WIN:
				if (m_iterator==1)
				{
					if (screenWin!=null)
					{
						m_screenWin = (GameObject)Instantiate(screenWin);
					}
				}
				break;
			
			///////////////////////////
			case STATE_LOSE:
				if (m_iterator==1)
				{
					if (screenLose!=null)
					{
						m_screenLose = (GameObject)Instantiate(screenLose);
					
						// PLAY SCREAM
						audio.Stop();
						if (gameOver!=null) audio.PlayOneShot(gameOver);				
					}
				}			
				break;
	
			///////////////////////////
			case STATE_PAUSE:
				break;
		}
	}
}