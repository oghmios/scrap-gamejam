using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public int scoreGoal;
    public Text textScoreGoal;
    public int currentScore = 0;
    public Text textCurrentScore;

    public BlockGridLogic blockGridLogic;

	public enum GameStates {START, GAME, VICTORY, LOSE }

	public GameStates state;
	private float temp;

	// INTERFACES
	public Transform interfaceGameOver;
    public Transform interfaceVictory;
	private PlayerLogic player;



	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		PlayerPrefs.SetString("Level",Application.loadedLevelName);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
        setStart();
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
        interfaceVictory.gameObject.SetActive(false);

        textScoreGoal.text = "<color=yellow>"+scoreGoal.ToString()+ "</color> GOAL";
        textCurrentScore.text =  currentScore.ToString() + " SCORE";
        state = GameStates.START;
	}

	public void setGame(){

		state = GameStates.GAME;
	}

	public void setVictory(){
		 temp = 0.25f;
        interfaceVictory.gameObject.SetActive(true);
        interfaceGameOver.gameObject.SetActive(false);
        blockGridLogic.SetNone();
        state = GameStates.VICTORY;
	}

	public void setLose(){
        interfaceVictory.gameObject.SetActive(false);
        interfaceGameOver.gameObject.SetActive(true);
        blockGridLogic.SetNone();
        player.setDie();
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
		/*temp -= Time.deltaTime;

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
			
		}*/
	} 

	private void LoseBehaviour(){


	}

    public void AddScore(int blockType) {
        if (blockType == 0)
            currentScore += 25;
        else if (blockType == 1)
            currentScore += 75;
        else if (blockType == 2)
            currentScore += 125;
        else if (blockType == 3)
            currentScore += 250;

        textCurrentScore.text = currentScore.ToString() + " SCORE";

        if (currentScore >= scoreGoal) {
            setVictory();
        }

    }

    public void AddPenalty(int blockType)
    {

        blockGridLogic.SetMove();

    }


}
