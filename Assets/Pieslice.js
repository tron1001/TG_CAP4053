/*
@author Thong Tran & Rene Gajardo

this source code creates 4 pie slice and detects where
each npc show up in which slice if the npc is within range
*/

public var cObject : Transform;
public var count : int;

function Update () {
	
	var fl = transform.TransformDirection(Vector3(-1,0,1));
	var fr = transform.TransformDirection(Vector3(-1,0,-1));
	var bl = transform.TransformDirection(Vector3(1,0,1));
	var br = transform.TransformDirection(Vector3(1,0,-1));
	//draws the 4 lines seperating the pie slices
	//Debug.DrawRay(transform.position, fl * 40, Color.green);
	//Debug.DrawRay(transform.position, fr * 40, Color.blue);
	//Debug.DrawRay(transform.position, bl * 6, Color.red);
	//Debug.DrawRay(transform.position, br * 6, Color.yellow);
	//these variables will be used to store the # of agents in each
	//pie slice section
	count = 0;
	
	//finds all agents with the tag, in other words, find all the npc
	var npc : GameObject[];
	npc = GameObject.FindGameObjectsWithTag("Player");
	//goes through each of the npc found and store in the GameObject array
	for (var nc : GameObject in npc) {
		findnpc (nc);
	}
	print("CUrrent Position: " + transform.position);
	print("Players: " + count);
}

function findnpc(nc) {
	print("In function");
	//
	var cPos : Vector3;
	var npcPos : Vector3;
	var ang : float;
	var xdist : float;
	var zdist : float;
	var dist : float;
	
	cPos = cObject.position;
	npcPos = nc.transform.position;
	//
	dist = Vector3.Distance(cPos, npcPos);
	
	
	if(dist < 40){
		//finds the distance between the npc and the player character
		xdist = npcPos.x - cPos.x;
		zdist = npcPos.z - cPos.z;
		//obtains the angle, mostly reused code from the radar.cs
		ang = Mathf.Atan2(xdist, zdist) * Mathf.Rad2Deg - cObject.eulerAngles.y;
		//angle fixing, code reused from radar.cs
		if(ang < 0)
			ang = (ang * -1) % 360;
		else
			ang = ang % 360;
		if(dist < 6 && (ang > 45 && ang < 315))
			count++;
		else if(ang <= 45 || ang >= 315)
			count++;
	}
}