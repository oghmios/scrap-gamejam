using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BlocksLine : MonoBehaviour {

    public GameObject[] blockTypes;
    private GameObject[] lineOfBlocks;
    public Vector3 position;
    public Quaternion rotation;
    private System.Random randomBlockNumber;
    
	// Use this for initialization
	void Start () {
        randomBlockNumber = new System.Random();
        lineOfBlocks = new GameObject[10];
        for(int i = 0; i < 10; i++)
        {
            int randomType = randomBlockNumber.Next(0, 4);
            lineOfBlocks[i] = (GameObject)Instantiate(blockTypes[randomType], position, rotation);
            position.x += 3;

        }
	}

}
