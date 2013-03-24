/*
 * @author Thong Tran
 * 
 * this script finds the target agent and moves toward the target agent
 * */
using UnityEngine;
using System.Collections;

public class Seek : MonoBehaviour {
	
	public Transform charObject;
	public Transform npc;
	public Vector3 movdirect = Vector3.forward;
	public float walkspd = 20.0f;
	
	
	// Update is called once per frame
	void Update () {
		
		//GameObject npc = GameObject.FindGameObjectWithTag("Agent");
		seektarget();
		//seektarget(npc);
	}
	
	public void seektarget() {
		//
		Vector3 charPos = charObject.position;
		Vector3 npcPos = npc.transform.position;
		
		Vector3 targetspd = movdirect * walkspd;
		Vector3 deltaspd = targetspd - rigidbody.velocity;
		
		//finds the distance between the player character and target npc
		float dist = Vector3.Distance(charPos, npcPos);
		//print ("dist is " + dist);	//testing purposes
		
		float xdist = npcPos.x - charPos.x;
		float zdist = npcPos.z - charPos.z;
		//gets the angle between the seeker and the target
		float angle = Mathf.Atan2(xdist, zdist) * Mathf.Rad2Deg - charObject.eulerAngles.y;
		
		rigidbody.AddForce(deltaspd, ForceMode.Acceleration);
		rigidbody.angularVelocity = (Vector3.up * angle);
		rigidbody.velocity = transform.rotation * movdirect;	//makes the target move straight
	}
}
