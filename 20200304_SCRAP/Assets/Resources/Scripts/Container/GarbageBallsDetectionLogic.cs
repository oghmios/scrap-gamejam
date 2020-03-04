using UnityEngine;

public class GarbageBallsDetectionLogic : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Bullet")
        {
                other.GetComponent<BulletDestroyScript>().DestroyBall();
        }
    }

}
