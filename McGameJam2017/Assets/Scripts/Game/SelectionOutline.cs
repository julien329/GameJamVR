using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class SelectionOutline : MonoBehaviour {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// VARIABLES
    ////////////////////////////////////////////////////////////////////////////////////////////////

    public Camera cameraVr;
    public GameObject outlineAccepted;
    public GameObject outlineRefused;

    private RaycastHit hit;
    [SerializeField]
    private LayerMask cubeMask;
    [SerializeField]
    private float unitWidth;
    [SerializeField]
    private float cameraHeight;


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// UNITY
    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Update () {
        UpdateOutlineBox();
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// METHODS
    ////////////////////////////////////////////////////////////////////////////////////////////////

    private void UpdateOutlineBox() {
        Transform objectHit = RaycastFloor();
        if (objectHit) {
            float deltaX = Mathf.Abs(objectHit.position.x - transform.position.x);
            float deltaZ = Mathf.Abs(objectHit.position.z - transform.position.z);
            if ((deltaX == unitWidth && deltaZ == 0.0f) || (deltaX == 0.0f && deltaZ == unitWidth)) {
                outlineAccepted.transform.position = objectHit.position;
                outlineAccepted.SetActive(true);
                outlineRefused.SetActive(false);
            }
            else if (deltaX == 0.0f && deltaZ == 0.0f) {
                outlineAccepted.SetActive(false);
                outlineRefused.SetActive(false);
            }
            else {
                outlineRefused.transform.position = objectHit.position;
                outlineAccepted.SetActive(false);
                outlineRefused.SetActive(true);
            }
        }
        else {
            outlineAccepted.SetActive(false);
            outlineRefused.SetActive(false);
        }
    }


    private Transform RaycastFloor() {
        Quaternion headRotation = InputTracking.GetLocalRotation(VRNode.Head);
        Vector3 rayRotation = headRotation * transform.forward;

        Ray rayDirection = new Ray(transform.position, rayRotation);
        if (Physics.Raycast(rayDirection, out hit, cubeMask)) {
            if ((transform.position.y - hit.transform.position.y) == cameraHeight) {
                return hit.transform;
            }
        }

        return null;
    }
}
