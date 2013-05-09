using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using pumpkin.events;
using pumpkin.display;
using pumpkin.text;
using pumpkin.tweener;

public class TeamFrame {
	
	List<Unit> units;
	List<UnitFrame> unitFrames = new List<UnitFrame>();
	public MovieClip frame;
	
	public TeamFrame(List<Unit> units){
	
		this.units = units;
		frame = new MovieClip ("Flash GUI/game_gui.swf:team_frame");
		
		foreach(Unit unit in units){
			UnitFrame unit_frame = new UnitFrame(unit);
			unitFrames.Add(unit_frame);
		}		
		
		PopulateFrame();
	}
	
	void PopulateFrame(){
		float yPos = 0;
		foreach(UnitFrame unit_frame in unitFrames){	
			frame.addChild(unit_frame.frame);
			unit_frame.frame.x = 0;
			unit_frame.frame.y = yPos;
			yPos += unit_frame.frame.srcHeight;
		}
	}
	

}
