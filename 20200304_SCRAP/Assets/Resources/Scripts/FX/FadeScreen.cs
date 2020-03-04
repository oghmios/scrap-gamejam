using UnityEngine;
using System.Collections;

public class FadeScreen : MonoBehaviour 
{
	public SpriteRenderer screen;
	public Color color;
	private float smooth;

	private float alpha;
	private bool doTransition;
	private bool fade; //fade = true --> fadeIN

	// Use this for initialization
	void Start () 
	{
		color.a = 1;
		screen.color = color;
        startFade(false, 1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(doTransition)
		{
			if (fade)
			{
				if(alpha <= 1.0f) alpha += smooth * Time.deltaTime;
				else doTransition = false;
			}
			else
			{
				if(alpha >= 0.0f) alpha -= smooth * Time.deltaTime;
				else doTransition = false;
			}
			color.a = alpha;
			screen.color = color;
		}
	}

	public void startFade (bool fadeIN, float smooth)
	{
		this.smooth = smooth;
		fade = fadeIN;
		doTransition = true;
        alpha = color.a;
	}

	public bool isFadeOut()
	{ 
		if (alpha <= 0.0f) return true;
		else return false;
	}
	public bool isFadeIn()
	{
		if (alpha >= 1.0f) return true;
		else return false;
	}
	public void setFadeSmooth(float newSmooth) { smooth = newSmooth; }
	public bool playingTransition() { return doTransition; }
	public void setStartBlack(bool black) 
	{ 
		if (black)
			alpha = 1;
		else
			alpha = 0;
		
		color.a = alpha;
		screen.color = color;
	}

	//FADE IN: a negro
	//FADE OUT: a transparente

}
