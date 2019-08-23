using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BlockGridLogic : MonoBehaviour {

    public enum BlockGridLogicStates { NONE, SLEEP, PREPARE, MOVE }
    public BlockGridLogicStates state;

    public GameObject[] blockTypes;
    public GameObject blockHeavy;
    public GameObject[][] lineOfBlocks; // = new GameObject[10][];
    private Quaternion rotation;
    private System.Random randomBlockNumber;
    private Vector3 position2Move;
    private float timeDecay, time2Move;
    private Transform myTransform;

    // Use this for initialization
    void Start () {
        myTransform = transform;
        position2Move = myTransform.position;

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
          
        }
        
    }
    // SETS

    public void setCreateBlocks(int numRows, int numColumns, int numHeavyRows, float time2MoveAux) {
        randomBlockNumber = new System.Random();

        int randomMax = blockTypes.Length;
        time2Move = time2MoveAux;
        timeDecay = time2Move;

        lineOfBlocks = new GameObject[numRows+ numHeavyRows][];

        for (int j = 0; j < numRows; j++)
        {
            lineOfBlocks[j] = new GameObject[numRows+ numHeavyRows];
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
                lineOfBlocks[j][i] = (GameObject)Instantiate(blockTypes[randomType], position2Move, rotation);
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

        // CREATION OF HEAVY BLOCKS (CAN'T DIG)
        for (int j = numRows; j < numRows+ numHeavyRows; j++)
        {
            lineOfBlocks[j] = new GameObject[numRows + numHeavyRows];
            for (int i = 0; i < numColumns ; i++)
            {
                lineOfBlocks[j][i] = (GameObject)Instantiate(blockHeavy, position2Move, rotation);
                lineOfBlocks[j][i].transform.parent = myTransform;
                position2Move.x += 4.5f;

            }
            position2Move.x = myTransform.position.x;
            position2Move.y -= 4.5f;
        }
        
    }


    public void SetNone()
    {
        state = BlockGridLogicStates.NONE;
    }

    public void SetSleep()
    {
        timeDecay = time2Move;
        state = BlockGridLogicStates.SLEEP;
    }

    public void SetPrepare()
    {
        timeDecay = 2;
        
        state = BlockGridLogicStates.PREPARE;
    }

    public void SetMove() {

        CameraShake.Shake(Vector3.one, 0.5f);
        state = BlockGridLogicStates.MOVE;
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

}
