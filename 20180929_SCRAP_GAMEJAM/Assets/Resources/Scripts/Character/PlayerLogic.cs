using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerLogic: MonoBehaviour {


	public enum PlayerStates {IDLE, DAMAGE, EAT, DIE, DIG, THROW_BULLET}
	public PlayerStates state;

	private GameLogic gameLogic;
	public Transform explosionBoss;
	public float lifeIniPlayer;
	public float humanityIniPlayer;
	private float lifePlayer;
	private float humanityPlayer;
	public Transform lifePlayerImage;
	public Transform playerLights;
	public Transform explosionPlayer;
	public Transform explosionPowerUp;
	public Text humanityText;
	public AudioManager audioManger;
	public SourceMovement sourceBullets;
	public Animator animatorCharacter;
	public SpriteRenderer spriteGlow;

    public BoxCollider colliderDig;

	public Sprite[] pieces;
	public Image piece1, piece2, piece3, piece4, piece5;

	private float temp;

	public float tempHumanityIni;
	private float tempHumanity;

	public Queue<int> piecesChar; 

	public float fireRate = 1F;
	private float nextFire = 0.0F;

	private void Start(){
		audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		piecesChar = new Queue<int>();
		humanityPlayer = humanityIniPlayer;
		tempHumanity = tempHumanityIni;
		humanityText.enabled = true;
		gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        piece5.sprite = null;
        piece5.color = new Color(0, 0, 0, 0);
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
            case PlayerStates.THROW_BULLET:
                ThrowBulletBehaviour();
                break;
            case PlayerStates.DIE:
                DieBehaviour();
                break;
        }


        // BOTON PARA TRANSFORMAR
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            audioManger.Play(audioManger.Shoot, transform.position);
            setDig();
        }
        else
        {
            // animatorCharacter.SetBool("isDig", false);
            setIdle();
        }

        if (Input.GetButton("Fire2"))
        {
            setThrowBullet();
        }
    }
	// END TRANSMUTATE

	public void setIdle(){
        colliderDig.enabled = false;
        animatorCharacter.SetBool("isDig", false);
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

        if (piecesChar.Count < 4)
        {
            animatorCharacter.SetBool("isDig", true);
            CameraShake.Shake(Vector3.one*0.25f, 0.25f);
            colliderDig.enabled = true;
            state = PlayerStates.DIG;
        }
        else {
            setIdle();
        }
    }


    public void setDamage(){

		if(state != PlayerStates.DIE ){
			lifePlayer--;
			audioManger.Play (audioManger.damagePlayer,transform.position);
			// lifePlayerImage.GetComponent<Renderer>().material.SetFloat("_Cutoff", 1-lifePlayer/lifeIniPlayer);
			
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
		
		
		audioManger.Play (audioManger.shotBulletBoss,transform.position);
		GameObject explosion = (GameObject)Instantiate(explosionPlayer.gameObject,transform.position,Quaternion.identity);
		Destroy (explosion,2);
		temp = 3f;
		GetComponent<MoveCharacter>().enabled = false;
		// transform.GetChild(0).gameObject.SetActive(false);
		// transform.GetChild(2).gameObject.SetActive(false);
		
		/* BoxCollider[] col = GetComponents<BoxCollider>();
		foreach(BoxCollider cola in col){
			cola.enabled = false;
		}*/
		state = PlayerStates.DIE;
	}

    public void setThrowBullet() {

        
        if (piecesChar.Count > 0 && sourceBullets.state == SourceMovement.PlayerAttackStates.NONE)
        {
            temp = 2;
            // SI SUPERA LAS PIEZAS A ACUMULAR SE QUITA LA ULTIMA
            int pieceMode = piecesChar.Dequeue();

            if (piecesChar.Count == 3) {
                piece4.sprite = null;
            } else if (piecesChar.Count == 2) {
                piece3.sprite = null;
            } else if (piecesChar.Count == 1)
            {
                piece2.sprite = null;
            }
            else if (piecesChar.Count == 0)
            {
                piece1.sprite = null;
                piece5.sprite = null;
                piece5.color = new Color(0, 0, 0, 0);
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

            humanityText.text = piecesChar.Count.ToString();
            sourceBullets.setRange(pieceMode);
            animatorCharacter.SetBool("isShoot", true);
            state = PlayerStates.THROW_BULLET;
        }
        else {
            setIdle();
        }
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
            setIdle();
        }
    }

    private void DieBehaviour(){
		temp -= Time.deltaTime;


		if(temp<0){
			// Destroy(this.gameObject);
			gameLogic.setLose();
		}
	}

    private void ThrowBulletBehaviour()
    {

    }


    void OnTriggerEnter(Collider other){

		if(other.tag == "EnemyDamage"){
			setDamage();
		}

		if(other.tag == "PowerUp"){
			GameObject explosion = (GameObject)Instantiate(explosionPowerUp.gameObject,transform.position,Quaternion.identity);
			Destroy (explosion,2);
			
		}

	}

	public void addPiece(int idPiece){
	


		// ADD PIECE
		piecesChar.Enqueue(idPiece);

        if (piecesChar.Count == 1)
        {
            piece5.sprite = pieces[4];
            piece5.color = new Color(1, 1, 0, 1);
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

       // humanityText.text = piecesChar.Count.ToString();

        Debug.Log (piecesChar.Count+" "+piecesChar.ToString());
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
