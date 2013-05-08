using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public List<string> player_one_units = new List<string>();
	public List<string> player_two_units = new List<string>();
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
	
	BattleManager bm;
	
	public void spawn_player(string unit, Vector3 spawn_position, int player, NetworkPlayer network_player)
	{
		GameObject new_player_transform = Network.Instantiate(Resources.Load(unit),spawn_position,Quaternion.identity, player + 1) as GameObject;
		Unit new_player = new_player_transform.GetComponent<Unit>();
		new_player.networkView.RPC ("set_owner", RPCMode.AllBuffered, network_player);
		new_player.networkView.RPC ("set_material", RPCMode.AllBuffered, player + 1);
		new_player.networkView.RPC ("set_team", RPCMode.AllBuffered, player + 1);
		if (player == 0) {
			new_player.tag = "team_1";
		} else {
			new_player.tag = "team_2";	
		}
		players.Add (new_player);
		players.Remove(null);
	}
	
	[RPC]
	public void start_battle()
	{
		bm.init(players);
	}
	
	[RPC]
	public void set_player_id(string np) {
		if (player_one_id == "--")
			player_one_id = np;
		else if (player_two_id == "--")
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
			player_one_units.Add(u);
		else if (nvid == player_two_id)
			player_two_units.Add(u);
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
			if (Network.isServer)
				if (GUILayout.Button ("Start Battle"))
				{
					networkView.RPC("start_battle", RPCMode.AllBuffered);	
				}
			GUILayout.Label("Player guid: "+Network.player.guid);
		}
	}
	
	[RPC]
	void OnLevelWasLoadedRPC() {
		OnLevelWasLoaded();	
	}
	
	void OnLevelWasLoaded() {
		if (Application.loadedLevelName == "battle_test") {
			team_materials.Add(Resources.Load("Materials/player_one_material") as Material);
			team_materials.Add(Resources.Load("Materials/player_two_material") as Material);
			Transform spawn_one = GameObject.Find("spawn_one").transform;
			Transform spawn_two = GameObject.Find("spawn_two").transform;
			bm = GameObject.Find("BattleManager").GetComponent("BattleManager") as BattleManager;
			
			if (Network.isServer) {
				for(int i = 0; i < player_one_units.Count; i++){
					spawn_player(player_one_units[i], new Vector3(-9 + i * 5, spawn_one.position.y, spawn_one.position.z), 0, Network.player);
					players.Remove(null);
				}
				
				for(int i = 0; i < player_two_units.Count; i++){
					spawn_player(player_two_units[i], new Vector3(-9 + i * 5, spawn_two.position.y, spawn_two.position.z), 1, Network.player);
					players.Remove(null);
				}
			}
		}
	}
	
	[RPC]
	void LoadLevel (string level, int levelPrefix)
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

		if (Network.isServer)
			networkView.RPC("OnLevelWasLoadedRPC", RPCMode.AllBuffered);	
	}
}
