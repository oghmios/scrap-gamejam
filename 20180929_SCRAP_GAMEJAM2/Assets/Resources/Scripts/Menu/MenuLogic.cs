using UnityEngine;
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
    public Text textThrowGuide;
    public Text textOneShot;
    public InputField fieldOneShot;
    public Slider sliderTimeThrow;
    public Text textSliderTimeThrow;
    public Button setImpulseButton;
    public SourceMovement sourceMovement;
    public PlayerLogic playerLogic;
    public AudioSource audioMusic;

    // Use this for initialization
    void Start () {

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audioMusic.Play();
            textAudio.text = "AUDIO ON";
        }
        else if (PlayerPrefs.GetInt("Sound") == 0)
        {
            audioMusic.Stop();
            textAudio.text = "AUDIO OFF";
        }

        if (PlayerPrefs.GetInt("GuideThrow") == 1)
        {

            textThrowGuide.text = "THROW GUIDE ON";
        }
        else if (PlayerPrefs.GetInt("GuideThrow") == 0)
        {
            textThrowGuide.text = "THROW GUIDE OFF";
        }

        if (PlayerPrefs.GetInt("OneShot") == 1)
        {
            textOneShot.text = "ONE SHOT ON";
            sliderTimeThrow.gameObject.SetActive(false);
            setImpulseButton.gameObject.SetActive(true);
            fieldOneShot.gameObject.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("OneShot") == 0)
        {
            textOneShot.text = "ONE SHOT OFF";
            sliderTimeThrow.gameObject.SetActive(true);
            setImpulseButton.gameObject.SetActive(false);
            fieldOneShot.gameObject.SetActive(false);
        }

        if (!PlayerPrefs.HasKey("ThrowSensitivity"))
        {
            PlayerPrefs.SetFloat("ThrowSensitivity", 0.8f);
            playerLogic.throwTime = 0.8f;
            sourceMovement.maxLimitImpulse = 0.8f;
            setSlideTimeThrow();
        }
        else
        {
            playerLogic.throwTime = PlayerPrefs.GetFloat("ThrowSensitivity");
            sourceMovement.maxLimitImpulse = PlayerPrefs.GetFloat("ThrowSensitivity");
            setSlideTimeThrow();
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
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            audioMusic.Play();
            textAudio.text = "AUDIO ON";
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            audioMusic.Stop();
            textAudio.text = "AUDIO OFF";
        }
    }

    public void setSwitchThrowGuide()
    {
        if (PlayerPrefs.GetInt("GuideThrow") == 0)
        {
            PlayerPrefs.SetInt("GuideThrow", 1);
            textThrowGuide.text = "THROW GUIDE ON";
        }
        else if (PlayerPrefs.GetInt("GuideThrow") == 1)
        {
            PlayerPrefs.SetInt("GuideThrow", 0);
            textThrowGuide.text = "THROW GUIDE OFF";
        }
    }

    public void setSwitchOneShot()
    {
        if (PlayerPrefs.GetInt("OneShot") == 0)
        {
            PlayerPrefs.SetInt("OneShot", 1);
            textOneShot.text = "ONE SHOT ON";
            setImpulseButton.gameObject.SetActive(true);
            fieldOneShot.gameObject.SetActive(true);
            sliderTimeThrow.gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("OneShot") == 1)
        {
            PlayerPrefs.SetInt("OneShot", 0);
            textOneShot.text = "ONE SHOT OFF";
            setImpulseButton.gameObject.SetActive(false);
            fieldOneShot.gameObject.SetActive(false);
            sliderTimeThrow.gameObject.SetActive(true);
        }
    }

    public void setTimeThrow() {
        PlayerPrefs.SetFloat("ThrowSensitivity", sliderTimeThrow.value);
        playerLogic.throwTime = sliderTimeThrow.value;
        sourceMovement.maxLimitImpulse = sliderTimeThrow.value;
        textSliderTimeThrow.text = "IMPULSE SENSITIVITY: " + sliderTimeThrow.value.ToString("0.0");
    }

    public void setSlideTimeThrow() {
        PlayerPrefs.SetFloat("ThrowSensitivity", sliderTimeThrow.value);
        sliderTimeThrow.value = playerLogic.throwTime;
        textSliderTimeThrow.text = "IMPULSE SENSITIVITY: " + sliderTimeThrow.value.ToString("0.0");
    }

    public void setOneShotImpulse()
    {
        
        sourceMovement.oneShotImpulse = float.Parse(fieldOneShot.text);
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
