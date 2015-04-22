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
namespace AssemblyCSharp
{
	public class AIBaseBuilding : BuildingEffect
	{
		
		public AIBaseBuilding (int x,int y,Faction ai) : base(x,y)
		{
			faction = ai;
			activates.Add("MakeUnit");
		}

		//takes a command that tells it what unit to make.
		//then it creates the unit at an available spot
		public override void activate(string activator){
			Vector3 openTile = findFreeSpace();
			if (openTile.x == -1) {//it would not let me return null, so -1 is the failure case.
				MonoBehaviour.print ("no open tiles for: " + activator + ".");
				return;
			}
			switch (activator) {
			case "basic" :
				//MonoBehaviour.Instantiate(Grid.prefabLoader.baseUnit,openTile,Quaternion.identity);
				faction.makeUnit((int)openTile.x,(int)openTile.y);
				break;
			default :
				MonoBehaviour.print ("no unit with the name: " + activator + " exists.");
				break;
			}
		}

		public override void onCreate(){

		}

		public override void destroy(string condition){
			
		}

		private Vector3 findFreeSpace(){
			int x = building.tile.x;
			int y = building.tile.y;
			Tile [,] map = Grid.map.map;
			if (Grid.map.onMap(x,y) && !map[x,y].hasUnit()) return new Vector3(x,y,0);
			if (Grid.map.onMap(x+1,y) && !map[x+1,y].hasUnit()) return new Vector3(x+1,y,0);
			if (Grid.map.onMap(x-1,y) && !map[x-1,y].hasUnit()) return new Vector3(x-1,y,0);
			if (Grid.map.onMap(x,y+1) && !map[x,y+1].hasUnit()) return new Vector3(x,y+1,0);
			if (Grid.map.onMap(x,y-1) && !map[x,y-1].hasUnit()) return new Vector3(x,y-1,0);
			else return new Vector3(-1,-1,-1);
		}
		
	}
}
