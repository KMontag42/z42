using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void TurnEnded(TurnInfo tI);

public class TurnBasedBattle : MonoBehaviour {

    public event TurnEnded enemy_turn_ended;
    public event TurnEnded player_turn_ended;
	private BattleManager bm;

    // Use this for initialization
    void Start () {
		bm = ((BattleManager)gameObject.GetComponent((typeof (BattleManager))));
        StartCoroutine(update_state());
        //Immediately start our loop
    }

    // Update is called once per frame
    IEnumerator update_state () {
		Debug.Log("battle started");
        for(;;) {
        //This is short hand for infinity loop.  Same as while(true).
			Debug.Log("new state with :" +bm.current_unit);
			
			if (bm.current_unit.tag == "Player")
			{
				yield return StartCoroutine(player_turn (bm.current_unit));
				break;
			} else if (bm.current_unit.tag == "Enemy") {
				yield return StartCoroutine(enemy_turn(bm.current_unit));
				break;
			}
			
			bm.next_unit();
        }
		yield return null;
		
    }

    IEnumerator player_turn(Unit u) {
		Debug.Log("player turn");
        //This could be placed in a higher scope for memory purposes.

        TurnInfo tI = new TurnInfo();
        //create a new turn info tracker.
		
		u.start_turn();
		
		while (u.has_current_turn)
		{
			
		}

        Debug.Log("Player Turn Ended");

        if(player_turn_ended != null) 
            player_turn_ended(tI);
		
		yield return null;
    }

    IEnumerator enemy_turn(Unit u) {
		Debug.Log("enemy turn");
        //This could be placed in a higher scope for memory purposes.

        TurnInfo tI = new TurnInfo();
        
		u.start_turn();
		
		while (u.has_current_turn)
		{
			
		}
		
        Debug.Log("Enemy Turn Ended");

        if(enemy_turn_ended != null) 
            enemy_turn_ended(tI);
		
		yield return null;

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