using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFireScript : MonoBehaviour {

    public float FireTime = .5f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Fire", FireTime, FireTime);
	}

    void Fire() {
        GameObject obj = NewObjectPoolerScript.current.GetPooledObject(0);

        if (obj == null) return;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
}
