using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    #region Fields
    public int[] coordinates = new int[3];
    public int[] moveTo = new int[3];
    #endregion

    #region Methods
    // Use this for initialization
	public virtual void Start () {
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate () {
        
		//unitMenuRect.x = Camera.main.WorldToScreenPoint(transform.position).x;
		//unitMenuRect.y = Camera.main.WorldToScreenPoint(transform.position).y;
	}
	
	public virtual void OnMouseDown()
	{
		Camera camera_3d = Camera.allCameras[0];
		Vector3 pos = camera_3d.WorldToScreenPoint(transform.position);
		GUIOverlay.showTooltip(pos.x, pos.y);
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
		
	}
	
	#endregion
}