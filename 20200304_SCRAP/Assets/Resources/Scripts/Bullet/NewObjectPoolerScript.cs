using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectPoolerScript : MonoBehaviour {

    public static NewObjectPoolerScript current;
    public GameObject pooledObjectBulletFlesh, pooledObjectBulletWeapon, pooledObjectBulletArmor, pooledObjectBulletCoin;
    public int pooledAmount = 10;
    public bool willGrow = true;

    public List<GameObject> pooledObjectsBulletFlesh;
    public List<GameObject> pooledObjectsBulletWeapon;
    public List<GameObject> pooledObjectsBulletArmor;
    public List<GameObject> pooledObjectsBulletCoin;

    private void Awake()
    {
        current = this;
    }


    // Use this for initialization
    void Start () {
        pooledObjectsBulletFlesh = new List<GameObject>();

        // POOL FLESH
        for (int i = 0; i < pooledAmount; i++) {
            GameObject obj = (GameObject)Instantiate(pooledObjectBulletFlesh);
            obj.SetActive(false);
            pooledObjectsBulletFlesh.Add(obj);
        }

        // POOL WEAPON
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObjectBulletWeapon);
            obj.SetActive(false);
            pooledObjectsBulletWeapon.Add(obj);
        }

        // POOL ARMOR
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObjectBulletArmor);
            obj.SetActive(false);
            pooledObjectsBulletArmor.Add(obj);
        }

        // POOL COIN
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObjectBulletCoin);
            obj.SetActive(false);
            pooledObjectsBulletCoin.Add(obj);
        }
    }
	
	public GameObject GetPooledObject(int modeBullet)
    {
        // BULLET FLESH
        if (modeBullet == 0)
        {
            for (int i = 0; i < pooledObjectsBulletFlesh.Count; i++)
            {
                if (!pooledObjectsBulletFlesh[i].activeInHierarchy)
                {
                    return pooledObjectsBulletFlesh[i];
                }
            }

            if (willGrow)
            {
                GameObject obj = (GameObject)Instantiate(pooledObjectBulletFlesh, transform.position, transform.rotation);
                pooledObjectsBulletFlesh.Add(obj);
                return obj;
            }
        }
        // BULLET WEAPON
        else if (modeBullet == 1) {
            for (int i = 0; i < pooledObjectsBulletWeapon.Count; i++)
            {
                if (!pooledObjectsBulletWeapon[i].activeInHierarchy)
                {
                    return pooledObjectsBulletWeapon[i];
                }
            }

            if (willGrow)
            {
                GameObject obj = (GameObject)Instantiate(pooledObjectBulletWeapon, transform.position, transform.rotation);
                pooledObjectsBulletWeapon.Add(obj);
                return obj;
            }
        }
        // BULLET ARMOR
        else if (modeBullet == 2)
        {
            for (int i = 0; i < pooledObjectsBulletArmor.Count; i++)
            {
                if (!pooledObjectsBulletArmor[i].activeInHierarchy)
                {
                    return pooledObjectsBulletArmor[i];
                }
            }

            if (willGrow)
            {
                GameObject obj = (GameObject)Instantiate(pooledObjectBulletArmor, transform.position, transform.rotation);
                pooledObjectsBulletArmor.Add(obj);
                return obj;
            }
        }
        // BULLET COIN
        else if (modeBullet == 3)
        {
            for (int i = 0; i < pooledObjectsBulletCoin.Count; i++)
            {
                if (!pooledObjectsBulletCoin[i].activeInHierarchy)
                {
                    return pooledObjectsBulletCoin[i];
                }
            }

            if (willGrow)
            {
                GameObject obj = (GameObject)Instantiate(pooledObjectBulletCoin, transform.position, transform.rotation);
                pooledObjectsBulletCoin.Add(obj);
                return obj;
            }
        }
        return null;
    }
}
