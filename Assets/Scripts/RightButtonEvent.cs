using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;


//To use: in the unity editor, go to component/event/RightButtonEvent
//Credit to jenci1990 at http://answers.unity3d.com/questions/879156/how-would-i-detect-a-right-click-with-an-event-tri.html
[ExecuteInEditMode]
[AddComponentMenu("Event/RightButtonEvent")]
public class RightButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[System.Serializable]public class RightButton : UnityEvent{}
	public RightButton onRightDown;
	public RightButton onRightUp;
	private bool isOver = false;
	void Start () {
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(1) && isOver) {
			onRightDown.Invoke();
		}
		if (Input.GetMouseButtonUp(1) && isOver) {
			onRightUp.Invoke();
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData) {
		isOver = true;
	}
	
	public void OnPointerExit(PointerEventData eventData) {
		isOver = false;
	}
}
