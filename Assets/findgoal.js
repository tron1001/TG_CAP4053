public var currentTarget : Transform;
public var targets : Transform;
public var navigation : NavMeshAgent;
 
private var i : int = 0;
 
 
function Start () {
 
    navigation.destination = targets.position;
 
}
 
function Update () {
    var dist = Vector3.Distance(targets.position,transform.position);
    
 
 
 /*
    //if npc reaches its destination (or gets close)...
    if (dist < 5){     
       if (i < targets.Length - 1){ //negate targets[0], since it's already set in destination.
         i++; //change next target
         navigation.destination = targets[i].position; //go to next target by setting it as the new destination
       }
    }*/
 
 
}