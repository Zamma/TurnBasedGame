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
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
		public class SkillPhase : Phase
		{
		public List<SkillMaker> skillOptions = new List<SkillMaker>();
		public SkillMaker selected; //this should be a list?
				public SkillPhase ()
				{
			type = "pick skill";
			skillOptions.Add(new AttackUpMaker());	//this is where to put all skills the player can gain on the skill phase.
			skillOptions.Add(new FirstStrikeMaker());

				}

				public SkillMaker pickRandomSkill(){
					float value,highest = 0;
					SkillMaker current = null;

					foreach(SkillMaker maker in skillOptions){ //assigns a random value to each element and chooses the highest.
						value = UnityEngine.Random.value;
						if (value > highest){
							highest = value;
							current = maker;
						}
					}
				MonoBehaviour.print ("skill is: " + current.description);
					return current;
				}

				public override void display(){

					Grid.turnManager.player.phase = this;

					Vector3 start = new Vector3(Grid.camera.transform.position.x,Grid.camera.transform.position.y,1);
					//makes the UI
					MonoBehaviour.print ("making skill menu");
					skillOptionUI ui = MonoBehaviour.Instantiate(Grid.prefabLoader.skillOptionUI,start,Quaternion.identity) as skillOptionUI;

					ui.transform.SetParent(Grid.camera.transform,true);

					//gives it information.
					ui.GetComponentsInChildren<SkillOption>()[0].giveSkillMaker(selected);
					
				}


				public override void activate(Tile tile){
					//termination of the phase is handled by the skillOption button.
					//activated is also marked as true there.
					selected = pickRandomSkill();
				}


		}
}

