using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using System;
using UnityEngine;
/* ALL ACTIVATE COMMANDS
 * 
 * gaining exp: "gainExp".
 * applied before attacking: "attackerInitial".
 * applied before defending: "attackedInitial".
 * applied when landing a hit on offense: "attackerHit".
 * applied when landing a hit on defense: "defenderHit".
 * applied when being hit on offense: "attackerStruck".
 * applied when being hit on defense: "defenderStruck".
 * applied when missing on offense: "attackerMiss".
 * applied when missing on defense: "defenderMiss".
 * applied when dodging on offense: "attackerDodge".
 * applied when dodging on defense: "defenderDodge".
 * applied when taking damage: "damaged".
 * 
 */

namespace AssemblyCSharp
{
		abstract public class Skill
		{
		public Unit unit;
		public List<string> activates;
		public GameState gameState;

		public Skill (Unit u)
		{
			unit = u;
			unit.addSkill(this);
			activates = new List<string>();
			gameState = Grid.gameState;
		}

		public bool checkActivate(string activator){
			if (activates.Contains(activator)){
				return true;
			}
			else return false;
		}
		//if the condition is met, activate it.
		public Skill doActivate(string activator){
			if (checkActivate(activator)){
				activate (activator);
				return this;
			}
			else return null;
		}

		//returns the skill if the condition is met whithout activating the skill.
		//this allows for more complex strings given to the activation function.
		public Skill getIf(string activator){
			if (checkActivate(activator)){
				return this;
			}
			else return null;
		}

		public abstract void activate(string activator);

		public abstract void destroy(string condition);

		//**************************************************************STATIC METHODS**********************************************************************

		//activates all given skills with the command.
		public static void activateSkills(List<Skill> foundSkills,string command){
			foreach(Skill skill in foundSkills){
				skill.activate(command);
			}
		}
	}
}

