using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRumbling: ITileEffect {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// VARIABLES
    ////////////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField]
    private float fallDelay = 2;
    [SerializeField]
    private float destroyDelay = 2;
    private Animator anim;
    private Rigidbody playerRigidBody;


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// UNITY
    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake() {
        GameObject player = GameObject.Find("PlayerVR");
        playerRigidBody = player.GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// METHODS
    ////////////////////////////////////////////////////////////////////////////////////////////////

    public override void PlayEffect() {
        anim.SetBool("isPlayerOnTile", true);
        Invoke("TileFall", fallDelay);
        playerRigidBody.useGravity = true;
        playerRigidBody.isKinematic = false;
    }


    private void TileFall() {
        anim.SetTrigger("TileFall");
        Destroy(this.transform.parent.gameObject, destroyDelay);
    }
}
