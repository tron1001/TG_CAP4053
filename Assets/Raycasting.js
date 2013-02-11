function Update () {
 
   var right = transform.TransformDirection(Vector3(-1,-1,0));
   var front = transform.TransformDirection(Vector3.left);
   var left = transform.TransformDirection(Vector3(-1,1,0));
   
   var hit : RaycastHit;  
   var box : Transform;
   var dist : float;
     
   Debug.DrawRay(transform.position, right * 2, Color.green);
   Debug.DrawRay(transform.position, front * 3, Color.yellow);
   Debug.DrawRay(transform.position, left * 2, Color.red);
 
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