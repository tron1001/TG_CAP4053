using UnityEngine;
using System.Collections;

public class FindNode : MonoBehaviour {

	public int hvalue = 0;
	public int movecost = 0;
	public int totalcost = 0;
	
	public FindNode initial = null;
	//nodes next to the current one starts from front -> right -> back -> left
	//in other words clockwise
	public FindNode one = null;
	public FindNode two = null;
	public FindNode three = null;
	public FindNode four = null;
	
	public int costvalue
	{
		get
		{
			return totalcost;
		}
	}
	
	public int h_value
	{
		//
		get
		{
			return hvalue;
		}
		set
		{
			hvalue = value;
		}
	}
	
	public int movCost
	{
		get
		{
			return movecost = 0;
		}
		set
		{
			movecost = value;
		}
	}
	
	public FindNode I
	{
		get 
		{
			return one;
		}
	}
	
	public FindNode II
	{
		get
		{
			return two;
		}
	}
	
	public FindNode III
	{
		get
		{
			return three;
		}
	}
	
	public FindNode IV
	{
		get
		{
			return four;
		}
	}
	
	public FindNode Init
	{
		get 
		{
			return initial;
		}
		set
		{
			initial = value;
		}
	}
	
	public void FindAdjacentNode()
	{	//going to use raycast to 'hit' adjacent nodes
		//don't care for efficient at this point
		RaycastHit hit;
		
		if(Physics.Raycast(this.transform.position, this.transform.forward, out hit) == true)
		{
			one = hit.collider.GetComponent<FindNode>();
		}
		
		if(Physics.Raycast(this.transform.position, this.transform.right, out hit) == true)
		{
			two = hit.collider.GetComponent<FindNode>();
		}
		
		if(Physics.Raycast(this.transform.position, this.transform.forward, out hit) == true)
		{
			three = hit.collider.GetComponent<FindNode>();
		}
		
		if(Physics.Raycast(this.transform.position, this.transform.right, out hit) == true)
		{
			four = hit.collider.GetComponent<FindNode>();
		}
	}
	
	public void findTotalcost()
	{	//gets the total cost for calculating the shortest path
		totalcost = movecost + hvalue;
	}
}
