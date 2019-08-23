using UnityEngine;
using System.Collections;

public class SpikeBehaviour : MonoBehaviour {
	//private AudioManager audioManger;
    private GameLogic gameLogic;
    //private PlayerLogic playerLogic;
	// Use this for initialization
	void Start () {
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        // playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
        //audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
	}
	
	void OnTriggerEnter(Collider other){
		
		if(other.tag == "Player"){
            gameLogic.setLose();
            //playerLogic.setDie();
        }

        if (other.tag == "Block")
        {
            gameLogic.setLose();


        }

    }


}
