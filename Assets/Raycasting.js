/*
@author Rene Gajardo & Thong Tran
this source code will create the feelers and return the distance between the player
character and the wall if the feeler comes into contact with the wall
*/

public var collide : boolean = false;
public var dist : float = 0;

function Update () {
	//sets the 3 vectors for the feelers
   var right = transform.TransformDirection(Vector3(1,0,1));
   var front = transform.TransformDirection(Vector3(0,0,1));
   var left = transform.TransformDirection(Vector3(-1,0,1));
   
   var hit : RaycastHit;  
   var box : Transform;
   
    //draws the 3 feelers in front of the player character 
   //Debug.DrawRay(transform.position, right * 2, Color.green);
   //Debug.DrawRay(transform.position, front * 3, Color.yellow);
   //Debug.DrawRay(transform.position, left * 2, Color.red);
 	//if the feelers hit the wall, the if statement will trigger
 	//returning the distance from the player character to the wall
 	//and which feeler triggered it
   if(Physics.Raycast(transform.position, right, hit, 35)){
   		dist = hit.distance;	
      	if(hit.transform.tag != "Player")
      		collide = true;
   }
   if(Physics.Raycast(transform.position, front, hit, 80)){
   		dist = hit.distance;
   		if(hit.transform.tag != "Player")
   			collide = true;
   }
   if(Physics.Raycast(transform.position, left, hit, 35)){
   		dist = hit.distance;
   		if(hit.transform.tag != "Player")
   			collide = true;
   }
   
}