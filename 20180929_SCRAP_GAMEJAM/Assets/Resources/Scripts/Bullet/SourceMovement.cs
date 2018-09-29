using UnityEngine;
using System.Collections;

public class SourceMovement : MonoBehaviour {

	public enum PlayerAttackStates {NONE, RANGE}

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

        temp = 0;
        setNone();

	}
	
	// Update is called once per frame
	void Update () {
		switch(state){
		case PlayerAttackStates.NONE:
				NoneBehaviour();
				break;
		case PlayerAttackStates.RANGE:
			RangeBehaviour();
				break;	}
	}

	public void setNone(){


        state = PlayerAttackStates.NONE;
	}

	public void setRange(){
		
		temp = tempRangeIni;

        animatorCharacter.SetBool("isShoot", true);
        StartCoroutine("ThrowGarbage");
        
        // bulletAux.GetComponent<Rigidbody>().velocity = bulletAux.transform.right * 15f;

        audioManger.Play(audioManger.Shoot, transform.position);

        state = PlayerAttackStates.RANGE;
		
	}

    IEnumerator ThrowGarbage()
    {
        yield return new WaitForSeconds(.225f);
        GameObject bulletAux = (GameObject)Instantiate(bullet.gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        bulletAux.GetComponent<Rigidbody>().AddForce(new Vector3(transform.localPosition.x, 6, 0) * 200);
        Destroy(bulletAux, 4f);
        
        
    }


    private void NoneBehaviour(){
		if(Input.GetButton("Fire2") && temp<=0)
        {
				setRange();
		}
	}

	private void RangeBehaviour(){
		// Logica del disparo
		temp -= Time.deltaTime;
		if(temp<0){
            animatorCharacter.SetBool("isShoot", false);
            setNone();
		}
	}


}
