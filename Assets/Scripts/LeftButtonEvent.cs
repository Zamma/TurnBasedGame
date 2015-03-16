using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;


//To use: in the unity editor, go to component/event/LeftButtonEvent
//Credit to jenci1990 at http://answers.unity3d.com/questions/879156/how-would-i-detect-a-right-click-with-an-event-tri.html
[ExecuteInEditMode]
[AddComponentMenu("Event/LeftButtonEvent")]
public class LeftButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[System.Serializable]public class LeftButton : UnityEvent{}
	public LeftButton onLeftDown;
	public LeftButton onLeftUp;
	private bool isOver = false;
	void Start () {
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && isOver) {
			onLeftDown.Invoke();
		}
		if (Input.GetMouseButtonUp(0) && isOver) {
			onLeftUp.Invoke();
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData) {
		isOver = true;
	}
	
	public void OnPointerExit(PointerEventData eventData) {
		isOver = false;
	}
}