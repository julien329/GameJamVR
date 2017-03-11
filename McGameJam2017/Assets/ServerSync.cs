using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerSync : NetworkBehaviour {

    [SyncVar]
    public Vector3 posMobile;

    [SyncVar]
    public Vector3 posPc;

	// Use this for initialization
	void Start () {
        posMobile = new Vector3();
        posPc = new Vector3();
	}
	
    [Command]
	public void CmdUpdatePosition(Vector3 pos)
    {
        if (!isServer)
            return;
        posMobile = pos;
        posPc = pos;
    }

    public void HardResetPos()
    {
        if (!isServer)
            return;
        var obj = GameObject.Find("PcPlayer");
        if(obj != null)
            obj.GetComponent<PlayerControl>().RpcUpdatePosition(posPc);
        obj = GameObject.Find("MobilePlayer");
        if (obj != (null))
            obj.GetComponent<PlayerControl>().RpcUpdatePosition(posMobile);
    }


}
