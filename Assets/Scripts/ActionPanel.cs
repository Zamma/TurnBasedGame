using UnityEngine;
using System.Collections;

public class ActionPanel : MonoBehaviour {
	public Controller controller;
	public Unit unit;
	// Use this for initialization
	void Start () {
	
	}

	public void attack(){
		controller.lookForAttack(unit);
	}

	//the end button was pressed.
	public void end(){
		controller.endUnitTurn(unit);
	}
}
