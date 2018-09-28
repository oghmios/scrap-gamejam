using UnityEngine;
using System.Collections;

public class SourceMovement : MonoBehaviour {

	public enum PlayerAttackStates {NONE, MELEE, RANGE}

	public PlayerAttackStates state;

	private float temp;
	public float tempMeleeIni;
	public float tempRangeIni;
	public Transform bullet;
	public Animator animatorCharacter;
	public AudioManager audioManger;
	public PlayerLogic playerLogic;
	public BoxCollider meleeCollider;

	public bool isMelee;

	void Start(){
		isMelee = true;
		meleeCollider = GetComponent<BoxCollider>();
		meleeCollider.enabled = false;
		setNone();
	}
	
	// Update is called once per frame
	void Update () {
		switch(state){
		case PlayerAttackStates.NONE:
				NoneBehaviour();
				break;
		case PlayerAttackStates.MELEE:
			MeleeBehaviour();
				break;
		case PlayerAttackStates.RANGE:
			RangeBehaviour();
				break;	}
	}

	public void setNone(){
		if(playerLogic.stateTrans == PlayerLogic.TransmutateStates.NORMAL)
			animatorCharacter.SetBool("isAttack", false);
		else if (playerLogic.stateTrans == PlayerLogic.TransmutateStates.MELEE)
			animatorCharacter.SetBool("isAttackPhase2", false);

		state = PlayerAttackStates.NONE;
	}

	public void setMelee(){
		

		if (temp<=0){
			temp = tempMeleeIni;
			if(playerLogic.stateTrans == PlayerLogic.TransmutateStates.NORMAL)
				animatorCharacter.SetBool("isAttack", true);
			else if (playerLogic.stateTrans == PlayerLogic.TransmutateStates.MELEE)
				animatorCharacter.SetBool("isAttackPhase2", true);
			meleeCollider.enabled = true;

			state = PlayerAttackStates.MELEE;
		} else {
			state = PlayerAttackStates.NONE;
		}
	}

	public void setRange(){
		if (temp<=0){
			temp = tempRangeIni;
			state = PlayerAttackStates.RANGE;
		} else {
			state = PlayerAttackStates.NONE;
		}
	}

	public void addPower(){
		bullet.GetComponent<BulletLogic>().damage +=5;
	}

	private void NoneBehaviour(){
		if(Input.GetButton("Fire1")){
			if(isMelee)
				setMelee();
			else 
				setRange();
		}
	}

	private void RangeBehaviour(){
		// Logica del disparo
		temp -= Time.deltaTime;
		if(temp<0){
			if(Input.GetButton("Fire1")){
				GameObject bulletAux = (GameObject) Instantiate(bullet.gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.localRotation);
				audioManger.Play(audioManger.Shoot,transform.position);
				Destroy (bulletAux,0.5f);
				temp = tempRangeIni;
			}
		}
	}

	private void MeleeBehaviour(){
		// Logica del ataque melee
		temp -= Time.deltaTime;
		if(temp<0){
			meleeCollider.enabled = false;
			setNone();
			}
		}


}
