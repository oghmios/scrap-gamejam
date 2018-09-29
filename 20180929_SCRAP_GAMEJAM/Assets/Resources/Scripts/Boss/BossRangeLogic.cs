using UnityEngine;
using System.Collections;

public class BossRangeLogic: MonoBehaviour {

	public enum BossStates {DAMAGE, MOVE, IDLE, DIE, CHANGE_DIRECTION}

	public BossStates state;

	public float lifeIni;
	private float life;
	public Transform enemyTransform;

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	//private CharacterController controller;
	private GameLogic gameLogic;
	private Transform player;
	public Transform bulletSource;
	public Transform pieceBoss;
	public Transform explosionBoss;
	private bool isKill;
	private float temp;
	private float tempSound;
	public AudioManager audioManger;
	private Color colorAux;
	private bool changeDir;

	private void Start(){
		audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		enemyTransform = this.transform;
		changeDir = false;
		life = lifeIni;
		colorAux = Color.white;
		isKill = false;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
		//controller = GetComponent<CharacterController>();
		tempSound = 10.5f;
		audioManger.Play(audioManger.bossIdle,transform.position);
		setIdle();
	}


	void Update() {



		switch(state){
		case BossStates.IDLE:
			IdleBehaviour();
			break;
		case BossStates.MOVE:
			MoveBehaviour();
			break;
		case BossStates.DAMAGE:
			DamageBehaviour();
			break;
		case BossStates.DIE:
			DieBehaviour();
			break;
		case BossStates.CHANGE_DIRECTION:
			ChangeDirectionBehaviour();
			break;
		}


		tempSound-=Time.deltaTime;
		if(tempSound<0){
			audioManger.Play(audioManger.bossIdle,transform.position);
			tempSound = 10.5f;
		}

	}

	// SETS
	public void setMove(){
		transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;


		state = BossStates.MOVE;
	}

	public void setIdle(){
		GetComponent<BoxCollider>().enabled = false;
		temp = 1f;
		state = BossStates.IDLE;
	}

	public void setChangeDirection(){
		transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		temp = 1f;
		state = BossStates.CHANGE_DIRECTION;
	}

	public void setDamage(){

		// lifeBossImage.GetComponent<Renderer>().material.SetFloat("_Cutoff", 1-life/lifeIni);
		transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
		temp = 0.1f;
		state = BossStates.DAMAGE;
	}

	public void setDie(){
		state = BossStates.DIE;
	}

	// BEHAVIOURS
	private void MoveBehaviour(){
		/* if (!changeDir)
			transform.Translate(Vector3.right * Time.deltaTime * speed);
		 else
			transform.Translate(Vector3.left * Time.deltaTime * speed);
		*/
	}

	private void IdleBehaviour(){
		/* temp-=Time.deltaTime;

		colorAux.a = Mathf.Lerp(0,1,temp/1);
		transform.GetComponentInChildren<SpriteRenderer>().color = colorAux;
		if(temp<0){
			gameLogic.setLose();
		} */

	}

	private void DamageBehaviour(){
		 /* if (!changeDir)
			transform.Translate(Vector3.right * Time.deltaTime * speed);
		 else
			transform.Translate(Vector3.left * Time.deltaTime * speed);
			*/
		temp -= Time.deltaTime;
		if(temp<0){
			setChangeDirection();
		}
	}

	private void ChangeDirectionBehaviour(){
		
		temp -= Time.deltaTime;
		if(temp<0){
			changeDir = !changeDir;
			enemyTransform.localScale = new Vector3(-enemyTransform.localScale.x, enemyTransform.localScale.y, enemyTransform.localScale.z);
			setMove();
		}
	}

	private void DieBehaviour(){

	}

	public void addDamage(float damage){

		setDamage();

		life-= damage;
		if(!isKill){
			if(life<0){
				Kill();
			}
		} 
	}

	private void Kill(){
		audioManger.Play(audioManger.bossExplosion,player.position);
		isKill = true;
		GetComponent<BoxCollider>().enabled = false;
		//GetComponent<CharacterController>().enabled = false;
		for (int i=0; i<2; i++){
			GameObject piece = (GameObject) Instantiate(pieceBoss.gameObject,new Vector3(transform.position.x, transform.position.y+2, transform.position.z), 
			            new Quaternion(Random.Range(0.1f,270.0f),Random.Range(0.1f,270.0f),Random.Range(0.1f,270.0f),Random.Range(0.1f,270.0f)));
			piece.GetComponent<Rigidbody>().AddForce(piece.transform.forward*150);
		}
		GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
		Destroy(explosionBossAux,0.5f);
		gameLogic.setVictory();
		Destroy(gameObject);
	}

}
