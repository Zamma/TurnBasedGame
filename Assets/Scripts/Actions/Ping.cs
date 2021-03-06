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
using AssemblyCSharp;

namespace AssemblyCSharp
{
		public class Ping : Action
		{
				public Ping ()
				{
			type = "Ping";
				}

		public override void display(){ //shows selectable tiles
			List<Tile> tiles = Grid.map.getAllVisibleTiles();
			foreach(Tile tile in tiles){
				tile.highlight(Highlight.SPELLHIGHLIGHT);
			}
		}
		public override void activate(Tile tile){ //does the action
			Faction player = Grid.turnManager.active;
			if (tile.hasUnit() && !tile.unit.faction.Equals(player)){
				tile.unit.takeRawDamage(1);
				foreach(Tile shaded in Grid.map.getAllVisibleTiles()){
					shaded.highlight(Highlight.CLEAR);
				}
				Grid.controller.endAction("finish");
			}
			else return;
		}
		public override void clickedTile(Tile tile){	//input from user
			foreach(Tile shaded in Grid.map.getAllVisibleTiles()){
				shaded.highlight(Highlight.CLEAR);
			}
			Grid.controller.endAction("cancel Spell");
		}
		public override void rightClickedTile(Tile tile){ //input from user
			if(Grid.map.getAllVisibleTiles().Contains (tile)){
				activate(tile);
			}
		}
		public override float evaluate(){ //for AI use
			return 0f;
		}
		}
}

