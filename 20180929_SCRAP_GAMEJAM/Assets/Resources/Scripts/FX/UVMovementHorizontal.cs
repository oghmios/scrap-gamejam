using UnityEngine;
using System.Collections;

public class UVMovementHorizontal : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		GetComponent<Renderer>().material.mainTextureOffset += new Vector2(speed*Time.deltaTime,0);

	}
}
