#pragma strict

function Update () {
	
	var fl = transform.TransformDirection(Vector3(-1,0,1));
	var fr = transform.TransformDirection(Vector3(-1,0,-1));
	var bl = transform.TransformDirection(Vector3(1,0,1));
	var br = transform.TransformDirection(Vector3(1,0,-1));
	
	Debug.DrawRay(transform.position, fl * 3, Color.green);
	Debug.DrawRay(transform.position, fr * 3, Color.blue);
	Debug.DrawRay(transform.position, bl * 3, Color.red);
	Debug.DrawRay(transform.position, br * 3, Color.yellow);
	
	
	
}