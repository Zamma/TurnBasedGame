using UnityEngine;
using System.Collections;

//credit to forum.unity3d.com/threads/click-drag-camera-movement.39513/

public class CameraDrag : MonoBehaviour {
	public float dragSpeed = 5f;
	public Vector3 dragOrigin;
	
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			dragOrigin = Input.mousePosition;
			return;
		}

		if (!Input.GetMouseButton(0)){
			return;
		}

		Vector3 pos = Camera.main.ScreenToViewportPoint(-1*(Input.mousePosition - dragOrigin));
		Vector3 move = new Vector3(pos.x * dragSpeed,pos.y*dragSpeed,0);

		if (pos.x < 0 && this.gameObject.GetComponent<RectTransform>().position.x < 0) return;
		if (pos.y < 0 && this.gameObject.GetComponent<RectTransform>().position.y < 0) return;
		if (pos.x > 0 && this.gameObject.GetComponent<RectTransform>().position.x > Map.WIDTH) return;
		if (pos.y > 0 && this.gameObject.GetComponent<RectTransform>().position.y > Map.HEIGHT) return;

		transform.Translate (move,Space.World);

	}
}
