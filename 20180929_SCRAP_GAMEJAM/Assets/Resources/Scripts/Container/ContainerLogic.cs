using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ContainerLogic : MonoBehaviour
{

    public enum ContainerLogicStates { SLEEP, OPEN, CLOSE, ALWAYSOPEN };
    public ContainerLogicStates state;



    public AudioManager audioManger;
    public int typeOfBlock;
    public int randomChange;
    public float timeSleep;
    public float timeOpened;
    private float timeSleepDecay;
    private float timeOpenedDecay;
    private GameLogic gameLogic;
    public ParticleSystem ps;

    // Use this for initialization
    void Start()
    {
        audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        timeSleepDecay = timeSleep;
        timeOpenedDecay = timeOpened;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case ContainerLogicStates.SLEEP:
                SleepBehaviour();
                break;
            case ContainerLogicStates.OPEN:
                OpenBehaviour();
                break;
            case ContainerLogicStates.CLOSE:
                CloseBehaviour();
                break;
            case ContainerLogicStates.ALWAYSOPEN:
                AlwaysOpenBehaviour();
                break;

        }
    }



    // SETS

    public void SetSleep()
    {
        state = ContainerLogicStates.SLEEP;
    }

    public void SetOpen()
    {
        state = ContainerLogicStates.OPEN;
    }

    public void SetClose()
    {
        state = ContainerLogicStates.OPEN;
    }

    public void SetAlwaysOpen()
    {
        state = ContainerLogicStates.ALWAYSOPEN;
    }

    // BEHAVIOURS

    void SleepBehaviour()
    {
        timeSleepDecay -= Time.deltaTime;

        if (timeSleepDecay < 0)
        {
            SetOpen();
        }
    }

    void OpenBehaviour()
    {
        timeOpenedDecay -= Time.deltaTime;

        if (timeOpenedDecay < 0)
        {
            SetClose();
        }
    }

    void CloseBehaviour()
    {
        SetSleep();

        System.Random random = new System.Random();
        float randomTime = random.Next(0, randomChange);
        float upOrDown = random.Next(0, 2);

        if (upOrDown == 0)
        {
            timeSleepDecay = timeSleep - randomTime;
        }
        else
        {
            timeSleepDecay = timeSleep + randomTime;
        }

        randomTime = random.Next(0, randomChange);
        upOrDown = random.Next(0, 2);

        if (upOrDown == 0)
        {
            timeOpenedDecay = timeOpened - randomTime;
        }
        else
        {
            timeOpenedDecay = timeOpened + randomTime;
        }
    }

    void AlwaysOpenBehaviour()
    {

    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Bullet")
        {
            
            if (other.GetComponent<GranadeLogic>()!=null && typeOfBlock == other.GetComponent<GranadeLogic>().typeBullet)
            {
                Destroy(other.gameObject);
                gameLogic.AddScore(typeOfBlock);
            }
            else {
                audioManger.Play(audioManger.damagePlayer, transform.position);
                // PENALIZA SCORE
                ps.Play();
                Destroy(other.gameObject);
                gameLogic.AddPenalty(typeOfBlock);
                

            }
        }
    }
}
