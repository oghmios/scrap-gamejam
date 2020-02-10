using UnityEngine;
using System.Collections;

public class SpikeBehaviour : MonoBehaviour {
	//private AudioManager audioManger;
    public GameLogic gameLogic;
    public PlayerLogic playerLogic;
	// Use this for initialization
	void Start () {
        // gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        // playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
        //audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
	}
	
	void OnTriggerEnter(Collider other){
		
		if(other.tag == "Player" && playerLogic.state!= PlayerLogic.PlayerStates.DIE)
        {
            // gameLogic.setLose();
            playerLogic.setDie(0);
        }

        if (other.tag == "Block")
        {
            gameLogic.setLose(1);

        }

    }


}
