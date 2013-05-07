using UnityEngine;
using System.Collections;

public class SelectionManager : MonoBehaviour
{
	
	GameManager gm;
	int player_one_points = 1000;
	int player_two_points = 1000;
	Rect player_box = new Rect (Screen.width / 3, Screen.height / 4, Screen.width / 3, 50);
	Rect points_left_box = new Rect (Screen.width / 3, Screen.height / 4 + 75, Screen.width / 3, 50);
	Rect prot_box = new Rect (25, Screen.height / 2, Screen.width / 6, 100);
	Rect thf_box = new Rect (25 + Screen.width * 1 / 5, Screen.height / 2, Screen.width / 6, 100);
	Rect gun_box = new Rect (25 + Screen.width * 2 / 5, Screen.height / 2, Screen.width / 6, 100);
	Rect mage_box = new Rect (25 + Screen.width * 3 / 5, Screen.height / 2, Screen.width / 6, 100);
	Rect heal_box = new Rect (25 + Screen.width * 4 / 5, Screen.height / 2, Screen.width / 6, 100);
	Rect done_box = new Rect (Screen.width / 3, Screen.height * 2 / 3, Screen.width / 3, 50);
	Rect server_team_one_box = new Rect( 50, Screen.height / 4, Screen.width / 6 , 150);
	Rect server_team_two_box = new Rect( 50 + Screen.width / 3, Screen.height / 4, Screen.width / 6 , 150);
	
	// Use this for initialization
	void Start ()
	{
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> () as GameManager;
	}
	
	public void OnConnectedToServer() {
		if (Network.isClient)
		{
			if (gm.player_one_id == "--"){
				gm.networkView.RPC("set_player_id", RPCMode.AllBuffered, Network.player.guid);
				gm.networkView.RPC("set_player", RPCMode.AllBuffered, Network.player, 1);
			} else if (gm.player_two_id == "--") {
				gm.networkView.RPC("set_player_id", RPCMode.AllBuffered, Network.player.guid);
				gm.networkView.RPC("set_player", RPCMode.AllBuffered, Network.player, 2);
			}
		}	
	}
	// Update is called once per frame
	void Update ()
	{
		if (gm.player_one_ready && gm.player_two_ready) {
			gm.networkView.RPC( "LoadLevel", RPCMode.AllBuffered, "battle_test", gm.lastLevelPrefix + 1);	
		}
	}
	
	void OnGUI ()
	{
		if (Network.isClient) {
			
			if (player_one_points > 249) {
				GUI.Box (player_box, "Player Select");
				GUI.Box (points_left_box, "Points left: " + player_one_points);
				if (GUI.Button (prot_box, "Protector\n" + ProtectorClass.cost + " points\nRectangle")) {
					gm.networkView.RPC("add_unit_from_player", RPCMode.AllBuffered, Network.player.guid, "Prefabs/protector");
					player_one_points -= ProtectorClass.cost;
				}
				if (GUI.Button (thf_box, "Theif\n" + TheifClass.cost + " points\nSquare")) {
					if (player_one_points > TheifClass.cost) {
						gm.networkView.RPC("add_unit_from_player", RPCMode.AllBuffered, Network.player.guid, "Prefabs/theif");
						player_one_points -= TheifClass.cost;
					}
				}
				if (GUI.Button (gun_box, "Gunner\n" + GunnerClass.cost + " points\nCylinder")) {
					if (player_one_points > GunnerClass.cost) {
						gm.networkView.RPC("add_unit_from_player", RPCMode.AllBuffered, Network.player.guid, "Prefabs/gunner");
						player_one_points -= GunnerClass.cost;
					}
				}
				if (GUI.Button (mage_box, "Mage\n" + MageClass.cost + " points\nCapsule")) {
					if (player_one_points > MageClass.cost) {
						gm.networkView.RPC("add_unit_from_player", RPCMode.AllBuffered, Network.player.guid, "Prefabs/mage");
						player_one_points -= MageClass.cost;
					}
				}
				if (GUI.Button (heal_box, "Healer\n" + HealerClass.cost + " points\nSphere")) {
					if (player_one_points > HealerClass.cost) {
						gm.networkView.RPC("add_unit_from_player", RPCMode.AllBuffered, Network.player.guid, "Prefabs/healer");
						player_one_points -= HealerClass.cost;
					}
				}
				if (GUI.Button (done_box, "Done")) {
					player_one_points = 0;	
				}
			} else {
				gm.networkView.RPC("set_player_ready", RPCMode.AllBuffered, Network.player.guid);
			}
		}
		
		if (Network.isServer) {
			GUI.Box(server_team_one_box, "");
			GUILayout.BeginArea(server_team_one_box);
			foreach (string u in gm.player_one_units) {
				GUILayout.Box(u);	
			}
			GUILayout.EndArea();
			
			GUI.Box(server_team_two_box, "");
			GUILayout.BeginArea(server_team_two_box);
			foreach (string u in gm.player_two_units) {
				GUILayout.Box(u);	
			}
			GUILayout.EndArea();
		}
	}
	
	
}
