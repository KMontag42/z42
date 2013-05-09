using UnityEngine;
using System.Collections;
using pumpkin.events;
using pumpkin.display;
using pumpkin.text;
using pumpkin.tweener;

public class UnitFrame {
	
	Unit unit;
	public MovieClip frame;
	TextField unit_name;
	public UnitFrame(Unit unit){
		this.unit = unit;	
		frame = new MovieClip ("Flash GUI/game_gui.swf:unit_frame");
		unit_name = frame.getChildByName("unit_name") as TextField;
		unit_name.text = unit.name;
	}
	


}
