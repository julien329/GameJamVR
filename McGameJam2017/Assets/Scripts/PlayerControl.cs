using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    [SyncVar]
    Vector3 vrPos =  new Vector3();

    string address;

    void Start()
    {
        if(!isLocalPlayer)
        {
#if UNITY_STANDALONE
            gameObject.transform.position = vrPos;
#endif
            gameObject.name = "OtherPlayer";
            return;
        }

        address = connectionToServer.address;

#if UNITY_STANDALONE
        gameObject.name = "PcPlayer";

#else
        gameObject.name = "MobilePlayer";
        gameObject.transform.position = vrPos;
#endif
        RpcUpdatePosition();
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
            var player = GameObject.Find("OtherPlayer");
            player.transform.position += new Vector3(5, 0, 0);
            vrPos = player.transform.position;
#endif
    }

    [ClientRpc]
    public void RpcUpdatePosition()
    {
        gameObject.transform.position = vrPos;
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
