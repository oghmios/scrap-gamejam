using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailCantainerLogic : MonoBehaviour {

    public enum JailContainerStates { JAIL_ON, JAIL_INCREASE, JAIL_OFF, JAIL_DECREASE }
    public JailContainerStates state;

    public ContainerLogic containerLogic;
    public Animator animJailBag;

    private int prevBlockType;
    private int JailBlockType = 10;
    private Transform myTransform;

    private float temp;
    public float timeOn, timeIncrease, timeOff, timeDecrease;

    // Use this for initialization
    void Start () {
        myTransform = this.transform;
        prevBlockType = containerLogic.typeOfBlock;
        setJailOff(); 
	}
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case JailContainerStates.JAIL_ON:
                JailOnBehaviour();
                break;
            case JailContainerStates.JAIL_INCREASE:
                JailIncreaseBehaviour();
                break;
            case JailContainerStates.JAIL_OFF:
                JailOffBehaviour();
                break;
            case JailContainerStates.JAIL_DECREASE:
                JailDecreaseBehaviour();
                break;
        }
    }

    // SETS
    public void setJailOn() {
        containerLogic.typeOfBlock = JailBlockType;
        temp = timeOn;
        animJailBag.speed = 1;
        animJailBag.SetTrigger("JailOn");
        CoreManager.Audio.Play(CoreManager.Audio.jailOn, myTransform.position);

        state = JailContainerStates.JAIL_ON;
    }

    public void setJailIncrease()
    {
        containerLogic.typeOfBlock = prevBlockType;
        temp = timeIncrease;
        animJailBag.speed = animJailBag.speed/timeIncrease;
        animJailBag.SetTrigger("JailIncrease");
        CoreManager.Audio.Play(CoreManager.Audio.jailIncrease, myTransform.position);
        state = JailContainerStates.JAIL_INCREASE;
    }

    public void setJailOff()
    {
        containerLogic.typeOfBlock = prevBlockType;
        temp = timeOff;
        animJailBag.speed = 1;
        animJailBag.SetTrigger("JailOff");
        CoreManager.Audio.Play(CoreManager.Audio.jailOff, myTransform.position);
        state = JailContainerStates.JAIL_OFF;
    }

    public void setJailDecrease()
    {
        containerLogic.typeOfBlock = prevBlockType;
        temp = timeDecrease;
        animJailBag.speed = animJailBag.speed/timeDecrease;
        animJailBag.SetTrigger("JailDecrease");
        CoreManager.Audio.Play(CoreManager.Audio.jailDecrease, myTransform.position);
        state = JailContainerStates.JAIL_DECREASE;
    }

    // BEHAVIOURS
    private void JailOnBehaviour() {
        temp -= Time.deltaTime;

        if (temp < 0) {
            setJailDecrease();
        }
    }

    private void JailIncreaseBehaviour()
    {
        temp -= Time.deltaTime;

        if (temp < 0)
        {
            setJailOn();
        }
    }

    private void JailOffBehaviour()
    {
        temp -= Time.deltaTime;

        if (temp < 0)
        {
            setJailIncrease();
        }
    }

    private void JailDecreaseBehaviour()
    {
        temp -= Time.deltaTime;

        if (temp < 0)
        {
            setJailOff();
        }
    }
}
