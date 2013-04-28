using UnityEngine;
using System.Collections;

public class cScreenLose : StateManager 
{
	// ----------------------------------------------
	// CONSTANTS
	// ----------------------------------------------
	public const int STATE_NULL    	= -1;
	
	public const int WIDTH_BUTTON  	= 100;
	public const int HEIGHT_BUTTON 	= 50;
	
	// ----------------------------------------------
	// VARIABLE MEMBERS
	// ----------------------------------------------	
	public GUISkin m_background;
	public Texture m_gameOverTex;

	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public cScreenLose() 
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

	// ----------------------------------------------
	// LISTENERS
	// ----------------------------------------------	
	void OnGUI () 
	{			
		GUI.skin = m_background;
		GUI.DrawTexture(new Rect(Screen.width/4,Screen.height/4, Screen.width/2,Screen.height/2),m_gameOverTex);
		
		if (GUI.Button(new Rect((Screen.width-WIDTH_BUTTON)/2
		                        ,Screen.height-3*HEIGHT_BUTTON		                        
		                        ,100
		                        ,50)
		               , "YOU LOSE"))
		{
			Application.LoadLevel("MenuMain");			
		}			
	}
	

	// ----------------------------------------------
	// PRIVATE/PROTECTED FUNCTIONS
	// ----------------------------------------------	
	
	// ----------------------------------------------
	// PUBLIC FUNCTIONS
	// ----------------------------------------------	
	
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
	}
}	
