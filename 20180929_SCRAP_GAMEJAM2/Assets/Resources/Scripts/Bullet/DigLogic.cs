using UnityEngine;
using System.Collections;

public class DigLogic: MonoBehaviour {

	public float damage;
	private Transform myTransform;
	public Transform prefabPSDamage;
	public Transform prefabPSFire;
    private GameObject prefabDamage;

	public AudioManager audioManger;
    private BoxCollider boxColliderDig;
    private PlayerLogic playerLogic;
	// Use this for initialization
	void Start () {
		audioManger = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerLogic = transform.parent.transform.GetComponent<PlayerLogic>();
        boxColliderDig = GetComponent<BoxCollider>();
		myTransform = this.transform;
        prefabDamage = (GameObject)Instantiate(prefabPSDamage.gameObject, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        prefabDamage.GetComponent<ParticleSystem>().Stop();
    }

	void OnTriggerEnter(Collider other){

		if(other.tag == "Block"){
            // IF IS NOT A HEAVY ROCK YOU CAN DIG
            if (other.GetComponent<BlockLogic>().type <= 3)
            {

                boxColliderDig.enabled = false;
                prefabDamage.transform.position = new Vector3(myTransform.position.x, myTransform.position.y - 1, myTransform.position.z);
                prefabDamage.GetComponent<ParticleSystem>().Play();
                playerLogic.addPiece(other.GetComponent<BlockLogic>().type);

                audioManger.Play(audioManger.catchPieceBoss, transform.position);
                Destroy(other.gameObject);
            }
		}

	}
}
