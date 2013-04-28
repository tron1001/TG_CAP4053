using UnityEngine;
using System.Collections;


public class cInterface : StateManager 
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
	private Protagonist m_protagonist;

	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public cInterface() 
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
		// SET THE PROTAGONIST GOAL
		GameObject protagonistModel = GameObject.FindGameObjectWithTag(Global.PLAYER_TAG);	
		if (protagonistModel!=null)
		{
			m_protagonist = protagonistModel.GetComponent<Protagonist>();
		}		
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
		GUI.Box (new Rect ((Screen.width-100)/2,20,100,20), "Lifes="+m_protagonist.Life);
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
