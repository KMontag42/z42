using UnityEngine;
using System.Collections;

/* 
*  This file is part of the Unity networking tutorial by M2H (http://www.M2H.nl)
*  The original author of this code Mike Hergaarden, even though some small parts 
*  are copied from the Unity tutorials/manuals.
*  Feel free to use this code for your own projects, drop me a line if you made something exciting! 
*/

public class csConnect : MonoBehaviour {

public
	string connectToIP = "127.0.0.1";
	int connectPort = 25001;
	
	public void Start() {}
	public void Update() {}

	//Obviously the GUI is for both client&servers (mixed!)
	public void OnGUI ()
	{
	
		if (Network.peerType == NetworkPeerType.Disconnected){
		//We are currently disconnected: Not a client or host
			GUILayout.Label("Connection status: Disconnected");
			
			connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
			int.TryParse(GUILayout.TextField(connectPort.ToString()), out connectPort);
			
			GUILayout.BeginVertical();
			if (GUILayout.Button ("Connect as client"))
			{
				//Connect to the "connectToIP" and "connectPort" as entered via the GUI
				//Ignore the NAT for now
				//Network.useNat = false;
				Network.Connect(connectToIP, connectPort);
			}
			
			if (GUILayout.Button ("Start Server"))
			{
				//Start a server for 32 clients using the "connectPort" given via the GUI
				//Ignore the nat for now	
				//Network.useNat = false;
				Network.InitializeServer(32, connectPort);
			}
			GUILayout.EndVertical();
			
			
		}else{
			//We've got a connection(s)!
			
	
			if (Network.peerType == NetworkPeerType.Connecting){
			
				GUILayout.Label("Connection status: Connecting");
				
			} else if (Network.peerType == NetworkPeerType.Client){
				
				GUILayout.Label("Connection status: Client!");
				GUILayout.Label("Ping to server: "+Network.GetAveragePing(  Network.connections[0] ) );		
				
			} else if (Network.peerType == NetworkPeerType.Server){
				
				GUILayout.Label("Connection status: Server!");
				GUILayout.Label("Connections: "+Network.connections.Length);
				if(Network.connections.Length>=1){
					GUILayout.Label("Ping to first player: "+Network.GetAveragePing(  Network.connections[0] ) );
				}
				GUILayout.Label("Address: "+Network.natFacilitatorIP);
			}
	
			if (GUILayout.Button ("Disconnect"))
			{
				Network.Disconnect(200);
			}
		}
		
	
	}

// NONE of the public voids below is of any use in this demo, the code below is only used for demonstration.
// First ensure you understand the code in the OnGUI() public void above.

	//Client public voids called by Unity
	public void OnConnectedToServer() {
		Debug.Log("This CLIENT has connected to a server");	
	}
	
	public void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("This SERVER OR CLIENT has disconnected from a server");
	}
	
	public void OnFailedToConnect(NetworkConnectionError error){
		Debug.Log("Could not connect to server: "+ error);
	}
	
	
	//Server public voids called by Unity
	public void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected from: " + player.ipAddress +":" + player.port);
	}
	
	public void OnServerInitialized() {
		Debug.Log("Server initialized and ready");
	}
	
	public void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player disconnected from: " + player.ipAddress+":" + player.port);
	}
	
	
	// OTHERS:
	// To have a full overview of all network public voids called by unity
	// the next four have been added here too, but they can be ignored for now
	
	public void OnFailedToConnectToMasterServer(NetworkConnectionError info){
		Debug.Log("Could not connect to master server: "+ info);
	}
	
	public void OnNetworkInstantiate (NetworkMessageInfo info) {
		Debug.Log("New object instantiated by " + info.sender);
	}
	
	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		//Custom code here (your code!)
	}
	
	public void SetTarget(Transform target)
	{
	    if (target != null) {
	        transform.position = target.position;
	        transform.rotation = target.rotation;
	    }
	
	    transform.parent = target;
	}

/* 
 The last networking public voids that unity calls are the RPC public voids.
 As we've added "OnSerializeNetworkView", you can't forget the RPC public voids 
 that unity calls..however; those are up to you to implement.
 
 @RPC
 public void MyRPCKillMessage(){
	//Looks like I have been killed!
	//Someone send an RPC resulting in this public void call
 }
*/

}
