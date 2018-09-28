using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerLogic: MonoBehaviour {

	public enum TransmutateStates {NORMAL, MELEE, RANGE, NONE}
	public TransmutateStates stateTrans;


	public enum PlayerStates {IDLE, DAMAGE, EAT, DIE}
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
	public Transform sourceBullets;
	public Animator animatorCharacter;
	public SpriteRenderer spriteGlow;


	public Sprite[] pieces;
	public Image piece1, piece2, piece3;

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
		humanityText.enabled = false;
		gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
		setIdle();
	}


	void Update(){

		switch(state){
		case PlayerStates.IDLE:
			IdleBehaviour();
			break;
		case PlayerStates.DAMAGE:
			DamageBehaviour();
			break;
		case PlayerStates.EAT:
			EatBehaviour();
			break;
		case PlayerStates.DIE:
			DieBehaviour();
			break;
		}

		switch(stateTrans){
		case TransmutateStates.MELEE:
			MeleeBehaviour();
			break;
		case TransmutateStates.RANGE:
			RangeBehaviour();
			break;
		case TransmutateStates.NONE:
			NoneBehaviour();
			break;
		}


		// BOTON PARA TRANSFORMAR
		if(Input.GetButton("Fire2") && Time.time > nextFire){
			nextFire = Time.time + fireRate;
			audioManger.Play(audioManger.Shoot,transform.position);
			Transmutate();
		}
	}

	// TRANSMUTATE

	public void setTransmutateNormal(){
		spriteGlow.color = new Color32 (98,112,255,50);
		GetComponent<AudioSource>().pitch = 1;
		transform.localScale = new Vector3(1.5f,1.5f,1);
		animatorCharacter.SetBool("isPhase2", false);
		audioManger.Play(audioManger.bossExplosion,transform.position);
		GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
		Destroy(explosionBossAux,0.5f);
		humanityText.enabled = false;
		stateTrans = TransmutateStates.NORMAL;
	}
	
	public void setTransmutateMelee(){
		spriteGlow.color = new Color32 (255,98,98,50);
		GetComponent<AudioSource>().pitch = 1;
		transform.localScale = new Vector3(3,3,1);
		animatorCharacter.SetBool("isPhase2", true);
		audioManger.Play(audioManger.bossExplosion,transform.position);
		GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
		Destroy(explosionBossAux,0.5f);
		tempHumanity = tempHumanityIni;
		humanityText.enabled = true;
		humanityText.color = Color.white;
		stateTrans = TransmutateStates.MELEE;
	}


	private void MeleeBehaviour(){
		tempHumanity -= Time.deltaTime;

		humanityText.text = tempHumanity.ToString("0");


		if(tempHumanity<5){
			GetComponent<AudioSource>().pitch = 3;
			if((int.Parse(tempHumanity.ToString("0")) % 2) == 0){
				humanityText.color = Color.red;
			} else {
				humanityText.color = Color.white;
			}
		}
		if(tempHumanity<0 && state != PlayerStates.DIE){
			humanityText.enabled = false;

			setDieTransmutate();
		}
	}

	private void RangeBehaviour(){
		tempHumanity -= Time.deltaTime;

		humanityText.text = tempHumanity.ToString("0");

		if(tempHumanity<0 && state != PlayerStates.DIE){

			humanityText.enabled = false;
			setDieTransmutate();
		}
	}

	private void NoneBehaviour(){

	}

	// END TRANSMUTATE

	public void setIdle(){

		if(stateTrans == TransmutateStates.NORMAL)
		animatorCharacter.SetBool("isEat", false);
		else if (stateTrans == TransmutateStates.MELEE)
		animatorCharacter.SetBool("isEatPhase2", false);

		transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		state = PlayerStates.IDLE;
	}

	public void setEat(){
		temp = 0.5f;
		if(stateTrans == TransmutateStates.NORMAL)
			animatorCharacter.SetBool("isEat", true);
		else if (stateTrans == TransmutateStates.MELEE)
			animatorCharacter.SetBool("isEatPhase2", true);

		state = PlayerStates.EAT;
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

		if (stateTrans == TransmutateStates.MELEE)
			animatorCharacter.SetBool("isDieTransmutatePhase2", true);

		animatorCharacter.GetComponent<SpriteRenderer>().flipX = !animatorCharacter.GetComponent<SpriteRenderer>().flipX;

		audioManger.Play(audioManger.shotBulletBoss,transform.position);
		GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
		Destroy(explosionBossAux,0.5f);

		temp = 3f;
		GetComponent<MoveCharacter>().enabled = false;

		state = PlayerStates.DIE;
	}

	public void setDie(){
		transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		
		if(stateTrans == TransmutateStates.NORMAL){
			animatorCharacter.SetBool("isDie", true);
			animatorCharacter.transform.localPosition = new Vector3(animatorCharacter.transform.localPosition.x, -0.5f, animatorCharacter.transform.localPosition.z); 
		}
		else if (stateTrans == TransmutateStates.MELEE){
			animatorCharacter.SetBool("isDiePhase2", true);
			animatorCharacter.transform.localPosition = new Vector3(animatorCharacter.transform.localPosition.x, -0.3f, animatorCharacter.transform.localPosition.z); 

		}
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

	private void DieBehaviour(){
		temp -= Time.deltaTime;


		if(temp<0){
			// Destroy(this.gameObject);
			gameLogic.setLose();
		}
	}


	void OnTriggerEnter(Collider other){

		if(other.tag == "EnemyDamage"){
			setDamage();
		}

		if(other.tag == "BossPiece"){

			if (other.name.Contains("Base")){
				addPiece(0);
			} else if (other.name.Contains("Melee")){
				addPiece(1);
			} 
				setEat();
				audioManger.Play(audioManger.catchPieceBoss,transform.position);
				Destroy(other.gameObject);
		}

		if(other.tag == "PowerUp"){
			GameObject explosion = (GameObject)Instantiate(explosionPowerUp.gameObject,transform.position,Quaternion.identity);
			Destroy (explosion,2);
			sourceBullets.GetComponent<SourceMovement>().addPower();
		}

	}

	private void addPiece(int idPiece){
	
		// SI SUPERA LAS PIEZAS A ACUMULAR SE QUITA LA ULTIMA
		if(piecesChar.Count > 2)
			piecesChar.Dequeue();

		// ADD PIECE
		piecesChar.Enqueue(idPiece);

		string text = "Pieces: ";
		int i = 0;
		foreach ( int piece in piecesChar ){
			text = text + piece.ToString();
			if(i==0){

				if(piece == 0)
				piece1.sprite = pieces[0];


				if(piece == 1)
				piece1.sprite = pieces[1];

				piece1.color = new Color(1,1,1,1);
			}

			if(i==1){

				if(piece == 0)
					piece2.sprite = pieces[0];

				if(piece == 1)
					piece2.sprite = pieces[1];


					piece2.color = new Color(1,1,1,1);
			}

			if(i==2){

				if(piece == 0)
					piece3.sprite = pieces[0];

				if(piece == 1)
					piece3.sprite = pieces[1];
					piece3.color = new Color(1,1,1,1);
			}
			i++;
		}

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

			
			setTransmutateNormal();
		} else if(i == 3){
			piecesChar.Clear();


			piece1.color = new Color(0,0,0,0);
			piece1.sprite = null;
			piece2.color = new Color(0,0,0,0);
			piece2.sprite = null;
			piece3.color = new Color(0,0,0,0);
			piece3.sprite = null;


			setTransmutateMelee();
		} 

	}

	/* void OnTriggerStay(Collider other){
		
		if(other.tag == "EnemyDamage"){
			setDamage();
		}
		
		if(other.tag == "BossPiece"){
			
			audioManger.Play(audioManger.catchPieceBoss,transform.position);
			gameLogic.addPieceBoss();
			Destroy(other.gameObject);
		}
		
		if(other.tag == "PowerUp"){
			GameObject explosion = (GameObject)Instantiate(explosionPowerUp.gameObject,transform.position,Quaternion.identity);
			Destroy (explosion,2);
			sourceBullets.GetComponent<SourceMovement>().addPower();
		}
		
	} */ 
	
}
