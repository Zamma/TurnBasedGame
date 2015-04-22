using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

//UI element that allows the player to select what skill they want during the skill phase.
public class SkillOption : MonoBehaviour {

	public SkillMaker skillM;

	public void giveSkillMaker(SkillMaker maker){
		//print (maker);
		skillM = maker;
		GetComponent<Text>().text = maker.description;
	}
	
	public void onClick(){
		skillM.giveToAllUnits();
		Destroy(this.GetComponentInParent<skillOptionUI>().gameObject); //sloppy way to get the parent?
		Grid.controller.endAction("finish");
		Grid.turnManager.player.phase.activated = true;
	}
}
