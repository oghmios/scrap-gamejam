using UnityEngine;

public class AudioSourceDestroy : MonoBehaviour {

    void OnEnable()
    {
       //  Invoke("DestroyAudioSource", 5f);
    }

    public void DestroyWithSeconds(float timeAux) {
        Invoke("DestroyAudioSource", timeAux);
    }

    public void DestroyAudioSource()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
