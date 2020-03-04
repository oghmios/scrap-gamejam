using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TutorialLogic : MonoBehaviour {

	public Transform[] panelTutorials;

	private int tutorialOption;

	// Use this for initialization
	void Start () {

        desactivateAllTutorials();

        tutorialOption = 0;
	
        panelTutorials[tutorialOption].gameObject.SetActive(true);
    }

    public void nextTip(){
        
        panelTutorials[tutorialOption].gameObject.SetActive(false);

        tutorialOption++;

        if (tutorialOption >= panelTutorials.Length)
        {
            tutorialOption = 0;
        }

        panelTutorials[tutorialOption].gameObject.SetActive(true);
    }

    public void prevTip()
    {

        panelTutorials[tutorialOption].gameObject.SetActive(false);

        tutorialOption--;
        if(tutorialOption < 0){
            tutorialOption = panelTutorials.Length-1;
        }

        panelTutorials[tutorialOption].gameObject.SetActive(true);
    }

    public void desactivateAllTutorials(){
        for (int i = 0; i < panelTutorials.Length; i++)
        {
            panelTutorials[tutorialOption].gameObject.SetActive(false);
        }
    }

}
