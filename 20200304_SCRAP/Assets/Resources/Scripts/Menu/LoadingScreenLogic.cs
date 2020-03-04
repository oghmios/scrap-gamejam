using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingScreenLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadAsyncOperation());
	}

    IEnumerator LoadAsyncOperation() {

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("Level "+ CoreManager.Level.getLevelName());

        while (gameLevel.progress < 1) {

            yield return new WaitForEndOfFrame();
        }
    }
}
