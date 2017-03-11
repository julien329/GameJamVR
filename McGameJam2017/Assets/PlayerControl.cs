﻿using System.Collections;
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
