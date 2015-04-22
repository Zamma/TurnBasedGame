using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class Unit : MonoBehaviour {
	public int maxHp,hp,skl,def,atk,dge,lvl,exp,maxMov,mov,vis,minRnge,maxRnge;
	public string type = "basic";
	public Faction faction;
	public Tile tile;
	public Controller controller;
	public Canvas menu;
	public TileCosts moveCosts;
	public List<Action> actions = new List<Action>();
	private List<Skill> skills = new List<Skill>();
	private GUIlist activeMenu;
	
	// Use this for initialization
	void Start() {

		//these are all 0 by default.
		maxHp += 10;
		hp += 10;
		atk += 7;
		def += 2;
		skl += 6;
		dge += 2;
		lvl += 1;
		exp += 0;
		maxMov += 4;
		mov += 4;
		vis += 5;
		minRnge += 1; //TODO range will probably be moved to an attack skill once skills are added.
		maxRnge += 1;

		controller = Grid.controller;

		moveCosts = new TileCosts();

		if (faction != null && faction.Equals(Grid.turnManager.factions[0])){
			refreshVision();
		}

		actions.Add(new NormalAttack(this));
		actions.Add(new EndTurn(this));
	}


	//called when the unit is clicked on
	public void clickedOn(){
		controller.clickedOnUnit(this.gameObject);
	}

	public void rightClickedOn(){
		controller.rightClickedOnUnit(this.gameObject);
	}
	
	public void setTile(Tile t){
		tile = t;
	}
	/***********************************UI************************************************************/
	//set up the action menu. This will need to be fleshed out more once special actions are added.
	public void makeActionsMenu(){
		float scale = Grid.map.transform.localScale.x;
		//activeMenu = Instantiate(Grid.prefabLoader.guiList,new Vector3((tile.x + .6f)*scale,(tile.y*scale + .3f)*scale,0),Quaternion.identity) as GUIlist;
		activeMenu = Instantiate(Grid.prefabLoader.guiList,new Vector3(tile.x,tile.y,0),Quaternion.identity) as GUIlist;
		activeMenu.transform.SetParent(Grid.map.transform);
		activeMenu.transform.position = transform.position;
		activeMenu.transform.position += new Vector3(.3f,.6f,0f);
		//ActionPanel panel = activeMenu.GetComponentInChildren<ActionPanel>();
		//panel.controller = controller;
		//panel.unit = this.GetComponent<Unit>();
		foreach(Action action in actions){
			activeMenu.addAction(action);
		}
	}

	public void deleteActionsMenu(){
		if (activeMenu != null) Destroy (activeMenu.gameObject);
	}
	/******************************************Colors and visibility****************************************************************************************/
	public void makeColor(Color color){
		GetComponent<SpriteRenderer>().color = color;
	}

	public void makeInvisible(){
		renderer.enabled = false;
	}

	public void makeVisible(){
		renderer.enabled = true;
	}

	public void refreshVision(){
		List<Tile> tiles = Grid.map.getTilesInMovement(tile.x,tile.y,TileCosts.Flat,vis);
		foreach(Tile t in tiles){
			t.reveal();
		}
	}

	public void setFaction(Faction fact){
		faction = fact;
		makeColor (faction.color);
	}

	public void restoreMove()
	{
		mov = maxMov;
	}

	//level up

	public void takeDamage(int damage,string type){ //TODO make this use different damage types
		int total = damage - def;
		takeRawDamage(total);
	}

	public void takeRawDamage(int damage){
		List<Skill> damageSkills = getSkills("damaged");
		Skill.activateSkills(damageSkills,""+damage);
		hp -= damage;
		if (isDead ()){
			die ();
		}
	}

	public bool isDead(){
		if (hp<=0){
			return true;
		}
		else return false;
	}

	public void die(){
		print("unit died");
		//faction.removeUnit(this); //think I'll do this on the faction's end?
		Destroy (this.gameObject);
	}

	//heal

	public void attack(Unit attackee){
		int hitChance = skl - attackee.dge;
		int hitFactor = Mathf.FloorToInt(Random.value*5); //0-4 (although it's inclusive so on rare occasions it could be 5).
		print ("has a " + hitChance*20 + "% chance to hit");
		if (hitChance - hitFactor <= 0){
			print ("missed");
		}
		else {
			int damage = atk - attackee.def;
			attackee.takeRawDamage(damage);
			print ("hit for: " + damage + " damage");
		}
	}
	//killed a unit
	public void victory(){
		tryActivateSkills("victory");

		gainExp();
	}

	public void gainExp(){
		List<Skill> expSkills;
		int expGain = 1;//this will be more complex later.
		expSkills = getSkills("gainExp");
		Skill.activateSkills(expSkills,"" + expGain);
		exp += expGain;
	}
	
	//attacked
	
	//move

	/******************************************************SKILL SECTION**************************************************************************/

	//procs any skills that match the activator. returns all the skills that do.
	public List<Skill> tryActivateSkills(string activator){
		List<Skill> activatedSkills = new List<Skill>();
		foreach(Skill skill in skills){
			if (skill.doActivate(activator) != null){
				activatedSkills.Add (skill);
			}
		}
		return activatedSkills;
	}
	//gets all skills that match the activation line without activating them.
	public List<Skill> getSkills(string activator){
		List<Skill> activatedSkills = new List<Skill>();
		foreach(Skill skill in skills){
			if (skill.getIf(activator) != null){
				activatedSkills.Add (skill);
			}
		}
		return activatedSkills;
	}


	public void addSkill(Skill skill){
		skills.Add (skill);
	}

	public void removeSkill(Skill skill, string cause){
		skill.destroy(cause);
		skills.Remove(skill);
	}
	//debug method
	public void printSkills(){
		foreach (Skill skill in skills){
			print (skill);
		}
	}




}
