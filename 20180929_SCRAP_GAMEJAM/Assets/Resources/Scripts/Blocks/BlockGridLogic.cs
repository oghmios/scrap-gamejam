using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BlockGridLogic : MonoBehaviour {

    public enum BlockGridLogicStates { NONE, SLEEP, PREPARE, MOVE }
    public BlockGridLogicStates state;

    public GameObject[] blockTypes;
    public GameObject[][] lineOfBlocks = new GameObject[10][];
    public Quaternion rotation;
    private System.Random randomBlockNumber;
    private Vector3 lineMovement;
    private Vector3 position2Move;
    public float time2Move;
    private float timeDecay;
    

    // Use this for initialization
    void Start () {
        randomBlockNumber = new System.Random();
        position2Move = transform.position;
        int randomMax = 4;
        timeDecay = time2Move;
        for(int j = 0; j < 10; j++)
        {
            lineOfBlocks[j] = new GameObject[10];
            int maxOfCoin = 0;
            for (int i = 0; i < 10; i++)
            {
                
                int randomType = randomBlockNumber.Next(0, randomMax);
                if(randomType == 3)
                {
                    maxOfCoin++;
                }
                if(maxOfCoin == 2)
                {
                    randomMax = 3;
                }
                //lineOfBlocks[j][i] = new GameObject();
                lineOfBlocks[j][i] = (GameObject)Instantiate(blockTypes[randomType], position2Move, rotation);
                lineOfBlocks[j][i].transform.parent = transform;
                position2Move.x += 3;
                //transform.position.Set(position2Move.x, position2Move.y, position2Move.z);

            }
            position2Move.x = transform.position.x;
            position2Move.y -= 3;
            maxOfCoin = 0;
            randomMax = 4;
            //transform.position.Set(position2Move.x, position2Move.y, position2Move.z);
        }
        //slineMovement.y = 3;


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
        lineMovement = transform.position;
        state = BlockGridLogicStates.PREPARE;
    }

    public void SetMove() {
        
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

        transform.Translate(Vector3.up * Time.deltaTime);
        timeDecay -= Time.deltaTime;

        if (timeDecay < 0)
        {
            SetSleep();

        }

    }

}
