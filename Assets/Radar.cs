/*
 * @author Thong Tran & Rene Gajardo
 * 
 * this code will created and radar and detect every npc within a
 * certain range
 * */

using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
	
	public Transform cObject;
	public string tag = "Agent";
	
	public void Update(){
		//finds each gameobject with the tag agent. in other words the npcs
		GameObject[] npc = GameObject.FindGameObjectsWithTag(tag);
		//goes through each npc found and stored in the GameObject array
		foreach (GameObject nc in npc)
			findnpc (nc);
	}
	
	public void findnpc(GameObject nc){
		//
		Vector3 cPos = cObject.position;
		Vector3 npcPos = nc.transform.position;
		//finds the distance between the player character and the npc
		float dist = Vector3.Distance(cPos, npcPos);
		//if the npc is within range, it will return the angle from the facing of the player character
		//and the distance between the npc and player character
		if(dist < 5)
		{	//obtains the x and z distance between the player character and npc
			float xdist = npcPos.x - cPos.x;
			float zdist = npcPos.z - cPos.z;
			//obtains the angle between the facing direction of the player character and the location of the npc
			float ang = Mathf.Atan2(xdist, zdist) * Mathf.Rad2Deg - cObject.eulerAngles.y;
			//this here fixes the issue with some of the angles being return as negative numbers or
			//larger than 360. Not sure why those numbers are returned by this will negate the negative angles
			//and reset the angles larger than 360
			if(ang < 0)
				ang = (ang * -1) % 360;
			else
				ang = ang % 360;
			print (ang + ", " + dist);
		}
	}
}
