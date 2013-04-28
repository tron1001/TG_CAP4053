using System;
using UnityEngine;
using System.Collections;

public class ShootParameters
{
	// ----------------------------------------------
	// VARIABLE MEMBERS
	// ----------------------------------------------	
	public GameObject origin;
	public Vector3 position;
	public int type;
	public float speed;
	public float damage;
		
	// ----------------------------------------------
	// CONSTRUCTOR
	// ----------------------------------------------	
	// -------------------------------------------
	/* 
	 * Constructor
	 */
	public ShootParameters(GameObject origin, Vector3 position, int type, float speed, float damage)
	{
		this.origin = origin;
		this.position = position;
		this.type = type;
		this.speed = speed;
		this.damage = damage;		
	}
}