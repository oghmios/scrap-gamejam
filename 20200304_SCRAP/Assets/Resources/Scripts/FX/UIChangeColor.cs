using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIChangeColor : MonoBehaviour
{
	public enum ColorStates {NONE,ANIMATION}

    public ColorStates states;
	public float tempAnimation;
    public float speedAnimation = 1;
	private float temp;
	private Color colorAux;
    private Image imageUI;

	void Start(){

        imageUI = GetComponent<Image>();

        colorAux = GetComponent<Image>().color;

		setNone();

	}

	void Update () {

		switch(states){
            case ColorStates.NONE:
				NoneBehaviour();
				break;
            case ColorStates.ANIMATION:
				AnimationBehaviour();
				break;
		}
	}

	// Sets
	public void setNone(){
        imageUI.color = colorAux;
        states = ColorStates.NONE;
	}

	public void setAnimation(){
		temp = tempAnimation;
        states = ColorStates.ANIMATION;
	}

    public void setDead() {
        imageUI.color = Color.black;
    }

	// Behaviours

	private void NoneBehaviour(){

	}

	private void AnimationBehaviour(){
		temp-= Time.deltaTime;

        imageUI.color = Color.Lerp(Color.red, colorAux, Mathf.Sin(Time.time*speedAnimation));

		if(temp<0){
			setNone();
		}
	}


}
