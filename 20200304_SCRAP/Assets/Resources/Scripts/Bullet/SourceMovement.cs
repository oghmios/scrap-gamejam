using UnityEngine;
using System.Collections;

public class SourceMovement : MonoBehaviour {

	public enum PlayerAttackStates {NONE, RANGE}
	public PlayerAttackStates state;

	private float temp;
	public float tempRangeIni;
	public Transform[] bullet;
	public Animator animatorCharacter;
	public PlayerLogic playerLogic;
    private float forceExpulsionAux;
    private float multiplyImpulse;
    public float minImpulse;
    // public float mediumImpulse;
    public float maxImpulse;
    public float minLimitImpulse, maxLimitImpulse; 
    public float yForce = 5f;
    public float oneShotImpulse;
    private Transform myTransform;
    public bool IsPlayer1 = true;

    void Start(){
        myTransform = this.transform;

        if (PlayerPrefs.GetFloat("ThrowSensitivity"+playerLogic.control) == 0)
        {
            PlayerPrefs.SetFloat("ThrowSensitivity"+playerLogic.control, 1f);
            maxLimitImpulse = 1;
        }
        else
        {
            maxLimitImpulse = PlayerPrefs.GetFloat("ThrowSensitivity"+playerLogic.control);
        }

        forceExpulsionAux = 0;
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

        CoreManager.Audio.Play(CoreManager.Audio.playerDigImpossible, myTransform.position);

        state = PlayerAttackStates.RANGE;
		
	}
    /*
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
        
        
    }*/


    IEnumerator ThrowGarbage(int modeAux)
    {
        yield return new WaitForSeconds(.005f); // (.225f);
        // Debug.Log("forceExpulsionAux: " + forceExpulsionAux);
       //  GameObject bulletAux = (GameObject)Instantiate(bullet[modeAux].gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

         GameObject bulletAux = NewObjectPoolerScript.current.GetPooledObject(modeAux);

        if (bulletAux == null) yield return null;

        // OLD (MORE HORIZONTAL) new Vector3(transform.localPosition.x, 5f, 0) 
        // forceExpulsionAux forceExpulsionAux <= 0.1f --> 700 --- forceExpulsionAux >= 0.4f --> 1100
        
        if (forceExpulsionAux <= minLimitImpulse)
        {
            multiplyImpulse = minImpulse;
        }
        else if (forceExpulsionAux >= maxLimitImpulse)
        {
            multiplyImpulse = maxImpulse;
        }
        else if (forceExpulsionAux > minLimitImpulse && forceExpulsionAux < maxLimitImpulse)
        {
                multiplyImpulse = (((forceExpulsionAux - minLimitImpulse) * (maxImpulse - minImpulse)) / (maxLimitImpulse - minLimitImpulse)) + minImpulse;
        }

        playerLogic.animatorCharacter.SetTrigger("IsThrow");
        // RESET THE RIGIDBODY PROPERTIES
        bulletAux.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bulletAux.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (IsPlayer1)
            bulletAux.GetComponent<GranadeLogic>().IsFromPlayer1 = true;
        else
            bulletAux.GetComponent<GranadeLogic>().IsFromPlayer1 = false;
        // bulletAux.transform.rotation = Quaternion.Euler(Vector3.zero);

        bulletAux.transform.position = myTransform.position;
        bulletAux.transform.rotation = Quaternion.identity;

        bulletAux.SetActive(true);

        if (PlayerPrefs.GetInt("OneShot"+playerLogic.control) == 1)
            bulletAux.GetComponent<Rigidbody>().AddForce(new Vector3(myTransform.localPosition.x * 1.25f, yForce, 0) * oneShotImpulse); // *1000);
        else
            bulletAux.GetComponent<Rigidbody>().AddForce(new Vector3(myTransform.localPosition.x * 1.25f, yForce, 0) * multiplyImpulse); // *1000);

    }

    private void NoneBehaviour(){
	
	}

	private void RangeBehaviour(){
		// Logica del disparo
		temp -= Time.deltaTime;
		if(temp<0){
            setNone();
		}
	}


}
