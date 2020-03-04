using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSortingLayer : MonoBehaviour {

    public int sortingLayerID;
    public int sortingOrder;

	// Use this for initialization
	void Start () {

        GetComponent<Renderer>().sortingLayerID = sortingLayerID;
        GetComponent<Renderer>().sortingOrder = sortingOrder;

	}
	
}
