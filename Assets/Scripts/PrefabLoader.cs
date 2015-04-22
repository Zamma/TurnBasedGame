using UnityEngine;
using System.Collections;

//place to access prefabs so they can be called during runtime
//there might be a better way to do this.

public class PrefabLoader : MonoBehaviour {
	public Unit baseUnit;
	public Tile baseTile;
	public Highlight highlight;
	public ActionButton actionButton;
	public GUIlist guiList;
	public Building building;
	public skillOptionUI skillOptionUI;
}
