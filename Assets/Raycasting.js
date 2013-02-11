/*
this source code will create the feelers and return the distance between the player
character and the wall if the feeler comes into contact with the wall
*/
function Update () {
	//sets the 3 vectors for the feelers
   var right = transform.TransformDirection(Vector3(-1,-1,0));
   var front = transform.TransformDirection(Vector3.left);
   var left = transform.TransformDirection(Vector3(-1,1,0));
   
   var hit : RaycastHit;  
   var box : Transform;
   var dist : float;
    //draws the 3 feelers in front of the player character 
   Debug.DrawRay(transform.position, right * 2, Color.green);
   Debug.DrawRay(transform.position, front * 3, Color.yellow);
   Debug.DrawRay(transform.position, left * 2, Color.red);
 	//if the feelers hit the wall, the if statement will trigger
 	//returning the distance from the player character to the wall
 	//and which feeler triggered it
   if(Physics.Raycast(transform.position, right, hit, 2)){
      	dist = hit.distance;
      	Debug.Log("Green Hit " + dist);
   }
   if(Physics.Raycast(transform.position, front, hit, 3)){
   		dist = hit.distance;
   		Debug.Log("Yellow Hit " + dist);
   }
   if(Physics.Raycast(transform.position, left, hit, 2)){
   		dist = hit.distance;
   		Debug.Log("Red Hit " + dist);
   }
   
}