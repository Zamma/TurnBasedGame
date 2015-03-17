using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

//knows just about everything that is happening in the world.

public class GameState : MonoBehaviour {

	public static int NUMFACTIONS = 1;

	public Controller controller;
	public CurrentAction currentAction;
	public Map map;
	public Unit selectedUnit;
	public Unit targetedUnit;
	public Unitmanager unitManager;
	public BattleEvent battle;
	public Faction[] factions;

	// Use this for initialization
	void Start () {
		currentAction = GetComponent<CurrentAction>();
		map = GetComponent<Map>();
		factions = new Faction[NUMFACTIONS];
		factions[0] = new PlayerFaction(controller);
		TurnManager manager = GetComponent<TurnManager>();
		manager.factions = factions;
		manager.active = factions[0];
	}

	public void selectUnit(Unit unit){
		selectedUnit = unit;
	}

	public void deselectUnit(){
		selectedUnit = null;
	}

	public List<Unit> allUnits(){
		List<Unit> units = new List<Unit>();
		foreach (Faction faction in factions){
			units.AddRange(faction.units);
		}
		return units;
	}
}
