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

#if UNITY_STANDALONE
        gameObject.name = "PcPlayer";
        GameObject.Find("PlayerMod").transform.SetParent(gameObject.transform);
        GameObject.Find("PlayerMod").transform.localPosition = Vector3.zero;
        GameObject.Find("ServerInfo").GetComponent<ServerSync>().HardResetPos();
        //Create empty game object to host our camera
        var newObject = GameObject.Instantiate(new GameObject());
        newObject.AddComponent<Camera>();
        newObject.transform.parent = gameObject.transform;
        gameObject.GetComponent<PlayerControlVR>().cameraVr = newObject.GetComponent<Camera>();
        gameObject.GetComponent<PlayerControlVR>().cameraVr.transform.localPosition = new Vector3(0, 10, 30);
        gameObject.GetComponent<PlayerControlVR>().cameraVr.transform.localRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
#else
     gameObject.name = "MobilePlayer";
        var cam = Resources.Load("Prefabs/MainCamera") as GameObject;
        Instantiate(cam, this.transform);
        var vr = Resources.Load("Prefabs/GvrViewerMain") as GameObject;
        gameObject.GetComponent<PlayerControlVR>().cameraVr = Instantiate(cam, this.transform).GetComponent<Camera>();
#endif

    }

    void Update()
    {
#if UNITY_ANDROID
        if (gameObject.name != "MobilePlayer")
            return;       
#endif
    }

    [Command]
    public void CmdReactToAction(NetworkMsg action)
    {
#if UNITY_STANDALONE
            Debug.Log("I am the computer reacting to an action! " + action);
            var player = GameObject.Find("PcPlayer");
            RpcUpdatePosition(player.transform.position);
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

    [Command]
    public void CmdMoveToDestination(Vector3 destination)
    {
        gameObject.GetComponent<PlayerControlVR>().nextDestination = destination;
        RpcMoveClientToDestination(destination);
    }

    [ClientRpc]
    void RpcMoveClientToDestination(Vector3 destination)
    {
        Debug.Log("Rpc time");
        gameObject.GetComponent<PlayerControlVR>().nextDestination = destination;
        gameObject.GetComponent<PlayerControlVR>().StartCoroutine(gameObject.GetComponent<PlayerControlVR>().MoveToOutline());
#if UNITY_STANDALONE
        var player = GameObject.Find("PcPlayer").GetComponent<PlayerControlVR>();
        player.nextDestination = destination;
        player.StartCoroutine(player.MoveToOutline());
#endif
    }

    void HardUpdate()
    {
        RpcUpdatePosition(gameObject.transform.position);
    }

}
