﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Jump") || (Input.GetKey(KeyCode.Escape)))
            SceneManager.LoadScene("Menu");
	}
}
