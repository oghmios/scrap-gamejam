using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour {

	public Transform panelMainMenu;
	public Transform panelControls;
    public Transform panelGraphics;
	public Transform panelTutorial;
	public Transform panelStage;
	public EventSystem eventsystem;
	public GameObject[] buttonsMainMenu;
    public GameObject buttonResume;
	public GameObject buttonControls;
	public GameObject buttonTutorial;
	public GameObject buttonStages;
    public GameObject buttonGraphics;
	private int prevOption;
    public Text textAudio;

    public AudioSource audioMusic;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;

        if(!PlayerPrefs.HasKey("Audio"))
            PlayerPrefs.SetInt("Audio", 1);

        if (PlayerPrefs.GetInt("Audio") == 1)
        {
            audioMusic.Play();
            textAudio.text = "AUDIO ON";
        }
        else if (PlayerPrefs.GetInt("Audio") == 0)
        {
            audioMusic.Stop();
            textAudio.text = "AUDIO OFF";
        }

        if (panelTutorial!=null)
		panelTutorial.gameObject.SetActive(false);

        if (panelControls != null)
            panelControls.gameObject.SetActive(false);
        if (panelGraphics!=null)
        panelGraphics.gameObject.SetActive(false);

        if (panelStage != null)
            panelStage.gameObject.SetActive(false);

		panelMainMenu.gameObject.SetActive(true);
		prevOption = 0;

        selectButton(buttonResume);
    }

	public void GoToControls(){
        if (panelStage != null)
            panelStage.gameObject.SetActive(false);
        if (panelMainMenu != null)
            panelMainMenu.gameObject.SetActive(false);
        if (panelTutorial != null)
            panelTutorial.gameObject.SetActive(false);
        if (panelGraphics != null && panelGraphics.gameObject!=null)
        {
            panelGraphics.gameObject.SetActive(false);
            prevOption = 3;
        } else {
            prevOption = 1;
        }
		panelControls.gameObject.SetActive(true);
        prevOption = 2;
        selectButton(buttonControls);

    }

    public void GoToGraphics()
    {
        if (panelStage != null)
            panelStage.gameObject.SetActive(false);
        if (panelMainMenu != null)
            panelMainMenu.gameObject.SetActive(false);
        if (panelTutorial != null)
            panelTutorial.gameObject.SetActive(false);
        if (panelControls != null)
            panelControls.gameObject.SetActive(false);

        if (panelGraphics != null)
        panelGraphics.gameObject.SetActive(true);

        prevOption = 1;
        selectButton(buttonGraphics);
        
    }

	public void GoToTutorial(){
        if (panelStage != null)
            panelStage.gameObject.SetActive(false);
        if (panelMainMenu != null)
            panelMainMenu.gameObject.SetActive(false);
        if (panelControls != null)
            panelControls.gameObject.SetActive(false);

        if (panelGraphics != null)
            panelGraphics.gameObject.SetActive(false);

		panelTutorial.gameObject.SetActive(true);
        prevOption = 3;
        selectButton(buttonTutorial);

    }

    public void GoToPlay() {
        SceneManager.LoadScene("Level 1");
    }

    public void GoToExit() {
        Application.Quit();
    }

	public void GotoMainMenu(){
        if (panelStage != null)
            panelStage.gameObject.SetActive(false);

        if (panelTutorial != null)
            panelTutorial.gameObject.SetActive(false);

        if (panelControls != null)
            panelControls.gameObject.SetActive(false);

        if (panelGraphics != null)
            panelGraphics.gameObject.SetActive(false);

        if (panelMainMenu != null)
            panelMainMenu.gameObject.SetActive(true);

        selectPrevButton(prevOption);
    }

    public void GoToStages(){
		panelMainMenu.gameObject.SetActive(false);
		panelTutorial.gameObject.SetActive(false);
		panelControls.gameObject.SetActive(false);
        if (panelGraphics != null)
        panelGraphics.gameObject.SetActive(false);
		panelStage.gameObject.SetActive(true);
		eventsystem.SetSelectedGameObject(buttonStages);
		prevOption = 0;
	}

    public void setSwitchMusicOnOff()
    {

        if (PlayerPrefs.GetInt("Audio") == 0)
        {
            PlayerPrefs.SetInt("Audio", 1);
            audioMusic.Play();
            textAudio.text = "AUDIO ON";
        }
        else if (PlayerPrefs.GetInt("Audio") == 1)
        {
            PlayerPrefs.SetInt("Audio", 0);
            audioMusic.Stop();
            textAudio.text = "AUDIO OFF";
        }
    }
    /*
    private void selectPrevOption(){
        if(buttonsMainMenu.Length == 4){
            if (prevOption == 0)
            {
                eventsystem.SetSelectedGameObject(buttonsMainMenu[0]);
            }
            else if (prevOption == 1)
            {
                eventsystem.SetSelectedGameObject(buttonsMainMenu[1]);
            }
            else if (prevOption == 2)
            {
                eventsystem.SetSelectedGameObject(buttonsMainMenu[2]);
            }
            else if (prevOption == 3)
            {
                eventsystem.SetSelectedGameObject(buttonsMainMenu[3]);
            }
        } else if(buttonsMainMenu.Length == 3 ){
            if (prevOption == 0)
            {
                eventsystem.SetSelectedGameObject(buttonsMainMenu[0]);
            }
            else if (prevOption == 1)
            {
                eventsystem.SetSelectedGameObject(buttonsMainMenu[1]);
            }
            else if (prevOption == 2)
            {
                eventsystem.SetSelectedGameObject(buttonsMainMenu[2]);
            }
        }

		
	}*/

    public void selectButton(GameObject buttonOption)
    {

        if (buttonResume != null)
        {
            StartCoroutine(WaitThen_AllowButtonClicks(buttonOption));
        }
    }

    private IEnumerator WaitThen_AllowButtonClicks(GameObject buttonOption)
    {
        yield return null;
        eventsystem.SetSelectedGameObject(null);
        eventsystem.SetSelectedGameObject(buttonOption);
    }

    public void selectPrevButton(int buttonOption)
    {

        if (buttonResume != null)
        {
            StartCoroutine(WaitThen_AllowPrevButtonClicks(buttonOption));
        }
    }

    private IEnumerator WaitThen_AllowPrevButtonClicks(int buttonOption)
    {
        yield return null;
        eventsystem.SetSelectedGameObject(null);

        if (prevOption == 0)
            eventsystem.SetSelectedGameObject(buttonsMainMenu[0].gameObject);
        else if (prevOption == 1)
            eventsystem.SetSelectedGameObject(buttonsMainMenu[1].gameObject);
        else if (prevOption == 2)
            eventsystem.SetSelectedGameObject(buttonsMainMenu[2].gameObject);
        else if (prevOption == 3)
            eventsystem.SetSelectedGameObject(buttonsMainMenu[3].gameObject);
        else
            eventsystem.SetSelectedGameObject(buttonsMainMenu[0].gameObject);
    }

}
