using UnityEngine;
using System.Collections;

public class SpikeBehaviour : MonoBehaviour {
	private AudioManager audioManger;
    private GameLogic gameLogic;
    private PlayerLogic playerLogic;
	// Use this for initialization
	void Start () {
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
        audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
	}
	
	void OnTriggerEnter(Collider other){
		
		if(other.tag == "Player"){
            playerLogic.setDie();
            /*
            audioManger.Play(audioManger.destroyPlayer,other.transform.position);
			GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
			Destroy(explosionBossAux,0.5f);
			// gameLogic.setDestroyBossVictory();
			Destroy(transform.parent.gameObject);
            */
		}

        if (other.tag == "Block")
        {
            gameLogic.setLose();


        }

    }


}
