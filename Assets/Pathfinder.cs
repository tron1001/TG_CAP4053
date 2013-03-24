using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {
	
	//will create two list to hold nodes
	//the open list will hold nodes that have not been checked yet
	//the close list will hold nodes that are already checked
	public List<FindNode> openlist = new List<FindNode>();
	public List<FindNode> closelist = new List<FindNode>();
	//will make a list of each node
	public FindNode nodecheck = null;	//next node on list
	public FindNode firstnode = null;	//very first node
	public FindNode startnode = null;	//starting point
	public FindNode endnode = null;		//goal
	
	public bool found = false;	//just to check if the end node was found
	
	public int basemovcost = 22;	//#number randomly chosen by rolling a d20 & d6
	public int nodenum = 0;
	
	// Use this for initialization
	void Start () {
		findHeuristics(firstnode);
		nodecheck = firstnode;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (found == false)
			bestpath();
		if (found == true)
			print ("yea!"); 
		//was supposed to start the tracing back from end node to start node,
		//however i couldn't figure out how to add nodes to a plane on unity.
	}
	
	public void findHeuristics(FindNode first)
	{
		//will calculate cost values nodes
		//will start by going row by row
		FindNode rstart = startnode;
		FindNode nextnode = rstart;
		
		int x1, x2, z1 , z2;
		float cost;
		
		while (rstart != null)
		{
			while(nextnode != null)
			{
				//will use the manhattan distance as heuristic
				x1 = Mathf.FloorToInt(nextnode.transform.position.x);
				x2 = Mathf.FloorToInt(endnode.transform.position.x);
				z1 = Mathf.FloorToInt(nextnode.transform.position.z);
				z2 = Mathf.FloorToInt(endnode.transform.position.z);
				cost = Mathf.Abs(x1 - x2) + Mathf.Abs(z1 - z2);
				nextnode.hvalue = (int)cost;
			}
			rstart = rstart.three;	//goes down to next node row by row
			nextnode = rstart;
		}
		
	}
	
	public void bestpath()
	{
		//this method will find the shortest path
		if(found == false)
		{
			//
			if(nodecheck.one != null)
				findnodecost(nodecheck, nodecheck.one);
			if(nodecheck.two != null)
				findnodecost(nodecheck, nodecheck.two);
			if(nodecheck.three != null)
				findnodecost(nodecheck, nodecheck.three);
			if(nodecheck.four != null)
				findnodecost(nodecheck, nodecheck.four);
			//moves the current node from open list to close list
			//as it already has been checked.
			Add2Close(nodecheck);
			RemoveOpen(nodecheck);
			
			nodecheck = minTotalcost();
			
			nodenum++;
			print ("Node number: #" + nodenum +"has been searched");
		}
	}
	
	public void findnodecost(FindNode node, FindNode nextnode)
	{
		//
		int newMoveCost;
		
		if(nextnode == null)
			return;
		
		if(nextnode == endnode)
		{
			endnode.Init = node;
			found = true;
			return;
		}
		
		if(nextnode.gameObject.tag == "Wall")
			return;
		//making sure node wasn't already checked
		if(closelist.Contains(nextnode) == false)
		{
			//making sure if the node is currently in the list of nodes not yet checked
			//
			if(openlist.Contains(nextnode) == true)
			{
				//getting new cost for movement
				newMoveCost = node.movCost + basemovcost;
				
				if(newMoveCost < nextnode.movecost)
				{
					//if new move cost is lower, then we found a new
					//node to use search from
					nextnode.Init = node;
					nextnode.movCost = newMoveCost;
					nextnode.findTotalcost();
				}
			}
			//otherwise calcuate the move cost and add it to open list
			else
			{
				//
				nextnode.Init = node;
				nextnode.movCost = node.movCost + basemovcost;
				nextnode.findTotalcost();
				Add2Open(nextnode);
			}
		}
	}
	
	public void Add2Open(FindNode node)
	{
		openlist.Add(node);
	}
	
	public void Add2Close(FindNode node)
	{
		closelist.Add(node);
	}
	
	public void RemoveOpen(FindNode node)
	{
		openlist.Remove(node);
	}
	
	public FindNode minTotalcost()
	{
		//simply gets the nodes with the min cost value
		float min = float.MaxValue;
		FindNode minNode = null;
		foreach(FindNode node in openlist)
		{
			//
			if(node.totalcost < min)
			{
				min = node.totalcost;
				minNode = node;
			}
		}
		return minNode;
	}
}
