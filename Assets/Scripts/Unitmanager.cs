﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Unitmanager : MonoBehaviour {
	public Camera camera;
	public List<Unit> units;
	public Unit prefabUnit;
	public Controller controller;
	
	// Use this for initialization
	void Start () {
	

	}

	public void createUnit(int x, int y, Tile tile){
		Object objectUnit = Instantiate(prefabUnit,new Vector3(x,y,0),Quaternion.identity); //object to convert from later.
		GameObject unit = objectUnit as GameObject;
		Unit newUnit = objectUnit as Unit; //reminder that the start function in unit is called after this function ends.
		units.Add (newUnit);
		tile.unit = newUnit;
		newUnit.setTile(tile);

		newUnit.controller = controller;

		//EventTrigger trigger = newUnit.GetComponent<EventTrigger>();
		//EventTrigger.Entry entry = new EventTrigger.Entry();
		//entry.eventID = EventTriggerType.PointerClick;	//this works
		//entry.callback.AddListener( (eventData) => { test (); } );
		//trigger.delegates.Add(entry);
	}

	public void addUnit(Unit unit){
		units.Add(unit);
	}

	public void test(){
		print ("test");
	}

	public void removeUnit(Unit unit){
		units.Remove(unit);
	}

	//update

}
