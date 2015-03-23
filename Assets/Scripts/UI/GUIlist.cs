using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GUIlist : MonoBehaviour {
	public List<ActionButton> actions;


	public void addAction(Action action){
		Transform trans = GetComponent<Transform>();
		ActionButton button = Instantiate(Grid.prefabLoader.actionButton,new Vector3(trans.position.x + .5f,trans.position.y - actions.Count*.5f,0),Quaternion.identity) as ActionButton;
		button.transform.SetParent(transform,true);

		button.setAction(action);

		actions.Add(button);
	}
}
