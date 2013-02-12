using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using pumpkin.events;
using pumpkin.display;
using pumpkin.text;
using pumpkin.tweener;

public class GUIOverlay : MonoBehaviour
{
	public
		float scale_factor = 1.0f;
	private
		Stage stage;
		List<MovieClip> buttonList;
		static MovieClip menu;
		MovieClip move_button;
		MovieClip attack_button;
		MovieClip defend_button;
		MovieClip magic_button;
		MovieClip wait_button;
		static Unit selected_player;
	
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
		move_button = (MovieClip)menu.getChildByName ("move_btn");
		attack_button = (MovieClip)menu.getChildByName ("attack_btn");
		defend_button = (MovieClip)menu.getChildByName ("defend_btn");
		magic_button = (MovieClip)menu.getChildByName ("magic_btn");
		wait_button = (MovieClip)menu.getChildByName ("wait_btn");
		
		move_button.addEventListener(MouseEvent.CLICK, onMoveButton);
		
		//create a list and store buttons in list
		buttonList = new List<MovieClip> ();
		buttonList.Add (move_button);
		buttonList.Add (attack_button);
		buttonList.Add (defend_button);
		buttonList.Add (magic_button);
		buttonList.Add (wait_button);
		
		menu.x = 100;
		menu.y = 100;
		menu.scaleX = scale_factor;
		menu.scaleY = scale_factor;
		
		//add listeners to each button in the list and stop the animations from playing
		updateListeners (buttonList);

		stage.addChild (menu);
		
		menu.visible = false;
	}

	void Update ()
	{

	}

	public static void showTooltip (Unit s_player, float x, float y)
	{
		selected_player = s_player;
		menu.visible = true;
		menu.x = x;
		menu.y = y;
	}

	public static void hideTooltip ()
	{
		menu.visible = false;
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
	
	void onMoveButton(CEvent e)
	{
		menu.visible = false;
		print (selected_player);
		StartCoroutine(selected_player.Move());
		// alert player unit that it is being moved
		// display move range
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
