using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {

    
    // GAME
	public enum GameStates {START, GAME, PAUSE, VICTORY, LOSE }
    [Header("GAME SETTINGS")]
    public AudioManager audioManger;
    public AudioSource audioMusic;
    private bool switchMusic = true;
    public GameStates state;
	private float temp;
    public int scoreGoal;
    public int currentScore = 0;
    public Text textScore;


    // BLOCKS
    [Header("BLOCKS SETTINGS")]
    public BlockGridLogic blockGridLogic;
    public int numRows, numColumns, numHeavyRows;
    public float timeToMoveBlocks;

    // COMBO
    public enum ComboStates { NONE, START, CONTINUE, FINISH }
    [Header("COMBO SETTINGS")]
    public ComboStates comboState;
    public float tempCombo;
    public float tempIniCombo;
    private int comboCount;
    private ContainerLogic lastContainerLogic;
    public int comboScore2 = 10, comboScore3 = 25, comboScore4 = 50, comboScoreMax = 100;

    // INTERFACES
    [Header("INTERFACE SETTINGS")]
    public Transform interfacePause;
    public Transform interfaceGameOver;
    public Transform interfaceVictory;
    public Text textAudio;
	private PlayerLogic player;
    public Image scoreImageMask;
    public Image scoreImage;
    private Color colorBarFill = new Color32(26, 219, 0, 255);
    private Color colorBarFull = new Color32(255, 237, 0, 255);


    // Use this for initialization
    void Start () {
		Cursor.visible = false;
        audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        PlayerPrefs.SetString("Level",SceneManager.GetActiveScene().name);
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

        case GameStates.PAUSE:
            PauseBehaviour();
            break;

        case GameStates.VICTORY:
		    VictoryBehaviour();
			break;

		case GameStates.LOSE:
			LoseBehaviour();
			break;

		}

        switch (comboState)
        {

            case ComboStates.NONE:
                ComboNoneBehaviour();
                break;

            case ComboStates.START:
                ComboStartBehaviour();
                break;

            case ComboStates.CONTINUE:
                ComboContinueBehaviour();
                break;

            case ComboStates.FINISH:
                ComboFinishBehaviour();
                break;

        }

        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(PlayerPrefs.GetString("Level"));
		} 

	}

	// SETS
	public void setStart(){
        Time.timeScale = 1;
        interfacePause.gameObject.SetActive(false);
        interfaceGameOver.gameObject.SetActive(false);
        interfaceVictory.gameObject.SetActive(false);

        // textScoreGoal.text = "<color=yellow>"+scoreGoal.ToString()+ "</color> GOAL";
        textScore.text =  currentScore.ToString() + "/"+ scoreGoal.ToString();
        scoreImageMask.fillAmount = (float)currentScore / scoreGoal;
        scoreImage.color = colorBarFill;

        // INITIALIZE BLOCKS
        blockGridLogic.setCreateBlocks(numRows, numColumns, numHeavyRows, timeToMoveBlocks);

        // INITIALIZE COMBO
        setComboNone();

        state = GameStates.START;
	}

	public void setGame(){
        Time.timeScale = 1;
        interfacePause.gameObject.SetActive(false);
        blockGridLogic.SetSleep();
        player.setIdle();
        player.transform.GetComponent<MoveCharacter>().enabled = true;
        player.enabled = true;

        state = GameStates.GAME;
	}

    public void setPause()
    {
        Time.timeScale = 0;
        interfacePause.gameObject.SetActive(true);
        blockGridLogic.SetNone();
        player.setNone();
        player.transform.GetComponent<MoveCharacter>().enabled = false;
        player.enabled = false;
        state = GameStates.PAUSE;
    }

    public void setVictory(){
		 temp = 0.25f;
        interfaceVictory.gameObject.SetActive(true);
        interfaceGameOver.gameObject.SetActive(false);
        audioManger.Play(audioManger.playerLaughtLong, transform.position);
        blockGridLogic.SetNone();
        player.setNone();
        state = GameStates.VICTORY;
	}

	public void setLose(){
        interfaceVictory.gameObject.SetActive(false);
        interfaceGameOver.gameObject.SetActive(true);
        blockGridLogic.SetNone();

        if (player.state != PlayerLogic.PlayerStates.DIE)
        player.setDie();

        player.transform.GetComponent<MoveCharacter>().enabled = false;
        player.enabled = false;

        temp = 5;

		state = GameStates.LOSE;
	}

    public void setSwitchMusicOnOff() {
        switchMusic = !switchMusic;

        if (switchMusic)
        {
            audioMusic.Play();
            textAudio.text = "AUDIO ON";
        }
        else
        {
            audioMusic.Stop();
            textAudio.text = "AUDIO OFF";
        }
    }

    public void setGoToMainMenu()
    {
        // Application.LoadLevel("Menu");
        SceneManager.LoadScene("Menu");
    }

    public void setRestartLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Level"));
        // Application.LoadLevel(PlayerPrefs.GetString("Level"));
    }

    // BEHAVIOURS

    private void StartBehaviour(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setPause();
        }
    }

	private void GameBehaviour()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            setPause();
        }
    }

    private void PauseBehaviour()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setGame();
        }
    }

    private void CleanEnvironmentVictoryBehaviour(){
		
	}

	private void VictoryBehaviour(){


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setPause();
        }
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setPause();
        }

    }

    public void AddScore(int blockType) {

         textScore.GetComponent<Animator>().SetTrigger("isIncreased");


        if (blockType == 0)
            currentScore += 25;
        else if (blockType == 1)
            currentScore += 75;
        else if (blockType == 2)
            currentScore += 125;
        else if (blockType == 3)
            currentScore += 250;

        if (currentScore < scoreGoal)
        {
            scoreImageMask.fillAmount = (float)currentScore / scoreGoal;
            scoreImage.color = colorBarFill;
        }
        else {
            scoreImageMask.fillAmount = 1;
            scoreImage.color = colorBarFull;
        }

        textScore.text = currentScore.ToString() + "/" + scoreGoal.ToString();
        if (currentScore >= scoreGoal) {
            setVictory();
        }

    }

    public void AddScoreCombo(int scoreCombo)
    {

        textScore.GetComponent<Animator>().SetTrigger("isIncreased");

        currentScore += scoreCombo;

        if (currentScore < scoreGoal)
        {
            scoreImageMask.fillAmount = (float)currentScore / scoreGoal;
            scoreImage.color = colorBarFill;
        }
        else
        {
            scoreImageMask.fillAmount = 1;
            scoreImage.color = colorBarFull;
        }

        textScore.text = currentScore.ToString() + "/" + scoreGoal.ToString();
        if (currentScore >= scoreGoal)
        {
            setVictory();
        }

    }

    public void AddPenalty(int blockType)
    {

        blockGridLogic.SetMove();

    }

    // COMBO SET STATES

    public void setComboNone() {
        tempCombo = 0;
        comboCount = 0;

       // if (lastContainerLogic!=null)
       // lastContainerLogic.ResetContainer();

        lastContainerLogic = null;
        comboState = ComboStates.NONE;
    }

    public void setComboStart()
    {
        tempCombo = tempIniCombo;
        comboCount = 1;
        comboState = ComboStates.START;
    }

    public void setComboContinue(ContainerLogic containerLogicAux)
    {
        tempCombo = tempIniCombo;
        comboCount += 1;
        lastContainerLogic = containerLogicAux;
        if (comboCount >= 2) {
            lastContainerLogic.PlayCombo(comboCount);
        }

        comboState = ComboStates.CONTINUE;
    }

    public void setComboFinish()
    {
        

        if (comboCount >= 2 && lastContainerLogic!=null) {
            setPlayComboFinish(comboCount);
        }

        Debug.Log("COMBO: " + comboCount+" !!");
        comboState = ComboStates.FINISH;
    }

    private void setPlayComboFinish(int comboCountAux) {
        if (comboCountAux == 2)
        {
            AddScoreCombo(comboScore2);
            lastContainerLogic.PlayComboFinish(comboCount, comboScore2);
        }
        else if (comboCountAux == 3)
        {
            AddScoreCombo(comboScore3);
            lastContainerLogic.PlayComboFinish(comboCount, comboScore3);
        }
        else if (comboCountAux == 4)
        {
            AddScoreCombo(comboScore4);
            lastContainerLogic.PlayComboFinish(comboCount, comboScore4);
        }
        else if (comboCountAux > 4)
        {
            AddScoreCombo(comboScoreMax);
            lastContainerLogic.PlayComboFinish(comboCount, comboScoreMax);
        }
    }

    // CHECKS COMBO STATES

    public bool IsComboNone()
    {
        if (comboState == ComboStates.NONE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsComboStart()
    {
        if (comboState == ComboStates.START)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsComboContinue()
    {
        if (comboState == ComboStates.CONTINUE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsComboFinish()
    {
        if (comboState == ComboStates.FINISH)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // COMBO BNEHAVIOUR STATES

    private void ComboNoneBehaviour() {

    }

    private void ComboStartBehaviour() {
        tempCombo -= Time.deltaTime;

        if (tempCombo < 0)
            setComboFinish();
    }

    private void ComboContinueBehaviour() {
        tempCombo -= Time.deltaTime;

        if (tempCombo < 0)
            setComboFinish();
    }

    private void ComboFinishBehaviour() {

        setComboNone();
    }

}
