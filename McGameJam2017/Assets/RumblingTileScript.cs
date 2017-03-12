using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumblingTileScript : MonoBehaviour {

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerVR")
        {
            anim.SetBool("isPlayerOnTile", true);
            Invoke("TileFall", 2);
        }
    }

    void TiteFall()
    {
        anim.SetTrigger("TileFall");
    }
}
