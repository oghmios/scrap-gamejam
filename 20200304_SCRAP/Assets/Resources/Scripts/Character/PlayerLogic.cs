using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerLogic: MonoBehaviour {


	public enum PlayerStates {IDLE, DIE, DIG, DIG_TO_IDLE, DASHDOWN, DASHDOWN_DIG, THROW_BULLET, CANCEL_THROW, NONE}
	public PlayerStates state;

    public string control;
    public GameLogic gameLogic;
	public Transform explosionBoss;
	public float lifeIniPlayer;
	private float lifePlayer;
	public Transform explosionPlayer;
	public Transform explosionPowerUp;
	public SourceMovement sourceBullets;
    public MoveCharacter moveCharacter;
	public Animator animatorCharacter;
	public SpriteRenderer spriteCharacter;
    public UIChangeColor colorIntenvory;

    public BoxCollider colliderDig;

	public Sprite[] pieces;
	public Image piece1, piece2, piece3, piece4, pieceGlow;

	private float temp;

	public Queue<int> piecesChar; 

	public float fireRate = 1F;
	private float nextFire = 0.0F;
    private bool isThrow;
    public float throwTime;
    private float throwTimeCounter;
    private float gravityOrig;
    public float gravityMultiplier = 4;
    public float tempDig = 2;
    public float tempDigDown = 1;
    public float tempDigToIdle = 0.5f;
    public Color colorPrepareThrow, colorPrepareThrowMax;
    public LineRenderer lineThrow;
    public TrailRenderer lineDigDash; 
    private bool isCanceledThrow;
    public Transform shovelDie;
    private Transform myTransform;
    private float percentMediumThrow;
    private bool increaseThrow;
    private bool isPoison;
    public float timePoison;
    private float timePoisonCounter, tempColor;
    private int iColor;
    private Color colorShow = new Color(1, 1, 1, 1);
    private Color colorHide = new Color(0, 0, 0, 0);

    // GENERAL POINT1
    private Vector3 shotPoint1N = new Vector3(-1.25f, 0.4f, 0);
    private Vector3 shotPoint1 = new Vector3(1.25f, 0.4f, 0);
    
    // ONE SHOT - NEGATIVE
    private Vector3 oneShotPoint2N = new Vector3(-8.75f, 17.5f, 0);
    private Vector3 oneShotPoint3N = new Vector3(-17f, 5.31f, 0);

    // ONE SHOT - POSITIVE
    private Vector3 oneShotPoint2 = new Vector3(8.75f, 17.5f, 0);
    private Vector3 oneShotPoint3 = new Vector3(17f, 5.31f, 0);

    // SHORT SHOT
    private Vector3 shortPoint2N = new Vector3(-4.375f, 8.75f, 0);
    private Vector3 shortPoint3N = new Vector3(-8.5f, 2.65625f, 0);

    // SHORT SHOT
    private Vector3 shortPoint2 = new Vector3(4.375f, 8.75f, 0);
    private Vector3 shortPoint3 = new Vector3(8.5f, 2.65625f, 0);

    // LARGE SHOT
    private Vector3 LongPoint2N = new Vector3(-17.5f, 35f, 0);
    private Vector3 LongPoint3N = new Vector3(-34, 10.625f, 0);

    // LARGE SHOT
    private Vector3 LongPoint2 = new Vector3(17.5f, 35f, 0);
    private Vector3 LongPoint3 = new Vector3(34, 10.625f, 0);

private void Start(){
        myTransform = this.transform;
        gravityOrig = moveCharacter.gravity;

        if (PlayerPrefs.GetInt("GuideThrow"+control) == 1 && PlayerPrefs.GetInt("OneShot"+control) == 0)
            lineThrow.enabled = true;
        else
            lineThrow.enabled = false;

        if (!PlayerPrefs.HasKey("ThrowSensitivity"+control))
        {
            PlayerPrefs.SetFloat("ThrowSensitivity"+control, 0.8f);
            throwTime = 0.8f;
        }
        else
        {
            throwTime = PlayerPrefs.GetFloat("ThrowSensitivity"+control);
        }

        piecesChar = new Queue<int>();

        pieceGlow.sprite = null;
        pieceGlow.color = colorHide;
        spriteCharacter.enabled = true;
        isThrow = false;
        isCanceledThrow = false;
        lineDigDash.enabled = false;
        increaseThrow = true;
        isPoison = false;
        iColor = 0;
        tempColor = 0.05f;
        setIdle();
	}

    public Vector3 point1, point2, point3;
    public int vertexCount = 12;
    List<Vector3> pointList;

    void Update()
    {

        switch (state)
        {
            case PlayerStates.IDLE:
                IdleBehaviour();
                break;
            case PlayerStates.DIG:
                DigBehaviour();
                break;
            case PlayerStates.DIG_TO_IDLE:
                DigToIdleBehaviour();
                break;
            case PlayerStates.DASHDOWN:
                DashDownBehaviour();
                break;
            case PlayerStates.DASHDOWN_DIG:
                DashDownDigBehaviour();
                break;
            case PlayerStates.THROW_BULLET:
                ThrowBulletBehaviour();
                break;
            case PlayerStates.CANCEL_THROW:
                CancelThrowBehaviour();
                break;
            case PlayerStates.DIE:
                DieBehaviour();
                break;
            case PlayerStates.NONE:
                NoneBehaviour();
                break;
        }

        if (state != PlayerStates.DIE)
        {
            if (isPoison) {
                timePoisonCounter -= Time.deltaTime;

                tempColor -= Time.deltaTime;

                if (tempColor < 0)
                {

                    if (iColor == 0)
                    {
                        spriteCharacter.color = Color.red;
                        iColor++;
                        tempColor = 0.05f;
                    }
                    else if (iColor == 1)
                    {
                        spriteCharacter.color = Color.yellow;
                        iColor++;
                        tempColor = 0.05f;
                    }
                    else if (iColor == 2)
                    {
                        spriteCharacter.color = Color.white;
                        iColor = 0;
                        tempColor = 0.05f;
                    }

                }

                if (timePoisonCounter < 0) {
                    isPoison = false;
                    spriteCharacter.color = Color.white;
                    moveCharacter.speed = moveCharacter.speed * 2;
                    moveCharacter.jumpSpeed = moveCharacter.jumpSpeed * 2;
                }
            }

            // BOTON PARA TRANSFORMAR
            if ((hInput.GetButton("Dig" + control) || hInput.GetAxis("Dig" + control) > 0 || hInput.GetAxis("Dig" + control) < 0) && moveCharacter.isGround && Time.time > nextFire &&
                state == PlayerStates.IDLE && (!hInput.GetButton("Throw" + control) || hInput.GetAxis("Throw" + control) == 0))
            {

                nextFire = Time.time + fireRate;

                setDig();
            }
            else if ((hInput.GetButton("Dig" + control) || hInput.GetAxis("Dig" + control) > 0 || hInput.GetAxis("Dig" + control) < 0) && !moveCharacter.isGround && Time.time > nextFire
                && state == PlayerStates.IDLE && (!hInput.GetButton("Throw" + control) || hInput.GetAxis("Throw" + control) == 0))
            {

                nextFire = Time.time + fireRate;

                setDashDown();
            }/*
        else if(Input.GetButton("Fire1") && (state != PlayerStates.DASHDOWN || state != PlayerStates.DASHDOWN_DIG || state != PlayerStates.DIG_TO_IDLE))
        {
            // animatorCharacter.SetBool("isDig", false);
            setIdle();
        }*/

            if ((hInput.GetButtonDown("Throw" + control) || hInput.GetAxis("Throw" + control) < 0 ||
                hInput.GetAxis("Throw" + control) > 0) && !isThrow && piecesChar.Count > 0 && !isCanceledThrow)
            {
                if (PlayerPrefs.GetInt("GuideThrow"+control) == 1 && PlayerPrefs.GetInt("OneShot"+control) == 0)
                    lineThrow.enabled = true;

                isThrow = true;
                // rigid.velocity = Vector3.up * jumpSpeed;

                if (state != PlayerStates.THROW_BULLET)
                {
                    animatorCharacter.SetTrigger("IsPrepareThrow");
                    spriteCharacter.color = colorPrepareThrow;
                    throwTimeCounter = throwTime;
                }
            }


            if ((hInput.GetButtonDown("Throw" + control) || hInput.GetAxis("Throw" + control) < 0 ||
                hInput.GetAxis("Throw" + control) > 0) && !isThrow && piecesChar.Count <= 0 && !isCanceledThrow)
            {
                
                lineThrow.enabled = false;

                colorIntenvory.setAnimation();
            }

            if (isThrow && (hInput.GetButton("Throw" + control) || hInput.GetAxis("Throw" + control) < 0 ||
                hInput.GetAxis("Throw" + control) > 0 && !isCanceledThrow))
            {
                // one shot discrete
                if (PlayerPrefs.GetInt("GuideThrow"+control) == 1 && PlayerPrefs.GetInt("OneShot"+control) == 1)
                {
                    lineThrow.enabled = true;
                    pointList = new List<Vector3>();

                    if (spriteCharacter.flipX)
                    {

                            // ONE SHOT
                            // private Vector3 shotPoint1N = new Vector3(-1.25f, 0.4f, 0);
                            // private Vector3 oneShotPoint2N = new Vector3(-8.75f, 17.5f, 0);
                            // private Vector3 oneShotPoint3N = new Vector3(-17f, 5.31f, 0);

                            // ONE SHOT
                            // point1 = new Vector3(-1.25f, 0.4f, 0);
                            // point2 = new Vector3(-8.75f, 17.5f, 0);
                            // point3 = new Vector3(-17f, 5.31f, 0);

                            // ONE SHOT
                            point1 = shotPoint1N;
                            point2 = oneShotPoint2N;
                            point3 = oneShotPoint3N;

                    }
                    else
                    {
                        // ONE SHOT
                        // private Vector3 shotPoint1 = new Vector3(1.25f, 0.4f, 0);
                        // private Vector3 oneShotPoint2 = new Vector3(8.75f, 17.5f, 0);
                        // private Vector3 oneShotPoint3 = new Vector3(17f, 5.31f, 0);

                        // ONE SHOT
                        // point1 = new Vector3(1.25f, 0.4f, 0);
                        // point2 = new Vector3(8.75f, 17.5f, 0);
                        // point3 = new Vector3(17f, 5.31f, 0);

                        // ONE SHOT
                        point1 = shotPoint1;
                        point2 = oneShotPoint2;
                        point3 = oneShotPoint3;

                    }

                    for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
                    {
                        Vector3 tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
                        Vector3 tangentLineVertex2 = Vector3.Lerp(point2, point3, ratio);
                        Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                        pointList.Add(bezierPoint);
                    }

                    lineThrow.positionCount = pointList.Count;
                    lineThrow.SetPositions(pointList.ToArray());
                }

                // organic shot
                if (throwTimeCounter > 0)
                {
                    if (!increaseThrow && ((throwTime - throwTimeCounter) <= sourceBullets.minLimitImpulse))
                        increaseThrow = true;
                    // rigid.velocity = Vector3.up * jumpSpeed;
                    //setThrowBullet(throwTime - throwTimeCounter);
                    //animatorCharacter.SetTrigger("IsPrepareThrow");

                        // CUANDO ESTÁ EN BUZZER TIME NO DEBE LANZAR TAN LENTO
                    if (gameLogic.state == GameLogic.GameStates.VICTORY || gameLogic.state == GameLogic.GameStates.VICTORY_ULTIMATE)
                    {
                        if (increaseThrow)
                            throwTimeCounter -= Time.deltaTime * 1.25f;
                        else
                            throwTimeCounter += Time.deltaTime * 1.25f;
                    }
                    else
                    {
                        if (increaseThrow)
                            throwTimeCounter -= Time.deltaTime;
                        else
                            throwTimeCounter += Time.deltaTime;
                    }

                    // SHOW THROW GUIDES
                    if (PlayerPrefs.GetInt("GuideThrow"+control) == 1 && PlayerPrefs.GetInt("OneShot"+control) == 0)
                    {
                        lineThrow.enabled = true;
                        pointList = new List<Vector3>();

                        if ((throwTime - throwTimeCounter) <= sourceBullets.minLimitImpulse)
                        {
                            if (spriteCharacter.flipX)
                            {
                                    // SHORT SHOT
                                    // private Vector3 shotPoint1N = new Vector3(-1.25f, 0.4f, 0);
                                    // private Vector3 shortPoint2N = new Vector3(-4.375f, 8.75f, 0);
                                    // private Vector3 shortPoint3N = new Vector3(-8.5f, 2.65625f, 0);

                                    // SHORT SHOT
                                    // point1 = new Vector3(-1.25f, 0.4f, 0);
                                    // point2 = new Vector3(-4.375f, 8.75f, 0);
                                    // point3 = new Vector3(-8.5f, 2.65625f, 0);

                                    // SHORT SHOT
                                    point1 = shotPoint1N;
                                    point2 = shortPoint2N;
                                    point3 = shortPoint3N;
                            }
                            else
                            {
                                // SHORT SHOT
                                // private Vector3 shotPoint1 = new Vector3(1.25f, 0.4f, 0);
                                // private Vector3 shortPoint2 = new Vector3(4.375f, 8.75f, 0);
                                // private Vector3 shortPoint3 = new Vector3(8.5f, 2.65625f, 0);

                                // SHORT SHOT
                                // point1 = new Vector3(1.25f, 0.4f, 0);
                                // point2 = new Vector3(4.375f, 8.75f, 0);
                                // point3 = new Vector3(8.5f, 2.65625f, 0);

                                // SHORT SHOT
                                point1 = shotPoint1;
                                point2 = shortPoint2;
                                point3 = shortPoint3;
                            }
                            
                        }
                        else if ((throwTime - throwTimeCounter) > sourceBullets.minLimitImpulse && (throwTime - throwTimeCounter) < sourceBullets.maxLimitImpulse)
                        {
                            percentMediumThrow = ((((throwTime - throwTimeCounter) - sourceBullets.minLimitImpulse)) / (sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse));


                            if (spriteCharacter.flipX)
                            {
                                // MEDIUM SHOT
                                // private Vector3 shotPoint1N = new Vector3(-1.25f, 0.4f, 0);
                                // point1 = new Vector3(-1.25f, 0.4f, 0);
                                point1 = shotPoint1N;

                                point2 = new Vector3(-(4.375f * 0.7f + (percentMediumThrow * (17.5f - 4.375f))),
                                       (8.75f * 0.7f + (percentMediumThrow * (35f - 8.75f))), 0);

                                point3 = new Vector3(-(8.5f * 0.7f + (percentMediumThrow * (34 - 8.5f))),
                                       (2.65625f * 0.7f + (percentMediumThrow * (10.625f - 2.65625f))), 0);

                                /*
                                point2 = new Vector3(-(((throwTime - throwTimeCounter) * 13.125f) / ((sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse) / 2 + sourceBullets.minLimitImpulse)), 
                                        (((throwTime - throwTimeCounter) * 26.25f) / ((sourceBullets.maxLimitImpulse-sourceBullets.minLimitImpulse)/2 + sourceBullets.minLimitImpulse)), 0);

                                point3 = new Vector3(-(((throwTime - throwTimeCounter) * 25.5f) / ((sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse) / 2 + sourceBullets.minLimitImpulse)), 
                                        (((throwTime - throwTimeCounter) * 7.96875f) / ((sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse) / 2 + sourceBullets.minLimitImpulse)), 0);
                                */
                            }
                            else
                            {
                                // MEDIUM SHOT
                                // private Vector3 shotPoint1 = new Vector3(1.25f, 0.4f, 0);
                                // point1 = new Vector3(1.25f, 0.4f, 0);
                                point1 = shotPoint1;
                                point2 = new Vector3(4.375f * 0.7f + (percentMediumThrow * (17.5f - 4.375f)),
                                       8.75f * 0.7f + (percentMediumThrow * (35f - 8.75f)), 0);

                                point3 = new Vector3(8.5f * 0.7f + (percentMediumThrow * (34 - 8.5f)),
                                       2.65625f*0.7f + (percentMediumThrow * (10.625f - 2.65625f)), 0);

                               // Debug.Log("PERCENT: " + percentMediumThrow + " point2: "+ point2 + "point3: "+point3);

                                /*
                               point2 = new Vector3((((throwTime - throwTimeCounter) * 10) / ((sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse) / 2 + sourceBullets.minLimitImpulse)), 
                                       (((throwTime - throwTimeCounter) * 18) / ((sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse) / 2 + sourceBullets.minLimitImpulse)), 0);

                                   point3 = new Vector3((((throwTime - throwTimeCounter) * 18) / ((sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse) / 2 + sourceBullets.minLimitImpulse)), 
                                       (((throwTime - throwTimeCounter) * 5) / ((sourceBullets.maxLimitImpulse - sourceBullets.minLimitImpulse) / 2 + sourceBullets.minLimitImpulse)), 0);
                               */
                            }
                           // Debug.Log("MEDIO " + (throwTime - throwTimeCounter).ToString());
                        }
                        else if ((throwTime - throwTimeCounter) >= sourceBullets.maxLimitImpulse)
                        {
   
                            if (spriteCharacter.flipX)
                            {
                                // LARGE SHOT
                                // private Vector3 shotPoint1N = new Vector3(-1.25f, 0.4f, 0);
                                // private Vector3 LongPoint2N = new Vector3(-17.5f, 35f, 0);
                                // private Vector3 LongPoint3N = new Vector3(-34, 10.625f, 0);

                                // LARGE SHOT
                                point1 = shotPoint1N;
                                point2 = LongPoint2N;
                                point3 = LongPoint3N;
                            }
                            else
                            {
                                // LARGE SHOT
                                // private Vector3 shotPoint1 = new Vector3(1.25f, 0.4f, 0);
                                // private Vector3 LongPoint2 = new Vector3(17.5f, 35f, 0);
                                // private Vector3 LongPoint3 = new Vector3(34, 10.625f, 0);

                                // LARGE SHOT
                                // point1 = new Vector3(1.25f, 0.4f, 0);
                                // point2 = new Vector3(17.5f, 35f, 0);
                                // point3 = new Vector3(34, 10.625f, 0);

                                // LARGE SHOT
                                point1 = shotPoint1;
                                point2 = LongPoint2;
                                point3 = LongPoint3;
                            }
                           // Debug.Log("LARGO "+ (throwTime - throwTimeCounter).ToString());
                        }


                        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
                        {
                            Vector3 tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
                            Vector3 tangentLineVertex2 = Vector3.Lerp(point2, point3, ratio);
                            Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                            pointList.Add(bezierPoint);
                        }

                        lineThrow.positionCount = pointList.Count;
                        lineThrow.SetPositions(pointList.ToArray());
                    }
                    spriteCharacter.color = colorPrepareThrow;
                }
                else
                {
                    if (PlayerPrefs.GetInt("GuideThrow"+control) == 1 && PlayerPrefs.GetInt("OneShot"+control) == 0)
                    {
                        lineThrow.enabled = true;
                        pointList = new List<Vector3>();
                        if (spriteCharacter.flipX)
                        {
                            // LARGE SHOT
                            // private Vector3 shotPoint1N = new Vector3(-1.25f, 0.4f, 0);
                            // private Vector3 LongPoint2N = new Vector3(-17.5f, 35f, 0);
                            // private Vector3 LongPoint3N = new Vector3(-34, 10.625f, 0);

                            // LARGE SHOT
                            // point1 = new Vector3(-1.25f, 0.4f, 0);
                            // point2 = new Vector3(-17.5f, 35f, 0);
                            // point3 = new Vector3(-34, 10.625f, 0);

                            // LARGE SHOT
                            point1 = shotPoint1N;
                            point2 = LongPoint2N;
                            point3 = LongPoint3N;
                        }
                        else
                        {
                            // LARGE SHOT
                            // private Vector3 shotPoint1 = new Vector3(1.25f, 0.4f, 0);
                            // private Vector3 LongPoint2 = new Vector3(17.5f, 35f, 0);
                            // private Vector3 LongPoint3 = new Vector3(34, 10.625f, 0);

                            // LARGE SHOT
                            // point1 = new Vector3(1.25f, 0.4f, 0);
                            // point2 = new Vector3(17.5f, 35f, 0);
                            // point3 = new Vector3(34, 10.625f, 0);

                            // LARGE SHOT
                            point1 = shotPoint1;
                            point2 = LongPoint2;
                            point3 = LongPoint3;
                        }


                        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
                        {
                            Vector3 tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
                            Vector3 tangentLineVertex2 = Vector3.Lerp(point2, point3, ratio);
                            Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                            pointList.Add(bezierPoint);
                        }

                        lineThrow.positionCount = pointList.Count;
                        lineThrow.SetPositions(pointList.ToArray());
                    }

                    

                        throwTimeCounter = 0;
                        increaseThrow = false;
                        throwTimeCounter = 0.001f;
                    // animatorCharacter.SetTrigger("IsPrepareThrow");
                    if (PlayerPrefs.GetInt("OneShot"+control) == 1)
                        spriteCharacter.color = colorPrepareThrow;
                    else
                        spriteCharacter.color = colorPrepareThrowMax;
                    // isThrow = false;
                }

                

                // Si se mantiene el lanzamiento y se da la tecla cavar, 
                // se cancela  el lanzamiento
                if (hInput.GetButton("Dig" + control) || hInput.GetAxis("Dig" + control) != 0)
                {

                    isThrow = false;
                    lineThrow.enabled = false;
                    isCanceledThrow = true;
                    animatorCharacter.SetTrigger("isForcedToIdle");
                    spriteCharacter.color = Color.white;
                    setCancelThrow();
                    //setIdle();
                }
            }

            if (isThrow && (hInput.GetButtonUp("Throw" + control) || hInput.GetAxis("Throw" + control) == 0) && !isCanceledThrow)
            {
                lineThrow.enabled = false;
                //animatorCharacter.SetBool("IsPrepareThrow", false);            
                setThrowBullet(throwTime - throwTimeCounter);

                spriteCharacter.color = Color.white;
            }

        }

    }

	public void setIdle(){
        moveCharacter.horizontalControl = true;
        moveCharacter.gravity = gravityOrig;
        colliderDig.enabled = false;
        myTransform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		state = PlayerStates.IDLE;
	}

    public void setCancelThrow() {
        state = PlayerStates.CANCEL_THROW;
    }

    public void setDig()
    {
        if (gameLogic.state != GameLogic.GameStates.START)
        {
            temp = tempDig;
            if (piecesChar.Count < 4 && moveCharacter.isGround)
            {
                CameraShake.Shake(Vector3.one * 0.5f, 0.25f);
                CoreManager.Audio.Play(CoreManager.Audio.playerDigImpossible, myTransform.position, Random.Range(0.8f, 1.2f));
                animatorCharacter.SetTrigger("IsDig");
                CameraShake.Shake(Vector3.one * 0.25f, 0.25f);
                colliderDig.enabled = true;
                state = PlayerStates.DIG;
            }
            else if (piecesChar.Count >= 4 && moveCharacter.isGround)
            {
                colorIntenvory.setAnimation();
                setIdle();
            }
        }
    }

    public void setDashDown()
    {
        if (gameLogic.state != GameLogic.GameStates.START)
        {
            CoreManager.Audio.Play(CoreManager.Audio.playerDigDash, myTransform.position, Random.Range(0.8f, 1.2f));
            animatorCharacter.SetBool("IsDigDash", true);
            lineDigDash.enabled = true;

            moveCharacter.horizontalControl = false;
            moveCharacter.gravity = gravityOrig * gravityMultiplier;


            state = PlayerStates.DASHDOWN;
        }
    }

    public void setDashDownDig()
    {
        if (gameLogic.state != GameLogic.GameStates.START)
        {
            moveCharacter.horizontalControl = false;
            temp = tempDigDown;
            moveCharacter.gravity = gravityOrig;

            if (piecesChar.Count < 4 && moveCharacter.isGround)
            {
                lineDigDash.enabled = false;
                CoreManager.Audio.Play(CoreManager.Audio.playerDigImpossible, myTransform.position, Random.Range(0.8f, 1.2f));
                animatorCharacter.SetBool("IsDigDash", true);
                CameraShake.Shake(Vector3.one * 0.5f, 0.35f);
                colliderDig.enabled = true;
                state = PlayerStates.DASHDOWN_DIG;
            }
            else if (piecesChar.Count >= 4 && moveCharacter.isGround)
            {
                lineDigDash.enabled = false;
                animatorCharacter.SetBool("IsDigDash", false);
                colorIntenvory.setAnimation();
                setIdle();
            }
        }
    }

    public void setDigToIdle()
    {
        animatorCharacter.SetBool("IsDigDash", false);
        moveCharacter.horizontalControl = true;
        colliderDig.enabled = false;
        temp = tempDigToIdle;
        state = PlayerStates.DIG_TO_IDLE;

    }

	public void setDie(int loseMode){
        //myTransform.GetComponentInChildren<SpriteRenderer>().color = Color.white;

        //animatorCharacter.SetBool("isDie", true);
        //animatorCharacter.transform.localPosition = new Vector3(animatorCharacter.transform.localPosition.x, -0.5f, animatorCharacter.transform.localPosition.z); 
        colorIntenvory.setDead();
        animatorCharacter.SetTrigger("isDied");
        
        // PLAYER DEAD  == 0
        if (loseMode == 0)
            CoreManager.Audio.Play(CoreManager.Audio.playerDie, myTransform.position);
        
        shovelDie.gameObject.SetActive(true);
        shovelDie.transform.parent = null;
        moveCharacter.enabled = false;
        GetComponent<Rigidbody>().mass = 10;
        GetComponent<Rigidbody>().angularDrag = 100;
        GetComponent<Rigidbody>().drag = 1;
        GetComponent<CapsuleCollider>().radius = 0;
        GetComponent<CapsuleCollider>().height = 0;
        
		// GameObject explosion = (GameObject)Instantiate(explosionPlayer.gameObject, myTransform.position,Quaternion.identity);
		//Destroy (explosion,2);
		temp = 3f;
		
        state = PlayerStates.DIE;
	}

    public void setThrowBullet(float forceExpulsion) {

        
        if (piecesChar.Count > 0 && sourceBullets.state == SourceMovement.PlayerAttackStates.NONE)
        {
            CoreManager.Audio.Play(CoreManager.Audio.playerThrow, myTransform.position, Random.Range(0.8f, 1.2f));
            // temp = 2;
            // SI SUPERA LAS PIEZAS A ACUMULAR SE QUITA LA ULTIMA
            int pieceMode = piecesChar.Dequeue();

            if (piecesChar.Count == 3) {
                piece4.sprite = null;
                piece4.color = colorHide;
            } else if (piecesChar.Count == 2) {
                piece3.sprite = null;
                piece3.color = colorHide;
            }
            else if (piecesChar.Count == 1)
            {
                piece2.sprite = null;
                piece2.color = colorHide;
            }
            else if (piecesChar.Count == 0)
            {
                piece1.sprite = null;
                piece1.color = colorHide;

                pieceGlow.sprite = null;
                pieceGlow.color = colorHide;
            }

            int i = 0;
            foreach (int piece in piecesChar)
            {
                
                if (i == 0)
                {

                    if (piece == 0)
                        piece1.sprite = pieces[0];


                    if (piece == 1)
                        piece1.sprite = pieces[1];

                    if (piece == 2)
                        piece1.sprite = pieces[2];

                    if (piece == 3)
                        piece1.sprite = pieces[3];

                    piece1.color = colorShow;

                }

                if (i == 1)
                {

                    if (piece == 0)
                        piece2.sprite = pieces[0];

                    if (piece == 1)
                        piece2.sprite = pieces[1];

                    if (piece == 2)
                        piece2.sprite = pieces[2];

                    if (piece == 3)
                        piece2.sprite = pieces[3];


                    piece2.color = colorShow;
                }

                if (i == 2)
                {

                    if (piece == 0)
                        piece3.sprite = pieces[0];

                    if (piece == 1)
                        piece3.sprite = pieces[1];

                    if (piece == 2)
                        piece3.sprite = pieces[2];

                    if (piece == 3)
                        piece3.sprite = pieces[3];

                    piece3.color = colorShow;
                }

                if (i == 3)
                {

                    if (piece == 0)
                        piece4.sprite = pieces[0];

                    if (piece == 1)
                        piece4.sprite = pieces[1];

                    if (piece == 2)
                        piece4.sprite = pieces[2];

                    if (piece == 3)
                        piece4.sprite = pieces[3];

                    piece4.color = colorShow;
                }
                i++;
            }
            piece1.GetComponent<Animator>().SetTrigger("isCatched");
            sourceBullets.setRange(pieceMode, forceExpulsion);
            animatorCharacter.SetTrigger("IsThrow");
            temp = 0.1f;
            state = PlayerStates.THROW_BULLET;
        }
        else {
            animatorCharacter.SetTrigger("isForcedToIdle");
            isThrow = false;
            setIdle();
        }
    }

    public void setNone()
    {

        moveCharacter.enabled = false;
        this.enabled = false; 
        state = PlayerStates.NONE;
    }

    // BEHAVIOURS
    private void CancelThrowBehaviour() {
        

        if (!hInput.GetButton("Throw" + control)) {
            isCanceledThrow = false;
            setIdle();
        }
    }

    private void DamageBehaviour(){

		temp -= Time.deltaTime;
		if(temp<0){
			setIdle();
		}
	}

	private void IdleBehaviour(){

	}

    private void DigBehaviour()
    {
        temp -= Time.deltaTime;


        if (temp < 0)
        {
            setDigToIdle();
        }
    }

    private void DashDownBehaviour()
    {

        if (moveCharacter.isGround && state == PlayerStates.DASHDOWN)
        {
            
            setDashDownDig();
        }
    }

    private void DashDownDigBehaviour()
    {
        temp -= Time.deltaTime;


        if (temp < 0)
        {
            setDigToIdle();
        }
    }

    private void DigToIdleBehaviour()
    {
        temp -= Time.deltaTime;


        if (temp <= 0)
        {
            setIdle();
        }
    }

    private void DieBehaviour(){
		temp -= Time.deltaTime;

		if(temp<0){
            
            if (gameLogic.state != GameLogic.GameStates.LOSE)
            {
                if (gameLogic is GameLogicCoop) {
                    
                    gameLogic.GetComponent<GameLogicCoop>().numPlayersLife--;

                    if (gameLogic.GetComponent<GameLogicCoop>().numPlayersLife == 0) {
                        gameLogic.setLose(0);
                    }

                    setNone();

                } else if (gameLogic is GameLogicPvP) {
                    gameLogic.GetComponent<GameLogicPvP>().numPlayersLife--;

                    gameLogic.setLose(2);
                    

                } else if (gameLogic is GameLogic)
                {
                    gameLogic.setLose(0);

                }
                else
                    this.enabled = false;
            }
		}
	}

    private void ThrowBulletBehaviour()
    {
        temp -= Time.deltaTime;
        if (temp < 0)
        {
            isThrow = false;
            setIdle();
        }
    }

    private void NoneBehaviour()
    {

    }

    public void addPiece(int idPiece){

        //StartCoroutine("FinishCatchAnimation");

        // ADD PIECE
        piecesChar.Enqueue(idPiece);

        if (piecesChar.Count == 1)
        {
            pieceGlow.sprite = pieces[4];
            pieceGlow.color = colorShow;
            piece1.GetComponent<Animator>().SetTrigger("isCatched");
        }
        else if (piecesChar.Count == 2) {
            piece2.GetComponent<Animator>().SetTrigger("IsCatched");
        }
        else if (piecesChar.Count == 3)
        {
            piece3.GetComponent<Animator>().SetTrigger("IsCatched");
        }
        else if (piecesChar.Count == 4)
        {
            piece4.GetComponent<Animator>().SetTrigger("IsCatched");
        }

		int i = 0;
		foreach ( int piece in piecesChar ){

			if(i==0){

				if(piece == 0)
				piece1.sprite = pieces[0];

				if(piece == 1)
				piece1.sprite = pieces[1];

                if (piece == 2)
                    piece1.sprite = pieces[2];

                if (piece == 3)
                    piece1.sprite = pieces[3];

                piece1.color = colorShow;
            }

			if(i==1){

				if(piece == 0)
					piece2.sprite = pieces[0];

				if(piece == 1)
					piece2.sprite = pieces[1];

                if (piece == 2)
                    piece2.sprite = pieces[2];

                if (piece == 3)
                    piece2.sprite = pieces[3];

                piece2.color = colorShow;
            }

			if(i==2){

				if(piece == 0)
					piece3.sprite = pieces[0];

				if(piece == 1)
					piece3.sprite = pieces[1];

                if (piece == 2)
                    piece3.sprite = pieces[2];

                if (piece == 3)
                    piece3.sprite = pieces[3];

                piece3.color = colorShow;
            }

            if (i == 3)
            {

                if (piece == 0)
                    piece4.sprite = pieces[0];

                if (piece == 1)
                    piece4.sprite = pieces[1];

                if (piece == 2)
                    piece4.sprite = pieces[2];

                if (piece == 3)
                    piece4.sprite = pieces[3];

                piece4.color = colorShow;
            }
            i++;
		}


	}

	private void Transmutate(){

		int i = 0;
		int j = 0;
		foreach ( int piece in piecesChar ){
			if(piece == 1){
				i++;
			}

			if(piece == 0){
				j++;
			}
		}

		if (j == 3) {
			piecesChar.Clear();
			

			piece1.color = colorHide;
			piece1.sprite = null;
			piece2.color = colorHide;
			piece2.sprite = null;
			piece3.color = colorHide;
			piece3.sprite = null;

			
			
		} else if(i == 3){
			piecesChar.Clear();

			piece1.color = colorHide;
			piece1.sprite = null;
			piece2.color = colorHide;
			piece2.sprite = null;
			piece3.color = colorHide;
			piece3.sprite = null;

		} 

	}

    private void ThrowGarbage() {

    }

    public void playerIsPoison() {
        if (!isPoison)
        {
            moveCharacter.speed = moveCharacter.speed / 2;
            moveCharacter.jumpSpeed = moveCharacter.jumpSpeed / 2;
            timePoisonCounter = timePoison;
            isPoison = true;
        }
        else {
            timePoisonCounter = timePoison;
        }
    }

}
