using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour {

	public GameState gameState;
	public Unit selectedUnit;
	public Map map;
	public Camera camera;
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
		gameState = GetComponent<GameState>();
	}

	void Update(){

	}

	public void clickedOnUnit(GameObject gameobject)
	{
		if (currentAction.equals("neutral") || currentAction.equals("selectedUnit")){
			Unit unit = gameobject.GetComponent<Unit>();
			selectUnit(unit);
		}
	}

	public void rightClickedOnUnit(GameObject gameObject){
		Unit unit = gameObject.GetComponent<Unit>();
		 if (currentAction.equals("looking") && unit.tile.AttackHighlighted()){
			Unit attacker = selectedUnit;
			map.highlight(attacker.tile.x,attacker.tile.y,attacker.minRnge,attacker.MaxRnge,Tile.WHITE);
			selectedUnit.mov = 0;
			battle(attacker,unit);
			deselectUnit();
		}
	}

	public void clickedOnTile(GameObject tile)
	{
		if (currentAction.equals("selectedUnit")){
			deselectUnit();
		}
		else if (currentAction.equals("movedUnit")){
			selectedUnit.deleteActionsMenu();
			returnUnitToTile();
		}
		else if (currentAction.equals ("looking")){
			Unit unit = selectedUnit;//to make the next line shorter.
			map.highlight(unit.tile.x,unit.tile.y,unit.minRnge,unit.MaxRnge,Tile.WHITE);
			selectedUnit.makeActionsMenu();
			currentAction.set("movedUnit");
		}
	}
	//the unit ends its movement
	public void endUnitTurn(Unit unit){
		unit.mov = 0;
		deselectUnit();
		unit.deleteActionsMenu();
	}

	public void lookForAttack(Unit unit){
		currentAction.set("looking");
		map.highlight(unit.tile.x,unit.tile.y,unit.minRnge,unit.MaxRnge,Tile.ATTACKHIGHLIGHT);
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
		else if (currentAction.equals("looking") && tile.hasUnit() && tile.AttackHighlighted()){
			Unit unit = selectedUnit;
			map.highlight(unit.tile.x,unit.tile.y,unit.minRnge,unit.MaxRnge,Tile.WHITE);
			selectedUnit.mov = 0;
			battle(unit,tile.unit);
			deselectUnit();
		}
	}

	public void battle(Unit attacker,Unit attackee){
		print ("battle!");
		gameState.battle = new BattleEvent(attacker,attackee,map);
		gameState.battle.battle();
		gameState.battle = null;
	}

	//select a unit, whether one was selected before or not.
	private void selectUnit(Unit unit)
	{
		returnUnitToTile(); //return any unit that may have been not full moved to its original location.
		selectedUnit = unit;
		gameState.selectUnit(unit);

		List<Tile> tiles = map.getTilesInMovement(unit.tile.x,unit.tile.y,unit.moveCosts,unit.mov);
		foreach(Tile tile in tiles){
			tile.highlight(Tile.MOVEHIGHLIGHT);
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
