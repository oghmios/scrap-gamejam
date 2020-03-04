using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameLogicCoop : GameLogic
{

    [Header("PLAYERS")]
    public PlayerLogic player2;
    public MoveCharacter player2Movement;

    public int numPlayersLife;
    public Text textScoreResult, textScoreResultGameOver;

	// SETS
	public override void setStart(){
        Time.timeScale = 1;

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

        textScore.text = currentScore.ToString(); 
        textScoreGoal.text = scoreGoal.ToString();
        textScorePerfect.text = scorePerfect.ToString();

        objectScoreGoal.localPosition = new Vector3(objectScoreGoal.localPosition.x+ (((float)scoreGoal / (float)scorePerfect) * 140f), objectScoreGoal.localPosition.y, objectScoreGoal.localPosition.z);
        scoreImageMask.fillAmount = (float)currentScore / scorePerfect;

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
        interfaceGameOver.gameObject.SetActive(false);

        if (currentScore >= scorePerfect)
        {
            ImgVictoryPerfect.enabled = true;
            ImgVictoryPerfect.color = Color.white;
            ImgVictorySlotPerfect.color = Color.blue;
            ImgVictoryPerfect.GetComponent<Animator>().SetTrigger("IsIncreased");
            textScoreResult.text = "<color=yellow>" + currentScore.ToString() + "</color>" + "/" + scorePerfect.ToString();
        }
        else {
            textScoreResult.text = currentScore.ToString() + "/" + "<color=yellow>" + scorePerfect.ToString() + "</color>";
        }

        if (imgJewelSecret != null && imgJewelSecret.enabled)
        {
            ImgVictoryPajaroto.enabled = true;
            ImgVictoryPajaroto.color = Color.white;
            ImgVictorySlotPajaroto.color = Color.blue;
            ImgVictoryPajaroto.GetComponent<Animator>().SetTrigger("IsIncreased");
        }

        if (!challengeBlocks && ImgVictoryChallengeBlocks != null && ImgVictorySlotChallengeBlocks != null)
        {
            ImgVictoryChallengeBlocks.enabled = true;
            ImgVictoryChallengeBlocks.color = Color.white;
            ImgVictorySlotChallengeBlocks.color = Color.blue;
            ImgVictoryChallengeBlocks.GetComponent<Animator>().SetTrigger("IsIncreased");
        }

        if (currentScore >= scorePerfect && imgJewelSecret != null && imgJewelSecret.enabled && !challengeBlocks)
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

    public override void setLose(int loseMode){
        temp = 4;

        interfaceVictory.gameObject.SetActive(false);
        interfaceGameplay.gameObject.SetActive(true);
        interfaceGameOver.gameObject.SetActive(true);
        blockGridLogic.SetNone();

        if (currentScore >= scorePerfect)
        {
            textScoreResultGameOver.text = "<color=red>" + currentScore.ToString() + "</color>" + "/" + scorePerfect.ToString();
        }
        else
        {
            textScoreResultGameOver.text = "<color=red>"+currentScore.ToString() + "</color>" + "/" + "<color=yellow>" + scorePerfect.ToString() + "</color>";
        }

        if (player1.state != PlayerLogic.PlayerStates.DIE)
        player1.setDie(loseMode);

        player1Movement.enabled = false;
        player1.enabled = false;


            if (player2.state != PlayerLogic.PlayerStates.DIE)
                player2.setDie(loseMode);

            player1Movement.enabled = false;
            player2.enabled = false;

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
            }

                if (player2Movement.isGround)
                {
                    player2.GetComponent<Rigidbody>().isKinematic = true;
                    player2.setIdle();
                    player2.setNone();
                }

            bulletsInTheAir = GameObject.FindGameObjectsWithTag("Bullet");

            if (bulletsInTheAir.Length <= 0 && (player1Movement.isGround || player2Movement.isGround))
            {
                    setVictoryUltimate();
            }
            
        }
        
    }

}
