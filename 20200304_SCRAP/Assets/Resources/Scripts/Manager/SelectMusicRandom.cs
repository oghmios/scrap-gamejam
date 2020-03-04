using UnityEngine;

public class SelectMusicRandom : MonoBehaviour {
    public AudioSource audioMusic;
    // Use this for initialization
    void Start()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
                audioMusic.clip = CoreManager.Audio.music01;
                break;
            case 1:
                audioMusic.clip = CoreManager.Audio.music02;
                break;
            case 2:
                audioMusic.clip = CoreManager.Audio.music03;
                break;
            case 3:
                audioMusic.clip = CoreManager.Audio.music04;
                break;
            case 4:
                audioMusic.clip = CoreManager.Audio.music05;
                break;
        }
        audioMusic.loop = true;

        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 1);

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audioMusic.Play();
        }
        else if (PlayerPrefs.GetInt("Sound") == 0)
        {
            audioMusic.Stop();

        }
        
    }

}
