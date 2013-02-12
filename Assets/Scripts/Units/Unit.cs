using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    #region Fields
    public int[] coordinates = new int[3];
    public int[] moveTo = new int[3];
	[SerializeField]
	public IUnit unitInfo;
	
    public GameObject onGO;
    public Transform _transform;
	public PlayerGUI pGUI;
	public Rect guiRect;
	
	public Rect unitMenuRect;
    #endregion

    #region Properties
    public Transform Transform
    {
        get { return _transform; }
    }
	public int MoveRange
	{
		get { return UnitInfo.Class.Stats[3].ModValue; }
	}
	public ISpell[] Spells
	{
		get { return UnitInfo.Class.SpMan.GetSpells(); }
	}
	// HP, AT, KI
	public int[] Vitals
	{
		get { return UnitInfo.Class.Vitals; }
	}
	public IStat[] Stats
	{
		get { return UnitInfo.Class.Stats; }
	}
	// HP, AT, KI
	public int[] CurrentVitals
	{
		get { return UnitInfo.Class.CurrentVitals; }
	}
	public Rect UnitGUIRect
	{
		get { return guiRect; }
	}
	public IUnit UnitInfo
	{
		get { return unitInfo; }
		set { unitInfo = value; }
	}
	public Rect UnitMenuRect
	{
		get { return unitMenuRect; }
		set { unitMenuRect = value; }
	}
    #endregion

    #region Methods
    // Use this for initialization
	public virtual void Start () {
        //transform.rotation.Set(90, 0, 0, 0);
        _transform = transform;
		
		pGUI = Camera.main.GetComponent<PlayerGUI>();
		
		guiRect = new Rect(Camera.main.WorldToScreenPoint(transform.position).x - 25, 
					  	   Camera.main.WorldToScreenPoint(transform.position).y - 35, 100, 200);
		
		unitMenuRect = new Rect(Camera.main.WorldToScreenPoint(transform.position).x - 15,
								Camera.main.WorldToScreenPoint(transform.position).y - 205, 100, 200);
		
		unitInfo.Class.Init();
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate () {
        
		//unitMenuRect.x = Camera.main.WorldToScreenPoint(transform.position).x;
		//unitMenuRect.y = Camera.main.WorldToScreenPoint(transform.position).y;
	}
	
	public virtual void OnMouseDown()
	{
		pGUI.UnitClicked = this;
		pGUI.IsUnitClicked = true;
		guiRect = new Rect(Input.mousePosition.x - 30, Screen.height - Input.mousePosition.y - 50, 200, 400);
	}
	
    public virtual IEnumerator MoveTo (Vector3 pos, float t)
	{
		Vector3 start = transform.position;
		Vector3 end = pos;
		
		float _t = 0;
		
		while (_t < 1)
		{
			_t += Time.deltaTime / t;
			transform.position = Vector3.Lerp(start, end, _t);
			yield return null;
		}
		transform.position = end;
	}
	
	public virtual string VitalReadout() {
		string r = "Outside Battle";
		if (UnitInfo != null) {
			if (UnitInfo.Class.Vitals.Length >= 3)
				r = "HP: "+UnitInfo.Class.Vitals[0]+"\nAT: "+UnitInfo.Class.Vitals[1]+"\nKI: "+UnitInfo.Class.Vitals[2];
		}		
		return r;
	}
	
	public virtual Unit[] FindNeighbors(int range) {
		List<Unit> a = new List<Unit>();
		a.Add(this);
		return a.ToArray();
	}
    #endregion
	
	#region Event Listeners
	void OnEnable()
	{
		Messenger<int[]>.AddListener("attacked",OnAttacked);
	}
	
	void OnDisable()
	{
		Messenger<int[]>.RemoveListener("attacked",OnAttacked);
	}
	
	void OnAttacked(int[] d) {
		unitInfo.TakeDamage(d);
	}
	
	#endregion
}