using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class Unit : MonoBehaviour {
	public int hp, maxHp, skl, def, atk, dge, lvl, exp, maxMov, mov, vis,minRnge,MaxRnge;
	public Tile tile;
	public Controller controller;
	public Canvas menu;
	public TileCosts moveCosts;
	private List<Skill> skills;
	private Canvas activeMenu;

	// Use this for initialization
	void Start() {
		maxHp = 10;
		hp = 10;
		atk = 7;
		def = 2;
		skl = 6;
		dge = 2;
		lvl = 1;
		exp = 0;
		maxMov = 4;
		mov = 4;
		vis = 5;
		minRnge = 1; //TODO range will probably be moved to an attack skill once skills are added.
		MaxRnge = 1;

		moveCosts = new TileCosts();

		skills = new List<Skill>();
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
	
	//set up the action menu. This will need to be fleshed out more once special actions are added.
	public void makeActionsMenu(){
		activeMenu = Instantiate(menu,new Vector3(tile.x + .6f,tile.y + .3f,0),Quaternion.identity) as Canvas;
		ActionPanel panel = activeMenu.GetComponentInChildren<ActionPanel>();
		panel.controller = controller;
		panel.unit = this.GetComponent<Unit>();
	}

	public void deleteActionsMenu(){
		Destroy (activeMenu.gameObject);
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




}
