using UnityEngine;
using System.Collections;

public class MoveCharacter : MonoBehaviour {


	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	// private CharacterController controller;
	private bool isJump;
	public bool isGround;
	private bool aircontrol;
	public Transform checkGround;
	public Transform colliderHitsRight;
	public Transform colliderHitsLeft;
	public Transform bulletSource;
	public LayerMask groundMask;
	public LayerMask groundDamageMask;
	private GameLogic gameLogic;
	public AudioManager audioManger;
	public SpriteRenderer spriteCharacter;
	public Rigidbody rigid;
	public Vector3 sizeSide;
	public Vector3 sizeGround;

	private void Start(){
		rigid = GetComponent<Rigidbody>();
		isJump = false;
		aircontrol = true;
		audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
		gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
		// controller = GetComponent<CharacterController>();
		moveDirection.y = 0;
	}


	void OnDrawGizmos(){
		Gizmos.DrawCube(checkGround.position, sizeGround);

		Gizmos.DrawCube(colliderHitsRight.position, sizeSide);

		Gizmos.DrawCube(colliderHitsLeft.position, sizeSide);
	}

	void Update() {


		float horizontalDirection = Input.GetAxis("Horizontal");

		Collider[] colliderRight = Physics.OverlapBox(colliderHitsRight.position, sizeSide, Quaternion.identity, groundMask);
		Collider[] colliderLeft = Physics.OverlapBox(colliderHitsLeft.position, sizeSide, Quaternion.identity, groundMask);

		Debug.Log(colliderRight.Length+" "+colliderLeft.Length);

		  if (colliderRight.Length<=0 || colliderLeft.Length<=0){
			
			rigid.velocity = new Vector3(horizontalDirection*speed*0.5f,rigid.velocity.y,0);  

		  }

		// Collider[] collidersHits = Physics.OverlapSphere(checkGround.position,0.05f,groundMask);
		Collider[] collidersHits = Physics.OverlapBox(checkGround.position,sizeGround, Quaternion.identity, groundMask);

		if(collidersHits.Length>0){
			isGround = true;
		
		} else {
			isGround = false;

		}

		Collider[] collidersDamageHits = Physics.OverlapSphere(checkGround.position,0.05f,groundDamageMask);;


		
		if(collidersDamageHits.Length>0){
			Debug.Log ("collidersDamageHits");
			transform.GetComponent<PlayerLogic>().setDie();
		}

		if(isGround){
			isJump = false;
			if (Input.GetButtonDown("Jump") && !isJump)  {
				audioManger.Play(audioManger.jumpPlayer,transform.position);
				isJump = true;
				rigid.velocity = new Vector3(rigid.velocity.x,0,0); 
				rigid.AddForce(0,jumpSpeed,0);
			}


		}

		if(horizontalDirection<0){
			bulletSource.localPosition = new Vector3(-3f,0,0);
			bulletSource.localRotation = Quaternion.Euler(new Vector3(0,-180,0));
			spriteCharacter.flipX = true;


		} else if(horizontalDirection>0) {
			bulletSource.localPosition = new Vector3(3f,0,0);
			bulletSource.localRotation = Quaternion.identity;
			spriteCharacter.flipX = false;

		}


	}
}
