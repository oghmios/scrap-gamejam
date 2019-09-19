using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlKeyboardJoystick : MonoBehaviour {

	public Transform panelKeyboard;
	public Transform panelJoystick;
    public Button buttonKeyboardPanel;
    public Button buttonJoystickPanel;
   
	void Start () {

        setChangePanelKeyboard();
        
	}

	public void setChangePanelKeyboard(){

		panelJoystick.gameObject.SetActive(false);
		panelKeyboard.gameObject.SetActive(true);
        buttonKeyboardPanel.Select();
    }

	public void setChangePanelJoystick(){
       
        //GetComponent<Button>().colors = 
        panelKeyboard.gameObject.SetActive(false);
		panelJoystick.gameObject.SetActive(true);
        buttonJoystickPanel.Select();
    }


}
