using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

public class ActionButton : MonoBehaviour {
	public Action action;

	public void setAction(Action a){
		action = a;

		GetComponent<Text>().text = action.type;
	}

	public void clickedOn(){
		Grid.controller.startingAction(action);
		Destroy(transform.parent.gameObject);
	}

}
