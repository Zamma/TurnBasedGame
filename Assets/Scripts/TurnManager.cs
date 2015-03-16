using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class TurnManager : MonoBehaviour {
	public int turnNumber;
	public List<Faction> factions;
	// Use this for initialization
	void Start () {
		int turnNumber = 0;
		factions = new List<Faction>();
		factions.Add(new Faction());
	}

	public void nextTurn(){
		turnNumber++;


	}
}
