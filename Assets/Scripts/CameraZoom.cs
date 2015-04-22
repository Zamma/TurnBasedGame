using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class CameraZoom : MonoBehaviour {

	public static float ZOOMSENSITIVITY = .35f;
	public static float ZOOMSMOOTHING = .1f;

	// Update is called once per frame
	void Update () {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		Grid.map.transform.localScale += new Vector3(ZOOMSENSITIVITY*scroll,ZOOMSENSITIVITY*scroll,0);
	}
}
