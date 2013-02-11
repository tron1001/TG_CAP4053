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
			float xdist = npcPos.x - cPos.x;
			float zdist = npcPos.z - cPos.z;
			
			float ang = Mathf.Atan2(xdist, zdist) * Mathf.Rad2Deg - cObject.eulerAngles.y;
			
			if(ang < 0)
				ang = (ang * -1) % 360;
			else
				ang = ang % 360;
			print (ang + ", " + dist);
		}
	}
}
