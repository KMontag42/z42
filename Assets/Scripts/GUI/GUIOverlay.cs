using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using pumpkin.events;
using pumpkin.display;
using pumpkin.text;
using pumpkin.tweener;

public class GUIOverlay : MonoBehaviour
{
	
	Stage stage;
	List<MovieClip> buttonList;
	MovieClip menu;
	MovieClip attack_button;
	MovieClip defend_button;
	MovieClip magic_button;
	MovieClip wait_button;
	void Start ()
	{
        
		// validate MovieClipOverlayCameraBehaviour ist attached to camera
		if (MovieClipOverlayCameraBehaviour.instance == null) {
			return;
		}
        
		// assign stage reference
		stage = MovieClipOverlayCameraBehaviour.instance.stage;
        
		//bottom timeline
		menu = new MovieClip ("Flash GUI/game_gui.swf:hover_menu");
		attack_button = (MovieClip)menu.getChildByName ("attack_btn");
		defend_button = (MovieClip)menu.getChildByName ("defend_btn");
		magic_button = (MovieClip)menu.getChildByName ("magic_btn");
		wait_button = (MovieClip)menu.getChildByName ("wait_btn");
	

		
		//create a list and store buttons in list
		buttonList = new List<MovieClip> ();
		buttonList.Add (attack_button);
		buttonList.Add (defend_button);
		buttonList.Add (magic_button);
		buttonList.Add (wait_button);

		menu.x = 500;
		menu.y = 500;
		
		//add listeners to each button in the list and stop the animations from playing
		updateListeners (buttonList);
		


		stage.addChild (menu);
	}

	void Update ()
	{

	}

	public static void showTooltip (string name, string val, float x, float y)
	{
		/*
		//show the tooltip and position it at the mouse coordinates
		tooltip.alpha = 1;
		tooltip.x = x;
		tooltip.y = y; 
		//set text fields in tooltip
		TextField tooltip_name = (TextField)tooltip.getChildByName ("country_name");
		TextField tooltip_value = (TextField)tooltip.getChildByName ("value");		
		tooltip_name.text = name;
		tooltip_value.text = val;
		*/
	}

	public static void hideTooltip ()
	{
//		tooltip.alpha = 0;
	}
	
	//button listener events
	void onButtonClick (CEvent e)
	{
		MovieClip m = e.currentTarget as MovieClip;
		m.gotoAndStop ("press");		
	}

	void onButtonEnter (CEvent e)
	{
		MovieClip m = e.currentTarget as MovieClip;
		m.gotoAndStop ("hover");		
	}

	void onButtonLeave (CEvent e)
	{
		MovieClip m = e.currentTarget as MovieClip;
		m.gotoAndStop ("static");		
	}
	
	void updateListeners (List<MovieClip> list)
	{
		foreach (MovieClip m in list) {
			m.addEventListener (MouseEvent.CLICK, onButtonClick);
			m.addEventListener (MouseEvent.MOUSE_ENTER, onButtonEnter);
			m.addEventListener (MouseEvent.MOUSE_LEAVE, onButtonLeave);
			
			m.stop ();
		}	
	}
	
}
