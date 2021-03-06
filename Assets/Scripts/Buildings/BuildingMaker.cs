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

//runs when the player has selected a building they would like to build.
//this highlights the tiles the player can put the building on.
//if the player right clicks one of those tiles BuildingMaker will place the building there and subtract the resource cost.
namespace AssemblyCSharp
{
		public class BuildingMaker : Action
		{
			public List<Tile> selectedTiles = new List<Tile>();
			public BuildingEffect build;

				public BuildingMaker (BuildingEffect buildingE)
				{
					build = buildingE;
					type = buildingE.name;
				}

		
		public override void display(){
			foreach (Tile tile in Grid.map.getAllVisibleTiles()){

				if (tile.building != null) continue; //do not return tiles that have a building.

				foreach(Tile t in Grid.map.getAdjacent(tile)){
					if(t.building != null){
						selectedTiles.Add(tile);
						tile.highlight(Highlight.SPELLHIGHLIGHT);
						break;
					}
				}
			}
		}

		public override void activate(Tile tile){

		}

		public override void clickedTile(Tile tile){
			end();
		}

		public override void rightClickedTile(Tile tile){

			if (!selectedTiles.Contains(tile)) return; //do nothing if the tile does not support the type of building.

			build.makeBuilding(tile.x,tile.y);
			Faction player = Grid.turnManager.player; //make the code shorter by assigning this
			player.makeBuilding(build);
			end ();
			/*
			switch (type){
			case "Citadel":
				player.makeBuilding(new Citadel(tile.x,tile.y));
				break;
			default:
				MonoBehaviour.print ("no buildings match that name");
				break;
			}*/
		}

		public void end(){
			foreach(Tile selecedTile in selectedTiles){
				selecedTile.clearHighlight();
			}
			selectedTiles.Clear();

			Grid.turnManager.player.phase.activated = true;
			Grid.turnManager.player.phase.display(); //this may need to also make the phase the active action on the controller?

			Grid.controller.endAction("finish");
		}

		//probably not ever run. should really make this function only required for unit actions.
		public override float evaluate(){
			return 0f;
		}
		}
}

