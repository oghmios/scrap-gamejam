using UnityEngine;
using System.Collections;

public class SpikeBehaviour : MonoBehaviour {
    public GameLogic gameLogic;
	
	void OnTriggerEnter(Collider other){
		
		if(other.tag == "Player" && other.GetComponent<PlayerLogic>().state!= PlayerLogic.PlayerStates.DIE)
        {
            other.GetComponent<PlayerLogic>().setDie(0);
        }

        if (other.tag == "Block")
        {
            gameLogic.setLose(1);

        }

    }


}
