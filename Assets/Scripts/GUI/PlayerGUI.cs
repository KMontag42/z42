using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGUI : MonoBehaviour
{

    #region Fields
	public Camera minimapCam;
	public GUISkin loadSkin;
	
    private Unit player;
	private Unit unitClicked;
    private bool isSkillMenuClicked = false;
    private bool isTileClicked = false;
	private bool isMoveMenuClicked = false;
	private bool isSkill1Clicked = false;
	private bool isAttackClicked = false;
	private bool inBattle = false;
	private bool isUnitClicked = false;
	private bool activeTurn = false;
	
    private Rect tileMenu;
	private GUIContent mouseContent;
	
	//private static Rect enemyAttackButtonRect = new Rect(20, enemyMenuRect.yMin + 5, 80, 20);
	//private static Rect enemyHarvestButtonRect = new Rect(20, enemyAttackButtonRect.yMax + 5, 80, 20);
	private static Rect playerMenuRect = new Rect(10, 10, 100, 160);
	
	private static Rect playerVitalsRect = new Rect(10,10,Screen.width/4,Screen.height/6 + 20);
	private static Rect playerPortraitRect = new Rect(playerVitalsRect.xMin+5,playerVitalsRect.yMin+5,
											          playerVitalsRect.width/3,playerVitalsRect.height-10);
	private static Rect playerNameRect = new Rect(playerPortraitRect.xMax+5,playerPortraitRect.yMin,
												  playerVitalsRect.width - playerPortraitRect.xMax,
												  playerVitalsRect.height/5);
	private static Rect playerHPBarRect = new Rect(playerPortraitRect.xMax+5,playerNameRect.yMax + 5,
												   playerVitalsRect.width - playerPortraitRect.xMax,
												   (playerVitalsRect.height/5)/1.5f);
	private static Rect playerKIBarRect = new Rect(playerPortraitRect.xMax+5,playerHPBarRect.yMax + 5,
												   playerVitalsRect.width - playerPortraitRect.xMax,
												   (playerVitalsRect.height/5)/1.5f);
	private static Rect playerATBarRect = new Rect(playerPortraitRect.xMax+5,playerKIBarRect.yMax + 5,
												   playerVitalsRect.width - playerPortraitRect.xMax,
												   1.6f*(playerVitalsRect.height/5));
	private static Rect miniMapRect = new Rect(Screen.width - (Screen.width/6), 10, 
											   Screen.width/6, Screen.width/6);
	private Rect curHPBarRect = playerHPBarRect;
	private Rect curKIBarRect = playerKIBarRect;
	private Rect curATBarRect = playerATBarRect;
	
	private static Rect turnDisplayRect = new Rect(miniMapRect.xMax - Screen.width/12 - 5, miniMapRect.yMax,
												   Screen.width/12, Screen.height - miniMapRect.yMax - 10);
	private static Rect skillBarRect = new Rect((1.6f * (Screen.width/6)),Screen.height - 55, 
												(3 * (Screen.width/6)),50);
	private static Rect buffDisplayRect = new Rect(playerVitalsRect.xMax + 5, 10, 
												   Screen.width/5, 50);
	private static Rect partyDisplayRect = new Rect(playerVitalsRect.xMin, playerVitalsRect.yMax + 5,
													playerVitalsRect.width/2, Screen.height/3);
	private static Rect chatRect = new Rect(partyDisplayRect.xMin,partyDisplayRect.yMax + ((Screen.height - partyDisplayRect.yMax) / 2),
											playerVitalsRect.width,((Screen.height - partyDisplayRect.yMax) / 2) - 5);
	
	private static Rect skill1Rect = new Rect(skillBarRect.xMin + 5, skillBarRect.yMin + 5, skillBarRect.width/11, skillBarRect.height - 10);
	private static Rect skill2Rect = new Rect(skill1Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill3Rect = new Rect(skill2Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill4Rect = new Rect(skill3Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill5Rect = new Rect(skill4Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill6Rect = new Rect(skill5Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill7Rect = new Rect(skill6Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill8Rect = new Rect(skill7Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill9Rect = new Rect(skill8Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	private static Rect skill0Rect = new Rect(skill9Rect.xMax + 5, skill1Rect.yMin, skill1Rect.width, skill1Rect.height);
	
	private BattleManager battleManager;
	private ISpell selectedSpell;
	#endregion

    #region Properties
    public bool IsTileClicked
    {
        get { return isTileClicked; }
        set { isTileClicked = value; }
    }

    public Rect TileMenu
    {
        get { return tileMenu; }
        set { tileMenu = value; }
    }
	
	public bool InBattle
	{
		get { return inBattle; }
		set { inBattle = value; }
	}
	
	public bool IsUnitClicked
	{
		get { return isUnitClicked; }
		set { isUnitClicked = value; }
	}
	
	public Unit UnitClicked
	{
		get { return unitClicked; }
		set { unitClicked = value; }
	}
	public bool ActiveTurn
	{
		get { return activeTurn; }
		set { activeTurn = value; }
	}
	public ISpell SelectedSpell
	{
		get { return selectedSpell; }
		set { selectedSpell = value; }
	}
    #endregion

    #region Methods
    // Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
		minimapCam.pixelRect = new Rect(miniMapRect.xMin+5,miniMapRect.yMax+(1.3f*miniMapRect.height),
										miniMapRect.width-10,miniMapRect.height-10);
		battleManager = (BattleManager)GameObject.Find("Managers").GetComponent("BattleManager");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    void OnGUI()
    {
		GUI.skin = loadSkin;
		
		#region Chat Box
		GUI.Box(chatRect, "Chat", GUI.skin.customStyles[0]);
		#endregion
		
		#region Vitals Box
		GUI.Box(playerVitalsRect,"Vitals");
		GUI.Box(playerPortraitRect,"Portrait");
		GUI.Box(playerNameRect, player.UnitInfo.Name);
		
		if(player.Vitals[0] != 0 && player.CurrentVitals[0] != 0) {
			curHPBarRect.width = playerHPBarRect.width * ((float)player.CurrentVitals[0] / (float)player.Vitals[0]);
		}
		
		GUI.Box(playerHPBarRect,"");
		GUI.Box(curHPBarRect,"HP: " + player.CurrentVitals[0], GUI.skin.customStyles[0]);
		
		if(player.Vitals[2] != 0 && player.CurrentVitals[2] != 0) {
			curKIBarRect.width = playerKIBarRect.width *  ((float)player.CurrentVitals[2] / (float)player.Vitals[2]);
		}
		GUI.Box(playerKIBarRect, "");
		GUI.Box(curKIBarRect, "KI: " + player.CurrentVitals[2]);
		
		if(player.Vitals[1] != 0 && player.CurrentVitals[1] != 0) {
			curATBarRect.width = playerATBarRect.width * ((float)player.CurrentVitals[1] / (float)player.Vitals[1]);
		} else {
			curATBarRect = playerATBarRect;
		}
		GUI.Box(playerATBarRect, "");
		GUI.Box(curATBarRect, "AT: "+player.CurrentVitals[1]);
		
		GUI.Box(buffDisplayRect, "Buffs");
		#endregion
		
		#region Party Display
		GUI.Box(partyDisplayRect, "Party");
		#endregion
		#region Skill Bar
		GUI.Box(skillBarRect, "Skills");
		if (player.Spells.Length > 9) {
			GUI.Box(skill1Rect, player.Spells[0].Name);
			GUI.Box(skill2Rect, player.Spells[1].Name);
			GUI.Box(skill3Rect, player.Spells[2].Name);
			GUI.Box(skill4Rect, player.Spells[3].Name);
			GUI.Box(skill5Rect, player.Spells[4].Name);
			GUI.Box(skill6Rect, player.Spells[5].Name);
			GUI.Box(skill7Rect, player.Spells[6].Name);
			GUI.Box(skill8Rect, player.Spells[7].Name);
			GUI.Box(skill9Rect, player.Spells[8].Name);
			GUI.Box(skill0Rect, player.Spells[9].Name);
		}
		#endregion
		
		#region Battle
		if(battleManager.InBattle) {
			if (player.CurrentVitals[1] < 100) player.CurrentVitals[1] += 1;
			#region Battle Menu
			// get info from the battle class itself
			#endregion
			
			#region Turn Display
			GUI.Box(turnDisplayRect, "Turns");
			for (int i = 0; i < battleManager.turnOrder.Count; i++) {
				if(battleManager.turnOrder[i] != null) {
					GUI.Box(new Rect(turnDisplayRect.xMin + 5, turnDisplayRect.yMin + 5 + ((turnDisplayRect.height*i) / 8), turnDisplayRect.width - 10, (turnDisplayRect.height / 8) - 10),
							battleManager.turnOrder[i].name);
				}
			}
			#endregion
			
			if (ActiveTurn) {
				/*
				ColorTiles(player.FindAdjTiles(player.MoveRange), Color.blue, true);
				if(IsTileClicked) {
					if(player.FindAdjTiles(player.MoveRange).Contains(tileClicked))
					{
			            ColorTiles(player.FindAdjTiles(player.MoveRange), Color.white, true);
			            player.moveTo = tileClicked.coordinates;
						TileClicked.LC = Color.white;
			            isTileClicked = false;
						isMoveMenuClicked = false;
						tileClicked = null;
					}
				}
				*/
			}
			
			#region hotbar
			if (player.Spells.Length > 9) {
				if (player.Spells[0] != null) {
					if (Input.GetKey(KeyCode.Alpha1)) {
						selectedSpell = player.Spells[0]; 
					}
				}
				if (player.Spells[1] != null) {
					if (Input.GetKey(KeyCode.Alpha2)) {
						selectedSpell = player.Spells[1];
					}
				}
				if (player.Spells[2] != null) {
					if (Input.GetKey(KeyCode.Alpha3)) {
						selectedSpell = player.Spells[2];
					}
				}
				if (player.Spells[3] != null) {
					if (Input.GetKey(KeyCode.Alpha4)) {
						selectedSpell = player.Spells[3];
					}
				}
				if (player.Spells[4] != null) {
					if (Input.GetKey(KeyCode.Alpha5)) {
						selectedSpell = player.Spells[4];
					}
				}
				if (player.Spells[5] != null) {
					if (Input.GetKey(KeyCode.Alpha6)) {
						selectedSpell = player.Spells[5];
					}
				}
				if (player.Spells[6] != null) {
					if (Input.GetKey(KeyCode.Alpha7)) {
						selectedSpell = player.Spells[6];
					}
				}
				if (player.Spells[7] != null) {
					if (Input.GetKey(KeyCode.Alpha8)) {
						selectedSpell = player.Spells[7];
					}
				}
				if (player.Spells[8] != null) {
					if (Input.GetKey(KeyCode.Alpha9)) {
						selectedSpell = player.Spells[8];
					}
				}
				if (player.Spells[9] != null) {
					if (Input.GetKey(KeyCode.Alpha0)) {
						selectedSpell = player.Spells[9];
					}
				}
				if (selectedSpell != null){
					GUI.Label(player.UnitMenuRect, selectedSpell.Name);
				}
			}
			#endregion
		}
		#endregion
		
		#region Out of Battle
		else {
			if (isUnitClicked) 
			{
				// when unit is clicked, open its context menu above capsule
				GUI.Label(UnitClicked.UnitMenuRect,UnitClicked.VitalReadout());
				GUI.Box(UnitClicked.UnitMenuRect, "Enemy");
				
				if (GUI.Button(new Rect(UnitClicked.UnitMenuRect.xMin + 10, UnitClicked.UnitMenuRect.yMin + 5, 80, 20), "Engage"))
				{
					List<Unit> com = new List<Unit>();
					com.Add(UnitClicked);
					com.Add(player);
					battleManager.Init(com);
					isUnitClicked = false;
				}
			}
		}
		#endregion
    }

	
//	IEnumerator MoveMenu()
//	{
//		Debug.Log(player.MoveRange);
//		ColorTiles(player.FindAdjTiles(player.MoveRange), true);
//		lastTilesColored = player.FindAdjTiles(player.MoveRange);
//		tileClicked = null;
//		yield return StartCoroutine(WaitForObjectSelect(!isTileClicked));
//		Debug.Log("Tile Selected!");
//		if(player.FindAdjTiles(player.MoveRange).Contains(tileClicked))
//		{
//            ColorTiles(player.FindAdjTiles(player.MoveRange), false);
//            player.moveTo = tileClicked.coordinates;
//            isTileClicked = false;
//			isMoveMenuClicked = false;
//			tileClicked = null;
//		}
//	}
	
//	// to be implemented to take a Skill s
//	IEnumerator SkillMenu()
//	{
//		ColorTiles(player.FindAdjTiles(6));
//		lastTilesColored = player.FindAdjTiles(6);
//		tileClicked = null;
//		unitClicked = null;
//		isTileClicked = false;
//		isUnitClicked = false;
//		yield return StartCoroutine(WaitForObjectSelect(!isTileClicked || !isUnitClicked));
//		Debug.Log("Tile Selected!");
//		if (isTileClicked)
//		{
//			if (tileClicked.IsOccupied)
//			{
//				unitClicked = tileClicked.IsOccupiedBy;
//				isUnitClicked = true;
//			}
//		}
//		if (isUnitClicked)
//		{
//			tileClicked = unitClicked.onTile;
//			//unitClicked.TakeDamage(40);
//		}
//		if(player.FindAdjTiles(6).Contains(tileClicked))
//		{
//            ColorTiles(player.FindAdjTiles(6));
//            tileClicked = null;
//			unitClicked = null;
//			isTileClicked = false;
//			isUnitClicked = false;
//			isAttackClicked = false;
//			isSkill1Clicked = false;
//		}	
//	}
	
//	IEnumerator AttackIEnum()
//	{
//		ColorTiles(player.FindAdjTiles(2));
//		lastTilesColored = player.FindAdjTiles(2);
//		tileClicked = null;
//		unitClicked = null;
//		isTileClicked = false;
//		isUnitClicked = false;
//		yield return StartCoroutine(WaitForObjectSelect(!isTileClicked || !isUnitClicked));
//		Debug.Log("Tile/Unit Selected!");
//		if(isTileClicked)
//		{
//			if (tileClicked.IsOccupied)
//			{
//				unitClicked = tileClicked.IsOccupiedBy;
//				isUnitClicked = true;
//			}
//		}
//		if (isUnitClicked)
//		{
//			tileClicked = unitClicked.onTile;
//			isTileClicked = true;
//			//unitClicked.TakeDamage(10);
//		}
//		if (player.FindAdjTiles(2).Contains(tileClicked))
//		{
//			ColorTiles(player.FindAdjTiles(2));
//			tileClicked = null;
//			unitClicked = null;
//			isTileClicked = false;
//			isUnitClicked = false;
//			isAttackClicked = false;
//		}
//	}
	
//	IEnumerator WaitForObjectSelect(bool b)
//	{
//		while (b == false)
//			yield return null;
//	}
//	
//	IEnumerator ShowEnemyMenu(Unit e) 
//	{
//		yield return 0;
//	}
	
	#endregion
}
