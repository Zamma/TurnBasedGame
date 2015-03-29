using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	public static Color MOVEHIGHLIGHT = new Color(.09f,.180f,.220f,.7f);
	public static Color CLEAR = new Color(255f,255f,255f,0f);
	public static Color WHITE = new Color(255f,255f,255f,1f);
	public static Color ATTACKHIGHLIGHT = new Color(255f,0f,0f,.5f);
	public static Color ENEMY = new Color(255f,0f,0f,1f);
	public static Color FOG = new Color(0,0,0,.3f);
	public static Color SPELLHIGHLIGHT = new Color(.150f,.40f,.180f,.8f);
	// Use this for initialization
	void Start () {
		//setColor(FOG);
	}

	public void setColor(Color color){
		GetComponent<SpriteRenderer>().color = color;
	}

	public Color getColor(){
		return GetComponent<SpriteRenderer>().color;
	}

}
