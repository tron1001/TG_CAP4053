using UnityEngine; 
using System.Collections; 

 

public class Feelers : MonoBehaviour { 

    public GameObject player; 
	public double distance = 0;

 

    public void OnTriggerStay(Collider other) { 
		distance = Vector3.Distance(player.transform.position, transform.position);
        print("Wall here! Distance " + distance);

    }
    /*
    void Update ()
    {
        //
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if (collider.Raycast (ray, hit, 100.0d))
        {
            Debug.DrawLine(ray.origin, hit.point);
        }

    }*/

    

}
