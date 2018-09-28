using UnityEngine;
using System.Collections;

public class ScaleChange : MonoBehaviour
{
	public enum ScaleStates {UP,UP_IDLE, DOWN, DOWN_IDLE}

	public ScaleStates states;
	public float tempUp;
	public float tempUpIdle;
	public float tempDown;
	public float tempDownIdle;
	private float temp;
	public float alphaMaxX;
	public float alphaMaxY;
	public float alphaMinX;
	public float alphaMinY;

	void Start(){

		setUp();

	}

	void Update () {

		switch(states){
		case ScaleStates.UP:
				UpBehaviour();
				break;
		case ScaleStates.UP_IDLE:
				UpIdleBehaviour();
				break;
		case ScaleStates.DOWN:
				DownBehaviour();
				break;
		case ScaleStates.DOWN_IDLE:
				DownIdleBehaviour();
				break;
		}
	}

	// Sets
	public void setUp(){

		temp = tempUp;
		states = ScaleStates.UP;
	}

	public void setUpIdle(){
		temp = tempUpIdle;
		states = ScaleStates.UP_IDLE;
	}

	public void setDown(){
		temp = tempDown;
		states = ScaleStates.DOWN;
	}

	public void setDownIdle(){
		temp = tempDownIdle;
		states = ScaleStates.DOWN_IDLE;
	}


	// Behaviours
	private void UpBehaviour(){

		temp-= Time.deltaTime;

		transform.localScale = new Vector3(Mathf.Lerp(alphaMinX,alphaMaxX,temp/tempUp),Mathf.Lerp(alphaMinY,alphaMaxY,temp/tempUp),transform.localScale.z);

		if(temp<0){
			setUpIdle();
		}

	}

	private void UpIdleBehaviour(){
		temp-= Time.deltaTime;
		
		if(temp<0){
			setDown();
		}
	}

	private void DownBehaviour(){
		temp-= Time.deltaTime;

		transform.localScale = new Vector3(Mathf.Lerp(alphaMaxX,alphaMinX,temp/tempDown),Mathf.Lerp(alphaMaxY,alphaMinY,temp/tempDown),transform.localScale.z);

		if(temp<0){
			setDownIdle();
		}
	}

	private void DownIdleBehaviour(){
		temp-= Time.deltaTime;
		
		if(temp<0){
			setUp();
		}
	}
}
