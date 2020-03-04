using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockGridLogic : MonoBehaviour {

    public enum BlockGridLogicStates { NONE, SLEEP, PREPARE, MOVE, STRESS_MODE }
    public BlockGridLogicStates state;
    public GameObject[] blockTypes;
    public GameObject blockHeavy;
    public GameObject[][] lineOfBlocks; // = new GameObject[10][];
    public ParticleSystem earthQuake;
    public GameLogic gameLogic;
    // private Quaternion rotation;
    private System.Random randomBlockNumber;
    private Vector3 position2Move;
    private float timeDecay, time2Move;
    private Transform myTransform;
    private Color colorChallenge = new Color(1, 0.7f, 0.7f);
    private int blocksRemaining;
    private ParticleSystem.MainModule earthQuakeMain;

    // Use this for initialization
    void Start () {
        myTransform = transform;
        position2Move = myTransform.position;
        earthQuakeMain = earthQuake.main;

    }
    void Update()
    {

        switch (state)
        {
            case BlockGridLogicStates.NONE:
                NoneBehaviour();
                break;
            case BlockGridLogicStates.SLEEP:
                SleepBehaviour();
                break;
            case BlockGridLogicStates.PREPARE:
                PrepareBehaviour();
                break;
            case BlockGridLogicStates.MOVE:
                MoveBehaviour();
                break;
            case BlockGridLogicStates.STRESS_MODE:
                StressModeBehaviour();
                break;

        }
        
    }
    // SETS

    public void setCreateBlocks(int numRows, int numColumns, int numChallengeRows, int numHeavyRows, float time2MoveAux) {
        randomBlockNumber = new System.Random();
        blocksRemaining = 0;
        int randomMax = blockTypes.Length;
        time2Move = time2MoveAux;
        timeDecay = time2Move;

        lineOfBlocks = new GameObject[numRows+ numChallengeRows+ numHeavyRows][];

        for (int j = 0; j < numRows; j++)
        {
            lineOfBlocks[j] = new GameObject[numRows+ numChallengeRows + numHeavyRows];
            int maxOfCoin = 0;
            for (int i = 0; i < numColumns; i++)
            {

                int randomType = randomBlockNumber.Next(0, randomMax);
                if (randomType == 3)
                {
                    maxOfCoin++;
                }
                if (maxOfCoin == 2)
                {
                    randomMax = 3;
                }
                //lineOfBlocks[j][i] = new GameObject();
                lineOfBlocks[j][i] = (GameObject)Instantiate(blockTypes[randomType], position2Move, Quaternion.identity); // rotation);
                blocksRemaining++;
                lineOfBlocks[j][i].transform.parent = myTransform;
                position2Move.x += 4.5f;
                //transform.position.Set(position2Move.x, position2Move.y, position2Move.z);

            }
            position2Move.x = myTransform.position.x;
            position2Move.y -= 4.5f;
            maxOfCoin = 0;
            randomMax = blockTypes.Length;
            //transform.position.Set(position2Move.x, position2Move.y, position2Move.z);
            
        }

        // CREATION OF  CHALLENGE BLOCKS
        for (int j = numRows; j < numRows + numChallengeRows; j++)
        {
            lineOfBlocks[j] = new GameObject[numRows + numChallengeRows + numHeavyRows];
            int maxOfCoin = 0;
            for (int i = 0; i < numColumns; i++)
            {
                int randomType = randomBlockNumber.Next(0, randomMax);
                if (randomType == 3)
                {
                    maxOfCoin++;
                }
                if (maxOfCoin == 2)
                {
                    randomMax = 3;
                }
                lineOfBlocks[j][i] = (GameObject)Instantiate(blockTypes[randomType], position2Move, Quaternion.identity); // rotation);
                blocksRemaining++;
                lineOfBlocks[j][i].transform.parent = myTransform;
                lineOfBlocks[j][i].GetComponent<BlockLogic>().challengeBlock = true;
                lineOfBlocks[j][i].GetComponent<SpriteRenderer>().color = colorChallenge;
                position2Move.x += 4.5f;

            }
            position2Move.x = myTransform.position.x;
            position2Move.y -= 4.5f;
        }

        // CREATION OF HEAVY BLOCKS (CAN'T DIG)
        for (int j = numRows + numChallengeRows; j < numRows + numChallengeRows + numHeavyRows; j++)
        {
            lineOfBlocks[j] = new GameObject[numRows + numChallengeRows + numHeavyRows];
            for (int i = 0; i < numColumns ; i++)
            {
                lineOfBlocks[j][i] = (GameObject)Instantiate(blockHeavy, position2Move, Quaternion.identity); // rotation);
                lineOfBlocks[j][i].transform.parent = myTransform;
                position2Move.x += 4.5f;

            }
            position2Move.x = myTransform.position.x;
            position2Move.y -= 4.5f;
        }

        gameLogic.blocksRemaining = blocksRemaining;


    }


    public void SetNone()
    {
        earthQuakeMain.loop = false;
        state = BlockGridLogicStates.NONE;
    }

    public void SetSleep()
    {
        // earthQuake.Stop();
        timeDecay = time2Move;
        state = BlockGridLogicStates.SLEEP;
    }

    public void SetPrepare()
    {
        timeDecay = 2;
        
        state = BlockGridLogicStates.PREPARE;
    }

    public void SetMove() {
        //  ParticleSystem.MainModule mainPS = earthQuake.main;
        //  mainPS.duration = 3;
        // CoreManager.Audio.Play(CoreManager.Audio.blocksMoving, transform.position);
        switch (Random.Range(0, 3))
        {
            case 0:
                CoreManager.Audio.Play(CoreManager.Audio.blocksMoving01, myTransform.position);
                break;
            case 1:
                CoreManager.Audio.Play(CoreManager.Audio.blocksMoving02, myTransform.position);
                break;
            case 2:
                CoreManager.Audio.Play(CoreManager.Audio.blocksMoving03, myTransform.position);
                break;
        }

        earthQuake.Play();
        CameraShake.Shake(Vector3.one, 0.5f);

        state = BlockGridLogicStates.MOVE;
    }

    public void SetStressMode() {
        timeDecay = 30f;
        time2Move = 0.0025f;


        earthQuakeMain.loop = true;
        earthQuake.Play();
        CameraShake.Shake(Vector3.one*0.5f, 30f);
        state = BlockGridLogicStates.STRESS_MODE;
    }


    // BEHAVIOURS

    void NoneBehaviour()
    {

    }

    void SleepBehaviour()
    {
        timeDecay -= Time.deltaTime;
        if(timeDecay < 0)
        {
            SetPrepare();
            
        }
    }

    void PrepareBehaviour()
    {
        //aqui va el shake antes de pasar al move

        SetMove();
    }

    void MoveBehaviour()
    {

        myTransform.Translate(Vector3.up * Time.deltaTime);
        timeDecay -= Time.deltaTime;

        if (timeDecay < 0)
        {
            SetSleep();

        }

    }

    void StressModeBehaviour()
    {

        myTransform.Translate(Vector3.up * Time.deltaTime * 2);
        timeDecay -= Time.deltaTime;

        if (timeDecay < 0)
        {
            SetSleep();

        }

    }

}
