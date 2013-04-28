using UnityEngine;
using System.Collections;

public class cScreenWin : StateManager 
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

	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public cScreenWin() 
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

		if (GUI.Button(new Rect((Screen.width-WIDTH_BUTTON)/2
		                        ,Screen.height-3*HEIGHT_BUTTON		                        
		                        ,100
		                        ,50)
		               , "You Win"))
		{
			Global.Level = Global.Level + 1;
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
