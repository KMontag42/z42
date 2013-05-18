using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public List<string> player_one_strings = new List<string>();
	public List<string> player_two_strings = new List<string>();
	
	public List<Material> team_materials = new List<Material>();
	public int winner;
	
	public List<Unit> players = new List<Unit>();
	public string player_one_id = "--";
	public NetworkPlayer player_one;
	public string player_two_id = "--";
	public NetworkPlayer player_two;
	
	public bool player_one_ready = false;
	public bool player_two_ready = false;
	
	public int lastLevelPrefix = 0;
	
	public int times_loaded_battle = 0;
	public int times_loaded_select_class = 0;
	
	public BattleManager bm;
	
	[RPC]
	public void spawn_player(string unit, Vector3 spawn_position, int player, NetworkPlayer network_player)
	{
		if (Network.isClient)
			return;
		if (Network.isServer) {
			print ("spawn_player from: " + network_player);
			GameObject new_player_transform = Network.Instantiate(Resources.Load(unit),spawn_position,Quaternion.identity, player) as GameObject;
			Unit new_player = new_player_transform.GetComponent<Unit>();
			//new_player.networkView.RPC ("set_view_id", RPCMode.All, viewID);
			new_player.networkView.RPC ("set_owner", RPCMode.All, network_player);
			new_player.networkView.RPC ("set_material", RPCMode.All, player);
			new_player.networkView.RPC ("set_team", RPCMode.All, player);
			if (player == 1) {
				new_player.tag = "team_1";
			} else if (player == 2) {
				new_player.tag = "team_2";	
			}
		}
	}
	
	[RPC]
	public void start_battle()
	{
		networkView.RPC("set_battle_manager", RPCMode.All);
		//SendMessage("set_all_players", SendMessageOptions.DontRequireReceiver);
		players.Sort (
			delegate(Unit x, Unit y) {
				if (x == null) {
					if (y == null) { return 0;}
					return -1;
				}
				if (y == null) {return 0;}
				return y._class.speed.CompareTo(x._class.speed);
			}
		);
		bm.init(players);
	}
	
	[RPC]
	public void set_all_players() {
		Unit[] gos = GameObject.FindSceneObjectsOfType(typeof(Unit)) as Unit[];
		for (int i = 0; i < gos.Length; i++) {
			players.Add (gos[i]);
			gos[i].special_id = i;
		}
	}
	
	[RPC]
	public void set_player_id(string np, int player) {
		if (player == 1)
			player_one_id = np;
		else if (player == 2)
			player_two_id = np;
		else
			print ("failed set_player_id");
	}
	
	[RPC]
	public void set_player(NetworkPlayer player, int number) {
		if (number == 1)
			player_one = player;
		else if (number == 2)
			player_two = player;
		else
			print ("failed set_player");
	}
	
	[RPC]
	public void add_unit_from_player(string nvid, string u) {
		if (nvid == player_one_id)
			player_one_strings.Add(u);
		else if (nvid == player_two_id)
			player_two_strings.Add(u);
		else
			print ("failed add_unit_from_player nvid: "+nvid);
	}
	
	[RPC]
	public void set_player_ready(string nvid) {
		if (nvid == player_one_id)
			player_one_ready = true;
		else if (nvid == player_two_id)
			player_two_ready = true;
		else
			print ("failed set_player_ready nvid: "+nvid);
	}
	
	[RPC]
	public void set_team_materials() {
		team_materials.Add(Resources.Load("Materials/player_one_material") as Material);
		team_materials.Add(Resources.Load("Materials/player_two_material") as Material);
	}
	
	[RPC]
	public void set_battle_manager() {
		bm = GameObject.Find("BattleManager").GetComponent("BattleManager") as BattleManager;	
	}
	
	public void print_players()
	{
		foreach (Unit u in players)
		{
			print (u);	
		}
	}
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {}
	
	public void OnGUI () {
		if (Application.loadedLevelName == "battle_test") {
			Network.isMessageQueueRunning = true;
			GUILayout.BeginVertical();
			if (Network.isServer) {
				if (GUILayout.Button ("Start Battle"))
				{
					networkView.RPC("start_battle", RPCMode.All);
				}
				
				foreach (NetworkPlayer np in Network.connections) {
					GUILayout.Label("player: " + np + " guid: " + np.guid);	
				}
			}
		}
	}
	
	void OnLevelWasLoaded() {
		if (Network.isClient)
			return;
		
		if (Network.isServer) {
			print ("server doing levelwasloaded");
			//networkView.RPC("OnNetworkLoadedLevel", RPCMode.All);
			OnNetworkLoadedLevel();
			//SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
		}	
	}
	
	//[RPC]
	void OnNetworkLoadedLevel() {
		if (Application.loadedLevelName == "battle_test") {
			print ("times loaded battle: " + times_loaded_battle);
			//if (times_loaded_battle == 0) {
			
			Transform spawn_one = GameObject.Find("spawn_one").transform;
			Transform spawn_two = GameObject.Find("spawn_two").transform;
			
			networkView.RPC("set_team_materials", RPCMode.All);
			if (Network.isServer && times_loaded_battle == 0) {
				for(int i = 0; i < player_one_strings.Count; i++){
					//NetworkViewID viewID = Network.AllocateViewID();
					//networkView.RPC("spawn_player", RPCMode.All, player_one_strings[i], new Vector3(-9 + i * 5, spawn_one.position.y, spawn_one.position.z), 1, Network.connections[0]);
					spawn_player(player_one_strings[i], new Vector3(spawn_one.position.x + i * 10, spawn_one.position.y + 3, spawn_one.position.z), 1, Network.connections[0]);
				}
				
				for(int i = 0; i < player_two_strings.Count; i++){
					//NetworkViewID viewID = Network.AllocateViewID();
					//networkView.RPC("spawn_player", RPCMode.All, player_two_strings[i], new Vector3(-9 + i * 5, spawn_two.position.y, spawn_two.position.z), 2, Network.connections[1]);
					spawn_player(player_two_strings[i], new Vector3(spawn_one.position.x + i * 10, spawn_two.position.y + 3, spawn_two.position.z), 2, Network.connections[1]);
				}
				
				times_loaded_battle++;
			}
			networkView.RPC("set_all_players", RPCMode.All);
				
				//
			//}
		}
		
		if (Application.loadedLevelName == "select_class_scene") {
			if (times_loaded_select_class == 0) {
				networkView.RPC("set_player_id", RPCMode.All, Network.connections[0].guid, 1);
				networkView.RPC("set_player", RPCMode.All, Network.connections[0], 1);
				networkView.RPC("set_player_id", RPCMode.All, Network.connections[1].guid, 2);
				networkView.RPC("set_player", RPCMode.All, Network.connections[1], 2);
				
				times_loaded_select_class++;
			}
		}
		
		
	}
	
	[RPC]
	void LoadLevel (string level, int levelPrefix, NetworkMessageInfo info)
	{
		lastLevelPrefix = levelPrefix;
	
		// There is no reason to send any more data over the network on the default channel,
		// because we are about to load the level, thus all those objects will get deleted anyway
		Network.SetSendingEnabled(0, false);	

		// We need to stop receiving because first the level must be loaded first.
		// Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
		Network.isMessageQueueRunning = false;

		// All network views loaded from a level will get a prefix into their NetworkViewID.
		// This will prevent old updates from clients leaking into a newly created scene.
		Network.SetLevelPrefix(levelPrefix);
		Application.LoadLevel(level);
		
		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		Network.SetSendingEnabled(0, true);
		
		print ("on load level: "+ level + " from: "+ info.sender.guid);
		//SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
//		if (Application.isLoadingLevel == false)
//			if (Network.isServer) {
//				print ("server doing this shit " + Network.isServer + " from: " + info.sender.guid);
//				//networkView.RPC("OnNetworkLoadedLevel", RPCMode.All);
//				OnNetworkLoadedLevel();
//				//SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
//			}
	}
}
