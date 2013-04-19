using UnityEngine;
using System.Collections;

public class WinManager : MonoBehaviour {
	
	Rect winner_box = new Rect(Screen.width / 3, Screen.height / 4, Screen.width / 3, 50);
	Rect winning_team_box = new Rect(Screen.width / 3, Screen.height / 4 + 75, Screen.width / 3, 50);
	GameManager gm;
	
	
	// Use this for initialization
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();		
	}
	
	// Update is called once per frame
	void Update () {}
	
	void OnGUI() {
		GUI.Box (winner_box, "THE WINNER IS:");
		if (gm.winner == 1)
			GUI.Box (winning_team_box, "RED TEAM");
		if (gm.winner == 2)
			GUI.Box (winning_team_box, "BLUE TEAM");
	}
}
