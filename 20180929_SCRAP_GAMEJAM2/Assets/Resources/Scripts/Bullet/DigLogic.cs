using UnityEngine;
using System.Collections;

public class DigLogic: MonoBehaviour {

	
    public ParticleSystem psDigStones;
    public ParticleSystem psDigStonesGround;
    public PlayerLogic playerLogic;

    private ParticleSystem.MainModule mainPSDigStones;
    private ParticleSystem.MainModule mainPSDigStonesGround;
    private BoxCollider boxColliderDig;
    private Transform myTransform;

	// Use this for initialization
	void Start () {
        myTransform = this.transform;
        //playerLogic = myTransform.parent.transform.GetComponent<PlayerLogic>();
        boxColliderDig = GetComponent<BoxCollider>();
        mainPSDigStones = psDigStones.main;
        mainPSDigStonesGround = psDigStonesGround.main;
        psDigStones.Stop();
        psDigStonesGround.Stop();
    }

	void OnTriggerEnter(Collider other){

		if(other.tag == "Block"){
            // IF IS NOT A HEAVY ROCK YOU CAN DIG
            if (other.GetComponent<BlockLogic>().type <= 3)
            {

                boxColliderDig.enabled = false;
                // psDigStones.transform.position = new Vector3(myTransform.position.x, myTransform.position.y - 1, myTransform.position.z);
                if (playerLogic.state == PlayerLogic.PlayerStates.DASHDOWN_DIG)
                {

                    mainPSDigStones.startSpeed = 40;
                    psDigStones.Play();
                    mainPSDigStonesGround.startSpeed = 50;
                    psDigStonesGround.Play();
                }
                else {
                    mainPSDigStones.startSpeed = 20;
                    psDigStones.Play();
                    mainPSDigStonesGround.startSpeed = 15;
                    psDigStonesGround.Play();
                }

                playerLogic.addPiece(other.GetComponent<BlockLogic>().type);
                
                CoreManager.Audio.Play(CoreManager.Audio.playerDig, myTransform.position);
                // Destroy(other.gameObject);
                other.gameObject.SetActive(false);
            }
		}

	}
}
