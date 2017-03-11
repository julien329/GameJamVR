using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    [Command]
    public void CmdDoAction(NetworkMsg action)
    {
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
