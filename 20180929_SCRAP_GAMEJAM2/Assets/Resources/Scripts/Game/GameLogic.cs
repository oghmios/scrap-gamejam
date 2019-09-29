using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {

    
    // GAME
	public enum GameStates {START, GAME, PAUSE, VICTORY, VICTORY_ULTIMATE, LOSE, RESULTS }
    [Header("GAME SETTINGS")]
    public GameStates state;
	private float temp;
    public int scoreGoal, scorePerfect;
    public int currentScore = 0;
    public Text textScore, textScoreGoal, textScorePerfect;
    public RectTransform objectScoreGoal;


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
    public MenuLogic menuLogic;
    public Transform interfaceGameplay;
    public Transform interfacePause;
    public Transform interfaceGameOver;
    public Transform interfaceVictory;
    public AudioManager audioManger;
    public PlayerLogic player;
    public MoveCharacter playerMovement;
    public FadeScreen fadeScreen;
    public Text infoText, infoTextPerfect;
    public Image scoreImageMask;
    public Image scoreImage;
    public Image imgStarGoal, imgTrophyPerfect;
    public Image imgBarGoal, imgBarPerfect;
    // private Color colorBarFill = new Color32(26, 219, 0, 255);
    private Color colorBarGoal = new Color32(255, 180, 0, 255);
    private Color colorBarFull = new Color32(255, 230, 0, 255);
    private float animationTime = 2f, currentNumber, initialNumber, desiredNumber;
    private bool getScoreGoal, getScorePerfect;

    // INTERFACES
    [Header("BUZZER TIME")]
    public float slowMotionTime;
    public float timeScaleBuzzerTime;
    public Transform panelBuzzTime;
    public Text textBuzzerTime;
    public Image frameBuzzer;
    private GameObject[] bulletsInTheAir;

    // Use this for initialization
    void Start () {
		Cursor.visible = false;
        // audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        PlayerPrefs.SetString("Level",SceneManager.GetActiveScene().name);
		// player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
        // playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveCharacter>();
        fadeScreen.startFade(false, 0.5f);
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

        case GameStates.VICTORY_ULTIMATE:
                VictoryUltimateBehaviour();
                break;

        case GameStates.RESULTS:
                ResultsBehaviour();
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

        // Animation Score Text
        if (currentNumber != desiredNumber) {

            if (initialNumber < desiredNumber) {
                currentNumber += (animationTime * Time.deltaTime) * (desiredNumber - initialNumber);

                if (currentNumber < scorePerfect && !getScorePerfect)
                {
                    scoreImageMask.fillAmount = (float)currentNumber / scorePerfect;

                    if (currentNumber > scoreGoal && !getScoreGoal)
                    {
                        scoreImage.color = colorBarGoal;
                        textScoreGoal.color = colorBarFull;
                        imgStarGoal.color = colorBarFull;
                        imgBarGoal.color = colorBarFull;
                        imgStarGoal.GetComponent<Animator>().SetTrigger("isIncreased");
                        getScoreGoal = true;
                    }
                    // else if (!getScoreGoal)
                    //    scoreImage.color = colorBarFill;
                }
                else if(!getScorePerfect)
                {
                   scoreImageMask.fillAmount = 1;
                    textScorePerfect.color = colorBarFull;
                    // scoreImage.color = colorBarFull;
                    imgBarPerfect.color = colorBarFull;
                    imgTrophyPerfect.color = colorBarFull;
                    imgTrophyPerfect.GetComponent<Animator>().SetTrigger("isIncreased");
                    getScorePerfect = true;
                }

                if (currentNumber >= desiredNumber)
                    currentNumber = desiredNumber;
            }
        }

        textScore.text = "Score: "+currentNumber.ToString("00"); // +"/"+ scorePerfect.ToString();

        if (state == GameStates.VICTORY || state == GameStates.VICTORY_ULTIMATE) {
            AudioSource[] sounds = Object.FindObjectsOfType<AudioSource>();

            for (int i = 0; i < sounds.Length; i++)
            {

                sounds[i].pitch = timeScaleBuzzerTime*2; // Time.timeScale;

            }
        }

    }

	// SETS
	public void setStart(){
        Time.timeScale = 1;
        


        if (scoreGoal > scorePerfect) {
            scorePerfect = scoreGoal;
        }
        getScorePerfect = false;
        getScoreGoal = false;
        // textScoreGoal.text = "<color=yellow>"+scoreGoal.ToString()+ "</color> GOAL";
        textScore.text = "Score: "+currentScore.ToString(); // + "/"+ scorePerfect.ToString();
        textScoreGoal.text = scoreGoal.ToString();
        textScorePerfect.text = scorePerfect.ToString();
        // new Vector3(((scoreGoal/scorePerfect)*140)+objectScoreGoal.position.x
        objectScoreGoal.localPosition = new Vector3(objectScoreGoal.localPosition.x+ (((float)scoreGoal / (float)scorePerfect) * 140f), objectScoreGoal.localPosition.y, objectScoreGoal.localPosition.z);
        scoreImageMask.fillAmount = (float)currentScore / scorePerfect;
        // scoreImage.color = colorBarFill;

        panelBuzzTime.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(false);
        interfaceGameOver.gameObject.SetActive(false);
        interfaceVictory.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(false);

        // INITIALIZE BLOCKS
        blockGridLogic.setCreateBlocks(numRows, numColumns, numHeavyRows, timeToMoveBlocks);
        blockGridLogic.SetNone();
        
        // INITIALIZE COMBO
        setComboNone();
        temp = 1;
        state = GameStates.START;
	}

    public void setGamePrevStart()
    {
        Time.timeScale = 1;

        menuLogic.prevOption = 0;
        interfacePause.gameObject.SetActive(false);
        infoText.text = "";
        interfaceGameplay.gameObject.SetActive(true);

        blockGridLogic.SetMove();

        state = GameStates.GAME;
    }

    public void setGame(){
        Time.timeScale = 1;

        menuLogic.prevOption = 0;
        interfacePause.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);

        blockGridLogic.SetSleep();
        player.setIdle();
        player.transform.GetComponent<MoveCharacter>().enabled = true;
        player.enabled = true;

        state = GameStates.GAME;
	}

    public void setPause()
    {
        Time.timeScale = 0;
        interfaceGameplay.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(true);
        menuLogic.GotoMainMenu();

        blockGridLogic.SetNone();
        player.setNone();
        player.transform.GetComponent<MoveCharacter>().enabled = false;
        player.enabled = false;

        
        state = GameStates.PAUSE;
    }

    public void setVictory() {

        Time.timeScale = timeScaleBuzzerTime;
        
        temp = slowMotionTime;
        // INCREASE THE SPEED OF THE PLAYER
        playerMovement.speed = playerMovement.speed * 1.5f;
        //playerMovement.jumpSpeed = playerMovement.jumpSpeed * 1.5f;
        playerMovement.gravity = playerMovement.gravity * 1.5f;
        player.tempDig = player.tempDig * 0.5f;
        player.tempDigDown = player.tempDigDown * 0.5f;
        player.tempDigToIdle = player.tempDigToIdle * 0.5f;
        player.animatorCharacter.speed = player.animatorCharacter.speed * 1.5f;
        // player.throwTime = player.throwTime * 0.5f;
        // playerMovement.gravity = player.gravityOrig * player.gravityMultiplier;

        audioManger.Play(audioManger.buzzerScore, transform.position);
        infoText.rectTransform.localPosition = new Vector3(0,175,0);
        infoText.rectTransform.localScale = new Vector3(1, 1, 0);
        infoText.color = colorBarGoal;
        infoText.text = "LEVEL COMPLETE!";

        // SHOW BUZZ TIME
        panelBuzzTime.gameObject.SetActive(true);

        

        blockGridLogic.SetNone();
        // player.enabled = false;

        state = GameStates.VICTORY;
	}

    public void setVictoryUltimate() {
        Time.timeScale = 1f;

        

        // playerMovement.enabled = false;
        //player.setNone();
        temp = 3;
        
        

        state = GameStates.VICTORY_ULTIMATE;
    }

    public void setResults() {

        AudioSource[] sounds = Object.FindObjectsOfType<AudioSource>();

        for (int i = 0; i < sounds.Length; i++)
        {

            sounds[i].pitch = 1; // Time.timeScale;

        }

        if (currentScore >= scorePerfect)
        {
            infoTextPerfect.text = "PERFECT!";
            audioManger.Play(audioManger.playerLaughtLong, transform.position);
        }

        interfaceGameplay.gameObject.SetActive(true);
        interfaceVictory.gameObject.SetActive(true);
        interfaceGameOver.gameObject.SetActive(false);

        state = GameStates.RESULTS;
    }

	public void setLose(){
        interfaceVictory.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);
        interfaceGameOver.gameObject.SetActive(true);
        blockGridLogic.SetNone();

        if (player.state != PlayerLogic.PlayerStates.DIE)
        player.setDie();

        player.transform.GetComponent<MoveCharacter>().enabled = false;
        player.enabled = false;

        temp = 5;

		state = GameStates.LOSE;
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
    private bool aux0, aux1, aux2, aux3;

    private void StartBehaviour(){
       /* if (hInput.GetButtonDown("Cancel_J1"))
        {
            setPause();
        }*/

        temp -= Time.deltaTime;

        if (!aux0) {

            if (temp < 0)
            {
                if (fadeScreen.isFadeOut())
                {
                    // sound.PlayClip(1);
                    // textBehaviour.setActive();
                    infoText.color = Color.red;
                    infoText.text = "READY?";
                    temp = 1;
                    aux0 = true;

                }
            }
        }
        else if (!aux1)
        {
            if (temp < 0)
            {
                infoText.color = Color.yellow;
                infoText.text = "GO!";
                temp = 1f;
                aux1 = true;
                
            }
            
        }
        else if (!aux2)
        {
            if (temp < 0)
            {
                /*music.Play();
                infoText.text = "";
                aux2 = true;
                framesCounter = 0;
                timeGame = timeGameIni;
                timeGameText.text = timeGame.ToString();
                player1.SetMovement();
                player2.SetMovement();
                tower1.SetActive();
                tower2.SetActive();
                powerUpManager.setPlay();
                aux0 = false;
                aux1 = false;
                aux2 = false;
                state = GameState.Game;
                */
                
                setGamePrevStart(); 
                
            }
            // else framesCounter++;
        }
        
    }

	private void GameBehaviour()
    {
        if (hInput.GetButtonDown("Cancel_J1"))
        {
            setPause();
        }
    }

    private void PauseBehaviour()
    {
        if (hInput.GetButtonDown("Cancel_J1"))
        {
            setGame();
        }
    }

    private void CleanEnvironmentVictoryBehaviour(){
		
	}

    private void VictoryBehaviour(){

        temp -= Time.deltaTime;

        textBuzzerTime.text = temp.ToString("00.0");

        if (temp <= 0)
        {

            textBuzzerTime.text = "00.0";
            textBuzzerTime.color = Color.red;
            frameBuzzer.color = Color.red;

            if (playerMovement.isGround)
            {
                player.GetComponent<Rigidbody>().isKinematic = true;
                player.setIdle();
                player.setNone();
            }

            bulletsInTheAir = GameObject.FindGameObjectsWithTag("Bullet");

            if (bulletsInTheAir.Length <= 0 && playerMovement.isGround)
            {
                setVictoryUltimate();
            }
        }
        
    }
    
    private void VictoryUltimateBehaviour() {

        temp -= Time.deltaTime;

        if (temp < 0 && comboCount == 0 && lastContainerLogic ==null && currentNumber == desiredNumber)
        {
            setResults();
            
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

    private void ResultsBehaviour() {
        if (hInput.GetButtonDown("Cancel_J1"))
        {
            setGoToMainMenu();
        }
        if (hInput.GetButtonDown("Submit_J1"))
        {
            setRestartLevel();
        }
    }

    private void LoseBehaviour(){
        if (hInput.GetButtonDown("Cancel_J1"))
        {
            setGoToMainMenu();
        }
        if (hInput.GetButtonDown("Submit_J1"))
        {
            setRestartLevel();
        }

    }

    public void AddScore(int blockType) {

         textScore.GetComponent<Animator>().SetTrigger("isIncreased");

        initialNumber = currentScore;

        if (blockType == 0)
            currentScore += 25;
        else if (blockType == 1)
            currentScore += 75;
        else if (blockType == 2)
            currentScore += 125;
        else if (blockType == 3)
            currentScore += 250;

        desiredNumber = currentScore;

       

        // textScore.text = currentScore.ToString() + "/" + scorePerfect.ToString();

        if (currentScore >= scoreGoal && (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE)) {
            Debug.Log("VICTORY SCORE");
            setVictory();
        }

    }

    public void AddScoreCombo(int scoreCombo)
    {

        textScore.GetComponent<Animator>().SetTrigger("isIncreased");

        initialNumber = currentScore;

        currentScore += scoreCombo;

        desiredNumber = currentScore;

        /*
        if (currentScore < scorePerfect)
        {
            scoreImageMask.fillAmount = (float)currentScore / scorePerfect;
            scoreImage.color = colorBarFill;
        }
        else
        {
            scoreImageMask.fillAmount = 1;
            scoreImage.color = colorBarFull;
        }

        textScore.text = currentScore.ToString() + "/" + scorePerfect.ToString();
        */
        if (currentScore >= scoreGoal && (state!=GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE))
        {
            Debug.Log("VICTORY COMBO");
            setVictory();
            
        }

    }

    public void AddPenalty(int blockType)
    {
        if (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE && state != GameStates.LOSE)
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

        // Debug.Log("COMBO: " + comboCount+" !!");
        comboState = ComboStates.FINISH;
    }

    private void setPlayComboFinish(int comboCountAux) {
        if (comboCountAux == 2)
        {
            
            lastContainerLogic.PlayComboFinish(comboCount, comboScore2);
            AddScoreCombo(comboScore2);
        }
        else if (comboCountAux == 3)
        {
            
            lastContainerLogic.PlayComboFinish(comboCount, comboScore3);
            AddScoreCombo(comboScore3);
        }
        else if (comboCountAux == 4)
        {
            
            lastContainerLogic.PlayComboFinish(comboCount, comboScore4);
            AddScoreCombo(comboScore4);
        }
        else if (comboCountAux > 4)
        {
            
            lastContainerLogic.PlayComboFinish(comboCount, comboScoreMax);
            AddScoreCombo(comboScoreMax);
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
