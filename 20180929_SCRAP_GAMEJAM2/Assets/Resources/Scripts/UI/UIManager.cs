using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text textTutorial;
	public string textTuto;
	// Use this for initialization
	void Start () {

	}
	

	void OnTriggerEnter(Collider other){

		if(other.tag == "Player"){
			textTutorial.text = textTuto;
		}

	}
}
