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
	Rect done_box = new Rect(Screen.width / 3, Screen.height * 2 / 3, Screen.width / 3, 50);
	
	// Use this for initialization
	void Start ()
	{
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> () as GameManager;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnGUI ()
	{
		if (player_one_points > 249) {
			GUI.Box (player_box, "Player 1 Select");
			GUI.Box (points_left_box, "Points left: " + player_one_points);
			if (GUI.Button (prot_box, "Protector\n" + ProtectorClass.cost + " points\nRectangle")) {
				gm.player_one_units.Add ("Prefabs/protector");
				player_one_points -= ProtectorClass.cost;
			}
			if (GUI.Button (thf_box, "Theif\n" + TheifClass.cost + " points\nSquare")) {
				if (player_one_points > TheifClass.cost) {
					gm.player_one_units.Add ("Prefabs/theif");
					player_one_points -= TheifClass.cost;
				}
			}
			if (GUI.Button (gun_box, "Gunner\n" + GunnerClass.cost + " points\nCylinder")) {
				if (player_one_points > GunnerClass.cost) {
					gm.player_one_units.Add ("Prefabs/gunner");
					player_one_points -= GunnerClass.cost;
				}
			}
			if (GUI.Button (mage_box, "Mage\n" + MageClass.cost + " points\nCapsule")) {
				if (player_one_points > MageClass.cost) {
					gm.player_one_units.Add ("Prefabs/mage");
					player_one_points -= MageClass.cost;
				}
			}
			if (GUI.Button (heal_box, "Healer\n" + HealerClass.cost + " points\nSphere")) {
				if (player_one_points > HealerClass.cost) {
					gm.player_one_units.Add ("Prefabs/healer");
					player_one_points -= HealerClass.cost;
				}
			}
			if (GUI.Button(done_box, "Done")) {
				player_one_points = 0;	
			}
		} else if (player_two_points > 249) {
			GUI.Box (player_box, "Player 2 Select");
			GUI.Box (points_left_box, "Points left: " + player_two_points);
			if (GUI.Button (prot_box, "Protector\n" + ProtectorClass.cost + " points\nRectangle")) {
				if (player_two_points > ProtectorClass.cost) {
					gm.player_two_units.Add ("Prefabs/protector");
					player_two_points -= ProtectorClass.cost;
				}
			}
			if (GUI.Button (thf_box, "Theif\n" + TheifClass.cost + " points\nSquare")) {
				if (player_two_points > TheifClass.cost) {
					gm.player_two_units.Add ("Prefabs/theif");
					player_two_points -= TheifClass.cost;
				}
			}
			if (GUI.Button (gun_box, "Gunner\n" + GunnerClass.cost + " points\nCylinder")) {
				if (player_two_points > GunnerClass.cost) {
					gm.player_two_units.Add ("Prefabs/gunner");
					player_two_points -= GunnerClass.cost;
				}
			}
			if (GUI.Button (mage_box, "Mage\n" + MageClass.cost + " points\nCapsule")) {
				if (player_two_points > MageClass.cost) {
					gm.player_two_units.Add ("Prefabs/mage");
					player_two_points -= MageClass.cost;
				}
			}
			if (GUI.Button (heal_box, "Healer\n" + HealerClass.cost + " points\nSphere")) {
				if (player_two_points > HealerClass.cost) {
					gm.player_two_units.Add ("Prefabs/healer");
					player_two_points -= HealerClass.cost;
				}
			}
			if (GUI.Button(done_box, "Done")) {
				player_two_points = 0;	
			}
		} else {
			Application.LoadLevel ("battle_test");	
		}
	}
}
