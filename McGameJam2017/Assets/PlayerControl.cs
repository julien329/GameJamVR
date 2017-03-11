using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    void Start()
    {

    }

    [Command]
    public void CmdDoAction(NetworkMsg action)
    {
        Debug.Log("Doing the action nao");
        if(Application.isMobilePlatform)
        {
            DispatchAction(action);
        }
        else
        {

        }
    }

    public void DoShit()
    {
        NetworkMsg action = NetworkMsg.MOVE_LEFT;
        Debug.Log("Invoke the shit");
        if (Application.isMobilePlatform)
            SceneManager.LoadSceneAsync("MaxMobile");
        else
            SceneManager.LoadSceneAsync("MaxPc");
        if (!isLocalPlayer)
            return;
        else
            CmdDoAction(action);
    }

    [ClientRpc]
    public void RpcReactToAction(NetworkMsg action)
    {
        if (Application.isMobilePlatform)
        {

        }
        else
        {
            Debug.Log("I am the computer reacting to an action! " + action);
        }
    }

    public void DispatchAction(NetworkMsg action)
    {
        if (!isServer)
            return;
        RpcReactToAction(action);
    }
}
