using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void TurnEnded(TurnInfo tI);

public class TurnBasedBattle : MonoBehaviour {

    public event TurnEnded enemyTurnEnded;
    public event TurnEnded playerTurnEnded;
	public List<Unit> turnOrder = new List<Unit>();
	private int curUnit = 0;
	private BattleManager bm;
	
	private ISpell activePlayerSpell = null;
	private Unit[] activePlayerTargets = null;

    // Use this for initialization
    void Start () {
		bm = ((BattleManager)gameObject.GetComponent((typeof (BattleManager))));
		turnOrder = bm.turnOrder;
        StartCoroutine(UpdateState());
        //Immediately start our loop
    }

    // Update is called once per frame
    IEnumerator UpdateState () {
		Debug.Log("battle started");
		Debug.Log(turnOrder.Count);
        for(;;) {
        //This is short hand for infinity loop.  Same as while(true).
			Debug.Log("new state with :" +curUnit);
			Unit _unit = turnOrder[curUnit];
			
			if (_unit.tag == "Player") {
				turnOrder.RemoveAt(curUnit);
				Destroy(_unit.gameObject);
				foreach(Unit u in turnOrder) {
					bm.EndBattle();
				}
				Debug.Log("battle ended");
				break;
			}
			
			else if (_unit.tag == "Enemy") {
				turnOrder.RemoveAt(curUnit);
				Destroy(_unit.gameObject);
				foreach(Unit u in turnOrder) {
					bm.EndBattle();
				}
				Debug.Log("battle ended");
				break;
			}
		
            //Do enemy loop, finish, restart loop
			
			curUnit++;
			if (curUnit > turnOrder.Count - 1)
				curUnit = 0;
        }
		yield return null;
		
    }

    IEnumerator PlayerTurn() {
		Debug.Log("player turn");
        activePlayerTargets = null;
		activePlayerSpell = null;
        //This could be placed in a higher scope for memory purposes.

        bool objectSelected = false;
        //have we selected an object.

        TurnInfo tI = new TurnInfo();
        //create a new turn info tracker.
		
		yield return StartCoroutine(MoveTo());
		// wait until the character moves
		
        yield return StartCoroutine(SelectAbility());
        //Wait until we find a target before continuing.
		
        if(activePlayerSpell != null) 
            yield return StartCoroutine(SelectTarget());
		
		if (activePlayerTargets != null)
			yield return StartCoroutine(Attack());
        //Wait until we find a target before continuing.
		

        Debug.Log("Player Turn Ended");

        if(playerTurnEnded != null) 
            playerTurnEnded(tI);
    }

    IEnumerator EnemyTurn() {
		Debug.Log("enemy turn");
        Unit target = null;
		ISpell spell = null;
		activePlayerSpell = null;
		activePlayerTargets = null;
        //This could be placed in a higher scope for memory purposes.

        bool objectSelected = false;
        //have we selected an object.

        TurnInfo tI = new TurnInfo();
        //create a new turn info tracker.

//        yield return StartCoroutine(SelectAbility());
//        //Wait until we find a target before continuing.
//
//        if(activePlayerTargets != null) 
//            yield return StartCoroutine(Attack());
        //Wait until we find a target before continuing.

		yield return new WaitForSeconds(2);
		
        Debug.Log("Attacked1");

        if(enemyTurnEnded != null) 
            enemyTurnEnded(tI);

    }

    IEnumerator SelectAbility () {
        bool abilitySelected = false;

        while (!abilitySelected) {

            yield return null;
        }
    }

    IEnumerator SelectTarget () {
		bool targetSelected = false;
		
		while (!targetSelected) {
						
			yield return null;
		}
	}	

    IEnumerator Attack () {
		Debug.Log("attack target");
        bool attacked = false;

        while(!attacked) {

            yield return null;
        }   

    }
	
	IEnumerator MoveTo() {
		bool targetSelected = false;
			
		while (!targetSelected)
		{
			yield return null;
		}
		
	}





}

public struct TurnInfo {
//custom Turn info struct.  
//Feel free add or remove any fields.

    private float m_DamageDone;
    private string m_MoveUsed;
    private float m_HealthLeft;             
    private bool m_TurnOver;

    public float DamageDone
    {
        get {
            return m_DamageDone;
        }

        set {
            m_DamageDone = value;
        }
    }
    //How much damage did we do.

    public string MoveUsed
    {
        get {
            return m_MoveUsed;
        }

        set {
            m_MoveUsed = value;
        }
    }
    //What move did we use.

    public float HealthLeft
    {
        get {
            return m_HealthLeft;
        }

        set {
            m_HealthLeft = value;
        }
    }
    //How much health do we have left.

    public bool TurnOver
    {
        get {
            return m_TurnOver;
            //read only
        }
    }
    //Should the turn end?

    //Constructor.
    public TurnInfo(float damage, string moveUsed, float healthLeft, bool turnOver) {   
        m_DamageDone = damage;
        m_MoveUsed = moveUsed;
        m_HealthLeft = healthLeft;
        m_TurnOver = turnOver;
    }
}