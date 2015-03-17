using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class TurnManager : MonoBehaviour {
	public int turnNumber;
	public Faction[] factions;
	public Faction active;
	// Use this for initialization
	void Start () {
		int turnNumber = 0;
		GameState state = GetComponent<GameState>(); //hopefull gamestate and turnMangager initialize in the right order.
		factions = state.factions; 
		active = factions[0];
	}

	void Update() {
		if (Input.GetKeyDown("return")){
			print("pressed enter");
			nextTurn();
		}
	}

	public void nextTurn(){
		turnNumber++;
		active.endTurn();
		active = factions[turnNumber % factions.Length];
		print (turnNumber % factions.Length);
		active.startTurn();
	}
}
