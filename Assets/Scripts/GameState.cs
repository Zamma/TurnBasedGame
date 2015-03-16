using UnityEngine;
using System.Collections;
using AssemblyCSharp;

//knows just about everything that is happening in the world.

public class GameState : MonoBehaviour {

	public CurrentAction currentAction;
	public Map map;
	public Unit selectedUnit;
	public Unit targetedUnit;
	public Unitmanager unitManager;
	public BattleEvent battle;

	// Use this for initialization
	void Start () {
		currentAction = GetComponent<CurrentAction>();
		map = GetComponent<Map>();
	}

	public void selectUnit(Unit unit){
		selectedUnit = unit;
	}

	public void deselectUnit(){
		selectedUnit = null;
	}
}
