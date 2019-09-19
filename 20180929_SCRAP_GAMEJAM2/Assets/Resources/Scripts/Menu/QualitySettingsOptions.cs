using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsOptions : MonoBehaviour {
    private Dropdown dropdown;

	// Use this for initialization
	void Start () {
        List<string> nameQualitySettigns = new List<string>(QualitySettings.names);
        dropdown = GetComponent<Dropdown>();

        dropdown.AddOptions(nameQualitySettigns);
        dropdown.value = QualitySettings.GetQualityLevel();

	}
	
    public void setChangeQualitySettings()
    {
        QualitySettings.SetQualityLevel(dropdown.value);

    }

}
