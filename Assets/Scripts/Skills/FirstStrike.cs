//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
//makes the unit always attack first (unless the attacker has some skill that prevents it)

namespace AssemblyCSharp
{
		public class FirstStrike : Skill
		{
		BattleEvent battle;
				public FirstStrike (Unit u) : base (u)
				{
					activates.Add("attackedInitial");
				}

				public override void activate(string activator){ //this may interfere with other skills if it is called in the wrong order.
					battle = gameState.battle;
					battle.attacked = battle.attacker;	//might need to add a priority field to skills to determine what gets called first.
					battle.attacker = unit;
				}

				public override void destroy(string condition){

				}
		}
}

