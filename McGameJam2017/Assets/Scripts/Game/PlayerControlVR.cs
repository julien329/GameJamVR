using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VR;

public class PlayerControlVR : NetworkBehaviour {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// VARIABLES
    ////////////////////////////////////////////////////////////////////////////////////////////////

    public Vector3 Direction { get { return direction; } }

    [SerializeField]
    public Camera cameraVr;
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

    public Vector3 nextDestination;

    private RaycastHit hit;
    private bool isMoving = false;
    private Vector3 direction;


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// UNITY
    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Start() {
#if UNITY_ANDROID
        //var cam = Resources.Load("Prefabs/MainCamera") as GameObject;
        //Instantiate(cam, this.transform);
        //var vr = Resources.Load("Prefabs/GvrViewerMain") as GameObject;
        //Instantiate(cam, this.transform);
#else
        
        //cameraVr = Instantiate(cameraVr, this.transform) as Camera;
#endif
        outlineAccepted = outlineTransform.GetChild(0).gameObject;
        outlineAccepted.SetActive(false);
        outlineRefused = outlineTransform.GetChild(1).gameObject;
        outlineRefused.SetActive(false);
    }


    void Update () {
        bool moveLegal = false;
#if UNITY_ANDROID
        if (Input.GetMouseButton(0)) {
            moveLegal = UpdateOutlineBox();
        }

        if(Input.GetMouseButtonUp(0)) {
            if (UpdateOutlineBox()) {
                gameObject.GetComponent<PlayerControl>().CmdMoveToDestination(outlineTransform.position + new Vector3(0, cameraHeight, 0));
                //gameObject.GetComponent<PlayerControl>().CmdMoveToDestination(new Vector3(5,5,5));

            }
            //gameObject.GetComponent<PlayerControl>().CmdMoveToDestination(new Vector3(5, 5, 5));

            outlineAccepted.SetActive(false);
            outlineRefused.SetActive(false);
        }
#endif
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// METHODS
    ////////////////////////////////////////////////////////////////////////////////////////////////
#if UNITY_ANDROID
    private bool UpdateOutlineBox() {

        Transform objectHit = RaycastFloor();
        if (objectHit && !isMoving) {
            outlineTransform.position = objectHit.position;
            float deltaX = Mathf.Abs(objectHit.position.x - transform.position.x);
            float deltaZ = Mathf.Abs(objectHit.position.z - transform.position.z);

            if ((deltaX == unitWidth && deltaZ == 0.0f) || (deltaX == 0.0f && deltaZ == unitWidth)) {
                outlineAccepted.SetActive(true);
                outlineRefused.SetActive(false);
                return true;
            }
            else if (deltaX == 0.0f && deltaZ == 0.0f) {
                outlineAccepted.SetActive(false);
                outlineRefused.SetActive(false);
                return false;
            }
            else {
                outlineAccepted.SetActive(false);
                outlineRefused.SetActive(true);
                return false;
            }
        }
        else {
            outlineAccepted.SetActive(false);
            outlineRefused.SetActive(false);
            return false;
        }
    }
    
#endif

    private Transform RaycastFloor() {

        Quaternion headRotation = InputTracking.GetLocalRotation(VRNode.Head);
        Vector3 rayRotation = headRotation * Vector3.forward;

        Ray rayDirection = new Ray(gameObject.transform.position, rayRotation);
        if (Physics.Raycast(rayDirection, out hit, cubeMask)) {
            if (Mathf.Abs(this.transform.position.y - hit.transform.position.y) <= 2.5f) {
                return hit.transform;
            }
        }

        return null;
    }


    public IEnumerator MoveToOutline() {

        Vector3 lastPos = this.transform.position;

        //Toggle animation on and off
#if UNITY_STANDALONE
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(nextDestination, Vector3.up) - Vector3.ProjectOnPlane(lastPos, Vector3.up), Vector3.up);
        var anim = gameObject.GetComponentInChildren<Animator>();
        if(anim != null)
            anim.SetFloat("MoveSpeed", 0.3f);
#endif

        //Vector3 targetPos = new Vector3(outlineTransform.position.x, this.transform.position.y, outlineTransform.position.z);
        isMoving = true;
        direction = Vector3.Normalize(nextDestination - this.transform.position);

        float distance = Vector3.Distance(this.transform.position, nextDestination);
        while(distance > 0) {
            this.transform.Translate(direction * Time.deltaTime * moveSpeed);
            distance -= Vector3.Distance(lastPos, this.transform.position);
            lastPos = this.transform.position;
            yield return null;
        }

        this.transform.position = nextDestination;
        isMoving = false;

#if UNITY_STANDALONE
        var anima = gameObject.GetComponentInChildren<Animator>();
        if(anima != null)
            anima.SetFloat("MoveSpeed", 0.0f);
#endif

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
