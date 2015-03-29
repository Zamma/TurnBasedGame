using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class TurnManager : MonoBehaviour {
	public int turnNumber;
	public Faction[] factions;
	public Faction active;
	public Faction player;
	// Use this for initialization
	void Start () {
		int turnNumber = 0;
		GameState state = Grid.gameState; //hopefull gamestate and turnMangager initialize in the right order.
		//factions = state.factions;
		//factions[0] = new PlayerFaction(); //TODO make the factions not be initialized by the GameState class
		//sfactions[1] = new AIFaction();
		active = factions[0];
		player = factions[0];
	}

	public void startFactions(){
		foreach (Faction faction in factions){
			faction.initiate();
		}
		factions[0].startTurn();
	}

	void Update() {
		if (Input.GetKeyDown("return")){
			print("next Turn");
			nextTurn();
		}
	}

	public void nextTurn(){
		turnNumber++;
		active.endTurn();
		active = factions[turnNumber % factions.Length];
		active.startTurn();
	}
}
