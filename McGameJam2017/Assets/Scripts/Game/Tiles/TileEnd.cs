using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEnd : MonoBehaviour {

    private GameObject playerVR;

	// Use this for initialization
	void Start () {
        playerVR = GameObject.FindGameObjectWithTag("PlayerVR");
	}
	
	// Update is called once per frame
	void Update () {
    }
}
