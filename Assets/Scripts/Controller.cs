using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour {

	public GameState gameState;
	public Unit selectedUnit;
	public Action selectedAction;
	public Map map;
	//private GameOptions options;
	private Tile returnTile;
	public CurrentAction currentAction;
	/*********************************
	 * Possible Actions:
	 * 	neutral : nothing of note is happening, the default.
	 *	selectedUnit : A unit is selected, but nothing has happened yet.
	 *	movedUnit	: A unit has moved and is selecting the options.
	 *	looking     : A unit has selected attack and is looking for opponents.
	 */

	// Use this for initialization
	void Start () {
		currentAction = GetComponent<CurrentAction>();
		gameState = Grid.gameState;
		map = Grid.map;
	}

	void Update(){

	}

	public void clickedOnUnit(GameObject gameobject)
	{
		Unit unit = gameobject.GetComponent<Unit>();
		//if either the unit is not under your control or it is not on your turn
		if (unit.mov == 0 || !unit.faction.Equals(Grid.turnManager.factions[0]) || !Grid.turnManager.factions[0].Equals(Grid.turnManager.active)){
			return;
		}
		if (currentAction.equals("neutral") || currentAction.equals("selectedUnit")){
			selectUnit(unit);
		}
	}

	public void rightClickedOnUnit(GameObject gameObject){
		Unit unit = gameObject.GetComponent<Unit>();
		 if (currentAction.equals("action") && unit.tile.AttackHighlighted()){
			/*Unit attacker = selectedUnit;
			map.highlight(attacker.tile.x,attacker.tile.y,attacker.minRnge,attacker.maxRnge,Highlight.CLEAR);
			selectedUnit.mov = 0;
			battle(attacker,unit);
			deselectUnit();*/
		}
	}

	public void clickedOnTile(GameObject t)
	{
		Tile tile = t.GetComponent<Tile>();
		if (currentAction.equals("selectedUnit")){
			deselectUnit();
		}
		else if (currentAction.equals("movedUnit")){
			selectedUnit.deleteActionsMenu();
			returnUnitToTile();
		}
		else if (currentAction.equals ("action")){
			selectedAction.clickedTile(tile);
			/*Unit unit = selectedUnit;//to make the next line shorter.
			map.highlight(unit.tile.x,unit.tile.y,unit.minRnge,unit.maxRnge,Highlight.CLEAR);
			selectedUnit.makeActionsMenu();
			currentAction.set("movedUnit");*/
		}
	}

	public void startingAction(Action action){
		currentAction.set ("action");
		selectedAction = action;
		selectedAction.display();
		if (selectedUnit != null) selectedUnit.deleteActionsMenu();
	}
	//ended the action. condition is either cancel or finish.
	public void endAction(string condition){
		print ("calledEnd");
		if (condition.Equals("cancel")){
			Unit unit = selectedUnit;//to make the next line shorter.
			map.highlight(unit.tile.x,unit.tile.y,unit.minRnge,unit.maxRnge,Highlight.CLEAR);
			selectedUnit.makeActionsMenu();
			currentAction.set("movedUnit");
		}
		else if(condition.Equals("cancel Spell")){
			currentAction.set ("neutral");
		}
		else if (condition.Equals("finish")){
			print ("finished");
			currentAction.set ("neutral");
			deselectUnit();
		}
	}
	//the unit ends its movement
	public void endUnitTurn(Unit unit){
		unit.mov = 0;
		deselectUnit();
		unit.deleteActionsMenu();
		if (unit.faction.Equals(Grid.turnManager.factions[0])){
			unit.refreshVision();
		}
	}
	//This can be deleted once the old menu is replaced
	public void lookForAttack(Unit unit){
		currentAction.set("action");
		map.highlight(unit.tile.x,unit.tile.y,unit.minRnge,unit.maxRnge,Highlight.ATTACKHIGHLIGHT);
		unit.deleteActionsMenu();
	}

	public void rightClickedOnTile(GameObject gameObject){
		Tile tile = gameObject.GetComponent<Tile>();
		if (currentAction.equals("selectedUnit") && map.getUnitMove(selectedUnit).Contains(tile)){ //tile.moveHighlighted()){
			returnTile = map.moveUnit(selectedUnit,tile.x,tile.y);//,options);
			//options = new GameOptions();
			selectedUnit.makeActionsMenu();
			currentAction.set ("movedUnit");
		}
		else if (currentAction.equals("action")){
			selectedAction.rightClickedTile(tile);
		}
		/*
		else if (currentAction.equals("action") && tile.hasUnit() && tile.AttackHighlighted()){
			Unit unit = selectedUnit;
			//map.highlight(unit.tile.x,unit.tile.y,unit.minRnge,unit.maxRnge,Highlight.CLEAR);
			//selectedUnit.mov = 0;
			battle(unit,tile.unit);
			if(unit != null && !unit.isDead()){
				endUnitTurn(unit);
			}
			//deselectUnit();
		}
		*/
	}

	public static void battle(Unit attacker,Unit attackee){
		print ("battle!");
		Grid.gameState.battle = new BattleEvent(attacker,attackee,Grid.map);
		Grid.gameState.battle.battle();
		Grid.gameState.battle = null;
	}

	//select a unit, whether one was selected before or not.
	private void selectUnit(Unit unit)
	{
		returnUnitToTile(); //return any unit that may have been not full moved to its original location.
		selectedUnit = unit;
		gameState.selectUnit(unit);

		List<Tile> tiles = map.getTilesInMovement(unit.tile.x,unit.tile.y,unit.moveCosts,unit.mov);
		foreach(Tile tile in tiles){
			tile.highlight(Highlight.MOVEHIGHLIGHT);
		}
		//options = map.findOptions(selectedUnit.tile.x,selectedUnit.tile.y,selectedUnit.mov);
		
		currentAction.set("selectedUnit");
	}


	private void returnUnitToTile() //handles moving a unit to its orginal tile before it made the move.
	{
		if (returnTile && selectedUnit)
		{
			map.revertUnitToTile(returnTile,selectedUnit);
			returnTile = null;
			deselectUnit();
		}
	}

	private void deselectUnit() //deselect any selected units.
	{
		if (selectedUnit == null){
			currentAction.set ("neutral");
			return;
		}

		if (returnTile != null) {
			map.deselect(returnTile.x,returnTile.y,selectedUnit.maxMov,selectedUnit.moveCosts);
		}
		else map.deselect(selectedUnit.tile.x,selectedUnit.tile.y,selectedUnit.maxMov,selectedUnit.moveCosts);
		selectedUnit = null;
		gameState.deselectUnit();
		returnTile = null; //I think I can say this?
		currentAction.set ("neutral");
	}
}
