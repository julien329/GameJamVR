using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CameraRaycastVR : MonoBehaviour {

    int cubeMask;

    void Awake(){
        cubeMask = LayerMask.GetMask("Cube");
    }
         
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public Transform positionMovementCube()
    {
        RaycastHit hit; 
        Quaternion headRotation = InputTracking.GetLocalRotation(VRNode.Head);
        Vector3 rayRotation = headRotation * transform.forward;
        Ray rayDirection = new Ray(transform.position, rayRotation );
        if( Physics.Raycast(rayDirection, out hit, cubeMask))
        {
            if(transform.position.x == Mathf.Abs(hit.transform.position.x) || transform.position.z == Mathf.Abs(hit.transform.position.z))
            {
                return hit.transform;
            }
        }
        return null;
       
    }
}
