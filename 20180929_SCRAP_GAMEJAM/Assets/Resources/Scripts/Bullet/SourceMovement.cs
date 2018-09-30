using UnityEngine;
using System.Collections;

public class SourceMovement : MonoBehaviour {

	public enum PlayerAttackStates {NONE, RANGE}
	public PlayerAttackStates state;

	private float temp;
	public float tempMeleeIni;
	public float tempRangeIni;
	public Transform[] bullet;
	public Animator animatorCharacter;
	public AudioManager audioManger;
	public PlayerLogic playerLogic;
    private int typeBullet;

	public bool isMelee;

	void Start(){
        typeBullet = 0;
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

	public void setRange(int modeBullet){
		
		temp = tempRangeIni;

        StartCoroutine("ThrowGarbage", modeBullet);
        
        // bulletAux.GetComponent<Rigidbody>().velocity = bulletAux.transform.right * 15f;

        audioManger.Play(audioManger.Shoot, transform.position);

        state = PlayerAttackStates.RANGE;
		
	}

    IEnumerator ThrowGarbage(int modeAux)
    {
        yield return new WaitForSeconds(.225f);
        GameObject bulletAux = (GameObject)Instantiate(bullet[modeAux].gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        bulletAux.GetComponent<Rigidbody>().AddForce(new Vector3(transform.localPosition.x, 5, 0) * 1000);
        Destroy(bulletAux, 4f);
        
        
    }


    private void NoneBehaviour(){
		
			//	setRange();
	
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
