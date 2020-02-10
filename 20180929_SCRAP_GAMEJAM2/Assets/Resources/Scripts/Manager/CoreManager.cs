/*************************************************************************
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  Description: Core Manager. Singleton object which access to other managers
  
 -------------------------------------------------------------------------
  Created by:
  - Pedro Fuentes

*************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(ScreenManager))]
[RequireComponent(typeof(AudioManager))]
//[RequireComponent(typeof(GameDataManager))]
// [RequireComponent(typeof(CameraManager))]
//[RequireComponent(typeof(DebugManager))]


public class CoreManager : MonoBehaviour
{
    // Component Managers

    // Game
    /*private static GameManager gameManager;
    public static GameManager Game
    {
        get { return gameManager; }
    }*/

    // Screen
    private static ScreenManager screenManager;
    public static ScreenManager Screen
    {
        get { return screenManager; }
    }

    // Audio
    private static AudioManager audioManager;
    public static AudioManager Audio
    {
        get { return audioManager; }
    }

    // Level
    private static LevelManager levelManager;
    public static LevelManager Level
    {
        get { return levelManager; }
    }

    // Gamedata
   	/* private static GameDataManager gamedataManager;
    public static GameDataManager Gamedata
    {
        get { return gamedataManager; }
    }*/

    // Camera
   /* private static CameraManager cameraManager;
    public static CameraManager aztCamera
    {
        get { return cameraManager; }
    } */

    // Debug
   /* private static DebugManager debugManager;
    public static DebugManager aztDebug
    {
        get { return debugManager; }
    }*/

       // Use this for initialization
    void Awake()
    {
        // Find the references
        // We must attach on the GameObject 
        // gameManager = GetComponent<GameManager>();
        levelManager = GetComponent<LevelManager>();
        screenManager = GetComponent<ScreenManager>();
        audioManager = GetComponent<AudioManager>();
       // gamedataManager = GetComponent<GameDataManager>();
       // cameraManager = GetComponent<CameraManager>();
       // debugManager = GetComponent<DebugManager>();
        
        //Make this game object persistant
        DontDestroyOnLoad(gameObject);
	}

    // Singleton Instantiation
    private static CoreManager instance = null;
    public static CoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("instantiate");
                GameObject go = new GameObject();
                instance = go.AddComponent<CoreManager>();
                go.name = "singleton";
            }

            return instance;
        }
    }

	void Start() {
		PlayerPrefs.SetInt("Sound",1);
        
        // TO DO: COMENTADO PARA APLICARLO DIRECTAMENTE EN EL NIVEL
        StartCoroutine(LoadAsyncOperation());

	}

    IEnumerator LoadAsyncOperation()
    {

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("Menu");

        while (gameLevel.progress < 1)
        {

            yield return new WaitForEndOfFrame();
        }
    }
}
