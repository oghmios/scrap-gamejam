using System.Collections;
using System.Collections.Generic;
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
    public Text textContainerHitBird;

    public GameLogic gameLogic;
    public SpriteRenderer spriteContainer;
    private Color spriteContainerColor;
    public int typeOfBlock;
    public int randomChange;
    public float timeSleep;
    public float timeOpened;
    private float timeSleepDecay;
    private float timeOpenedDecay;
    private Transform myTransform;
    public bool bulletHitsBird = false;
    public ComboSystemLogic comboSystemLogic;
    public ParticleSystem psContainerFumes, psContainerStones;
    public Rigidbody rbody;

    // Use this for initialization
    void Start()
    {
        // CoreManager.Audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        // gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        myTransform = this.transform;
        timeSleepDecay = timeSleep;
        timeOpenedDecay = timeOpened;
        spriteContainerColor = spriteContainer.color;
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
                textContainerHitBird.text = "";
                bulletHitsBird = false;
                bulletHitsBird = other.GetComponent<GranadeLogic>().isTouchedBird;

                // RESET BULLET HIT BIRD
                StartCoroutine(ColorGoodContainer());

                // SI ES PÉNDULO, QUE APLIQUE FUERZA
                if (GetComponent<HingeJoint>() != null)
                {
                    if (myTransform.position.x > 0 && other.transform.position.x > 0)
                    {
                        if (myTransform.position.x > other.transform.position.x)
                            rbody.AddForce(other.transform.position * 0.5f);
                        else
                            rbody.AddForce(other.transform.position * -0.5f);
                    }
                    else if (myTransform.position.x < 0 && other.transform.position.x < 0)
                    {
                        if (myTransform.position.x > other.transform.position.x)
                            rbody.AddForce(other.transform.position * -2f);
                        else
                            rbody.AddForce(other.transform.position * 2f);
                    }
                    else {
                        if (myTransform.position.x > other.transform.position.x)
                            rbody.AddForce(other.transform.position * 0.5f);
                        else
                            rbody.AddForce(other.transform.position * -0.5f);
                    }
                }
                other.GetComponent<BulletDestroyScript>().DestroyBall();
                // Destroy(other.gameObject);

                switch (Random.Range(0, 9)) {
                    case 0:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort00, myTransform.position);
                        break;
                    case 1:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort01, myTransform.position);
                        break;
                    case 2:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort02, myTransform.position);
                        break;
                    case 3:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort03, myTransform.position);
                        break;
                    case 4:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort04, myTransform.position);
                        break;
                    case 5:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort05, myTransform.position);
                        break;
                    case 6:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort06, myTransform.position);
                        break;
                    case 7:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort07, myTransform.position);
                        break;
                    case 8:
                        CoreManager.Audio.Play(CoreManager.Audio.playerLaughtShort08, myTransform.position);
                        break;
                }

                    if (typeOfBlock == 0)
                        textContainerScoreAdd.text = "+25";
                    else if (typeOfBlock == 1)
                        textContainerScoreAdd.text = "+75";
                    else if (typeOfBlock == 2)
                        textContainerScoreAdd.text = "+125";
                    else if (typeOfBlock == 3)
                        textContainerScoreAdd.text = "+250";

                textContainerScoreAdd.GetComponent<Animator>().SetTrigger("AddScore");

                if (bulletHitsBird)
                {
                    textContainerHitBird.text = "HIT PAJAROTO\nBONUS!+50";
                    textContainerHitBird.GetComponent<Animator>().SetTrigger("AddBonus");
                }
                else {
                    textContainerHitBird.text = "";
                }
                //StartCoroutine(WaitAddScoreAnimation());

                // IF THERE IS NO COMBO (NONE OR FINISH) --> START
                // IF THERE IS COMBO (START OR CONTINUE) --> CONTINUE
                if (comboSystemLogic.IsComboNone())
                    comboSystemLogic.setComboStart(typeOfBlock);
                else if(comboSystemLogic.IsComboFinish())
                    comboSystemLogic.setComboStart(typeOfBlock);
                else if (comboSystemLogic.IsComboStart() || comboSystemLogic.IsComboContinue())
                    comboSystemLogic.setComboContinue(this, typeOfBlock);

                gameLogic.AddScore(typeOfBlock, bulletHitsBird);

                
            }
            else {
                // FALLA LA PUNTUACION
                StartCoroutine(ColorBadContainer());

                // SI ES PÉNDULO, QUE APLIQUE FUERZA
                if (GetComponent<HingeJoint>() != null)
                {
                    if (myTransform.position.x > 0 && other.transform.position.x > 0)
                    {
                        if (myTransform.position.x > other.transform.position.x)
                            rbody.AddForce(other.transform.position * 2f);
                        else
                            rbody.AddForce(other.transform.position * -2f);
                    }
                    else if (myTransform.position.x < 0 && other.transform.position.x < 0)
                    {
                        if (myTransform.position.x > other.transform.position.x)
                            rbody.AddForce(other.transform.position * -4f);
                        else
                            rbody.AddForce(other.transform.position * 4f);
                    }
                    else
                    {
                        if (myTransform.position.x > other.transform.position.x)
                            rbody.AddForce(other.transform.position * 2f);
                        else
                            rbody.AddForce(other.transform.position * -2f);
                    }
                }

                if (typeOfBlock == 10) // JAIL
                    CoreManager.Audio.Play(CoreManager.Audio.jailHit, myTransform.position);
                else
                    CoreManager.Audio.Play(CoreManager.Audio.BagWrong, myTransform.position);

                textContainerScoreFail.text = "X";
                textContainerScoreFail.GetComponent<Animator>().SetTrigger("AddFail");

                // If you fail, cut the combo system
                if (comboSystemLogic.IsComboStart() || comboSystemLogic.IsComboContinue())
                    comboSystemLogic.setComboFinish();

                //StartCoroutine(WaitFailScoreAnimation());
                // PENALIZA SCORE
                psContainerFumes.Play();
                psContainerStones.Play();

                //Destroy(other.gameObject);
                other.GetComponent<BulletDestroyScript>().DestroyBall();
                gameLogic.AddPenalty(typeOfBlock);
                

            }

            // restart bird
            bulletHitsBird = false;
        }
    }

    public void PlayCombo(int comboN) {
        textContainerCombo.text = "Combo X"+comboN;
        textContainerCombo.GetComponent<Animator>().SetTrigger("AddCombo");
        textContainerComboFinish.text = "";
    }

    public void PlayComboFinish(int comboN, int comboScore, bool isComboVariety)
    {
        textContainerComboFinish.text = "";
        if (comboN == 2)
        {
            CoreManager.Audio.Play(CoreManager.Audio.combox2, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "VARIETY SUPER COMBO\n" + "+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "SUPER COMBO\n" + "+"+ comboScore.ToString();
        }
        else if (comboN == 3)
        {
            CoreManager.Audio.Play(CoreManager.Audio.combox3, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "VARIETY MASTER COMBO!\n+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "MASTER COMBO!\n+" + comboScore.ToString();
        }
        else if (comboN == 4)
        {
            CoreManager.Audio.Play(CoreManager.Audio.combox4, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "VARIETY SAVAGE COMBO!!\n+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "SAVAGE COMBO!!\n+" + comboScore.ToString();
        }
        else if (comboN == 5)
        {
            CoreManager.Audio.Play(CoreManager.Audio.combox5, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "VARIETY HYPER COMBO!!\n+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "HYPER COMBO!!\n+" + comboScore.ToString();
        }
        else if (comboN == 6)
        {
            CoreManager.Audio.Play(CoreManager.Audio.combox6, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "VARIETY GOLDEN COMBO!!\n+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "GOLDEN COMBO!!\n+" + comboScore.ToString();
        }
        else if (comboN == 7)
        {
            CoreManager.Audio.Play(CoreManager.Audio.combox7, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "VARIETY ADAMANTIUM COMBO!!\n+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "ADAMANTIUM COMBO!!\n+" + comboScore.ToString();
        }
        else if (comboN == 8)
        {
            CoreManager.Audio.Play(CoreManager.Audio.combox8, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "MYSTIC OBSIDIAN\nVARIETY COMBO!!\n+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "MYSTIC OBSIDIAN COMBO!!\n+" + comboScore.ToString();
        }
        else if (comboN > 8)
        {
            CoreManager.Audio.Play(CoreManager.Audio.comboxFINAL, myTransform.position);

            if (isComboVariety)
                textContainerComboFinish.text = "VARIETY ALPHA\nSUPER HYPER\n ULTRA EX' TURBOCOMBOOO!!!\n+" + comboScore.ToString();
            else
                textContainerComboFinish.text = "ALPHA SUPER HYPER\n ULTRA EX' TURBOCOMBOOO!!!\n+" + comboScore.ToString();
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

    IEnumerator ColorBadContainer()
    {
        spriteContainer.GetComponent<Animator>().SetTrigger("ContainerFail");
        spriteContainer.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        spriteContainer.color = spriteContainerColor;
    }

    IEnumerator ColorGoodContainer()
    {
        spriteContainer.GetComponent<Animator>().SetTrigger("ContainerSuccess");
        spriteContainer.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        spriteContainer.color = spriteContainerColor;


    }
}
