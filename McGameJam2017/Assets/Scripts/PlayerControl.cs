using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    string address;

    void Start()
    {
        address = connectionToServer.address;
        if(!isLocalPlayer)
        {
            gameObject.name = "OtherPlayer";
            return;
        }


#if UNITY_STANDALONE
    gameObject.name = "PcPlayer";
#else
    gameObject.name = "MobilePlayer";
#endif

    }

    void Update()
    {
#if UNITY_ANDROID
        if (gameObject.name != "MobilePlayer")
            return;

        Touch myTouch = Input.touches[0];
        if(myTouch.phase == TouchPhase.Began)
        {
            CmdReactToAction(NetworkMsg.MOVE_UP);
        }
#endif
    }

    [Command]
    public void CmdReactToAction(NetworkMsg action)
    {
#if UNITY_STANDALONE
            Debug.Log("I am the computer reacting to an action! " + action);
            var player = GameObject.Find("PcPlayer");
            player.transform.position += new Vector3(5, 0, 0);
#endif
    }

    public void OnDisconnectedFromServer(NetworkDisconnection info)
    {
#if UNITY_ANDROID
        Debug.Log("Disconnected cuz : " + info);

        CustomNetworkManager.singleton.networkAddress = address;
        CustomNetworkManager.singleton.StartClient();
# endif


    }
}
