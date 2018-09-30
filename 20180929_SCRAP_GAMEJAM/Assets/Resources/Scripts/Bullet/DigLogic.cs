using UnityEngine;
using System.Collections;

public class DigLogic: MonoBehaviour {

	public float damage;
	private Transform myTransform;
	public Transform prefabPSDamage;
	public Transform prefabPSFire;

	public AudioManager audioManger;
    private BoxCollider boxColliderDig;
    private PlayerLogic playerLogic;
	// Use this for initialization
	void Start () {
		audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerLogic = transform.parent.transform.GetComponent<PlayerLogic>();
        boxColliderDig = GetComponent<BoxCollider>();
		myTransform = this.transform;

	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnTriggerEnter(Collider other){

		//GameObject prefabFire = (GameObject) Instantiate(prefabPSFire.gameObject, new Vector3(transform.position.x, transform.position.y,-2), Quaternion.identity);
		//Destroy(prefabFire,1);

		if(other.tag == "Block"){
            boxColliderDig.enabled = false;
            playerLogic.addPiece(other.GetComponent<BlockLogic>().type);
            audioManger.Play(audioManger.catchPieceBoss, transform.position);
            Destroy(other.gameObject);
				// Instanciamos daño
			/*	GameObject prefabDamage = (GameObject) Instantiate(prefabPSDamage.gameObject,new Vector3(transform.position.x, transform.position.y,-2), Quaternion.identity);
				Destroy(prefabDamage,1);
				audioManger.Play(audioManger.impactBoss,transform.position);
				other.GetComponent<BossLogic>().addDamage(damage);
				*/

		}

	}
}
