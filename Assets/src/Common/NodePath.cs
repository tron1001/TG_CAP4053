using System;
using UnityEngine;
using System.Collections;

public class NodePath {
		
	public int m_x;
	public int m_y;
	public int m_directionInitial;
	public int m_hasBeenVisited;	
	public float m_value;	
	public int m_previousCell;	
	
	// -------------------------------------------
	/* 
	 * Use this for initialization
	 */	
	public NodePath () 	
	{
		Reset();
	}

	// -------------------------------------------
	/* 
	 * Reset
	 */		
	public void Reset()	 
	{
		m_x = -1;
		m_y = -1;
		m_directionInitial = Global.DIRECTION_NONE;
		m_hasBeenVisited = -1;	
		m_value = 10000000.0f;	
		m_previousCell = -1;	
	}	
}
	
