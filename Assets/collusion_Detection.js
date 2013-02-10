//Basic collision detection checking for two differently named objects
function OnCollisionEnter(theCollision : Collision){

	//Debug.Log("On the Ground");
	//print("on the ground");
    if(theCollision.gameObject.name == "Cube1"){
        Debug.Log("Hit the wall 1");
    }
    else if(theCollision.gameObject.name == "Cube2"){
    	Debug.Log("Hit the wall 2");
    }
    else if(theCollision.gameObject.name == "Cube3"){
    	Debug.Log("Hit the wall 3");
    }
    else if(theCollision.gameObject.name == "Wall 1"){
        Debug.Log("Hit the invisible wall");
    }
    else if(theCollision.gameObject.name == "Wall 2"){
    	Debug.Log("Hit the other invisible wall");
    }
    else if (theCollision.gameObject.name == "Wall 3"){
    	Debug.Log("Hit the 3rd invisible wall");
    }
    else if(theCollision.gameObject.name == "Wall 4"){
    	Debug.Log("Hit the 4th invisible wall");
    }
    else if(theCollision.gameObject.name == "Agent 1"){
    	Debug.Log("Hello Agent 1");
    }
    else if(theCollision.gameObject.name == "Agent 2"){
    	Debug.Log("Hello Agent 2");
    }
    else if(theCollision.gameObject.name == "Agent 3"){
    	Debug.Log("Hello Agent 3");
    }
}