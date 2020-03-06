using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public GameObject GetRaycastCollision(){
        Ray ray = new Ray(Camera.main.transform.position - new Vector3(0,1,0), Camera.main.transform.forward*5000 - new Vector3(0,1,0));
        RaycastHit hit;

        Debug.DrawRay(Camera.main.transform.position - new Vector3(0,1,0), Camera.main.transform.forward*5000 - new Vector3(0,1,0), Color.green);
        
        if (Physics.Raycast(ray, out hit, 5000f)) {
            return hit.collider.gameObject;
        }
        return null;  
    }
}
