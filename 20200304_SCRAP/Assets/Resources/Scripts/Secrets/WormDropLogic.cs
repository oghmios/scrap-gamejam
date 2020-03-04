using UnityEngine;

public class WormDropLogic : MonoBehaviour {

    public ParticleSystem psDigStones;
    public ParticleSystem psDigStonesGround;
    public GameLogic gameLogic;

    private ParticleSystem.MainModule mainPSDigStones;
    private ParticleSystem.MainModule mainPSDigStonesGround;
    private BoxCollider boxColliderDig;
    private SpriteRenderer spriteDrop;
    private Transform myTransform;
    private Rigidbody rigidDrop;
    public bool isToxic;
    public Sprite blockFlesh;


    // Use this for initialization
    void Start () {

        myTransform = this.transform;
        boxColliderDig = GetComponent<BoxCollider>();
        spriteDrop = GetComponent<SpriteRenderer>();
        rigidDrop = GetComponent<Rigidbody>();
        mainPSDigStones = psDigStones.main;
        mainPSDigStonesGround = psDigStonesGround.main;
        psDigStones.Stop();
        psDigStonesGround.Stop();

        hiddeDrop();
    }

    public void restoreDrop(Vector3 positionBubble, Color colorBubble) {
        spriteDrop.color = colorBubble;
        boxColliderDig.enabled = true;
        spriteDrop.enabled = true;
        rigidDrop.detectCollisions = true;
        myTransform.position = positionBubble;
        rigidDrop.useGravity = true;
        if (isToxic)
        {
            // green
            rigidDrop.mass = 1;
            myTransform.localScale = Vector3.one * 3;
        }
        else {
            // purple (poison)
            rigidDrop.mass = 3;
            myTransform.localScale = Vector3.one * 6;
        }
    }

    public void hiddeDrop() {
        // rigidbodyDrop.velocity = Vector3.zero;
        // transformDrop.gameObject.SetActive(false);
        // transformDrop.GetComponent<SpriteRenderer>().color = spriteBubble.color;
        boxColliderDig.enabled = false;
        spriteDrop.enabled = false;
        rigidDrop.detectCollisions = false;
        rigidDrop.velocity = Vector3.zero;
        rigidDrop.useGravity = false;

    }

    void OnTriggerEnter(Collider other)
    {

        // IF DROP TOUCHES BLOCK
        if (other.tag == "Block")
        {
            // DROP TOXIC - DEATH (GREEN)
            if (isToxic)
            {
                // IF IS NOT A HEAVY ROCK YOU CAN DIG
                if (other.GetComponent<BlockLogic>().type <= 3)
                {
                    if (other.GetComponent<BlockLogic>().challengeBlock)
                    {
                        gameLogic.setChallengeBlock();
                    }
                    hiddeDrop();
                    mainPSDigStones.startSpeed = 40;
                    psDigStones.Play();
                    mainPSDigStonesGround.startSpeed = 50;
                    psDigStonesGround.Play();
                    CoreManager.Audio.Play(CoreManager.Audio.playerDig, myTransform.position);
                    other.gameObject.SetActive(false);
                    gameLogic.substractBlockRamaining();

                }
            }
            // DROP POISON (PURPLE)
            else {
                    if (other.GetComponent<BlockLogic>().type <= 3)
                    {
                        hiddeDrop();
                        mainPSDigStones.startSpeed = 40;
                        psDigStones.Play();
                        mainPSDigStonesGround.startSpeed = 50;
                        psDigStonesGround.Play();
                        other.GetComponent<SpriteRenderer>().sprite = blockFlesh;
                        other.GetComponent<BlockLogic>().type = 0;
                        CoreManager.Audio.Play(CoreManager.Audio.wormEggExplosion, myTransform.position);
                    }
                }
            // IF DROP TOUCHES PLAYER
        } else if (other.tag == "Player" && other.GetComponent<PlayerLogic>().state != PlayerLogic.PlayerStates.DIE)
        {
            hiddeDrop();

            if (isToxic)
                other.GetComponent<PlayerLogic>().setDie(0);
            else
                other.GetComponent<PlayerLogic>().playerIsPoison();
            
        }

    }
	
}
