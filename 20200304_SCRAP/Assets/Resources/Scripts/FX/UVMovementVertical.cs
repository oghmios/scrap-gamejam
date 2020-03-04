using UnityEngine;
using System.Collections;

public class UVMovementVertical : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0,speed*Time.deltaTime);

	}
}
