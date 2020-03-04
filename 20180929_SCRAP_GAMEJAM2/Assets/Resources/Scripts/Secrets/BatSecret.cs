using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSecret : MonoBehaviour {

    public enum BatSecretStates { SLEEP, IDLE, CHANGE_DIRECTION, DISSAPEAR, EXPLODE, EXPLODE_WRONG, DECREASE_BATLAYER }
    public BatSecretStates state;

    [Header("LEVEL BAT")]
    // BAT TYPE (based on remains)
    // 0 = flesh
    // 1 = weapons
    // 2 = armor
    // 3 = jewel
    // 4 = ALL (MULTI)
    public int[] batType;
    private int batTypeIndex;
    // batScore = 0 --> Secret found!
    // batScore > 0 --> Bonus Score!
    public int batScore;

    [Header("BAT SETTINGS")]
    public float secondsWaitingSleepToIdle;

    // NUMBER OF TIME CHANGE DIRECTION - ONLY EVEN!! (PARES)
    public int numChangeDirectionOrig;
    private int numChangeDirection;
    public float secondsChagingDirection;
    public ParticleSystem psExplosion;
    public ParticleSystem psLayerFlesh, psLayerWeapon, psLayerArmor, psLayerCoin;
    public SpriteRenderer spriteBat;

    public float speed;
    public bool isFacingRight;
    public GameLogic gameLogic;
    private float temp, tempColor;
    private int iColor;
    private Transform myTransform;
    private Vector3 positionOrig;
    private bool isFacingOrig;
    
    // Use this for initialization
    void Start () {

        // ONLY FOR TEST --> Pick randomized the bat type
        for (int i = 0; i < batType.Length; i++) {
            batType[i] = Random.Range(0, 5);
        }

        batTypeIndex = batType.Length-1;

        if (batType[batTypeIndex] == 0)
            spriteBat.color = Color.red;
        else if (batType[batTypeIndex] == 1)
            spriteBat.color = Color.blue;
        else if (batType[batTypeIndex] == 2)
            spriteBat.color = Color.green;
        else if (batType[batTypeIndex] == 3)
            spriteBat.color = Color.yellow;
           
        // FOR MULTIPCOLOR batType == 4
        iColor = 0;
        tempColor = 0.05f;

        psExplosion.Stop();
        psLayerFlesh.Stop();
        psLayerWeapon.Stop();
        psLayerArmor.Stop();
        psLayerCoin.Stop();
        myTransform = this.transform;
        positionOrig = myTransform.position;
        isFacingOrig = isFacingRight;

        setSleep();
	}
	
	// Update is called once per frame
	void Update () {
        
        // SLEEP, IDLE, CHANGE_DIRECTION, DISSAPEAR, EXPLODE
        switch (state)
        {
            case BatSecretStates.SLEEP:
                SleepBehaviour();
                break;
            case BatSecretStates.IDLE:
                IdleBehaviour();
                break;
            case BatSecretStates.CHANGE_DIRECTION:
                ChangeDirectionBehaviour();
                break;
            case BatSecretStates.DISSAPEAR:
                DissapearBehaviour();
                break;
            case BatSecretStates.EXPLODE:
                ExplodeBehaviour();
                break;
            case BatSecretStates.EXPLODE_WRONG:
                ExplodeWrongBehaviour();
                break;
            case BatSecretStates.DECREASE_BATLAYER:
                DecreaseBatLayerBehaviour();
                break; 
        }

        // FOR MULTIPCOLOR batType == 4
        if (batType[batTypeIndex] == 4) {

            tempColor -= Time.deltaTime;

            if (tempColor < 0) {

                if (iColor == 0)
                {
                    spriteBat.color = Color.red;
                    iColor++;
                    tempColor = 0.05f;
                }
                else if (iColor == 1)
                {
                    spriteBat.color = Color.blue;
                    iColor++;
                    tempColor = 0.05f;
                }
                else if (iColor == 2)
                {
                    spriteBat.color = Color.green;
                    iColor++;
                    tempColor = 0.05f;
                }
                else if (iColor == 3)
                {
                    spriteBat.color = Color.yellow;
                    iColor = 0;
                    tempColor = 0.05f;
                }

            }
        }
    }

    // SETS
    public void setSleep() {
        numChangeDirection = numChangeDirectionOrig;
        temp = secondsWaitingSleepToIdle;
        myTransform.position = positionOrig;
        isFacingRight = isFacingOrig;

        if (isFacingRight)
            myTransform.localScale = new Vector3(-(Mathf.Abs(myTransform.localScale.x)) , myTransform.localScale.y, myTransform.localScale.z);
        else
            myTransform.localScale = new Vector3(Mathf.Abs(myTransform.localScale.x), myTransform.localScale.y, myTransform.localScale.z);

        state = BatSecretStates.SLEEP;
    }

    public void setIdle() {

        CoreManager.Audio.Play(CoreManager.Audio.batAppear, myTransform.position);

        if (numChangeDirection > 0) {
            if (numChangeDirection == numChangeDirectionOrig)
                temp = secondsChagingDirection;
            else
                temp = secondsChagingDirection - Random.Range(0, secondsChagingDirection);
        }
        

        state = BatSecretStates.IDLE;
    }

    public void setChangeDirection()
    {

        temp = 0.1f;

        Flip();

        state = BatSecretStates.CHANGE_DIRECTION;
    }

    public void setDisappear()
    {

        state = BatSecretStates.DISSAPEAR;
    }

    public void setExplode(bool isFromPlayer1)
    {
        CoreManager.Audio.Play(CoreManager.Audio.batCrow, myTransform.position);
        CoreManager.Audio.Play(CoreManager.Audio.batExplosion, myTransform.position);
        psExplosion.Play();
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().detectCollisions = false;
        spriteBat.enabled = false;

        gameLogic.batSecretFound(batScore, isFromPlayer1);

        state = BatSecretStates.EXPLODE;
    }

    public void setDecreaseBatLayer() {

            //CoreManager.Audio.Play(CoreManager.Audio.batCrow, myTransform.position);
            CoreManager.Audio.Play(CoreManager.Audio.batBreakLayer, myTransform.position);

            batTypeIndex--;

            if (batType[batTypeIndex] == 0)
                spriteBat.color = Color.red;
            else if (batType[batTypeIndex] == 1)
                spriteBat.color = Color.blue;
            else if (batType[batTypeIndex] == 2)
                spriteBat.color = Color.green;
            else if (batType[batTypeIndex] == 3)
                spriteBat.color = Color.yellow;

            temp = 0.25f;
            state = BatSecretStates.DECREASE_BATLAYER;
    }

    // IMPACTS THE BAT WITH THE WRONG BULLET --> INCREASE SPEED
    public void setExplodeWrong()
    {
        CoreManager.Audio.Play(CoreManager.Audio.batFail, myTransform.position);
        CoreManager.Audio.Play(CoreManager.Audio.batExplosionFail, myTransform.position);
        temp = 0.25f;
        speed+=5;
        state = BatSecretStates.EXPLODE_WRONG;
    }

    // BEHAVIOURS
    private void SleepBehaviour()
    {
        temp -= Time.deltaTime;

        if (temp < 0) {
            setIdle();
        }
    }

    
    public void Flip()
    {
        //myTransform.localScale = new Vector3(-myTransform.localScale.x, myTransform.localScale.y, myTransform.localScale.z);
        spriteBat.flipX = !spriteBat.flipX;
        isFacingRight = !isFacingRight; // myTransform.localScale.x > 0;
    }
     
    private void IdleBehaviour()
    {
     
        if (isFacingRight)
            myTransform.Translate((Vector3.right) * speed * Time.deltaTime);
        else
            myTransform.Translate((Vector3.left) * speed* Time.deltaTime);

        if (numChangeDirection > 0)
        {
            temp -= Time.deltaTime;

            if (temp < 0)
                setChangeDirection();
        }
    }

    private void ChangeDirectionBehaviour()
    {

        temp -= Time.deltaTime;

        if (temp < 0)
        {
            numChangeDirection--;
            setIdle();
        }
    }

    private void DissapearBehaviour()
    {

        
    }

    private void ExplodeBehaviour()
    {

        
    }

    private void ExplodeWrongBehaviour()
    {
        temp -= Time.deltaTime;

        if (temp < 0)
        {
            setIdle();
        }
    }

    private void DecreaseBatLayerBehaviour()
    {
        temp -= Time.deltaTime;

        if (temp < 0)
        {
            setIdle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "BatSecretTriggerEnd") {
            setSleep();
        }

        if (other.tag == "Bullet") {
            // Type 4 MultiColor ---> EXPLODE!
            if (batType[batTypeIndex] == 4) {

                other.GetComponent<GranadeLogic>().isTouchedBird = true;

                if (batTypeIndex <= 0)
                    setExplode(other.GetComponent<GranadeLogic>().IsFromPlayer1);
                else
                {
                    psLayerFlesh.Play(); 
                    psLayerWeapon.Play();
                    psLayerArmor.Play();
                    psLayerCoin.Play();

                    setDecreaseBatLayer();
                }
                    
            }
            // Same type ---> EXPLODE!
            else if(other.GetComponent<GranadeLogic>().typeBullet == batType[batTypeIndex]) {

                other.GetComponent<GranadeLogic>().isTouchedBird = true;

                if (batTypeIndex <= 0)
                    setExplode(other.GetComponent<GranadeLogic>().IsFromPlayer1);
                else
                {
                    if (batType[batTypeIndex] == 0)
                        psLayerFlesh.Play(); 
                    else if (batType[batTypeIndex] == 1)
                        psLayerWeapon.Play();
                    else if (batType[batTypeIndex] == 2)
                        psLayerArmor.Play();
                    else if (batType[batTypeIndex] == 3)
                        psLayerCoin.Play();
                    setDecreaseBatLayer();
                }
            }
            // IMPACTS THE BAT WITH THE WRONG BULLET --> EXPLODE WRONG! INCREASE SPEED
            else if (other.GetComponent<GranadeLogic>().typeBullet != batType[batTypeIndex])
            {
                setExplodeWrong();
            }
        }
        
    }

}
