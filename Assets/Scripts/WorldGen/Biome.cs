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
		public abstract class Biome
		{
		//chance is the weight of that type being chosen. Strength effects how far it spreads linearly. Falloff affects how sharply it cuts out.
		//If you want sharp borders between the terrain types while keeping the area about the same size, increase falloff and strength.
		//If you want gradual borders reduce strength and reduce falloff.
		//falloff is more powerfull than strength (exponential compared to linear).
		public static float WATERCHANCE = 1;
		public static float GRASSCHANCE = 30;
		public static float FORESTCHANCE= 8;

		public float variance;
		public float strength;
		public float falloff;
		public string type = "default";
		public int x,y;

		//area is the distance from the center that it will try to put terrain features in.
		public static int area = Map.BIOMESEPERATION;

				public Biome (int xcor,int ycor)
				{
					x = xcor;
					y = ycor;
				}

		public abstract void makeFeatures();

				//pulls a random corrdinate from its area
				public Vector2 randomTileInArea(){
					Vector2 position = new Vector2();
					int xcor = (int) (UnityEngine.Random.value*area*2 - area + x);
					if (xcor < 0) xcor = 0;
					if (xcor > Map.WIDTH-1) xcor = Map.WIDTH -1;
					int ycor = (int) (UnityEngine.Random.value*area*2 - area + y);
					if (ycor < 0) ycor = 0;
					if (ycor > Map.WIDTH-1) ycor = Map.WIDTH -1;
					position.x = xcor;
					position.y = ycor;
					return position;
				}
		}
}

