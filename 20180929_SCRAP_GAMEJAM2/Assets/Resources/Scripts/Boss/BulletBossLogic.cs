using UnityEngine;
using System.Collections;

public class BulletBossLogic: MonoBehaviour {

	public float speed;
	public float damage;
	private Transform myTransform;
	public Transform prefabPSDamage;
	public Transform prefabPSFire;

	// Use this for initialization
	void Start () {
	
		myTransform = this.transform;

	}
	
	// Update is called once per frame
	void Update () {
	
		// Desplazamiento del proyectil
		transform.Translate(Vector3.left * Time.deltaTime * speed);
		//transform.Translate(Vector3.right * Time.deltaTime * speed);
	

	}

	void OnTriggerEnter(Collider other){

		GameObject prefabFire = (GameObject) Instantiate(prefabPSFire.gameObject, new Vector3(transform.position.x, transform.position.y,-2), Quaternion.identity);
		Destroy(prefabFire,1);


		if(other.tag == "Player"){
			Debug.Log("DAÑO");
			// Instanciamos daño
			GameObject prefabDamage = (GameObject) Instantiate(prefabPSDamage.gameObject,new Vector3(transform.position.x, transform.position.y,-2), Quaternion.identity);


			Destroy(prefabDamage,1);
			CoreManager.Audio.Play(CoreManager.Audio.impactBoss,transform.position);
			other.GetComponent<PlayerLogic>().setDamage();
			Destroy(myTransform.gameObject);
			
		}

	}
}
