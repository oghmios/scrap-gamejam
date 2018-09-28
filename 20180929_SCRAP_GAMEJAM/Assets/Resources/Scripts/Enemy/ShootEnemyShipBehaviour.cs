using UnityEngine;
using System.Collections;

public class ShootEnemyShipBehaviour : MonoBehaviour {

	public float currentTimeSpawn = 1f;
	private float currentTime;

	private Transform myTransform;

	public Transform prefabShoot;

	private GameObject[] arrowsTrail;
	
	public enum BowArrowStates { SLEEP, READY_THROW, WAIT_THROW }
	
	public BowArrowStates state;

	private Transform locationPlayer;
	public AudioManager audioManger;
	// Use this for initialization
	void Start () {

		locationPlayer = GameObject.FindGameObjectWithTag("Player").transform;


		myTransform = transform;

		setReadyThrowState();
	}
	
	// Update is called once per frame
	void Update () {

		switch (state)
        {
		case BowArrowStates.SLEEP:
			SleepBehaviour();
			break;
		case BowArrowStates.READY_THROW:
                ReadyThrowBehaviour();
                break;
		case BowArrowStates.WAIT_THROW:
                WaitThrowBehaviour();
                break;
        }
	}

	public void setSleep(){
		state = BowArrowStates.SLEEP;
	}

	public void setReadyThrowState()
    {
		// Spawn();


		// if(instance != null) {
		//	instance.Initialize(myTransform.position, myTransform.rotation);
			
		Instantiate(prefabShoot.gameObject,myTransform.position, myTransform.rotation);
		audioManger.Play(audioManger.shotBulletBoss,transform.position);

		state = BowArrowStates.READY_THROW;

    }

    public void setWaitThrowState()
    {
		// spawnMax = 1;
		// CancelInvoke("Spawn");
		currentTime = currentTimeSpawn;
		state = BowArrowStates.WAIT_THROW;
    }
	
	
	/*****
     *  BEHAVIOURS STATE
     * */
    private void ReadyThrowBehaviour()
    {
		setWaitThrowState();
    }
	
	private void WaitThrowBehaviour()
    {
		if(locationPlayer!=null){
		// The Cannon Source rotate with the Player Position
		float rot = Mathf.Atan2(( transform.position.y - locationPlayer.position.y), ( transform.position.x - locationPlayer.position.x ))*Mathf.Rad2Deg;

		if(rot < 35 && rot > -35)
			transform.eulerAngles = new Vector3(0,0,rot);


		currentTime-=Time.deltaTime;

		if (currentTime<0){
			setReadyThrowState();
		}
		}
    }

	private void SleepBehaviour(){

	}
	
	void Spawn() {

			
		/*	
		CannonBallEnemyBehaviour instance = GetNextAvailiableInstanceShoot01();
			if(instance != null) {
				instance.Initialize(bow.position, bow.rotation);
				// Audio  Cannon Player
				// CoreManager.Audio.Play(CoreManager.Audio.ThrowArrow,Camera.main.transform.position, 0.5f);
				
			}

			Invoke("Spawn", spawnMax);
		*/
	}


	
	
}
