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
using System.Collections.Generic;

namespace AssemblyCSharp
{
		public class PlayerFaction : Faction
		{
		private Controller controller;

				public PlayerFaction () : base()
				{
				spells = new List<Action>();
				spells.Add(new Ping());
				controller = Grid.controller;
				}

		public override void initiate(){
			makeUnit(10,10);
			makeUnit (11,10);
			//new WaitForSeconds(1);
			foreach(Unit unit in units){
				new PlusAttackSkill(unit,5);
				new FirstStrike(unit);
			}
		}

		public override void startTurn(){
			refreshAllVision();
			//controller.enabled = true;
		}

		public override void endTurn(){
			Controller.print("end");
			restoreMove();
			controller.enabled = false;
			foreach(Unit unit in units){
				unit.printSkills();
			}
			Grid.map.makeAllFog();
			refreshAllVision();
		}

		public void refreshAllVision(){
			foreach(Unit unit in units){
				unit.refreshVision();
			}
		}


		}
}

