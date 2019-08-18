using UnityEngine;
using System.Collections;

public class SourceMovement : MonoBehaviour {

	public enum PlayerAttackStates {NONE, RANGE}
	public PlayerAttackStates state;

	private float temp;
	public float tempRangeIni;
	public Transform[] bullet;
	public Animator animatorCharacter;
	public AudioManager audioManger;
	public PlayerLogic playerLogic;
    private int typeBullet;
    private float forceExpulsionAux;
	public bool isMelee;
    private float multiplyImpulse;
    public float minImpulse;
    public float maxImpulse;
    public float yForce = 5f;

	void Start(){
        forceExpulsionAux = 0;
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

	public void setRange(int modeBullet, float forceExpulsion){
		
		temp = tempRangeIni;
        forceExpulsionAux = forceExpulsion;
        StartCoroutine("ThrowGarbage", modeBullet);
        
        // bulletAux.GetComponent<Rigidbody>().velocity = bulletAux.transform.right * 15f;

        audioManger.Play(audioManger.Shoot, transform.position);

        state = PlayerAttackStates.RANGE;
		
	}

    IEnumerator ThrowGarbage(int modeAux)
    {
        yield return new WaitForSeconds(.005f); // (.225f);
        Debug.Log("forceExpulsionAux: " + forceExpulsionAux);
        GameObject bulletAux = (GameObject)Instantiate(bullet[modeAux].gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        // OLD (MORE HORIZONTAL) new Vector3(transform.localPosition.x, 5f, 0) 
        // forceExpulsionAux forceExpulsionAux <= 0.1f --> 700 --- forceExpulsionAux >= 0.4f --> 1100

        if (forceExpulsionAux <= 0.1f)
        {
            multiplyImpulse = minImpulse;
        }
        else if (forceExpulsionAux >= 0.4f) {
            multiplyImpulse = maxImpulse;
        }
        else if(forceExpulsionAux > 0.1f && forceExpulsionAux < 0.4f)
        {
            multiplyImpulse = (((forceExpulsionAux - 0.1f) * (maxImpulse-minImpulse)) / 0.3f) + minImpulse;
        }
            Debug.Log("MULTIPLIER: " + multiplyImpulse);
            bulletAux.GetComponent<Rigidbody>().AddForce(new Vector3(transform.localPosition.x, yForce, 0) * multiplyImpulse); // *1000);
        Destroy(bulletAux, 4f);
        
        
    }


    private void NoneBehaviour(){
		
			//	setRange();
	
	}

	private void RangeBehaviour(){
		// Logica del disparo
		temp -= Time.deltaTime;
		if(temp<0){
            setNone();
		}
	}


}
