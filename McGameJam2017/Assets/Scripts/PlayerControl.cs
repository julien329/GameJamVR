using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.name = "OtherPlayer";
            return;
        }

        if(Application.isMobilePlatform)
        {
            gameObject.name = "MobilePlayer";
        }
        else
        {
            gameObject.name = "PcPlayer--";
        }
    }

    void Update()
    {
        if (gameObject.name != "PcPlayerC")
            return;

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            CmdReactToAction(NetworkMsg.MOVE_UP);
        }
    }

    [Command]
    public void CmdReactToAction(NetworkMsg action)
    {
        if (Application.isMobilePlatform)
        {

        }
        else
        {
            Debug.Log("I am the computer reacting to an action! " + action);
            gameObject.transform.position += new Vector3(5, 0, 0);
        }
    }
}
