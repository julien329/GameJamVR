using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    void Start()
    {
        if(isServer)
        {
#if UNITY_STANDALONE

#endif
        }

        if (!isLocalPlayer)
        {
            gameObject.name = "OtherPlayer";

            return;
        }

#if UNITY_STANDALONE
    gameObject.name = "PcPlayer";
        GameObject.Find("PlayerMod").transform.SetParent(gameObject.transform);
        GameObject.Find("ServerInfo").GetComponent<ServerSync>().HardResetPos();
#else
     gameObject.name = "MobilePlayer";
        GameObject.Find("ServerInfo").GetComponent<ServerSync>().HardResetPos();
#endif

    }

    void Update()
    {
#if UNITY_ANDROID
        if (gameObject.name != "MobilePlayer")
            return;

        if(Input.GetMouseButtonDown(0))
        {
            if (isLocalPlayer) {
                transform.position += new Vector3(5, 0, 0);
                CmdReactToAction(NetworkMsg.MOVE_UP);
            }
        }
#endif
    }

    [Command]
    public void CmdReactToAction(NetworkMsg action)
    {
#if UNITY_STANDALONE
            Debug.Log("I am the computer reacting to an action! " + action);
            var player = GameObject.Find("PcPlayer");
            player.transform.position += new Vector3(0.01f, 0, 0);
            RpcUpdatePosition(player.transform.position);
            //GameObject.Find("ServerInfo").GetComponent<ServerSync>().posPc = player.transform.position;
            //GameObject.Find("ServerInfo").GetComponent<ServerSync>().posMobile = player.transform.position;
            //GameObject.Find("ServerInfo").GetComponent<ServerSync>().CmdUpdatePosition(player.transform.position);
#endif
    }

    [ClientRpc]
    public void RpcUpdatePosition(Vector3 position)
    {
#if UNITY_STANDALONE
        var obj = GameObject.Find("PcPlayer");
        if(obj != null)
            obj.transform.position = position;
        obj = GameObject.Find("OtherPlayer");
        if (obj != null)
            obj.transform.position = position;
#elif UNITY_ANDROID
        var obj = GameObject.Find("MobilePlayer");
        if (obj != null)
            obj.transform.position = position;
        obj = GameObject.Find("OtherPlayer");
        if (obj != null)
            obj.transform.position = position;
#endif
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
            Debug.Log("Hey Baby");
    }

    void HardUpdate()
    {
        RpcUpdatePosition(gameObject.transform.position);
    }

}
