﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour {

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
	public int prevOption;
    public Text textAudio;

    public AudioSource audioMusic;

    // Use this for initialization
    void Start () {

        if (!PlayerPrefs.HasKey("Audio"))
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
		// selectPrevOption();

        selectResumeButton();
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
        eventsystem.SetSelectedGameObject(buttonControls);
		
	}

    public void GoToGraphics()
    {
        panelStage.gameObject.SetActive(false);
        panelMainMenu.gameObject.SetActive(false);
        panelTutorial.gameObject.SetActive(false);
        panelControls.gameObject.SetActive(false);

        if (panelGraphics != null)
        panelGraphics.gameObject.SetActive(true);

        eventsystem.SetSelectedGameObject(buttonGraphics);
        prevOption = 2;
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
        prevOption = 1;
        eventsystem.SetSelectedGameObject(buttonTutorial);
		
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

        selectPrevButton();
    }

    public void selectResumeButton() {

        if (buttonResume != null)
        {
            StartCoroutine(WaitThen_AllowButtonClicks());
        }
    }

    private IEnumerator WaitThen_AllowButtonClicks()
    {
        yield return null;
        eventsystem.SetSelectedGameObject(null);
        eventsystem.SetSelectedGameObject(buttonResume);
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

    public void selectPrevButton()
    {

        if (buttonResume != null)
        {
            StartCoroutine(WaitThen_AllowPrevButtonClicks(prevOption));
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
        else
            eventsystem.SetSelectedGameObject(buttonsMainMenu[0].gameObject);
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

}
