using UnityEngine;
using System.Collections;

public class MenuLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Jump"))
			Application.LoadLevel ("Level 1");

		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}
}
