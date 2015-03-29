using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Tile : MonoBehaviour {
	
	public Highlight shade;
	public int moveCost = 1,x,y;
	public string type = "default";
	public bool visible = true;
	public int height = 0;
	public Unit unit;
	public Controller controller;

	// Use this for initialization
	void Start () {
		controller = Grid.controller;
		shade = Instantiate(Grid.prefabLoader.highlight,new Vector3(x,y,0),Quaternion.identity) as Highlight;
		shade.transform.parent = transform;
		highlight (Highlight.CLEAR);
		makeFog();
	}

	public void setPosition(int xcor, int ycor)
	{
		x = xcor;
		y = ycor;
	}

	//called when the tile is clicked on.
	public void clickedOn()
	{
		controller.clickedOnTile(this.gameObject);
	}

	public void rightClickedOn(){
		controller.rightClickedOnTile(this.gameObject);
	}

	public void highlight(Color color){
		shade.setColor(color);
	}

	public void clearHighlight(){
		highlight (Highlight.CLEAR);
	}

	public Color getHighlight(){
		return shade.getColor();
	}

	public bool white(){
		Color color = getHighlight();
		if (color.b == 255f && color.r == 255f && color.g == 255f){
			return true;
		}
		else {
			return false;
		}
	}

	public void makeFog(){
		highlight(Highlight.FOG);
		if (unit != null) unit.makeInvisible();
	}

	public void reveal(){
		shade.setColor(Highlight.CLEAR);
		if (unit != null) unit.makeVisible();
	}

	public bool moveHighlighted(){
		Color color = getHighlight();
		if (color.Equals (Highlight.MOVEHIGHLIGHT)){
			return true;
		}
		else {
			return false;
		}
	}
	
	public bool AttackHighlighted(){
		Color color = getHighlight();
		if (color.Equals (Highlight.ATTACKHIGHLIGHT)){
			return true;
		}
		else {
			return false;
		}
	}

	public void MakeGrass(){
		moveCost = 1;
		type = "Grass";
		changeSprite("grassland");
	}

	public void MakeForest(){
		moveCost = 2;
		type = "Forest";
		changeSprite("forest");
	}

	public void MakeWater(){
		moveCost = 10000;
		type = "Water";
		changeSprite ("water");
	}

	public void MakeHill(){
		moveCost = 2;
		type = "Hill";
		changeSprite("hill");
	}

	public void MakeMountain(){
		moveCost = 5000;
		type = "Mountain";
		height = 2;
		changeSprite ("mountain");
	}

	private void changeSprite(string type){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAssetAtPath("Assets/Art/" + type + ".png",typeof(Sprite)) as Sprite;
	}

	public bool hasUnit(){ //true if there is a unit in the tile.
		if (unit){
			return true;
		}
		else {
			return false;
		}
	}

}
