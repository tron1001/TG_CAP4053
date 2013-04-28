using UnityEngine;
using System.Collections;

public class MenuMainScript : StateManager 
{
	// ----------------------------------------------
	// CONSTANTS
	// ----------------------------------------------
	public const int STATE_NULL    	= -1;
	
	public const int WIDTH_BUTTON  	= 300;
	public const int HEIGHT_BUTTON 	= 70;
	
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
	public MenuMainScript() 
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
		                        ,Screen.height-4*HEIGHT_BUTTON		                        
		                        ,300
		                        ,HEIGHT_BUTTON)
		               , "PLAY TUTORIAL"))
		{
			Application.LoadLevel(Global.NAME_LEVEL);			
		}			
		
		// CREDITS
		GUI.Box(new Rect( ((Screen.width)/10)
		                        ,Screen.height-2f*HEIGHT_BUTTON		                        
		                        ,WIDTH_BUTTON
		                        ,HEIGHT_BUTTON)
		              			, "Programmer:\n Esteban Gallardo\n www.freecreationgames.net");		
		
		// CONTROLS
		GUI.Box(new Rect( ((6.5f*Screen.width)/10)
		                        ,Screen.height-2.4f*HEIGHT_BUTTON		                        
		                        ,WIDTH_BUTTON
		                        ,100)
		              			,"Controls:\n-Move with arrow keys\n-Shoot with mouse\n-Zoom with mouse wheel");		
		
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
