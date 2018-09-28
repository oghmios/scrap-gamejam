using UnityEngine;
using System.Collections;

public class UpDownMovement : MonoBehaviour {

	public float speed;


	// Use this for initialization
	void Start () {
	
	}
	

		private void Update(){
		transform.Translate(0,Mathf.Lerp(0,0.25f,Time.deltaTime*speed),0);
		}

}
