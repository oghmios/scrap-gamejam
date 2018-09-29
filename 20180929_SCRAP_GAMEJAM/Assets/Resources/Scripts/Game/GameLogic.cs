using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public enum GameStates {START, GAME, VICTORY, LOSE }

	public GameStates state;
	private float temp;

	// INTERFACES
	public Transform interfaceGameOver;
	private Transform player;



	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		PlayerPrefs.SetString("Level",Application.loadedLevelName);
		player = GameObject.FindGameObjectWithTag("Player").transform;

	}
	
	// Update is called once per frame
	void Update () {
	
		switch(state){

		case GameStates.START:
			StartBehaviour();
			break;
		
		case GameStates.GAME:
			GameBehaviour();
			break;

		case GameStates.VICTORY:
			VictoryBehaviour();
			break;

		case GameStates.LOSE:
			LoseBehaviour();
			break;

		}

		if(Input.GetKeyDown(KeyCode.R)){
			Application.LoadLevel(PlayerPrefs.GetString("Level"));
		} else if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel("Menu");
		}

	}

	// SETS
	public void setStart(){

		interfaceGameOver.gameObject.SetActive(false);

		state = GameStates.START;
	}

	public void setGame(){

		state = GameStates.GAME;
	}

	public void setVictory(){
		 temp = 0.25f;

		interfaceGameOver.gameObject.SetActive(false);

		state = GameStates.VICTORY;
	}

	public void setLose(){

		interfaceGameOver.gameObject.SetActive(true);
		
		temp = 5;

		state = GameStates.LOSE;
	}

	// BEHAVIOURS

	private void StartBehaviour(){

	}

	private void GameBehaviour(){
		
	}

	private void CleanEnvironmentVictoryBehaviour(){
		
	}

	private void VictoryBehaviour(){
		temp -= Time.deltaTime;

		if(temp<0){

			string level = PlayerPrefs.GetString("Level");
			string[] cadenas = level.Split(" "[0]);
			// Parse.int(cadenas[1]);
			Debug.Log (cadenas[1]);
			int contLevel =  int.Parse(cadenas[1])+1;
			if(contLevel<=5){
				Application.LoadLevel("Level "+(contLevel).ToString());
			} else {
				Application.LoadLevel("Menu");
			}
			
		}
	} 

	private void LoseBehaviour(){


	}



}
