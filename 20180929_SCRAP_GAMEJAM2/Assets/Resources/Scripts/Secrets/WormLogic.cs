using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormLogic : MonoBehaviour {

    public enum WormStates { SLEEP, IDLE, CHANGE_DIRECTION }
    public WormStates state;

    [Header("LEVEL WORM")]
    // WORM TYPE (based on effects)
    // 0 = 
    // 1 = 
    // 2 = 
    // 3 = 
    public int batType;

    [Header("WORM SETTINGS")]
    public float secondsChagingDirection;

    public SpriteRenderer spriteWorm;

    public float speed;
    public bool isFacingRight;

    public WormBubbleLogic wormBubble;
    public float tempWaitBubble;
    private float tempBubble;

    private float temp, tempColor;
    private int iColor;
    private Transform myTransform;
    private Vector3 positionOrig;
    private bool isFacingOrig;
    public GameLogic gameLogic;
    
    // Use this for initialization
    void Start () {
        /*
            batType = Random.Range(0, 4);

        if (batType == 0)
            spriteWorm.color = Color.red;
        else if (batType == 1)
            spriteWorm.color = Color.blue;
        else if (batType == 2)
            spriteWorm.color = Color.green;
        else if (batType == 3)
            spriteWorm.color = Color.yellow;*/
        spriteWorm.color = new Color(0.2f, 0.8f, 0.2f);
        wormBubble.GetComponent<SpriteRenderer>().color = spriteWorm.color;

        // FOR MULTIPCOLOR batType == 4
        iColor = 0;
        tempColor = 0.05f;

        myTransform = this.transform;
        positionOrig = myTransform.position;
        isFacingOrig = isFacingRight;

        tempBubble = tempWaitBubble;

        setIdle();
	}
	
	// Update is called once per frame
	void Update () {
        
        // SLEEP, IDLE, CHANGE_DIRECTION,
        switch (state)
        {
            case WormStates.IDLE:
                IdleBehaviour();
                break;
            case WormStates.CHANGE_DIRECTION:
                ChangeDirectionBehaviour();
                break;
            case WormStates.SLEEP:
                SleepBehaviour();
                break;
        }

        tempBubble -= Time.deltaTime;

        if (tempBubble < 0 && gameLogic.state != GameLogic.GameStates.RESULTS ) {
            wormBubble.transform.position = myTransform.position;
            wormBubble.setIncrease();
            tempBubble = tempWaitBubble;
        }

    }

    // SETS
    public void setSleep() {

        myTransform.position = positionOrig;
        isFacingRight = isFacingOrig;

        if (!isFacingRight)
            myTransform.localScale = new Vector3(-(Mathf.Abs(myTransform.localScale.x)) , myTransform.localScale.y, myTransform.localScale.z);
        else
            myTransform.localScale = new Vector3(Mathf.Abs(myTransform.localScale.x), myTransform.localScale.y, myTransform.localScale.z);

        state = WormStates.SLEEP;
    }

    public void setIdle() {

        CoreManager.Audio.Play(CoreManager.Audio.wormAppear, myTransform.position);

        temp = secondsChagingDirection - Random.Range(0, secondsChagingDirection);
        

        state = WormStates.IDLE;
    }

    public void setChangeDirection()
    {

        temp = 0.1f;

        Flip();

        state = WormStates.CHANGE_DIRECTION;
    }

    // BEHAVIOURS
    private void SleepBehaviour()
    {
        /*
        temp -= Time.deltaTime;

        if (temp < 0) {
            setIdle();
        }*/
    }

    
    public void Flip()
    {
        //myTransform.localScale = new Vector3(-myTransform.localScale.x, myTransform.localScale.y, myTransform.localScale.z);
        spriteWorm.flipX = !spriteWorm.flipX;
        isFacingRight = !isFacingRight; // myTransform.localScale.x > 0;
    }
     
    private void IdleBehaviour()
    {
     
        if (isFacingRight)
            myTransform.Translate((Vector3.right) * speed * Time.deltaTime);
        else
            myTransform.Translate((Vector3.left) * speed* Time.deltaTime);

            temp -= Time.deltaTime;

            if (temp < 0)
                setChangeDirection();
    }

    private void ChangeDirectionBehaviour()
    {

        temp -= Time.deltaTime;

        if (temp < 0)
        {
            setIdle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "WormTriggerEnd") {
            setChangeDirection();
        }
        
    }

}
