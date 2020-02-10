using UnityEngine;

public class WormDropLogic : MonoBehaviour {

    public ParticleSystem psDigStones;
    public ParticleSystem psDigStonesGround;
    public PlayerLogic playerLogic;

    private ParticleSystem.MainModule mainPSDigStones;
    private ParticleSystem.MainModule mainPSDigStonesGround;
    private BoxCollider boxColliderDig;
    private SpriteRenderer spriteDrop;
    private Transform myTransform;
    private Rigidbody rigidDrop;


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


        if (other.tag == "Block")
        {
            Debug.Log(other.name);
            // IF IS NOT A HEAVY ROCK YOU CAN DIG
            if (other.GetComponent<BlockLogic>().type <= 3)
            {
                hiddeDrop();
                mainPSDigStones.startSpeed = 40;
                psDigStones.Play();
                mainPSDigStonesGround.startSpeed = 50;
                psDigStonesGround.Play();
                /*
                // psDigStones.transform.position = new Vector3(myTransform.position.x, myTransform.position.y - 1, myTransform.position.z);
                if (playerLogic.state == PlayerLogic.PlayerStates.DASHDOWN_DIG)
                {

                    mainPSDigStones.startSpeed = 40;
                    psDigStones.Play();
                    mainPSDigStonesGround.startSpeed = 50;
                    psDigStonesGround.Play();
                }
                else
                {
                    mainPSDigStones.startSpeed = 20;
                    psDigStones.Play();
                    mainPSDigStonesGround.startSpeed = 15;
                    psDigStonesGround.Play();
                }*/

                CoreManager.Audio.Play(CoreManager.Audio.playerDig, myTransform.position);
                // Destroy(other.gameObject);
                other.gameObject.SetActive(false);
                
            }
        } else if (other.tag == "Player" && playerLogic.state != PlayerLogic.PlayerStates.DIE)
        {
            hiddeDrop();

            playerLogic.setDie(0);
            
        }

    }
	
}
