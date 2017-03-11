using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public NetworkDiscovery discovery;

    void Start()
    {
        if(Application.isMobilePlatform)
        {
            this.StartClient();
        }
        else
        {
            this.StartHost();
        }

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
        if (Application.isMobilePlatform)
        {
            discovery.StartAsClient();
        }
        else
        {

        }
    }

    public override void OnStopClient()
    {
        discovery.StopBroadcast();
        discovery.showGUI = true;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        if (Application.isMobilePlatform)
        {
            Debug.Log("Mobile is connected now");
        }
        else
        {
            Debug.Log("I am a normal boring client");
        }
    }

    public override void OnServerConnect(NetworkConnection conn) {

    }


}
