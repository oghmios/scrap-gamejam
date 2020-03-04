using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameLogic : MonoBehaviour {

    [Header("PLAYERS")]
    public PlayerLogic player1;
 
    public MoveCharacter player1Movement;

    public enum GameStates {START, GAME, PAUSE, VICTORY, VICTORY_ULTIMATE, LOSE, RESULTS }
    [Header("GAME SETTINGS")]
    public GameStates state;
    protected float temp;
    public int scoreGoal, scorePerfect;
    public int currentScore = 0;
    public Text textScore, textScoreGoal, textScorePerfect;
    public RectTransform objectScoreGoal;
    public bool challengeBlocks;

    public EventSystem eventsystem;
    public GameObject buttonVictoryNextLevel, buttonLoseRestart;

    // COMBO SYSTEM
    public ComboSystemLogic comboSystem;

    // MAIN SCORE
    [Header("MAIN SCORE")]
    public int scoreFlesh;
    public int scoreWeapon;
    public int scoreArmor;
    public int scoreCoin;
    public int scoreHitsBird;

    // BLOCKS
    [Header("BLOCKS SETTINGS")]
    public BlockGridLogic blockGridLogic;
    public int numRows, numColumns, numChallengeRows, numHeavyRows;
    public float timeToMoveBlocks;

    // INTERFACES
    [Header("BUZZER TIME")]
    public float slowMotionTime;
    public float timeScaleBuzzerTime;
    public Transform panelBuzzTime;
    public Text textBuzzerTime;
    public Image frameBuzzer;
    protected GameObject[] bulletsInTheAir;
    protected Transform myTransform;

    // INTERFACES
    [Header("INTERFACE SETTINGS")]
    public MenuLogic menuLogic;
    public Transform interfaceGameplay;
    public Transform interfacePause;
    public Transform interfaceGameOver;
    public Transform interfaceVictory;
    public FadeScreen fadeScreen;
    public Text infoText, infoTextPerfect, textSecretScore, TextChallengeBlockFail;
    public Image scoreImageMask;
    public Image scoreImage;
    public Image imgStarGoal, imgTrophyPerfect;
    public Image imgBarGoal, imgBarPerfect, imgJewelSecret, imgCrossChallenge;
    public Animator wallBlocksAnim;
    public ParticleSystem psWallBlocks;
    public int blocksRemaining;
    // private Color colorBarFill = new Color32(26, 219, 0, 255);
    protected Color colorBarGoal = new Color32(255, 180, 0, 255);
    protected Color colorBarGoalScore = new Color32(255, 200, 0, 255);
    protected Color colorBarFull = new Color32(255, 230, 0, 255);
    protected Color colorBarFullScore = new Color32(255, 250, 0, 255);
    protected float animationTime = 2f, currentNumber, initialNumber, desiredNumber;
    protected bool getScoreGoal, getScorePerfect;
    protected bool isFinishRestart = false, isFinishNext = false;
    public Image ImgVictoryPerfect, ImgVictoryPajaroto, ImgVictoryChallengeBlocks, ImgVictorySlotPerfect, ImgVictorySlotPajaroto, ImgVictorySlotChallengeBlocks;

    // Use this for initialization
    void Start () {

        myTransform = this.transform;
        Cursor.visible = false;
        PlayerPrefs.SetString("Level",SceneManager.GetActiveScene().name);
        fadeScreen.startFade(false, 0.5f);
        setStart();
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
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

        // Animation Score Text
        if (currentNumber != desiredNumber) {

            if (initialNumber < desiredNumber) {
                currentNumber += (animationTime * Time.deltaTime) * (desiredNumber - initialNumber);

                if (currentNumber < scorePerfect && !getScorePerfect)
                {
                    scoreImageMask.fillAmount = (float)currentNumber / scorePerfect;

                    if (currentNumber > scoreGoal && !getScoreGoal)
                    {
                        scoreImage.color = colorBarGoalScore;
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
                    scoreImage.color = colorBarFullScore;
                    imgBarPerfect.color = colorBarFull;
                    imgTrophyPerfect.color = colorBarFull;
                    imgTrophyPerfect.GetComponent<Animator>().SetTrigger("isIncreased");
                    getScorePerfect = true;
                }

                if (currentNumber >= desiredNumber)
                    currentNumber = desiredNumber;
            }
        }

        textScore.text = currentNumber.ToString("00"); // +"/"+ scorePerfect.ToString();

        if (state == GameStates.VICTORY || state == GameStates.VICTORY_ULTIMATE) {
            AudioSource[] sounds = Object.FindObjectsOfType<AudioSource>();

            for (int i = 0; i < sounds.Length; i++)
            {

                sounds[i].pitch = timeScaleBuzzerTime*2; // Time.timeScale;

            }
        }

    }

	// SETS
	public virtual void setStart(){
        Time.timeScale = 1;

        if (scoreGoal > scorePerfect) {
            scorePerfect = scoreGoal;
        }

        isFinishRestart = false;
        isFinishNext = false;

        getScorePerfect = false;
        getScoreGoal = false;

        if (!interfaceGameplay.gameObject.activeSelf)
            interfaceGameplay.gameObject.SetActive(true);
        // textScoreGoal.text = "<color=yellow>"+scoreGoal.ToString()+ "</color> GOAL";
        textScore.text = currentScore.ToString(); // + "/"+ scorePerfect.ToString();
        textScoreGoal.text = scoreGoal.ToString();
        textScorePerfect.text = scorePerfect.ToString();
        // new Vector3(((scoreGoal/scorePerfect)*140)+objectScoreGoal.position.x
        objectScoreGoal.localPosition = new Vector3(objectScoreGoal.localPosition.x+ (((float)scoreGoal / (float)scorePerfect) * 140f), objectScoreGoal.localPosition.y, objectScoreGoal.localPosition.z);
        scoreImageMask.fillAmount = (float)currentScore / scorePerfect;
        // scoreImage.color = colorBarFill;

        challengeBlocks = false;

        panelBuzzTime.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(false);
        interfaceGameOver.gameObject.SetActive(false);
        interfaceVictory.gameObject.SetActive(false);
        

        // INITIALIZE BLOCKS
        blockGridLogic.setCreateBlocks(numRows, numColumns, numChallengeRows, numHeavyRows, timeToMoveBlocks);
        blockGridLogic.SetNone();
        
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

    public virtual void setGame(){
        Time.timeScale = 1;

        menuLogic.prevOption = 0;
        interfacePause.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);

        if (blockGridLogic.state != BlockGridLogic.BlockGridLogicStates.STRESS_MODE)
            blockGridLogic.SetSleep();
        else
            blockGridLogic.SetStressMode();
        player1.setIdle();
        player1Movement.enabled = true;
        player1.enabled = true;

        state = GameStates.GAME;
	}

    public virtual void setPause()
    {

        Time.timeScale = 0;
        interfaceGameplay.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(true);
        menuLogic.GotoMainMenu();

        blockGridLogic.SetNone();
        player1.setNone();
        player1Movement.enabled = false;
        player1.enabled = false;

        state = GameStates.PAUSE;
    }

    public virtual void setVictory() {

        Time.timeScale = timeScaleBuzzerTime;
        
        temp = slowMotionTime;
        // INCREASE THE SPEED OF THE PLAYER
        player1Movement.speed = player1Movement.speed * 1.5f;
        player1Movement.gravity = player1Movement.gravity * 1.5f;
        player1.tempDig = player1.tempDig * 0.5f;
        player1.tempDigDown = player1.tempDigDown * 0.5f;
        player1.tempDigToIdle = player1.tempDigToIdle * 0.5f;
        player1.animatorCharacter.speed = player1.animatorCharacter.speed * 1.5f;

        CoreManager.Audio.Play(CoreManager.Audio.buzzerScore, myTransform.position);
        /*
        infoText.rectTransform.localPosition = new Vector3(0,175,0);
        infoText.rectTransform.localScale = new Vector3(1, 1, 0);
        infoText.color = colorBarGoal;
        infoText.text = "LEVEL COMPLETE!";
        */
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

    public virtual void setResults() {

        AudioSource[] sounds = Object.FindObjectsOfType<AudioSource>();

        for (int i = 0; i < sounds.Length; i++)
        {

            sounds[i].pitch = 1; // Time.timeScale;

        }

        panelBuzzTime.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);
        interfaceVictory.gameObject.SetActive(true);
        interfaceGameOver.gameObject.SetActive(false);

        if (currentScore >= scorePerfect)
        {
            ImgVictoryPerfect.enabled = true;
            ImgVictoryPerfect.color = Color.white;
            ImgVictorySlotPerfect.color = Color.blue;
            ImgVictoryPerfect.GetComponent<Animator>().SetTrigger("IsIncreased");
        }

        if (imgJewelSecret != null && imgJewelSecret.enabled) {
            ImgVictoryPajaroto.enabled = true;
            ImgVictoryPajaroto.color = Color.white;
            ImgVictorySlotPajaroto.color = Color.blue;
            ImgVictoryPajaroto.GetComponent<Animator>().SetTrigger("IsIncreased");
        }

        if (!challengeBlocks && ImgVictoryChallengeBlocks!=null && ImgVictorySlotChallengeBlocks!=null) {
            ImgVictoryChallengeBlocks.enabled = true;
            ImgVictoryChallengeBlocks.color = Color.white;
            ImgVictorySlotChallengeBlocks.color = Color.blue;
            ImgVictoryChallengeBlocks.GetComponent<Animator>().SetTrigger("IsIncreased");
        }

        if (currentScore >= scorePerfect && imgJewelSecret!=null && imgJewelSecret.enabled && !challengeBlocks)
        {
            infoTextPerfect.text = "PERFECT!";
            switch (Random.Range(0, 2))
            {
                case 0:
                    CoreManager.Audio.Play(CoreManager.Audio.playerLaughtLong01, myTransform.position);
                    break;
                case 1:
                    CoreManager.Audio.Play(CoreManager.Audio.playerLaughtLong02, myTransform.position);
                    break;
            }

        }
        else
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    CoreManager.Audio.Play(CoreManager.Audio.playerLaughtMedium01, myTransform.position);
                    break;
                case 1:
                    CoreManager.Audio.Play(CoreManager.Audio.playerLaughtMedium02, myTransform.position);
                    break;
            }
        }

        temp = 3;
        eventsystem.SetSelectedGameObject(buttonVictoryNextLevel);

        state = GameStates.RESULTS;
    }

	public virtual void setLose(int loseMode){
        temp = 4;

        interfaceVictory.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);
        interfaceGameOver.gameObject.SetActive(true);
        blockGridLogic.SetNone();

        if (player1.state != PlayerLogic.PlayerStates.DIE)
        player1.setDie(loseMode);

        player1Movement.enabled = false;
        player1.enabled = false;

            // BLOCKS TOUCH SPIKES == 1
            // Particle System blocks
            // BlockWall Movement down to up
            if (loseMode == 1)
            {
                psWallBlocks.Play();
                CameraShake.Shake(Vector3.one * 3, 2f);
                CoreManager.Audio.Play(CoreManager.Audio.explosionStones, myTransform.position, 2);
                wallBlocksAnim.SetTrigger("isUp");
            }

        eventsystem.SetSelectedGameObject(buttonLoseRestart);

        state = GameStates.LOSE;
	}

    public void setGoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void setRestartLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Level"));
    }

    // BEHAVIOURS
    protected bool aux0, aux1, aux2, aux3;

    protected void StartBehaviour(){
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

    protected void GameBehaviour()
    {
        if (hInput.GetButtonDown("Cancel_J1"))
        {
            setPause();
        }
    }

    protected void PauseBehaviour()
    {
        if (hInput.GetButtonDown("Cancel_J1"))
        {
            setGame();
        }
    }

    protected void CleanEnvironmentVictoryBehaviour(){
		
	}

    public virtual void VictoryBehaviour(){

        temp -= Time.deltaTime;

        textBuzzerTime.text = temp.ToString("00.0");

        if (temp <= 0)
        {

            textBuzzerTime.text = "00.0";
            textBuzzerTime.color = Color.red;
            frameBuzzer.color = Color.red;

            if (player1Movement.isGround)
            {
                player1.GetComponent<Rigidbody>().isKinematic = true;
                player1.setIdle();
                player1.setNone();
            }

            bulletsInTheAir = GameObject.FindGameObjectsWithTag("Bullet");

            
                if (bulletsInTheAir.Length <= 0 && player1Movement.isGround)
                {
                    setVictoryUltimate();
                }

        }
        
    }

    public virtual void VictoryUltimateBehaviour() {

        temp -= Time.deltaTime;

        if (temp < 0 && comboSystem.comboCount == 0 && comboSystem.lastContainerLogic == null && currentNumber == desiredNumber)
        {
            setResults();
            
        }

    }

    public void setGoToNextLevel() {
        fadeScreen.startFade(true, 2);
        isFinishNext = true;
        infoText.text = "";
        panelBuzzTime.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(false);
        if (interfaceGameOver!=null)
        interfaceGameOver.gameObject.SetActive(false);
        interfaceVictory.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(false);
    }

    public void setGoRestartLevel()
    {
        fadeScreen.startFade(true, 2);
        isFinishNext = true;
        infoText.text = "";
        panelBuzzTime.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(false);
        if (interfaceGameOver != null)
            interfaceGameOver.gameObject.SetActive(false);
        interfaceVictory.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(false);
    }

    protected void ResultsBehaviour() {  

        if (fadeScreen.isFadeIn())
        {
            if (isFinishRestart)
                setGoToMainMenu();

            if (isFinishNext)
                setRestartLevel();
        }
    }

    protected void LoseBehaviour(){

        if (fadeScreen.isFadeIn())
        {
            if (isFinishRestart)
                setGoToMainMenu();

            if (isFinishNext)
                setRestartLevel();
        }
    }

    public virtual void AddScore(bool IsPlayer1, int blockType, bool hitsBird) {

         textScore.GetComponent<Animator>().SetTrigger("isIncreased");

        initialNumber = currentScore;
        /*
        if (blockType == 0)
            currentScore += 25;
        else if (blockType == 1)
            currentScore += 75;
        else if (blockType == 2)
            currentScore += 125;
        else if (blockType == 3)
            currentScore += 250;
        */
        if (blockType == 0)
            currentScore += scoreFlesh;
        else if (blockType == 1)
            currentScore += scoreWeapon;
        else if (blockType == 2)
            currentScore += scoreArmor;
        else if (blockType == 3)
            currentScore += scoreCoin;
        /*
        if (hitsBird)
            currentScore += 50;
        */
        if (hitsBird)
            currentScore += scoreHitsBird;

        desiredNumber = currentScore;
        // textScore.text = currentScore.ToString() + "/" + scorePerfect.ToString();

        if (currentScore >= scoreGoal && (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE)) {
            
            setVictory();
        }

    }

    public virtual void AddScoreCombo(int scoreCombo, bool isPlayer1)
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
            // Debug.Log("VICTORY COMBO");
            setVictory();
            
        }

    }

    public void AddPenalty(int blockType)
    {
        if (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE && state != GameStates.LOSE)
        {
            if (blockGridLogic.state != BlockGridLogic.BlockGridLogicStates.STRESS_MODE)
                blockGridLogic.SetMove();
        }

    }

    // BAT SCORE
    public virtual void batSecretFound(int batScore, bool isPlayer1) {
        // batScore <= 0 --> Secret found!
        // batScore > 0 --> Bonus Score!
        if (batScore <= 0)
        {
            textSecretScore.GetComponent<Animator>().SetTrigger("ShowText");
            // SECRET FOUND
            if (imgJewelSecret!=null)
                imgJewelSecret.enabled = true;
        }
        else {
            // ADDITIONAL SCORE
            textSecretScore.GetComponent<Animator>().SetTrigger("ShowText");
            if (imgJewelSecret != null)
                imgJewelSecret.enabled = true;

            AddScoreCombo(batScore, isPlayer1);
        }
    }

    public void setChallengeBlock()
    {
        // PLAYER FAILS THE CHALLENGE BLOCKS
        if (!challengeBlocks && state != GameStates.RESULTS)
        {
            CoreManager.Audio.Play(CoreManager.Audio.challengeFail, myTransform.position, 2);
            challengeBlocks = true;
            if (imgCrossChallenge != null)
                imgCrossChallenge.enabled = true;

            TextChallengeBlockFail.GetComponent<Animator>().SetTrigger("ShowText");
        }
    }

    public void substractBlockRamaining() {
        blocksRemaining--;

        // IF ALL BLOCKS ARE DIG, ACTIVATE STRESS MODE
        if (blocksRemaining <= 0) {
            blockGridLogic.SetStressMode();
        }
    }

}
