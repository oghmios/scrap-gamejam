using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystemLogic : MonoBehaviour {

    // COMBO
    public enum ComboStates { NONE, START, CONTINUE, FINISH }
    [Header("COMBO SETTINGS")]
    public ComboStates comboState;
    public float tempCombo = 0;
    public float tempIniCombo = 2;
    public int comboCount;
    public ContainerLogic lastContainerLogic;
    public int comboScore2 = 10, comboScore3 = 25, comboScore4 = 50, 
        comboScore5 = 55, comboScore6 = 60, comboScore7 = 70 , comboScore8 = 75, comboScoreMax = 100;
    public GameLogic gameLogic;
    public List<int> listCombo;
    private bool isComboVariety = false;
    private Transform myTransform;
    public bool isPlayer1 = true;

    // Use this for initialization
    void Start () {
        myTransform = this.transform;
        // INITIALIZE COMBO
        setComboNone();
    }
	
	// Update is called once per frame
	void Update () {
        switch (comboState)
        {

            case ComboStates.NONE:
                ComboNoneBehaviour();
                break;

            case ComboStates.START:
                ComboStartBehaviour();
                break;

            case ComboStates.CONTINUE:
                ComboContinueBehaviour();
                break;

            case ComboStates.FINISH:
                ComboFinishBehaviour();
                break;

        }
    }

    // COMBO SET STATES

    public void setComboNone()
    {
        tempCombo = 0;
        comboCount = 0;

        // if (lastContainerLogic!=null)
        // lastContainerLogic.ResetContainer();
        listCombo.Clear();
        lastContainerLogic = null;
        comboState = ComboStates.NONE;
    }

    public void setComboStart(int typeOfBlockAux)
    {
        tempCombo = tempIniCombo;
        comboCount = 1;
        listCombo.Add(typeOfBlockAux);
        CoreManager.Audio.Play(CoreManager.Audio.comboHitx1, myTransform.position);
        comboState = ComboStates.START;
    }

    public void setComboContinue(ContainerLogic containerLogicAux, int typeOfBlockAux)
    {
        tempCombo = tempIniCombo;
        comboCount += 1;
        listCombo.Add(typeOfBlockAux);
        
        lastContainerLogic = containerLogicAux;
        if (comboCount >= 2)
        {
            lastContainerLogic.PlayCombo(comboCount, isPlayer1);
        }

        if (comboCount == 2)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitx2, myTransform.position);
        else if (comboCount == 3)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitx3, myTransform.position);
        else if (comboCount == 4)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitx4, myTransform.position);
        else if (comboCount == 5)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitx5, myTransform.position);
        else if (comboCount == 6)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitx6, myTransform.position);
        else if (comboCount == 7)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitx7, myTransform.position);
        else if (comboCount == 8)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitx8, myTransform.position);
        else if (comboCount > 8)
            CoreManager.Audio.Play(CoreManager.Audio.comboHitxFINAL, myTransform.position);

        comboState = ComboStates.CONTINUE;
    }

    public void setComboFinish()
    {


        if (comboCount >= 2 && lastContainerLogic != null)
        {
            setPlayComboFinish(comboCount);
        }
        else {
            listCombo.Clear();
        }

        // Debug.Log("COMBO: " + comboCount+" !!");
        comboState = ComboStates.FINISH;
    }

    private bool checkIfComboVariety() {

        int i = 0;

        if (listCombo.Contains(0))
            i++;
        if (listCombo.Contains(1))
            i++;
        if (listCombo.Contains(2))
            i++;
        if (listCombo.Contains(3))
            i++;

        listCombo.Clear();

        if (i > 1)
                return true;
            else
                return false;
    }

    private void setPlayComboFinish(int comboCountAux)
    {
        isComboVariety = checkIfComboVariety();

        if (comboCountAux == 2)
        {
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScore2 * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScore2 * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScore2, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScore2, isPlayer1);
            }
        }
        else if (comboCountAux == 3)
        {
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScore3 * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScore3 * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScore3, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScore3, isPlayer1);
            }
        }
        else if (comboCountAux == 4)
        {
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScore4 * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScore4 * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScore4, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScore4, isPlayer1);
            }
        }
        else if (comboCountAux == 5)
        {
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScore5 * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScore5 * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScore5, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScore5, isPlayer1);
            }
        }
        else if (comboCountAux == 6)
        {
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScore6 * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScore6 * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScore6, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScore6, isPlayer1);
            }
        }
        else if (comboCountAux == 7)
        {
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScore7 * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScore7 * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScore7, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScore7, isPlayer1);
            }
        }
        else if (comboCountAux == 8)
        {     
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScore8 * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScore8 * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScore8, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScore8, isPlayer1);
            }
        }
        else if (comboCountAux > 8)
        {
            // GAME LOGIC CALL
            if (isComboVariety)
            {
                lastContainerLogic.PlayComboFinish(comboCount, Mathf.RoundToInt(comboScoreMax * 1.25f), isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(Mathf.RoundToInt(comboScoreMax * 1.25f), isPlayer1);
            }
            else
            {
                lastContainerLogic.PlayComboFinish(comboCount, comboScoreMax, isComboVariety, isPlayer1);
                gameLogic.AddScoreCombo(comboScoreMax, isPlayer1);
            }
        }

    }

    // CHECKS COMBO STATES

    public bool IsComboNone()
    {
        if (comboState == ComboStates.NONE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsComboStart()
    {
        if (comboState == ComboStates.START)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsComboContinue()
    {
        if (comboState == ComboStates.CONTINUE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsComboFinish()
    {
        if (comboState == ComboStates.FINISH)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // COMBO BNEHAVIOUR STATES

    private void ComboNoneBehaviour()
    {

    }

    private void ComboStartBehaviour()
    {
        tempCombo -= Time.deltaTime;

        if (tempCombo < 0)
            setComboFinish();
    }

    private void ComboContinueBehaviour()
    {
        tempCombo -= Time.deltaTime;

        if (tempCombo < 0)
            setComboFinish();
    }

    private void ComboFinishBehaviour()
    {

        setComboNone();
    }
}
