using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public List<string> player_one_units = new List<string>();
	public List<string> player_two_units = new List<string>();
	public List<Material> team_materials = new List<Material>();
	public int winner;
	
	public List<Unit> players = new List<Unit>();
	BattleManager bm;
	
	public void spawn_player(string unit, Vector3 spawn_position, int player)
	{
		Unit new_player = ((GameObject)GameObject.Instantiate(Resources.Load(unit),spawn_position,Quaternion.identity)).GetComponent<Unit>();
		new_player.renderer.material = team_materials[player];
		new_player.team = player + 1;
		players.Add (new_player);
	}
	
	public void start_battle()
	{
		bm.init(players);
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
			GUILayout.BeginVertical();
			if (GUILayout.Button ("Start Battle"))
			{
				start_battle();	
			}
		}
	}
	
	void OnLevelWasLoaded() {
		if (Application.loadedLevelName == "battle_test") {
			team_materials.Add(Resources.Load("Materials/player_one_material") as Material);
			team_materials.Add(Resources.Load("Materials/player_two_material") as Material);
			Transform spawn_one = GameObject.Find("spawn_one").transform;
			Transform spawn_two = GameObject.Find("spawn_two").transform;
			bm = GameObject.Find("BattleManager").GetComponent("BattleManager") as BattleManager;
			for(int i = 0; i < player_one_units.Count; i++){
				spawn_player (player_one_units[i], new Vector3(-9 + i * 5, spawn_one.position.y, spawn_one.position.z), 0);
				players.Remove(null);
			}
			
			for(int i = 0; i < player_two_units.Count; i++){
				spawn_player (player_two_units[i], new Vector3(-9 + i * 5, spawn_two.position.y, spawn_two.position.z), 1);
				players.Remove(null);
			}
		}
	}
}
