using UnityEngine;

public class WormBubbleLogic : MonoBehaviour {

    public enum WormBubbleStates { SLEEP, INCREASE, EXPLODE }
    public WormBubbleStates state;

    public float tempIncrease;
    private float temp;
    private Transform myTransform;
    public Vector3 scaleOrig = Vector3.one;
    public Vector3 scaleChange = new Vector3(0.01f, 0.01f, 0.01f);
    private SpriteRenderer spriteBubble;
    public WormDropLogic wormDrop;

    // Use this for initialization
    void Start () {

        myTransform = this.transform;
        spriteBubble = GetComponent<SpriteRenderer>();

        setSleep();
    }
	
	// Update is called once per frame
	void Update () {
        // SLEEP, INCREASE, EXPLODE,
        switch (state)
        {
            case WormBubbleStates.SLEEP:
                SleepBehaviour();
                break;
            case WormBubbleStates.INCREASE:
                IncreaseBehaviour();
                break;
            case WormBubbleStates.EXPLODE:
                ExplodeBehaviour();
                break;
            
        }
    }
    // SETS
    public void setSleep()
    {
        myTransform.localScale = scaleOrig;
        spriteBubble.enabled = false;
        state = WormBubbleStates.SLEEP;
    }

    public void setIncrease() {
        temp = tempIncrease;
        myTransform.localScale = scaleOrig;
        CoreManager.Audio.Play(CoreManager.Audio.wormPutEgg, myTransform.position);
        wormDrop.hiddeDrop();

        spriteBubble.enabled = true;

        state = WormBubbleStates.INCREASE;
    }

    public void setExplode() {

        wormDrop.restoreDrop(myTransform.position, spriteBubble.color);
        CoreManager.Audio.Play(CoreManager.Audio.wormEggExplosion, myTransform.position);
        state = WormBubbleStates.EXPLODE;
    }

    // BEHAVIOURS
    private void SleepBehaviour()
    {

        
    }

    private void IncreaseBehaviour()
    {

        temp -= Time.deltaTime;

        myTransform.localScale += scaleChange;

        if (temp < 0)
            setExplode();

    }

    private void ExplodeBehaviour()
    {

        setSleep();
    }

}
