using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public NetworkDiscovery discovery;
    NetworkClient myComputerClient;
    public NetworkClient myClientVR;

    public override void OnStartHost()
    {
        discovery.Initialize();
        discovery.StartAsServer();
    }

    public override void OnStartClient(NetworkClient client)
    {
        discovery.showGUI = false;
        ///computer connect to the server
        myComputerClient.Connect(Network.player.ipAddress.ToString(), Network.player.port);
    }

    public override void OnStopClient()
    {
        discovery.StopBroadcast();
        discovery.showGUI = true;
    }

    public override void OnClientConnect(NetworkConnection conn) {

        //VR connect after computer
        if (this.numPlayers == 2) {
            Debug.Log("2 players");
        }
    }
}
