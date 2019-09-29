using UnityEngine;
using System.Collections;

public class BossLogic: MonoBehaviour {

	public enum BossStates {DAMAGE, MOVE, IDLE, DIE, CHANGE_DIRECTION, ATTACK}

	public BossStates state;

	// MODE --> IDLE = STACIONARY , MOVE = PATROL
	public BossStates mode;

	public float lifeIni;
	private float life;
	public Transform enemyTransform;

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	// private Vector3 moveDirection = Vector3.zero;
	//private CharacterController controller;
	// private GameLogic gameLogic;
	private Transform player;
	public Transform bulletSource;
	public Transform[] pieceBoss;
	public Transform explosionBoss;
	private bool isKill;
	private float temp;
	private float tempAttack;
	//private float tempSound;
	public float tempIniPatrol;
	public AudioManager audioManger;
	// private Color colorAux;
	private bool changeDir;
	private BoxCollider damageCollider;

	// 0=BASE
	// 1=BIG
	// 2=FLY
	public int typeEnemy;
	public SpriteRenderer spriteGlow;
	public SpriteRenderer spriteEnemy;
	public Animator anim;


	private void Start(){

		if(typeEnemy==0){
			spriteGlow.color = new Color32(241,255,0,30);
		} else if(typeEnemy==1){
			spriteGlow.color = new Color32(255,0,0,50);
		}

		audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		damageCollider = GetComponent<BoxCollider>();
		enemyTransform = this.transform;
		changeDir = false;
		life = lifeIni;
		// colorAux = Color.white;
		isKill = false;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		// gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
		// controller = GetComponent<CharacterController>();
		// tempSound = 10.5f;
		// audioManger.Play(audioManger.bossIdle,transform.position);

		if(mode == BossStates.IDLE)
			setIdle();
		if(mode == BossStates.MOVE)
			setMove();
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
		case BossStates.ATTACK:
			AttackBehaviour();
			break;
		}

		/*
		tempSound-=Time.deltaTime;
		if(tempSound<0){
			audioManger.Play(audioManger.bossIdle,transform.position);
			tempSound = 10.5f;
		}
		*/

	}

	// SETS
	public void setMove(){
		spriteEnemy.color = Color.white;

		damageCollider.enabled = true;

		temp  = tempIniPatrol;

		state = BossStates.MOVE;
	}

	public void setIdle(){
		/* GetComponent<BoxCollider>().enabled = false;
		temp = 1f;
		state = BossStates.IDLE;*/
		spriteEnemy.color = Color.white;
		
		damageCollider.enabled = true;
		state = BossStates.IDLE;
	}

	public void setChangeDirection(){
		spriteEnemy.color = Color.white;
		temp = 1f;
		state = BossStates.CHANGE_DIRECTION;
	}

	public void setDamage(){

			damageCollider.enabled = false;
			spriteEnemy.color = Color.red;
			temp = 0.1f;
			state = BossStates.DAMAGE;

	}

	public void setDie(){
		state = BossStates.DIE;
	}

	// BEHAVIOURS
	private void MoveBehaviour(){

		temp-=Time.deltaTime;
		if(temp<0)
		setChangeDirection();

			
		 if (!changeDir)
			transform.Translate(Vector3.right * Time.deltaTime * speed);
		 else
			transform.Translate(Vector3.left * Time.deltaTime * speed);

	}

	private void IdleBehaviour(){
		/*temp-=Time.deltaTime;

		colorAux.a = Mathf.Lerp(0,1,temp/1);
		transform.GetComponentInChildren<SpriteRenderer>().color = colorAux;
		if(temp<0){
			gameLogic.setLose();
		}*/

	}

	private void DamageBehaviour(){
		 if (!changeDir)
			transform.Translate(Vector3.right * Time.deltaTime * speed);
		 else
			transform.Translate(Vector3.left * Time.deltaTime * speed);
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

		if(typeEnemy != 2){

			setDamage();

			life-= damage;
			if(!isKill){
				if(life<0){
					Kill();
				}
			} 
		}
	}

	public void setAttack(){
		tempAttack = 1f;
		anim.SetBool("isAttack", true);

		state = BossStates.ATTACK;
	}

	public void AttackBehaviour(){
		tempAttack -= Time.deltaTime;

		if(tempAttack<0){
			anim.SetBool("isAttack", false);

			if(mode == BossStates.IDLE){
				state = BossStates.IDLE;
			}

			if(mode == BossStates.MOVE){
				state = BossStates.MOVE;
			}
		}
	}

	private void Kill(){
		audioManger.Play(audioManger.bossExplosion,player.position);
		isKill = true;
		GetComponent<BoxCollider>().enabled = false;
		//GetComponent<CharacterController>().enabled = false;
		for (int i=0; i<4; i++){
			GameObject piece = (GameObject) Instantiate(pieceBoss[i].gameObject,new Vector3(transform.position.x, transform.position.y+2, transform.position.z), 
			            new Quaternion(Random.Range(0.1f,270.0f),Random.Range(0.1f,270.0f),Random.Range(0.1f,270.0f),Random.Range(0.1f,270.0f)));
			piece.GetComponent<Rigidbody>().AddForce(piece.transform.forward*10);
		
		}
		GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
		Destroy(explosionBossAux,0.5f);
		// gameLogic.setDestroyBossVictory();
		Destroy(gameObject);
	}

}
