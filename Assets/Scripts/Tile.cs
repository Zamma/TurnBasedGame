using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public static Color MOVEHIGHLIGHT = new Color(.09f,.180f,.220f);
	public static Color WHITE = new Color(255f,255f,255f);
	public static Color ATTACKHIGHLIGHT = new Color(255f,0f,0f);
	public int moveCost = 1,x,y;
	public string type = "default";
	public bool visible = true;
	public int height = 0;
	public Unit unit;
	public Controller controller;

	// Use this for initialization
	void Start () {
		highlight (new Color( 255f, 255f, 255f));
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
		SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
		sp.color = color;
	}

	public void clearHighlight(){
		Color color = WHITE;
		highlight (color);
	}

	public Color getHighlight(){
		Color color = gameObject.GetComponent<SpriteRenderer>().color;
		return color;
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

	public bool moveHighlighted(){
		Color color = getHighlight();
		if (color.Equals (MOVEHIGHLIGHT)){
			return true;
		}
		else {
			return false;
		}
	}
	
	public bool AttackHighlighted(){
		Color color = getHighlight();
		if (color.Equals (ATTACKHIGHLIGHT)){
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
