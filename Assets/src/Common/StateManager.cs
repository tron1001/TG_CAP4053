using System;
using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour 
{
	protected int m_state;
	protected int m_lastState;
	protected int m_iterator;
	protected float m_timeAcum;
	
	// -------------------------------------------
	/* 
	 * Constructor		
	 */
	public StateManager()
	{
		m_iterator=0;
		m_state=-1;
	}
	
	// ----------------------------------------------
	// GETTERS/SETTERS
	// ----------------------------------------------		
	public int State
    {
        get { return m_state; }
        set { m_state = value; }
    }
	
	// -------------------------------------------
	/* 
	 * Change the state of the object		
	 */
	public void ChangeState(int newState)
	{
		m_lastState=m_state;
		m_iterator=0;
		m_state=newState;
		m_timeAcum=0.0f;
	}
		
	// -------------------------------------------
	/* 
	 * Update		
	 */
	public virtual void Update()
	{
		if (m_iterator<100) m_iterator++;
	}
}
