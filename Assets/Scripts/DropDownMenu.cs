using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;

public class DropDownMenu : MonoBehaviour {
	public GUIlist list;
	//public bool active = false;


	public void pressed(){
		if (!Grid.controller.currentAction.equals("neutral")){ //really more simple if the menu is only clickable from neutral.
			return;
		}

		if (list != null){
			//active = false;
			print ("destroy");
			Destroy(list.gameObject);
		}
		else{
			list = Instantiate(Grid.prefabLoader.guiList,new Vector3(transform.position.x - .5f,transform.position.y - GUIlist.SEPERATION - .2f,0),Quaternion.identity) as GUIlist;
			//active = true;
			list.transform.SetParent(transform,true);
			List<Action> spells = Grid.turnManager.factions[0].spells;
			foreach(Action spell in spells){
				list.addAction(spell);
			}
		}
	}

}
