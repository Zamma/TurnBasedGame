using UnityEngine;
using System.Collections;

//class to make sure the state does not update more than once per frame.

public class CurrentAction : MonoBehaviour {
	public string action;
	public bool updated;
	// Use this for initialization
	void Start () {
		action = "neutral";
		updated = false;
	}
	
	// Update is called once per frame
	void Update () {
		updated = false;
	}

	public void set(string newaction){
		if (!updated){
			action = newaction;
			updated = true;
		}
	}

	public bool equals(string check){
		if (action.Equals(check) && !updated){
			return true;
		}
		else {
			return false;
		}
	}
}
