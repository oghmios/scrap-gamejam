using UnityEngine;
using System.Collections;

public class SpikeBehaviour : MonoBehaviour {
	private AudioManager audioManger;
	public Transform explosionBoss;
    private GameLogic gameLogic;
	// Use this for initialization
	void Start () {
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
	}
	
	void OnTriggerEnter(Collider other){
		
		if(other.tag == "Player"){

			audioManger.Play(audioManger.destroyPlayer,other.transform.position);
			GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
			Destroy(explosionBossAux,0.5f);
			// gameLogic.setDestroyBossVictory();
			Destroy(transform.parent.gameObject);
		}

        if (other.tag == "Block")
        {
            gameLogic.setLose();


        }

    }


}
