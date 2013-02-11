/*
@author Thong Tran & Rene Gajardo

this source code creates 4 pie slice and detects where
each npc show up in which slice if the npc is within range
*/

public var cObject : Transform;
public var front : int;
public var back : int;
public var right : int;
public var left : int;

function Update () {
	
	var fl = transform.TransformDirection(Vector3(-1,0,1));
	var fr = transform.TransformDirection(Vector3(-1,0,-1));
	var bl = transform.TransformDirection(Vector3(1,0,1));
	var br = transform.TransformDirection(Vector3(1,0,-1));
	//draws the 4 lines seperating the pie slices
	Debug.DrawRay(transform.position, fl * 3, Color.green);
	Debug.DrawRay(transform.position, fr * 3, Color.blue);
	Debug.DrawRay(transform.position, bl * 3, Color.red);
	Debug.DrawRay(transform.position, br * 3, Color.yellow);
	//these variables will be used to store the # of agents in each
	//pie slice section
	front = 0;
	back = 0;
	left = 0;
	right = 0;
	//finds all agents with the tag, in other words, find all the npc
	var npc : GameObject[];
	npc = GameObject.FindGameObjectsWithTag("Agent");
	//goes through each of the npc found and store in the GameObject array
	for (var nc : GameObject in npc) {
		findnpc (nc);
	}
	print("Front: " + front);
	print("Left: " + left);
	print("Back: " + back);
	print("Right: " + right);
}

function findnpc(nc) {
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
	if(dist < 3){
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
		//will keep count of how many agents are in which part of the pie slice
		if(ang <= 45 || ang >= 315)
			front++;
		else if(ang > 45 && ang <= 135)
			left++;
		else if(ang > 135 && ang <= 225)
			back++;
		else if(ang > 225 && ang < 315)
			right++;
	}
}