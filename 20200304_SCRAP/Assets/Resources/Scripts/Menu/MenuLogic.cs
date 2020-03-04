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
    public Transform panelPlayerOptions;
	public EventSystem eventsystem;
	public GameObject[] buttonsMainMenu;
    public GameObject buttonResume;
	public GameObject buttonControls;
	public GameObject buttonTutorial;
	public GameObject buttonStages;
    public GameObject buttonGraphics;
    public GameObject buttonP1PlayerControls;
	public int prevOption;
    public Text textAudio;
    public Text textThrowGuide, textThrowGuide2;
    public Text textOneShot, textOneShot2;
    public Slider sliderTimeThrow, sliderTimeThrow2;
    public Text textSliderTimeThrow, textSliderTimeThrow2;
    public SourceMovement sourceMovement, sourceMovement2;
    public PlayerLogic playerLogic, playerLogic2;
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

        if (PlayerPrefs.GetInt("GuideThrow_J1") == 1)
        {
            textThrowGuide.text = "P1 THROW GUIDE ON";
        } else if (PlayerPrefs.GetInt("GuideThrow_J1") == 0)
        {
            textThrowGuide.text = "P1 THROW GUIDE OFF";
        }

        if (PlayerPrefs.GetInt("GuideThrow_J2") == 1)
        {
            if(textThrowGuide2!=null)
            textThrowGuide2.text = "P2 THROW GUIDE ON";
        }
        else if (PlayerPrefs.GetInt("GuideThrow_J2") == 0)
        {
            if (textThrowGuide2 != null)
                textThrowGuide2.text = "P2 THROW GUIDE OFF";
        }

        if (PlayerPrefs.GetInt("OneShot_J1") == 1)
        {
            textOneShot.text = "P1 ONE SHOT ON";
            sliderTimeThrow.gameObject.SetActive(false);
        } else if (PlayerPrefs.GetInt("OneShot_J1") == 0)
        {
            textOneShot.text = "P1 ONE SHOT OFF";
            sliderTimeThrow.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("OneShot_J2") == 1)
        {
            if (textOneShot2 != null && sliderTimeThrow2)
            {
                textOneShot2.text = "P2 ONE SHOT ON";
                sliderTimeThrow2.gameObject.SetActive(false);
            }
        }
        else if (PlayerPrefs.GetInt("OneShot_J2") == 0)
        {
            if (textOneShot2 != null && sliderTimeThrow2)
            {
                textOneShot2.text = "P2 ONE SHOT OFF";
                sliderTimeThrow2.gameObject.SetActive(true);
            }
        }

        if (!PlayerPrefs.HasKey("ThrowSensitivity_J1"))
        {
            PlayerPrefs.SetFloat("ThrowSensitivity_J1", 0.8f);
            playerLogic.throwTime = 0.8f;
            sourceMovement.maxLimitImpulse = 0.8f;
            setSlideTimeThrow("_J1");
        }
        else
        {
            playerLogic.throwTime = PlayerPrefs.GetFloat("ThrowSensitivity_J1");
            sourceMovement.maxLimitImpulse = PlayerPrefs.GetFloat("ThrowSensitivity_J1");
            setSlideTimeThrow("_J1");
        }

        if (!PlayerPrefs.HasKey("ThrowSensitivity_J2"))
        {
            if (playerLogic2 != null && sourceMovement2 != null)
            {
                PlayerPrefs.SetFloat("ThrowSensitivity_J2", 0.8f);
                playerLogic2.throwTime = 0.8f;
                sourceMovement2.maxLimitImpulse = 0.8f;
                setSlideTimeThrow("_J2");
            }
        }
        else
        {
            if (playerLogic2 != null && sourceMovement2 != null)
            {
                playerLogic2.throwTime = PlayerPrefs.GetFloat("ThrowSensitivity_J2");
                sourceMovement2.maxLimitImpulse = PlayerPrefs.GetFloat("ThrowSensitivity_J2");
                setSlideTimeThrow("_J2");
            }
        }

        if (panelTutorial!=null)
		panelTutorial.gameObject.SetActive(false);

        if (panelPlayerOptions != null)
            panelPlayerOptions.gameObject.SetActive(false);

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
        if (panelPlayerOptions != null)
            panelPlayerOptions.gameObject.SetActive(false);
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

    public void GoToPlayerOptions()
    {
        if (panelControls != null)
            panelControls.gameObject.SetActive(false);
        if (panelStage != null)
            panelStage.gameObject.SetActive(false);
        if (panelMainMenu != null)
            panelMainMenu.gameObject.SetActive(false);
        if (panelTutorial != null)
            panelTutorial.gameObject.SetActive(false);
        if (panelGraphics != null && panelGraphics.gameObject != null)
        {
            panelGraphics.gameObject.SetActive(false);
            prevOption = 4;
        }
        else
        {
            prevOption = 1;
        }
        
        panelPlayerOptions.gameObject.SetActive(true);

        prevOption = 3;
        eventsystem.SetSelectedGameObject(buttonP1PlayerControls);

    }

    public void GoToGraphics()
    {
        panelStage.gameObject.SetActive(false);
        panelMainMenu.gameObject.SetActive(false);
        panelTutorial.gameObject.SetActive(false);
        panelControls.gameObject.SetActive(false);

        if (panelGraphics != null)
        panelGraphics.gameObject.SetActive(true);

        if (panelPlayerOptions != null)
            panelPlayerOptions.gameObject.SetActive(false);

        eventsystem.SetSelectedGameObject(buttonGraphics);
        prevOption = 2;
    }

	public void GoToTutorial(){
        if (panelPlayerOptions != null)
            panelPlayerOptions.gameObject.SetActive(false);

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
        if (panelPlayerOptions != null)
            panelPlayerOptions.gameObject.SetActive(false);

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

    public void setSwitchThrowGuide(string player)
    {
        if (player == "_J1")
        {
            if (PlayerPrefs.GetInt("GuideThrow" + player) == 0)
            {
                PlayerPrefs.SetInt("GuideThrow" + player, 1);
                textThrowGuide.text = "P1 THROW GUIDE ON";
            }
            else if (PlayerPrefs.GetInt("GuideThrow" + player) == 1)
            {
                PlayerPrefs.SetInt("GuideThrow" + player, 0);
                textThrowGuide.text = "P1 THROW GUIDE OFF";
            }
        } else if (player == "_J2") {
            if (textThrowGuide2 != null)
            {
                if (PlayerPrefs.GetInt("GuideThrow" + player) == 0)
                {
                    PlayerPrefs.SetInt("GuideThrow" + player, 1);
                    textThrowGuide2.text = "P2 THROW GUIDE ON";
                }
                else if (PlayerPrefs.GetInt("GuideThrow" + player) == 1)
                {
                    PlayerPrefs.SetInt("GuideThrow" + player, 0);
                    textThrowGuide2.text = "P2 THROW GUIDE OFF";
                }
            }
        }
    }

    public void setSwitchOneShot(string player)
    {
        if (player == "_J1")
        {
            if (PlayerPrefs.GetInt("OneShot"+player) == 0)
            {
                PlayerPrefs.SetInt("OneShot"+player, 1);
                textOneShot.text = "P1 ONE SHOT ON";
                sliderTimeThrow.gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("OneShot"+player) == 1)
            {
                PlayerPrefs.SetInt("OneShot"+player, 0);
                textOneShot.text = "P1 ONE SHOT OFF";
                sliderTimeThrow.gameObject.SetActive(true);
            }
        } else if (player == "_J2")
        {
            if (textOneShot2 != null && sliderTimeThrow2 != null)
            {
                if (PlayerPrefs.GetInt("OneShot" + player) == 0)
                {
                    PlayerPrefs.SetInt("OneShot" + player, 1);
                    textOneShot2.text = "P2 ONE SHOT ON";
                    sliderTimeThrow2.gameObject.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("OneShot" + player) == 1)
                {
                    PlayerPrefs.SetInt("OneShot" + player, 0);
                    textOneShot2.text = "P2 ONE SHOT OFF";
                    sliderTimeThrow2.gameObject.SetActive(true);
                }
            }
        }
    }

    public void setTimeThrow(string player) {
        if (player == "_J1")
        {
            PlayerPrefs.SetFloat("ThrowSensitivity"+player, sliderTimeThrow.value);
            playerLogic.throwTime = sliderTimeThrow.value;
            sourceMovement.maxLimitImpulse = sliderTimeThrow.value;
            textSliderTimeThrow.text = "P1 IMPULSE SENSITIVITY: " + sliderTimeThrow.value.ToString("0.0");
        } else if (player == "_J2")
        {
            if (playerLogic2 != null && sourceMovement2 != null && textSliderTimeThrow2 != null) {
                
                    PlayerPrefs.SetFloat("ThrowSensitivity" + player, sliderTimeThrow2.value);
                    playerLogic2.throwTime = sliderTimeThrow2.value;
                    sourceMovement2.maxLimitImpulse = sliderTimeThrow2.value;
                    textSliderTimeThrow2.text = "P2 IMPULSE SENSITIVITY: " + sliderTimeThrow2.value.ToString("0.0");
                }
        }
    }

    public void setSlideTimeThrow(string player) {
        if (player == "_J1")
        {
            PlayerPrefs.SetFloat("ThrowSensitivity"+player, sliderTimeThrow.value);
            sliderTimeThrow.value = playerLogic.throwTime;
            textSliderTimeThrow.text = "P1 IMPULSE SENSITIVITY: " + sliderTimeThrow.value.ToString("0.0");
        } else if (player == "_J2")
        {
            if (sliderTimeThrow2 != null && textSliderTimeThrow2 != null)
            {
                PlayerPrefs.SetFloat("ThrowSensitivity" + player, sliderTimeThrow2.value);
                sliderTimeThrow2.value = playerLogic2.throwTime;
                textSliderTimeThrow2.text = "P2 IMPULSE SENSITIVITY: " + sliderTimeThrow2.value.ToString("0.0");
            }
        }
    }
    /*
    public void setOneShotImpulse()
    {
        
        sourceMovement.oneShotImpulse = float.Parse(fieldOneShot.text);
    }*/

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
        else if (prevOption == 3)
            eventsystem.SetSelectedGameObject(buttonsMainMenu[3].gameObject);
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
