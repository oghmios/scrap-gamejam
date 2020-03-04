using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectPoolerAudio : MonoBehaviour {

    public static NewObjectPoolerAudio current;
    public GameObject pooledObjectAudioSource;
    public int pooledAmount = 10;
    public bool willGrow = true;

    public List<GameObject> pooledObjectsAudioSource;

    private void Awake()
    {
        current = this;
    }


    // Use this for initialization
    void Start () {
        pooledObjectsAudioSource = new List<GameObject>();

        // POOL AUDIO SOURCE
        for (int i = 0; i < pooledAmount; i++) {
            GameObject obj = (GameObject)Instantiate(pooledObjectAudioSource);
            obj.SetActive(false);
            pooledObjectsAudioSource.Add(obj);
        }

    }
	
	public GameObject GetPooledAudioSource()
    {
 
            for (int i = 0; i < pooledObjectsAudioSource.Count; i++)
            {
                if (!pooledObjectsAudioSource[i].activeInHierarchy)
                {
                    return pooledObjectsAudioSource[i];
                }
            }

            if (willGrow)
            {
                GameObject obj = (GameObject)Instantiate(pooledObjectAudioSource, transform.position, transform.rotation);
                pooledObjectsAudioSource.Add(obj);
                return obj;
            }
        
        return null;
    }
}
