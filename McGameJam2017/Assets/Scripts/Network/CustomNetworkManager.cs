using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public NetworkDiscovery discovery;
    NetworkClient myComputerClient;
    public NetworkClient myClientVR;

    void Start()
    {
        if(Application.isMobilePlatform)
        {
            //this.StartClient();
        }
        else
        {
            if(this.numPlayers < 1)
                myComputerClient = this.StartHost();
        }

    }

    public override void OnStartHost()
    {
        Debug.Log("Am I host lol?");
        discovery.Initialize();
        discovery.StartAsServer();
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("Server is ready to go");
    }

    public override void OnStartClient(NetworkClient client)
    {
        //discovery.showGUI = false;
        ///computer connect to the server
        Debug.Log("Am I connect lol?");
        if (Application.isMobilePlatform)
        {
            myClientVR = client;
            discovery.StartAsClient();
        }
        else
        {
            myComputerClient = client;
            Debug.Log(myComputerClient.serverIp);
            Debug.Log(myComputerClient.serverPort);
        }
        //myComputerClient.Connect(client.serverIp.ToString(), myComputerClient.serverPort);
    }

    public override void OnStopClient()
    {
        discovery.StopBroadcast();
        discovery.showGUI = true;
    }

    public override void OnServerConnect(NetworkConnection conn) {
        Debug.Log("Cx has connected");
        Debug.Log(numPlayers);
        //VR connect after computer
        if (this.numPlayers == 1) {
            Debug.Log("2 players");
        }
    }


}
