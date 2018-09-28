using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public enum GameStates {DESTROY_BOSS, DESTROY_BOSS_VICTORY, CLEAN_ENVIRONMENT, CLEAN_ENVIRONMENT_VICTORY, VICTORY_RESULTS, LOSE }

	public GameStates state;
	private float temp;

	// INTERFACES
	public Transform interfaceGameOver;

	// public Transform ubicationBoss;
	// public Transform ubicationPortal;
	// public Transform textUbicationBoss;
	// private float distanceBoss;
	// private float distanceBossIni;
	private Transform player;



	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		PlayerPrefs.SetString("Level",Application.loadedLevelName);
		player = GameObject.FindGameObjectWithTag("Player").transform;
		// textCountPiece.gameObject.SetActive(false);
		// textTime.gameObject.SetActive(false);
		// pieceCountBoss = 0;
		// setDestroyBoss();

	}
	
	// Update is called once per frame
	void Update () {
	
		switch(state){

		case GameStates.DESTROY_BOSS:
			DestroyBossBehaviour();
			break;
		
		case GameStates.DESTROY_BOSS_VICTORY:
			DestroyBossVictoryBehaviour();
			break;

		case GameStates.CLEAN_ENVIRONMENT_VICTORY:
			CleanEnvironmentVictoryBehaviour();
			break;

		case GameStates.VICTORY_RESULTS:
			VictoryResultsBehaviour();
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
	public void setDestroyBoss(){

		interfaceGameOver.gameObject.SetActive(false);

		temp = 4;
		/*textMission.gameObject.SetActive(true);
		textMission.GetComponent<TextMesh>().text = "DESTROY THE BOSS BEFORE\nIT REACHES THE PORTAL!";
		textMission.GetComponent<TextMesh>().text.Replace("\\n","");
		textUbicationBoss.gameObject.SetActive(true);
		distanceBoss = ubicationPortal.position.x-ubicationBoss.position.x;
		distanceBossIni = ubicationBoss.position.x;
		Debug.Log ("POSICION"+textUbicationBoss.localPosition.x);*/
		state = GameStates.DESTROY_BOSS;
	}

	public void setDestroyBossVictory(){

		interfaceGameOver.gameObject.SetActive(false);

		/*textUbicationBoss.gameObject.SetActive(false);
		temp = 0.5f;
		textMission.gameObject.SetActive(true);
		textMission.GetComponent<TextMesh>().text = "GOOD!! BUT...";
		textMission.GetComponent<TextMesh>().characterSize =  0.3f;
		pieceCountBoss = GameObject.FindGameObjectsWithTag("BossPiece").Length;
		textCountPiece.gameObject.SetActive(true);
		textCountPiece.GetComponent<TextMesh>().text = pieceCountBoss.ToString();*/

		state = GameStates.DESTROY_BOSS_VICTORY;
	}

	public void setCleanEnvironment(){
		/*interfaceGameplay.gameObject.SetActive(true);
		interfaceGameOver.gameObject.SetActive(false);
		textUbicationBoss.gameObject.SetActive(false);
		textMission.gameObject.SetActive(true);
		textMission.GetComponent<TextMesh>().text = "WHAT DO WE DO NOW?? \n collect the remains of Boss!";
		textMission.GetComponent<TextMesh>().characterSize = 0.3f;
		textMission.GetComponent<TextMesh>().text.Replace("\\n","");*/

		// textTime.gameObject.SetActive(true);
		temp = 20;
		// textTime.GetComponent<TextMesh>().text = temp.ToString(); 
		// textTime.GetComponent<TextMesh>().text = ((temp).ToString("##"+"."+".#"));
		state = GameStates.CLEAN_ENVIRONMENT;
	}

	public void setCleanEnvironmentVictory(){
		/* textUbicationBoss.gameObject.SetActive(false);
		textTime.gameObject.SetActive(false);
		textCountPiece.gameObject.SetActive(false);

		interfaceGameplay.gameObject.SetActive(true);
		interfaceGameOver.gameObject.SetActive(false);

		textMission.gameObject.SetActive(true);
		textMission.GetComponent<TextMesh>().text = "GOOD!!\nNOW PASS TO THE PORTAL!\nNEW BOSSES ARE COMING...";
		textMission.GetComponent<TextMesh>().text.Replace("\\n",""); */

		state = GameStates.CLEAN_ENVIRONMENT_VICTORY;
	}

	public void setVictoryResults(){
		 temp = 0.25f;

		interfaceGameOver.gameObject.SetActive(false);

		state = GameStates.VICTORY_RESULTS;
	}

	public void setLose(){

		interfaceGameOver.gameObject.SetActive(true);
		
		temp = 5;


		// if(player!=null)
		//	player.gameObject.SetActive(false);

		state = GameStates.LOSE;
	}

	// BEHAVIOURS

	private void DestroyBossBehaviour(){

		// MEDIA WAPA
		/* textUbicationBoss.localPosition = Vector3.Lerp(new Vector3(0, textUbicationBoss.transform.localPosition.y, textUbicationBoss.transform.localPosition.z), 

		                                                                               new Vector3(9.19f,
		                                                                               textUbicationBoss.transform.localPosition.y,
		                                               	                                textUbicationBoss.transform.localPosition.z), 
		                                               									ubicationBoss.transform.position.x/distanceBoss);

	
	*/

	}

	private void DestroyBossVictoryBehaviour(){
		temp -= Time.deltaTime;
		if(temp<0){
			setCleanEnvironment();
		}
	}

	private void CleanEnvironmentVictoryBehaviour(){
		
	}

	private void VictoryResultsBehaviour(){
		temp -= Time.deltaTime;
		/*
		if(temp<1.25f){
			if(player!=null)
				player.gameObject.SetActive(false);
		} */

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
			//Application.LoadLevel(Application.loadedLevelName)
		}
	} 

	private void LoseBehaviour(){
		/*temp -= Time.deltaTime;
		if(temp<0){
			Application.LoadLevel("Level1");
		}*/


	}



}
