using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerLogic: MonoBehaviour {


	public enum PlayerStates {IDLE, DAMAGE, EAT, DIE, DIG, DIG_TO_IDLE, DASHDOWN, DASHDOWN_DIG, THROW_BULLET, NONE}
	public PlayerStates state;

    public string control;
    private GameLogic gameLogic;
	public Transform explosionBoss;
	public float lifeIniPlayer;
	private float lifePlayer;
	public Transform explosionPlayer;
	public Transform explosionPowerUp;
	public AudioManager audioManger;
	public SourceMovement sourceBullets;
    public MoveCharacter moveCharacter;
	public Animator animatorCharacter;
	public SpriteRenderer spriteCharacter;
    public UIChangeColor colorIntenvory;

    public BoxCollider colliderDig;

	public Sprite[] pieces;
	public Image piece1, piece2, piece3, piece4, pieceGlow;

	private float temp;

	public float tempHumanityIni;
	// private float tempHumanity;

	public Queue<int> piecesChar; 

	public float fireRate = 1F;
	private float nextFire = 0.0F;
    private bool isThrow;
    public float throwTime;
    private float throwTimeCounter;
    private float gravityOrig;
    public float gravityMultiplier = 4;
    public float tempDig = 2;
    public float temDigDown = 1;
    public float tempDigToIdle = 0.5f;
    public Color colorPrepareThrow, colorPrepareThrowMax;


    private void Start(){

        gravityOrig = moveCharacter.gravity;
        audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		piecesChar = new Queue<int>();
		gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        pieceGlow.sprite = null;
        pieceGlow.color = new Color(0, 0, 0, 0);
        spriteCharacter.enabled = true;
        isThrow = false;
        setIdle();
	}


    void Update()
    {
        
        switch (state)
        {
            case PlayerStates.IDLE:
                IdleBehaviour();
                break;
            case PlayerStates.DAMAGE:
                DamageBehaviour();
                break;
            case PlayerStates.EAT:
                EatBehaviour();
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
            case PlayerStates.DIE:
                DieBehaviour();
                break;
            case PlayerStates.NONE:
                NoneBehaviour();
                break;
        }


        // BOTON PARA TRANSFORMAR
        if ((hInput.GetButton("Dig"+control) || hInput.GetAxis("Dig" + control) > 0 || hInput.GetAxis("Dig" + control) < 0) && moveCharacter.isGround && Time.time > nextFire &&
            state == PlayerStates.IDLE && ( !hInput.GetButton("Throw"+control) || hInput.GetAxis("Throw" + control) == 0))
        {

            nextFire = Time.time + fireRate;
            
            setDig();
        }
        else if ((hInput.GetButton("Dig"+control) || hInput.GetAxis("Dig" + control) > 0 || hInput.GetAxis("Dig" + control) < 0) && !moveCharacter.isGround && Time.time > nextFire
            && state == PlayerStates.IDLE && (!hInput.GetButton("Throw"+control) || hInput.GetAxis("Throw" + control) == 0))
        {
            
            nextFire = Time.time + fireRate;

                setDashDown();
        }/*
        else if(Input.GetButton("Fire1") && (state != PlayerStates.DASHDOWN || state != PlayerStates.DASHDOWN_DIG || state != PlayerStates.DIG_TO_IDLE))
        {
            // animatorCharacter.SetBool("isDig", false);
            Debug.Log("3");
            setIdle();
        }*/

        if ((hInput.GetButtonDown("Throw"+control) || hInput.GetAxis("Throw" + control) < 0 || hInput.GetAxis("Throw" + control) > 0) && !isThrow && piecesChar.Count > 0)
            {

            isThrow = true;
            // rigid.velocity = Vector3.up * jumpSpeed;
            animatorCharacter.SetTrigger("IsPrepareThrow");
            spriteCharacter.color = colorPrepareThrow;
            throwTimeCounter = throwTime;
        }

        if ((hInput.GetButtonDown("Throw"+control) || hInput.GetAxis("Throw" + control) < 0 || hInput.GetAxis("Throw" + control) > 0) && !isThrow && piecesChar.Count <= 0)
        {
            colorIntenvory.setAnimation();
        }

        if (isThrow && (hInput.GetButton("Throw"+control) || hInput.GetAxis("Throw" + control) < 0 || hInput.GetAxis("Throw" + control) > 0))
        {
            
            if (throwTimeCounter > 0)
            {

                // rigid.velocity = Vector3.up * jumpSpeed;
                //setThrowBullet(throwTime - throwTimeCounter);
                //animatorCharacter.SetTrigger("IsPrepareThrow");
                throwTimeCounter -= Time.deltaTime;
                spriteCharacter.color = colorPrepareThrow;
            }
            else
            {
                
                throwTimeCounter = 0;
                // animatorCharacter.SetTrigger("IsPrepareThrow");
                spriteCharacter.color = colorPrepareThrowMax;
                // isThrow = false;
            }

            // Si se mantiene el lanzamiento y se da la tecla cavar, 
            // se cancela  el lanzamiento
            if (hInput.GetButton("Dig"+control) || hInput.GetAxis("Dig" + control) != 0)
            {
                isThrow = false;
                animatorCharacter.SetTrigger("IsThrow");
                spriteCharacter.color = Color.white;
                setIdle();
            }
        }

        if (isThrow && (hInput.GetButtonUp("Throw"+control) || hInput.GetAxis("Throw" + control) == 0))
        {
            //animatorCharacter.SetBool("IsPrepareThrow", false);
            setThrowBullet(throwTime - throwTimeCounter);
            isThrow = false;
            spriteCharacter.color = Color.white;
        }

    }
	// END TRANSMUTATE

	public void setIdle(){
        moveCharacter.horizontalControl = true;
        moveCharacter.gravity = gravityOrig;
        colliderDig.enabled = false;
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		state = PlayerStates.IDLE;
	}

	public void setEat(){
		temp = 0.5f;

			animatorCharacter.SetBool("isEat", true);

		state = PlayerStates.EAT;
	}

    public void setDig()
    {
        temp = tempDig;
        if (piecesChar.Count < 4 && moveCharacter.isGround)
        {
            audioManger.Play(audioManger.Shoot, transform.position);
            animatorCharacter.SetTrigger("IsDig");
            CameraShake.Shake(Vector3.one*0.25f, 0.25f);
            colliderDig.enabled = true;
            state = PlayerStates.DIG;
        }
        else if (piecesChar.Count >= 4 && moveCharacter.isGround )
        {
            colorIntenvory.setAnimation();
            setIdle();
        }
    }

    public void setDashDown()
    {
        animatorCharacter.SetBool("IsDigDash", true);
        moveCharacter.horizontalControl = false;
        moveCharacter.gravity = gravityOrig * gravityMultiplier;
        

        state = PlayerStates.DASHDOWN;

    }

    public void setDashDownDig()
    {
        moveCharacter.horizontalControl = false;
        temp = temDigDown;
        moveCharacter.gravity = gravityOrig;
        
        if (piecesChar.Count < 4 && moveCharacter.isGround)
        {
            audioManger.Play(audioManger.Shoot, transform.position);
            animatorCharacter.SetBool("IsDigDash", true);
            CameraShake.Shake(Vector3.one * 0.5f, 0.5f);
            colliderDig.enabled = true;
            state = PlayerStates.DASHDOWN_DIG;
        }
        else if (piecesChar.Count >= 4 && moveCharacter.isGround)
        {
            animatorCharacter.SetBool("IsDigDash", false);
            colorIntenvory.setAnimation();
            setIdle();
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

    public void setDamage(){

		if(state != PlayerStates.DIE ){
			lifePlayer--;
			audioManger.Play (audioManger.damagePlayer,transform.position);
			
			transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
			temp = 0.25f;

			if(lifePlayer<=0){
				setDie();
			} else {

			state = PlayerStates.DAMAGE;
			}
		}
	}
	
	public void setDieTransmutate(){
		transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		GetComponent<AudioSource>().pitch = 1;

		animatorCharacter.GetComponent<SpriteRenderer>().flipX = !animatorCharacter.GetComponent<SpriteRenderer>().flipX;

		audioManger.Play(audioManger.shotBulletBoss,transform.position);
		GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
		Destroy(explosionBossAux,0.5f);

		temp = 3f;
		GetComponent<MoveCharacter>().enabled = false;

		state = PlayerStates.DIE;
	}

	public void setDie(){
		//transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		
			//animatorCharacter.SetBool("isDie", true);
			//animatorCharacter.transform.localPosition = new Vector3(animatorCharacter.transform.localPosition.x, -0.5f, animatorCharacter.transform.localPosition.z); 
		
		
		audioManger.Play (audioManger.bossExplosion,transform.position);
		GameObject explosion = (GameObject)Instantiate(explosionPlayer.gameObject,transform.position,Quaternion.identity);
		Destroy (explosion,2);
		temp = 3f;
		GetComponent<MoveCharacter>().enabled = false;
        spriteCharacter.enabled = false;
        // transform.GetChild(0).gameObject.SetActive(false);
        // transform.GetChild(2).gameObject.SetActive(false);

            /* BoxCollider[] col = GetComponents<BoxCollider>();
            foreach(BoxCollider cola in col){
                cola.enabled = false;
            }*/
            state = PlayerStates.DIE;
	}

    public void setThrowBullet(float forceExpulsion) {

        
        if (piecesChar.Count > 0 && sourceBullets.state == SourceMovement.PlayerAttackStates.NONE)
        {
            audioManger.Play(audioManger.playerThrow, transform.position);
            temp = 2;
            // SI SUPERA LAS PIEZAS A ACUMULAR SE QUITA LA ULTIMA
            int pieceMode = piecesChar.Dequeue();

            if (piecesChar.Count == 3) {
                piece4.sprite = null;
                piece4.color = new Color(0, 0, 0, 0);
            } else if (piecesChar.Count == 2) {
                piece3.sprite = null;
                piece3.color = new Color(0, 0, 0, 0);
            } else if (piecesChar.Count == 1)
            {
                piece2.sprite = null;
                piece2.color = new Color(0, 0, 0, 0);
            }
            else if (piecesChar.Count == 0)
            {
                piece1.sprite = null;
                piece1.color = new Color(0, 0, 0, 0);

                pieceGlow.sprite = null;
                pieceGlow.color = new Color(0, 0, 0, 0);
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

                    piece1.color = new Color(1, 1, 1, 1);
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


                    piece2.color = new Color(1, 1, 1, 1);
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

                    piece3.color = new Color(1, 1, 1, 1);
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

                    piece4.color = new Color(1, 1, 1, 1);
                }
                i++;
            }
            piece1.GetComponent<Animator>().SetTrigger("isCatched");
            sourceBullets.setRange(pieceMode, forceExpulsion);
            animatorCharacter.SetTrigger("IsThrow");
            state = PlayerStates.THROW_BULLET;
        }
        else {
            setIdle();
        }
    }

    public void setNone()
    {

        GetComponent<MoveCharacter>().enabled = false;
        this.enabled = false; 
        state = PlayerStates.NONE;
    }

    // BEHAVIOURS
    private void DamageBehaviour(){

		temp -= Time.deltaTime;
		if(temp<0){
			setIdle();
		}
	}

	private void IdleBehaviour(){

	}

	private void EatBehaviour(){
		temp -= Time.deltaTime;
		
		
		if(temp<0){
			setIdle();
		}
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


        if (temp < 0)
        {
            setIdle();
        }
    }

    private void DieBehaviour(){
		temp -= Time.deltaTime;


		if(temp<0){
			// Destroy(this.gameObject);
            if (gameLogic.state != GameLogic.GameStates.LOSE)
			gameLogic.setLose();
		}
	}

    private void ThrowBulletBehaviour()
    {
        setIdle();
    }

    private void NoneBehaviour()
    {

    }


    void OnTriggerEnter(Collider other){

        /*
		if(other.tag == "EnemyDamage"){
			setDamage();
		}

		if(other.tag == "PowerUp"){
			GameObject explosion = (GameObject)Instantiate(explosionPowerUp.gameObject,transform.position,Quaternion.identity);
			Destroy (explosion,2);
			
		}*/

	}

	public void addPiece(int idPiece){

        

        //StartCoroutine("FinishCatchAnimation");

        // ADD PIECE
        piecesChar.Enqueue(idPiece);

        if (piecesChar.Count == 1)
        {
            pieceGlow.sprite = pieces[4];
            pieceGlow.color = new Color(1, 1, 1, 1);
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


        string text = "Pieces: ";
		int i = 0;
		foreach ( int piece in piecesChar ){
			text = text + piece.ToString();
			if(i==0){

				if(piece == 0)
				piece1.sprite = pieces[0];

				if(piece == 1)
				piece1.sprite = pieces[1];

                if (piece == 2)
                    piece1.sprite = pieces[2];

                if (piece == 3)
                    piece1.sprite = pieces[3];

                piece1.color = new Color(1,1,1,1);
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

                piece2.color = new Color(1,1,1,1);
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

                piece3.color = new Color(1,1,1,1);
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
                piece4.color = new Color(1, 1, 1, 1);
            }
            i++;
		}


        // Debug.Log (piecesChar.Count+" "+piecesChar.ToString());
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
			

			piece1.color = new Color(0,0,0,0);
			piece1.sprite = null;
			piece2.color = new Color(0,0,0,0);
			piece2.sprite = null;
			piece3.color = new Color(0,0,0,0);
			piece3.sprite = null;

			
			
		} else if(i == 3){
			piecesChar.Clear();


			piece1.color = new Color(0,0,0,0);
			piece1.sprite = null;
			piece2.color = new Color(0,0,0,0);
			piece2.sprite = null;
			piece3.color = new Color(0,0,0,0);
			piece3.sprite = null;


			
		} 

	}

    private void ThrowGarbage() {

    }


}
