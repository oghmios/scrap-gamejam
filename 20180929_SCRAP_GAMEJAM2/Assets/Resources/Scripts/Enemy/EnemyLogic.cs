using UnityEngine;
using System.Collections;

public class EnemyLogic: MonoBehaviour {

	public enum EnemyStates {DAMAGE, MOVE, IDLE, DIE}

	public EnemyStates state;

	public float lifeIni;
	private float life;
	public Transform powerUpBullet;
	public float speed = 6.0F;

	private Vector3 moveDirection = Vector3.zero;


	public Transform explosionBoss;
	private bool isKill;
	private float temp;
	private float tempSound;

	private Color colorAux;

	private void Start(){

		life = lifeIni;
		colorAux = Color.white;
		isKill = false;

		setMove();
	}


	void Update() {



		switch(state){
		case EnemyStates.IDLE:
			IdleBehaviour();
			break;
		case EnemyStates.MOVE:
			MoveBehaviour();
			break;
		case EnemyStates.DAMAGE:
			DamageBehaviour();
			break;
		case EnemyStates.DIE:
			DieBehaviour();
			break;
		}


	}

	// SETS
	public void setMove(){
		// transform.GetComponentInChildren<AlphaChangeTintColor>().enabled = true;
		// transform.GetComponentInChildren<Renderer>().material.SetColor("_TintColor",Color.white);


		state = EnemyStates.MOVE;
	}

	public void setIdle(){
		GetComponent<BoxCollider>().enabled = false;
		temp = 1f;
		state = EnemyStates.IDLE;
	}

	public void setDamage(){


		// transform.GetComponentInChildren<AlphaChangeTintColor>().enabled = false;
		// transform.GetComponentInChildren<Renderer>().material.SetColor("_TintColor",Color.red);
		temp = 0.1f;
		state = EnemyStates.DAMAGE;
	}

	public void setDie(){
		state = EnemyStates.DIE;
	}

	// BEHAVIOURS
	private void MoveBehaviour(){
		//transform.Translate(Vector3.right * Time.deltaTime * speed);

	}

	private void IdleBehaviour(){


	}

	private void DamageBehaviour(){
		transform.Translate(Vector3.right * Time.deltaTime * speed);
		temp -= Time.deltaTime;
		if(temp<0){
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


		//Instantiate(powerUpBullet.gameObject,transform.position,Quaternion.identity);

		isKill = true;
		GetComponent<BoxCollider>().enabled = false;

		CoreManager.Audio.Play(CoreManager.Audio.enemyExplosion,transform.position);

		GameObject explosionBossAux = (GameObject) Instantiate(explosionBoss.gameObject,transform.position, Quaternion.identity);
		explosionBossAux.GetComponent<ParticleSystem>().startColor = Color.green;
		Destroy(explosionBossAux,0.5f);

		Destroy(gameObject);
	}

}
