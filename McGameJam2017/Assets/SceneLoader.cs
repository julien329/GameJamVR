using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("This is how many people are connected! " + CustomNetworkManager.singleton.numPlayers);
        if(CustomNetworkManager.singleton.numPlayers > 1)
            DoShit();

    }

    public void DoShit()
    {
        NetworkMsg action = NetworkMsg.MOVE_LEFT;
        Debug.Log("Invoke the shit");
        if (Application.isMobilePlatform)
            SceneManager.LoadScene("MaxMobile");
        else
            SceneManager.LoadScene("MaxPc");
        
    }
}
