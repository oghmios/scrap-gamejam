using UnityEngine;
using UnityEngine.UI;

public class GetInputControl : MonoBehaviour
{
    public enum InputControlState { None, AssignControl }
    public InputControlState state;

    public string inputControl;
    public KeyTarget keyTarget;
    public Text textControl;
    public bool inputAxis = false;
    public int joystickNum;
    // Use this for initialization
    void Start()
    {
        // hInput.SetKey("Attack_J1_Keyboard", KeyCode.O);
        // UnityEngine.KeyCode keyButton = hInput.DetailsFromKey(inputControl, keyTarget)

        if (!inputAxis)
        textControl.text = (hInput.DetailsFromKey(inputControl, keyTarget)).ToString();
        
    }

    private void Update()
    {
        switch (state)
        {
            case InputControlState.None:
                NoneBehaviour();
                break;
            case InputControlState.AssignControl:
                AssignControlBehaviour();
                break;
            default:
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
                inputAxis = true;
                setNone();
                return;
            }
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
