using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerLogicPvP : ContainerLogic
{

    public Text textContainerScoreAddP2;
    public Text textContainerScoreFailP2;
    public Text textContainerComboP2;
    public Text textContainerComboFinishP2;
    public Text textContainerHitBirdP2;

    public ComboSystemLogic comboSystemLogicP2;

    public override void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Bullet")
        {
            // AQUI DETECTAMOS SI ES DE UN JUGADOR O DE OTRO
            // PLAYER 1
            if (other.GetComponent<GranadeLogic>().IsFromPlayer1)
            {
                if (other.GetComponent<GranadeLogic>() != null && typeOfBlock == other.GetComponent<GranadeLogic>().typeBullet)
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

                    // IF THERE IS NO COMBO (NONE OR FINISH) --> START
                    // IF THERE IS COMBO (START OR CONTINUE) --> CONTINUE
                    if (comboSystemLogic.IsComboNone())
                        comboSystemLogic.setComboStart(typeOfBlock);
                    else if (comboSystemLogic.IsComboFinish())
                        comboSystemLogic.setComboStart(typeOfBlock);
                    else if (comboSystemLogic.IsComboStart() || comboSystemLogic.IsComboContinue())
                        comboSystemLogic.setComboContinue(this, typeOfBlock);

                    // PLAYER 1
                    gameLogic.AddScore(true, typeOfBlock, bulletHitsBird);

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

                // PLAYER 2
            } else if (!other.GetComponent<GranadeLogic>().IsFromPlayer1) {
                if (other.GetComponent<GranadeLogic>() != null && typeOfBlock == other.GetComponent<GranadeLogic>().typeBullet)
                {
                    textContainerHitBirdP2.text = "";
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
                        else
                        {
                            if (myTransform.position.x > other.transform.position.x)
                                rbody.AddForce(other.transform.position * 0.5f);
                            else
                                rbody.AddForce(other.transform.position * -0.5f);
                        }
                    }
                    other.GetComponent<BulletDestroyScript>().DestroyBall();

                    switch (Random.Range(0, 9))
                    {
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
                        textContainerScoreAddP2.text = "+25";
                    else if (typeOfBlock == 1)
                        textContainerScoreAddP2.text = "+75";
                    else if (typeOfBlock == 2)
                        textContainerScoreAddP2.text = "+125";
                    else if (typeOfBlock == 3)
                        textContainerScoreAddP2.text = "+250";

                    textContainerScoreAddP2.GetComponent<Animator>().SetTrigger("AddScore");

                    if (bulletHitsBird)
                    {
                        textContainerHitBirdP2.text = "HIT PAJAROTO\nBONUS!+50";
                        textContainerHitBirdP2.GetComponent<Animator>().SetTrigger("AddBonus");
                    }
                    else
                    {
                        textContainerHitBirdP2.text = "";
                    }

                    // IF THERE IS NO COMBO (NONE OR FINISH) --> START
                    // IF THERE IS COMBO (START OR CONTINUE) --> CONTINUE
                    if (comboSystemLogicP2.IsComboNone())
                        comboSystemLogicP2.setComboStart(typeOfBlock);
                    else if (comboSystemLogicP2.IsComboFinish())
                        comboSystemLogicP2.setComboStart(typeOfBlock);
                    else if (comboSystemLogicP2.IsComboStart() || comboSystemLogicP2.IsComboContinue())
                        comboSystemLogicP2.setComboContinue(this, typeOfBlock);

                    gameLogic.AddScore(false, typeOfBlock, bulletHitsBird);


                }
                else
                {
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

                    textContainerScoreFailP2.text = "X";
                    textContainerScoreFailP2.GetComponent<Animator>().SetTrigger("AddFail");

                    // If you fail, cut the combo system
                    if (comboSystemLogicP2.IsComboStart() || comboSystemLogicP2.IsComboContinue())
                        comboSystemLogicP2.setComboFinish();

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
    }

    public override void PlayCombo(int comboN, bool isPlayer1) {
        if (isPlayer1)
        {
            textContainerCombo.text = "Combo X" + comboN;
            textContainerCombo.GetComponent<Animator>().SetTrigger("AddCombo");
            textContainerComboFinish.text = "";
        }
        else if (!isPlayer1) {
            textContainerComboP2.text = "Combo X" + comboN;
            textContainerComboP2.GetComponent<Animator>().SetTrigger("AddCombo");
            textContainerComboFinishP2.text = "";
        }
    }

    public override void PlayComboFinish(int comboN, int comboScore, bool isComboVariety, bool isPlayer1)
    {
        if (isPlayer1)
        {
            textContainerComboFinish.text = "";
            if (comboN == 2)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox2, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinish.text = "VARIETY SUPER COMBO\n" + "+" + comboScore.ToString();
                else
                    textContainerComboFinish.text = "SUPER COMBO\n" + "+" + comboScore.ToString();
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
        } else if (!isPlayer1) {
            textContainerComboFinishP2.text = "";
            if (comboN == 2)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox2, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "VARIETY SUPER COMBO\n" + "+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "SUPER COMBO\n" + "+" + comboScore.ToString();
            }
            else if (comboN == 3)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox3, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "VARIETY MASTER COMBO!\n+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "MASTER COMBO!\n+" + comboScore.ToString();
            }
            else if (comboN == 4)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox4, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "VARIETY SAVAGE COMBO!!\n+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "SAVAGE COMBO!!\n+" + comboScore.ToString();
            }
            else if (comboN == 5)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox5, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "VARIETY HYPER COMBO!!\n+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "HYPER COMBO!!\n+" + comboScore.ToString();
            }
            else if (comboN == 6)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox6, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "VARIETY GOLDEN COMBO!!\n+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "GOLDEN COMBO!!\n+" + comboScore.ToString();
            }
            else if (comboN == 7)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox7, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "VARIETY ADAMANTIUM COMBO!!\n+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "ADAMANTIUM COMBO!!\n+" + comboScore.ToString();
            }
            else if (comboN == 8)
            {
                CoreManager.Audio.Play(CoreManager.Audio.combox8, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "MYSTIC OBSIDIAN\nVARIETY COMBO!!\n+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "MYSTIC OBSIDIAN COMBO!!\n+" + comboScore.ToString();
            }
            else if (comboN > 8)
            {
                CoreManager.Audio.Play(CoreManager.Audio.comboxFINAL, myTransform.position);

                if (isComboVariety)
                    textContainerComboFinishP2.text = "VARIETY ALPHA\nSUPER HYPER\n ULTRA EX' TURBOCOMBOOO!!!\n+" + comboScore.ToString();
                else
                    textContainerComboFinishP2.text = "ALPHA SUPER HYPER\n ULTRA EX' TURBOCOMBOOO!!!\n+" + comboScore.ToString();
            }

            textContainerComboFinishP2.GetComponent<Animator>().SetTrigger("AddComboFinish");
        }
    }

    public override void ResetContainer(bool isPlayer1)
    {
        if (isPlayer1)
        {
            textContainerCombo.text = "";
            textContainerComboFinish.text = "";
        } else if (!isPlayer1) {
            textContainerComboP2.text = "";
            textContainerComboFinishP2.text = "";
        }
    }
  
}
