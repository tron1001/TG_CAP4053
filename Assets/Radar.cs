using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
	
	public Transform cObject;
	public string tag = "Agent";
	
	public void Update(){
		//
		GameObject[] npc = GameObject.FindGameObjectsWithTag(tag);
		
		foreach (GameObject nc in npc)
			findnpc (nc);
	}
	
	public void findnpc(GameObject nc){
		//
		Vector3 cPos = cObject.position;
		Vector3 npcPos = nc.transform.position;
		
		float dist = Vector3.Distance(cPos, npcPos);
		if(dist < 5)
		{
			float xdist = cPos.x - npcPos.x;
			float zdist = cPos.z - npcPos.z;
			
			float ang = Mathf.Atan2(xdist, zdist) * Mathf.Rad2Deg - cObject.eulerAngles.y;
			/*if(xdist <= 0){
				if(zdist <0){
					ang+=90;
				}
				else if(xdist > 0){
					ang+=180;	
				}
			}
			else{
				ang+=270;
			}*/
			
			print (ang + ", " + dist);
		}
	}
}
