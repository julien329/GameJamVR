using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class PlayerControlVR : MonoBehaviour {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// VARIABLES
    ////////////////////////////////////////////////////////////////////////////////////////////////

    public Vector3 Direction { get { return direction; } }

    [SerializeField]
    private Camera cameraVr;
    [SerializeField]
    private Transform outlineTransform;
    [SerializeField]
    private GameObject outlineAccepted;
    [SerializeField]
    private GameObject outlineRefused;
    [SerializeField]
    private LayerMask cubeMask;
    [SerializeField]
    private float unitWidth;
    [SerializeField]
    private float cameraHeight;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float targetableHeightMargin = 0.2f;

    private RaycastHit hit;
    private bool isMoving = false;
    private Vector3 direction;


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// UNITY
    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake() {
        outlineAccepted = outlineTransform.GetChild(0).gameObject;
        outlineAccepted.SetActive(false);
        outlineRefused = outlineTransform.GetChild(1).gameObject;
        outlineRefused.SetActive(false);
    }


    void Update () {
#if UNITY_ANDROID
        if (Input.GetMouseButton(0)) {
            UpdateOutlineBox();
        }

        if(Input.GetMouseButtonUp(0)) {
            if (outlineAccepted.activeSelf) {
                StartCoroutine(MoveToOutline());
            }

            outlineAccepted.SetActive(false);
            outlineRefused.SetActive(false);
        }
#endif
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// METHODS
    ////////////////////////////////////////////////////////////////////////////////////////////////

    private void UpdateOutlineBox() {
#if UNITY_ANDROID
        Transform objectHit = RaycastFloor();
        if (objectHit && !isMoving) {
            outlineTransform.position = objectHit.position;
            float deltaX = Mathf.Abs(objectHit.position.x - transform.position.x);
            float deltaZ = Mathf.Abs(objectHit.position.z - transform.position.z);

            if ((deltaX == unitWidth && deltaZ == 0.0f) || (deltaX == 0.0f && deltaZ == unitWidth)) {
                outlineAccepted.SetActive(true);
                outlineRefused.SetActive(false);
            }
            else if (deltaX == 0.0f && deltaZ == 0.0f) {
                outlineAccepted.SetActive(false);
                outlineRefused.SetActive(false);
            }
            else {
                outlineAccepted.SetActive(false);
                outlineRefused.SetActive(true);
            }
        }
        else {
            outlineAccepted.SetActive(false);
            outlineRefused.SetActive(false);
        }
#endif
    }


    private Transform RaycastFloor() {
#if UNITY_STANDALONE
        Quaternion headRotation = InputTracking.GetLocalRotation(VRNode.Head);
        Vector3 rayRotation = headRotation * cameraVr.transform.forward;
#else
        Quaternion headRotation = InputTracking.GetLocalRotation(VRNode.Head);
        Vector3 rayRotation = headRotation * Vector3.forward;
#endif

        Ray rayDirection = new Ray(transform.position, rayRotation);
        if (Physics.Raycast(rayDirection, out hit, cubeMask)) {
            if ((this.transform.position.y - hit.transform.position.y) == cameraHeight) {
                return hit.transform;
            }
        }

        return null;
    }


    private IEnumerator MoveToOutline() {

        Vector3 lastPos = this.transform.position;
        Vector3 targetPos = new Vector3(outlineTransform.position.x, this.transform.position.y, outlineTransform.position.z);
        isMoving = true;
        direction = Vector3.Normalize(targetPos - this.transform.position);

        float distance = Vector3.Distance(this.transform.position, targetPos);
        while(distance > 0) {
            this.transform.Translate(direction * Time.deltaTime * moveSpeed);
            distance -= Vector3.Distance(lastPos, this.transform.position);
            lastPos = this.transform.position;
            yield return null;
        }

        this.transform.position = targetPos;
        isMoving = false;

        CheckTileEffect();
    }


    private void CheckTileEffect() {
        Ray rayDirection = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(rayDirection, out hit, cubeMask)) {
            if (Mathf.Abs(this.transform.position.y - hit.transform.position.y) <= cameraHeight + targetableHeightMargin) {
                ITileEffect tileEffect = hit.transform.gameObject.GetComponent<ITileEffect>();
                if(tileEffect) {
                    tileEffect.PlayEffect();
                }
            }
        }
    }
}
