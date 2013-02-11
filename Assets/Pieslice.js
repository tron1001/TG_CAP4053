//#pragma strict

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
	
	Debug.DrawRay(transform.position, fl * 3, Color.green);
	Debug.DrawRay(transform.position, fr * 3, Color.blue);
	Debug.DrawRay(transform.position, bl * 3, Color.red);
	Debug.DrawRay(transform.position, br * 3, Color.yellow);
	
	front = 0;
	back = 0;
	left = 0;
	right = 0;
	
	var npc : GameObject[];
	npc = GameObject.FindGameObjectsWithTag("Agent");
	
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
	
	dist = Vector3.Distance(cPos, npcPos);
	if(dist < 3){
		xdist = npcPos.x - cPos.x;
		zdist = npcPos.z - cPos.z;
		
		ang = Mathf.Atan2(xdist, zdist) * Mathf.Rad2Deg - cObject.eulerAngles.y;
		
		if(ang < 0)
			ang = (ang * -1) % 360;
		else
			ang = ang % 360;
			
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