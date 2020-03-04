using UnityEngine;
using System.Collections;

public class AdjustToScreen : MonoBehaviour {

		public float x;
		public float y;
		public float z;
	
	
	// Use this for initialization
	void Start () {
		Vector3 v3Pos = new Vector3(x, y, z);
		transform.position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ViewportToWorldPoint(v3Pos);
	}
	
 
}
