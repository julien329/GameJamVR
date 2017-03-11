using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public NetworkDiscovery discovery;

    void Start()
    {
        connectionConfig.NetworkDropThreshold = 99;
        connectionConfig.DisconnectTimeout = 100000;
#if UNITY_ANDROID
        this.StartClient();
#else
        this.StartHost();
#endif
    }

    public override void OnStartHost()
    {
        discovery.Initialize();
        discovery.StartAsServer();
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("Server is ready to go");
    }

    public override void OnStartClient(NetworkClient client)
    {
        discovery.showGUI = false;
        ///computer connect to the server

#if UNITY_ANDROID
        discovery.StartAsClient();
#endif

    }

    public override void OnStopClient()
    {
        discovery.StopBroadcast();
        discovery.showGUI = true;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
#if UNITY_ANDROID
            Debug.Log("Mobile is connected now");
#endif
    }

    public override void OnServerConnect(NetworkConnection conn) {

    }


}
