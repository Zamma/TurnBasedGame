using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

public class ResourceCounter : MonoBehaviour {

	public string type;
	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = Grid.turnManager.player.getInfo(type);
	}
}
