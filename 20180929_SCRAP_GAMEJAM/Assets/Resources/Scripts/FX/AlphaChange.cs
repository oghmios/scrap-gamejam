using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlphaChange : MonoBehaviour
{
	public enum AlphaStates {UP,UP_IDLE, DOWN, DOWN_IDLE}

	public AlphaStates states;
	public float tempUp;
	public float tempUpIdle;
	public float tempDown;
	public float tempDownIdle;
	private float temp;
	public float alphaMax;
	public float alphaMin;

	private Color colorAux;

	void Start(){

		colorAux = GetComponent<Image>().color;

		setUp();

	}

	void Update () {

		switch(states){
			case AlphaStates.UP:
				UpBehaviour();
				break;
			case AlphaStates.UP_IDLE:
				UpIdleBehaviour();
				break;
			case AlphaStates.DOWN:
				DownBehaviour();
				break;
			case AlphaStates.DOWN_IDLE:
				DownIdleBehaviour();
				break;
		}
	}

	// Sets
	public void setUp(){

		temp = tempUp;
		states = AlphaStates.UP;
	}

	public void setUpIdle(){
		temp = tempUpIdle;
		states = AlphaStates.UP_IDLE;
	}

	public void setDown(){
		temp = tempDown;
		states = AlphaStates.DOWN;
	}

	public void setDownIdle(){
		temp = tempDownIdle;
		states = AlphaStates.DOWN_IDLE;
	}


	// Behaviours
	private void UpBehaviour(){

		temp-= Time.deltaTime;

		colorAux.a = Mathf.Lerp(alphaMin,alphaMax,temp/tempUp);

		GetComponent<Image>().color = colorAux;

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

		colorAux.a = Mathf.Lerp(alphaMax,alphaMin,temp/tempDown);

		GetComponent<Image>().color = colorAux;

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
