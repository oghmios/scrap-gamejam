using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameLogicPvP : GameLogic
{

    [Header("PLAYERS")]
    public PlayerLogic player2;
    public MoveCharacter player2Movement;

    public int numPlayersLife;

    public Text textScore2, textScoreGoal2, textScorePerfect2;
    public RectTransform objectScoreGoal2;
    public Image scoreImageMask2;
    public Image scoreImage2;
    public Image imgStarGoal2;
    public Image imgBarGoal2;
    private float animationTime2 = 2f, currentNumber2, initialNumber2, desiredNumber2;
    private bool getScoreGoal2, getScorePerfect2;
    public int currentScore2 = 0;
    public Text textResults, textP1Score, textP2Score;
    private bool P1IsGround, P2IsGround;
    // COMBO SYSTEM
    public ComboSystemLogic comboSystemP2;

    // Update is called once per frame
    public override void Update()
    {

        switch (state)
        {

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

        // PLAYER 1 Animation Score Text
        if (currentNumber != desiredNumber)
        {

            if (initialNumber < desiredNumber)
            {
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
                    
                }
                else if (!getScorePerfect)
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

        // PLAYER 2 Animation Score Text
        if (currentNumber2 != desiredNumber2)
        {

            if (initialNumber2 < desiredNumber2)
            {
                currentNumber2 += (animationTime2 * Time.deltaTime) * (desiredNumber2 - initialNumber2);

                if (currentNumber2 < scorePerfect && !getScorePerfect2)
                {
                    scoreImageMask2.fillAmount = (float) currentNumber2 / scorePerfect;

                    if (currentNumber2 > scoreGoal && !getScoreGoal2)
                    {
                        scoreImage2.color = colorBarGoalScore;
                        textScoreGoal2.color = colorBarFull;
                        imgStarGoal2.color = colorBarFull;
                        imgBarGoal2.color = colorBarFull;
                        imgStarGoal2.GetComponent<Animator>().SetTrigger("isIncreased");
                        getScoreGoal2 = true;
                    }

                }
                else if (!getScorePerfect2)
                {
                    scoreImageMask2.fillAmount = 1;
                    textScorePerfect2.color = colorBarFull;
                    scoreImage2.color = colorBarFullScore;
                    imgBarPerfect.color = colorBarFull;
                    imgTrophyPerfect.color = colorBarFull;
                    imgTrophyPerfect.GetComponent<Animator>().SetTrigger("isIncreased");
                    getScorePerfect2 = true;
                }

                if (currentNumber2 >= desiredNumber2)
                    currentNumber2 = desiredNumber2;
            }
        }

        textScore.text = currentNumber.ToString("00");
        textScore2.text = currentNumber2.ToString("00");

        if (state == GameStates.VICTORY || state == GameStates.VICTORY_ULTIMATE)
        {
            AudioSource[] sounds = Object.FindObjectsOfType<AudioSource>();

            for (int i = 0; i < sounds.Length; i++)
            {

                sounds[i].pitch = timeScaleBuzzerTime * 2; // Time.timeScale;

            }
        }

    }

    // SETS
    public override void setStart(){
        Time.timeScale = 1;

        P1IsGround = false;
        P2IsGround = false;

        numPlayersLife = 2;

        if (scoreGoal > scorePerfect) {
            scorePerfect = scoreGoal;
        }

        isFinishRestart = false;
        isFinishNext = false;

        getScorePerfect = false;
        getScoreGoal = false;

        if (!interfaceGameplay.gameObject.activeSelf)
            interfaceGameplay.gameObject.SetActive(true);

        // P1
        textScore.text = currentScore.ToString(); 
        textScoreGoal.text = scoreGoal.ToString();
        textScorePerfect.text = scorePerfect.ToString();
        objectScoreGoal.localPosition = new Vector3(objectScoreGoal.localPosition.x+ (((float)scoreGoal / (float)scorePerfect) * 110f), objectScoreGoal.localPosition.y, objectScoreGoal.localPosition.z);
        scoreImageMask.fillAmount = (float) currentScore / scorePerfect;

        // P2
        textScore2.text = currentScore2.ToString();
        textScoreGoal2.text = scoreGoal.ToString();
        objectScoreGoal2.localPosition = new Vector3(objectScoreGoal2.localPosition.x - (((float)scoreGoal / (float)scorePerfect) * 110f), objectScoreGoal2.localPosition.y, objectScoreGoal2.localPosition.z);
        scoreImageMask2.fillAmount = (float) currentScore2 / scorePerfect;

        challengeBlocks = false;

        panelBuzzTime.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(false);
        // interfaceGameOver.gameObject.SetActive(false);
        interfaceVictory.gameObject.SetActive(false);
        

        // INITIALIZE BLOCKS
        blockGridLogic.setCreateBlocks(numRows, numColumns, numChallengeRows, numHeavyRows, timeToMoveBlocks);
        blockGridLogic.SetNone();
        
        temp = 1;
        state = GameStates.START;
	}

    public override void setGame(){
        Time.timeScale = 1;

        menuLogic.prevOption = 0;
        interfacePause.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);

        blockGridLogic.SetSleep();
        player1.setIdle();
        player1Movement.enabled = true;
        player1.enabled = true;

        player2.setIdle();
        player2Movement.enabled = true;
        player2.enabled = true;

        state = GameStates.GAME;
	}

    public override void setPause()
    {
        Time.timeScale = 0;
        interfaceGameplay.gameObject.SetActive(false);
        interfacePause.gameObject.SetActive(true);
        menuLogic.GotoMainMenu();

        blockGridLogic.SetNone();
        player1.setNone();
        player1Movement.enabled = false;
        player1.enabled = false;

            player2.setNone();
            player2Movement.enabled = false;
            player2.enabled = false;

        state = GameStates.PAUSE;
    }

    public override void setVictory() {

        Time.timeScale = timeScaleBuzzerTime;
        
        temp = slowMotionTime;
        // INCREASE THE SPEED OF THE PLAYER
        player1Movement.speed = player1Movement.speed * 1.5f;
        player1Movement.gravity = player1Movement.gravity * 1.5f;
        player1.tempDig = player1.tempDig * 0.5f;
        player1.tempDigDown = player1.tempDigDown * 0.5f;
        player1.tempDigToIdle = player1.tempDigToIdle * 0.5f;
        player1.animatorCharacter.speed = player1.animatorCharacter.speed * 1.5f;

            player2Movement.speed = player2Movement.speed * 1.5f;
            player2Movement.gravity = player2Movement.gravity * 1.5f;
            player2.tempDig = player2.tempDig * 0.5f;
            player2.tempDigDown = player2.tempDigDown * 0.5f;
            player2.tempDigToIdle = player2.tempDigToIdle * 0.5f;
            player2.animatorCharacter.speed = player2.animatorCharacter.speed * 1.5f;

        CoreManager.Audio.Play(CoreManager.Audio.buzzerScore, myTransform.position);
        
        // SHOW BUZZ TIME
        panelBuzzTime.gameObject.SetActive(true);

        blockGridLogic.SetNone();
        // player.enabled = false;

        state = GameStates.VICTORY;
	}

    public override void setResults()
    {

        AudioSource[] sounds = Object.FindObjectsOfType<AudioSource>();

        for (int i = 0; i < sounds.Length; i++)
        {

            sounds[i].pitch = 1; // Time.timeScale;

        }

        panelBuzzTime.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);
        interfaceVictory.gameObject.SetActive(true);
        // interfaceGameOver.gameObject.SetActive(false);

        // textP1Score.text = currentScore.ToString();
        // textP2Score.text = currentScore2.ToString();
        textP1Score.color = Color.blue;
        textP2Score.color = Color.red;

        if (currentScore > currentScore2)
        {
            textResults.color = Color.blue;
            // textP1Score.fontSize = 16;
            textResults.text = "PLAYER1\nWINS!";
            PlayerPrefs.SetInt("P1TotalWins", PlayerPrefs.GetInt("P1TotalWins") + 1);
        }
        else if (currentScore < currentScore2)
        {
            textResults.color = Color.red;
            // textP2Score.fontSize = 16;
            textResults.text = "PLAYER2\nWINS!";
            PlayerPrefs.SetInt("P2TotalWins", PlayerPrefs.GetInt("P2TotalWins") + 1);
        }
        else if (currentScore == currentScore2) {
            textResults.color = Color.white;
            // textP1Score.fontSize = 16;
            // textP2Score.fontSize = 16;
            textResults.text = "DRAW!";
        }

        if (currentScore >= scorePerfect || currentScore2 >= scorePerfect)
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


        textP1Score.text = "Wins:" + PlayerPrefs.GetInt("P1TotalWins").ToString() + " | " + currentScore.ToString();
        textP2Score.text = "Wins:" + PlayerPrefs.GetInt("P2TotalWins").ToString() + " | " + currentScore2.ToString();

        temp = 3;
        eventsystem.SetSelectedGameObject(buttonVictoryNextLevel);

        state = GameStates.RESULTS;
    }

    public override void setLose(int loseMode){
        temp = 4;

        // BLOCKS TOUCH SPIKES == 1
        // Particle System blocks
        // BlockWall Movement down to up
        if (loseMode == 1)
        {
            if (player1.state != PlayerLogic.PlayerStates.DIE)
                player1.setDie(loseMode);

            player1Movement.enabled = false;
            player1.enabled = false;


            if (player2.state != PlayerLogic.PlayerStates.DIE)
                player2.setDie(loseMode);

            player1Movement.enabled = false;
            player2.enabled = false;


            interfaceVictory.gameObject.SetActive(true);
            interfaceGameplay.gameObject.SetActive(true);
            // interfaceGameOver.gameObject.SetActive(true);

            textResults.color = Color.white;
            // textP1Score.fontSize = 16;
            // textP2Score.fontSize = 16;
            textResults.text = "DRAW!";

            blockGridLogic.SetNone();

            psWallBlocks.Play();
            CameraShake.Shake(Vector3.one * 3, 2f);
            CoreManager.Audio.Play(CoreManager.Audio.explosionStones, myTransform.position, 2);
            wallBlocksAnim.SetTrigger("isUp");

            // ONE PLAYER IS DEATH
        } else if (loseMode == 2) {
            
            // if (player1.state != PlayerLogic.PlayerStates.DIE)
            //    player1.setDie(loseMode);

            player1Movement.enabled = false;
            player1.enabled = false;


            // if (player2.state != PlayerLogic.PlayerStates.DIE)
            //    player2.setDie(loseMode);

            player1Movement.enabled = false;
            player2.enabled = false;

            panelBuzzTime.gameObject.SetActive(false);
            interfaceGameplay.gameObject.SetActive(true);
            interfaceVictory.gameObject.SetActive(true);
            // interfaceGameOver.gameObject.SetActive(false);

            // textP1Score.text = currentScore.ToString();
            // textP2Score.text = currentScore2.ToString();
            textP1Score.color = Color.blue;
            textP2Score.color = Color.red;

            blockGridLogic.SetNone();

            if (player1.state != PlayerLogic.PlayerStates.DIE && player2.state == PlayerLogic.PlayerStates.DIE)
            {
                textResults.color = Color.blue;
                // textP1Score.fontSize = 16;
                textResults.text = "PLAYER1\nWINS!";

                PlayerPrefs.SetInt("P1TotalWins", PlayerPrefs.GetInt("P1TotalWins")+1);

                if (currentScore >= scorePerfect)
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
            }
            else if (player1.state == PlayerLogic.PlayerStates.DIE && player2.state != PlayerLogic.PlayerStates.DIE)
            {
                textResults.color = Color.red;
                // textP2Score.fontSize = 16;
                textResults.text = "PLAYER2\nWINS!";

                PlayerPrefs.SetInt("P2TotalWins", PlayerPrefs.GetInt("P2TotalWins") + 1);

                if (currentScore2 >= scorePerfect)
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
            }
            else if (player1.state == PlayerLogic.PlayerStates.DIE && player2.state == PlayerLogic.PlayerStates.DIE)
            {
                textResults.color = Color.white;
                // textP1Score.fontSize = 16;
                // textP2Score.fontSize = 16;
                textResults.text = "DRAW!";
            }

        }

        textP1Score.text = "Wins:" + PlayerPrefs.GetInt("P1TotalWins").ToString() + " | " + currentScore.ToString();
        textP2Score.text = "Wins:" + PlayerPrefs.GetInt("P2TotalWins").ToString() + " | " + currentScore2.ToString();


        // eventsystem.SetSelectedGameObject(buttonLoseRestart);
        eventsystem.SetSelectedGameObject(buttonVictoryNextLevel);

        state = GameStates.LOSE;
	}

    public override void VictoryUltimateBehaviour()
    {

        temp -= Time.deltaTime;

        if (temp < 0 && (comboSystem.comboCount == 0 && comboSystem.lastContainerLogic == null && currentNumber == desiredNumber) &&
            (comboSystemP2.comboCount == 0 && comboSystemP2.lastContainerLogic == null && currentNumber2 == desiredNumber2) )
        {
            setResults();

        }

    }

    public override void VictoryBehaviour(){

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
                P1IsGround = true;
                
            }

                if (player2Movement.isGround)
                {
                    player2.GetComponent<Rigidbody>().isKinematic = true;
                    player2.setIdle();
                    player2.setNone();
                    P2IsGround = true;
                }

            bulletsInTheAir = GameObject.FindGameObjectsWithTag("Bullet");

            if (bulletsInTheAir.Length <= 0 && P1IsGround && P2IsGround)
            {
                player1.GetComponent<Rigidbody>().isKinematic = true;
                player1.setIdle();
                player1.setNone();

                player2.GetComponent<Rigidbody>().isKinematic = true;
                player2.setIdle();
                player2.setNone();

                setVictoryUltimate();
            }
            
        }
        
    }

    public override void AddScore(bool IsPlayer1, int blockType, bool hitsBird)
    {
        if (!IsPlayer1)
        {
            textScore2.GetComponent<Animator>().SetTrigger("isIncreased");

            initialNumber2 = currentScore2;

            if (blockType == 0)
                currentScore2 += scoreFlesh;
            else if (blockType == 1)
                currentScore2 += scoreWeapon;
            else if (blockType == 2)
                currentScore2 += scoreArmor;
            else if (blockType == 3)
                currentScore2 += scoreCoin;

            if (hitsBird)
                currentScore2 += scoreHitsBird;

            desiredNumber2 = currentScore2;

            if (currentScore2 >= scoreGoal && (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE))
            {

                setVictory();
            }

        }
        else
        {
            textScore.GetComponent<Animator>().SetTrigger("isIncreased");

            initialNumber = currentScore;

            if (blockType == 0)
                currentScore += scoreFlesh;
            else if (blockType == 1)
                currentScore += scoreWeapon;
            else if (blockType == 2)
                currentScore += scoreArmor;
            else if (blockType == 3)
                currentScore += scoreCoin;

            if (hitsBird)
                currentScore += scoreHitsBird;

            desiredNumber = currentScore;

            if (currentScore >= scoreGoal && (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE))
            {

                setVictory();
            }
        }
    }

    public override void AddScoreCombo(int scoreCombo, bool isPlayer1)
    {
        if (isPlayer1)
        {
            textScore.GetComponent<Animator>().SetTrigger("isIncreased");

            initialNumber = currentScore;

            currentScore += scoreCombo;

            desiredNumber = currentScore;

            if (currentScore >= scoreGoal && (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE))
            {
                setVictory();
            }
        }
        else if (!isPlayer1) {
            textScore2.GetComponent<Animator>().SetTrigger("isIncreased");

            initialNumber2 = currentScore2;

            currentScore2 += scoreCombo;

            desiredNumber2 = currentScore2;

            if (currentScore2 >= scoreGoal && (state != GameStates.VICTORY && state != GameStates.VICTORY_ULTIMATE))
            {
                setVictory();
            }
        }
    }

}
