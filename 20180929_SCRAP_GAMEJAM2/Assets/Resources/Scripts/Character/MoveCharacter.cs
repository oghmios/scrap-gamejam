﻿using UnityEngine;
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
    public Animator animatorCharacter;
    private float horizontalDirection;
    public float jumpTime = 0.5f; 
    private float jumpTimeCounter;
    public bool horizontalControl;

    private void Start(){
        horizontalControl = true;
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

    void FixedUpdate()
    {
        if (horizontalControl)
            horizontalDirection = Input.GetAxis("Horizontal");
        else
            horizontalDirection = 0;

        rigid.velocity = new Vector3(horizontalDirection * speed, rigid.velocity.y, 0);
        
        if (!isGround) {
            rigid.velocity -= new Vector3(0,gravity,0);
        }
    }

    void Update() {


		

        // && !animatorCharacter.GetCurrentAnimatorStateInfo(0).IsName("Jump")

        if (!isGround)
        {
            animatorCharacter.SetBool("isWalking", false);
            animatorCharacter.SetBool("isFall", true);
            
        }

        if (horizontalDirection != 0 && isGround)
        {
            animatorCharacter.SetBool("isFall", false);
            animatorCharacter.SetBool("isJump", false);
            animatorCharacter.SetBool("isWalking", true);
            
        }

        if (horizontalDirection == 0 && isGround) {
            animatorCharacter.SetBool("isFall", false);
            animatorCharacter.SetBool("isJump", false);
            animatorCharacter.SetBool("isWalking", false);
            
        }



        Collider[] colliderRight = Physics.OverlapBox(colliderHitsRight.position, sizeSide, Quaternion.identity, groundMask);
		Collider[] colliderLeft = Physics.OverlapBox(colliderHitsLeft.position, sizeSide, Quaternion.identity, groundMask);

        // Debug.Log(rigid.velocity);

        if (colliderRight.Length<=0 || colliderLeft.Length<=0){
			
			rigid.velocity = new Vector3(horizontalDirection*speed*0.5f,rigid.velocity.y,0);
            
		  }

		// Collider[] collidersHits = Physics.OverlapSphere(checkGround.position,0.05f,groundMask);
		Collider[] collidersHits = Physics.OverlapBox(checkGround.position,sizeGround, Quaternion.identity, groundMask);

        

        if (collidersHits.Length>0){
			isGround = true;
            
        } else {
			isGround = false;
        }

		Collider[] collidersDamageHits = Physics.OverlapSphere(checkGround.position,0.05f,groundDamageMask);


		
		if(collidersDamageHits.Length>0){
			
			transform.GetComponent<PlayerLogic>().setDie();
		}
        /*
		if(isGround){
			isJump = false;
            if (Input.GetButtonDown("Jump") && !isJump && rigid.velocity.y == 0)
            {
                animatorCharacter.SetBool("isJump", true);
                audioManger.Play(audioManger.jumpPlayer, transform.position);
                isJump = true;
                rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }


		}
        */

        // incrementar la deceleracion en el salto
        if (!isGround && isJump && rigid.velocity.y < 0) {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y*2000, rigid.velocity.z);
        }

        // PROBAR
        if (isGround && Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            animatorCharacter.SetBool("isJump", true);
            jumpTimeCounter = jumpTime;
            rigid.velocity = Vector3.up * jumpSpeed;
        }

        if (isJump && Input.GetButton("Jump"))
        {
            if (jumpTimeCounter > 0)
            {
                animatorCharacter.SetBool("isJump", true);
                rigid.velocity = Vector3.up * jumpSpeed;
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJump = false;
            }
        }

        if (isJump && Input.GetButtonUp("Jump"))
        {
            isJump = false;
        }

            if (horizontalDirection<0){
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
