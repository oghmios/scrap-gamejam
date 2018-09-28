using UnityEngine;
using System.Collections;

public class EndZoneLogic: MonoBehaviour {

	private GameLogic gameLogic;
	private BossLogic bossLogic;
	public Transform fxTeleport;
	public AudioManager audioManger;

	private void Start(){
		// bossLogic = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossLogic>();
		gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
	}


	void OnTriggerEnter(Collider other){


		if(other.tag == "Boss"){

			bossLogic.setIdle();

		}

		if(other.tag == "Player"){

			gameLogic.setVictoryResults();
			// audioManger.Play(audioManger.teleportPlayer,other.transform);


			
		}

	}
}
