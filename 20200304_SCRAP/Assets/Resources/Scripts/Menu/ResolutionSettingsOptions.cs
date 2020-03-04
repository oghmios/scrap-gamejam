using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettingsOptions : MonoBehaviour {

    private Dropdown dropdown; 

	// Use this for initialization
	void Start () {
        dropdown = GetComponent<Dropdown>();
        List<string> nameresolutions = new List<string>();

        int j = 0;
        for (int i = 0; i < Screen.resolutions.Length;i++){
            nameresolutions.Add(Screen.resolutions[i].width + "x" + Screen.resolutions[i].height);

            if (Screen.currentResolution.width == Screen.resolutions[i].width && Screen.currentResolution.height == Screen.resolutions[i].height)
                j = i;  

        }

        dropdown.AddOptions(nameresolutions);
        dropdown.value = j;
        dropdown.RefreshShownValue();
	}

    public void setChangeResolution(){
        
        string[] stringChar = dropdown.options[dropdown.value].text.Split('x');
        Screen.SetResolution(int.Parse(stringChar[0]), int.Parse(stringChar[1]), true);
        dropdown.RefreshShownValue();
    }
	
}
