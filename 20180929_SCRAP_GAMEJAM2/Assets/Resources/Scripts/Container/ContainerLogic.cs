using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ContainerLogic : MonoBehaviour
{

    public enum ContainerLogicStates { SLEEP, OPEN, CLOSE, ALWAYSOPEN };
    public ContainerLogicStates state;

    public Text textContainerScoreAdd;
    public Text textContainerScoreFail;
    public Text textContainerCombo;
    public Text textContainerComboFinish;

    public AudioManager audioManger;
    public int typeOfBlock;
    public int randomChange;
    public float timeSleep;
    public float timeOpened;
    private float timeSleepDecay;
    private float timeOpenedDecay;
    private GameLogic gameLogic;
    public ParticleSystem ps;
    public Rigidbody rbJoin;

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
                rbJoin.AddForce(other.transform.position);
                Destroy(other.gameObject);
                audioManger.Play(audioManger.playerLaughtShort, transform.position);

                if (typeOfBlock == 0)
                    textContainerScoreAdd.text = "+25";
                else if(typeOfBlock == 1)
                    textContainerScoreAdd.text = "+75";
                else if (typeOfBlock == 2)
                    textContainerScoreAdd.text = "+125";
                else if (typeOfBlock == 3)
                    textContainerScoreAdd.text = "+250";

                textContainerScoreAdd.GetComponent<Animator>().SetTrigger("AddScore");

                //StartCoroutine(WaitAddScoreAnimation());

                // IF THERE IS NO COMBO (NONE OR FINISH) --> START
                // IF THERE IS COMBO (START OR CONTINUE) --> CONTINUE
                if (gameLogic.IsComboNone())
                    gameLogic.setComboStart();
                else if(gameLogic.IsComboFinish())
                    gameLogic.setComboStart();
                else if (gameLogic.IsComboStart() || gameLogic.IsComboContinue())
                    gameLogic.setComboContinue(this);

                gameLogic.AddScore(typeOfBlock);
            }
            else {
                rbJoin.AddForce(other.transform.position);
                audioManger.Play(audioManger.damagePlayer, transform.position);

                textContainerScoreFail.text = "X";
                textContainerScoreFail.GetComponent<Animator>().SetTrigger("AddFail");

                // If you fail, cut the combo system
                if (gameLogic.IsComboStart() || gameLogic.IsComboContinue())
                    gameLogic.setComboFinish();

                //StartCoroutine(WaitFailScoreAnimation());
                // PENALIZA SCORE
                ps.Play();
                
                Destroy(other.gameObject);
                gameLogic.AddPenalty(typeOfBlock);
                

            }
        }
    }

    public void PlayCombo(int comboN) {
        textContainerCombo.text = "Combo X"+comboN;
        textContainerCombo.GetComponent<Animator>().SetTrigger("AddCombo");
        textContainerComboFinish.text = "";
    }

    public void PlayComboFinish(int comboN, int comboScore)
    {
        textContainerComboFinish.text = "";
        if (comboN == 2)
        {
            textContainerComboFinish.text = "SUPER COMBO\n" + "+"+ comboScore.ToString();
        }
        else if (comboN == 3)
        {
            textContainerComboFinish.text = "MASTER COMBO!\n+" + comboScore.ToString();
        }
        else if (comboN == 4)
        {
            textContainerComboFinish.text = "HYPER COMBO!!\n+" + comboScore.ToString();
        }
        else if (comboN > 4)
        {
            textContainerComboFinish.text = "ULTRAA COMBOOO!!!\n+" + comboScore.ToString();
        }

        textContainerComboFinish.GetComponent<Animator>().SetTrigger("AddComboFinish");
    }

    public void ResetContainer()
    {       
           textContainerCombo.text = "";
           textContainerComboFinish.text = "";
    }

        IEnumerator WaitAddScoreAnimation() {
        
        
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator WaitFailScoreAnimation()
    {
        
        
        yield return new WaitForSeconds(0.5f);
    }
}
