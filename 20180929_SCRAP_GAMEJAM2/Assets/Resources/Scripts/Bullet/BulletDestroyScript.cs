using System.Collections;
using UnityEngine;

public class BulletDestroyScript : MonoBehaviour {

    void OnEnable()
    {
        Invoke("DestroyBall", 5f);
    }

    public void DestroyBall()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
