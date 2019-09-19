using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GetInputJoystickControl : MonoBehaviour
{
    public enum InputControlState { None, AssignControl }
    public InputControlState state;

    public string inputControl;
    public KeyTarget keyTarget;
    public Text textControl;
    public bool inputAxis = false;
    public int joystickNum;
    public EventSystem eventSystem;
    
    // Use this for initialization
    void Start()
    {

        if (!inputAxis)
        textControl.text = (hInput.DetailsFromKey(inputControl, keyTarget)).ToString();
        
        setNone();
    }

    void Update()
    {
        switch (state)
        {
            case InputControlState.None:    
                NoneBehaviour();
                break;
            case InputControlState.AssignControl:
                AssignControlBehaviour();
                break;
        }

    }

    // SETS

    public void setNone()
    {
        state = InputControlState.None;
    }

    public void setAssignControl()
    {
        textControl.text = "PRESS";

        state = InputControlState.AssignControl;
    }
    
    // BEHAVIOURS
    private void NoneBehaviour()
    {
            // ASIGNAR CON EL MANDO (ACTUALMENTE NO FUNCIONA)
            if (hInput.GetButton("Submit_J1") && eventSystem.currentSelectedGameObject == this.gameObject)
            {
                setAssignControl();
            }
        
    }
  
    private void AssignControlBehaviour()
    {

            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey) && !Input.GetKey(KeyCode.Return) && !Input.GetMouseButtonDown(0))
                {
                    if (vKey.ToString().Contains("Joystick"))
                    {
                        string[] joyChar = vKey.ToString().Split(new string[] { "Joystick" }, System.StringSplitOptions.RemoveEmptyEntries);

                        if (joyChar[0] != null && joyChar[0] != "" && joyChar[0].Contains("Button"))
                        {
                            hInput.SetKey(inputControl, (KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + joystickNum.ToString() + joyChar[0]), keyTarget);
                        }
                        else
                            hInput.SetKey(inputControl, vKey, keyTarget);
                    }
                    else
                    {
                        hInput.SetKey(inputControl, vKey, keyTarget);
                    }

                    textControl.text = (hInput.DetailsFromKey(inputControl, keyTarget)).ToString();

                    if (hInput.DetailsFromKey("Submit_J1_Joystick", KeyTarget.PositivePrimary).ToString().Contains("Joystick") &&
                       hInput.DetailsFromKey("Submit_J1_Joystick", KeyTarget.PositivePrimary).ToString().Contains("Button0") &&
                          textControl.text.Contains("Joystick") && textControl.text.Contains("Button0"))
                    {
                        //inputModuleStand.submitButton = "Submit_J1";
                        //canSubmit = false;
                    }


                    inputAxis = true;
                    setNone();
                    return;

                }
            }

            if (Input.GetAxis("CrosspadX_J1") < -0.5f || Input.GetAxis("CrosspadX_J1") > 0.5f)
            {
                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis7, HardShellStudios.CompleteControl.TargetController.Joystick1);
                textControl.text = "J1 CROSSPAD X";
                inputAxis = true;
                setNone();
                return;
            }

            if (Input.GetAxis("CrosspadY_J1") < -0.5f || Input.GetAxis("CrosspadY_J1") > 0.5f)
            {
                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis8, HardShellStudios.CompleteControl.TargetController.Joystick1);
                textControl.text = "J1 CROSSPAD Y";
                inputAxis = true;
                setNone();
                return;
            }

            if (Input.GetAxis("TriggerLeft_J1") < -0.5f || Input.GetAxis("TriggerLeft_J1") > 0.5f)
            {
                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis9, HardShellStudios.CompleteControl.TargetController.Joystick1);
                textControl.text = "JOYSTICK1 TL";
                inputAxis = true;
                setNone();
                return;
            }

            if (Input.GetAxis("TriggerRight_J1") < -0.5f || Input.GetAxis("TriggerRight_J1") > 0.5f)
            {
                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis10, HardShellStudios.CompleteControl.TargetController.Joystick1);
                textControl.text = "JOYSTICK1 TR";
                inputAxis = true;
                setNone();
                return;
            }

            if (Input.GetAxis("Horizontal_J1") < -0.5f || Input.GetAxis("Horizontal_J1") > 0.5f)
            {

                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis1, HardShellStudios.CompleteControl.TargetController.Joystick1);
                textControl.text = "JOYSTICK1 X AXIS";
                inputAxis = true;
                setNone();
                return;
            }



            if (Input.GetAxis("Horizontal_J2") < -0.5f || Input.GetAxis("Horizontal_J2") > 0.5f)
            {

                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis1, HardShellStudios.CompleteControl.TargetController.Joystick2);
                textControl.text = "JOYSTICK2 X AXIS";
                inputAxis = true;
                setNone();
                return;
            }



            if (Input.GetAxis("Vertical_J1") < -0.5f || Input.GetAxis("Vertical_J1") > 0.5f)
            {

                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis2, HardShellStudios.CompleteControl.TargetController.Joystick1);
                textControl.text = "JOYSTICK1 Y AXIS";
                inputAxis = true;
                setNone();
                return;
            }



            if (Input.GetAxis("Vertical_J2") < -0.5f || Input.GetAxis("Vertical_J2") > 0.5f)
            {

                hInput.SetKey(inputControl, HardShellStudios.CompleteControl.AxisCode.Axis2, HardShellStudios.CompleteControl.TargetController.Joystick2);
                textControl.text = "JOYSTICK2 Y AXIS";
                inputAxis = true;
                setNone();
                return;
            }
    }
}
