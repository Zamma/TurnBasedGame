using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;

public class Building : MonoBehaviour {

	public List<BuildingEffect> effects = new List<BuildingEffect>();
	public Faction faction;
	public string name;
	public Tile tile;

	public void destroy(){
		foreach(BuildingEffect effect in effects){
			effect.destroy(name);
		}
		Destroy(gameObject);
	}
	
	public void addBuildingEffect(BuildingEffect b){
		effects.Add(b);
	}

}
