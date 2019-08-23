using UnityEngine;
using System.Collections;

public class GranadeLogic: MonoBehaviour {

	private Transform myTransform;
	public Transform prefabPSDamage;
	public Transform prefabPSFire;
	public AudioManager audioManger;
    public int typeBullet;
	// Use this for initialization
	void Start () {
        
        myTransform = this.transform;

	}

    // Update is called once per frame
    /*
    void Update () {
	
		// transform.Translate(Vector3.right * Time.deltaTime * speed);
		// myTransform.Translate(Vector3.forward*Time.deltaTime*speed);

	}
    
	void OnTriggerEnter(Collider other){

		GameObject prefabFire = (GameObject) Instantiate(prefabPSFire.gameObject, new Vector3(transform.position.x, transform.position.y,-2), Quaternion.identity);
		Destroy(prefabFire,1);


		if(other.tag == "Boss"){

				// Instanciamos daño
				GameObject prefabDamage = (GameObject) Instantiate(prefabPSDamage.gameObject,new Vector3(transform.position.x, transform.position.y,-2), Quaternion.identity);
				Destroy(prefabDamage,1);
			audioManger.Play(audioManger.impactBoss,transform.position);
				// other.GetComponent<BossLogic>().addDamage(damage);
				Destroy(myTransform.gameObject);

		}

		if(other.tag == "Enemy"){
			
			// Instanciamos daño
			GameObject prefabDamage = (GameObject) Instantiate(prefabPSDamage.gameObject,new Vector3(transform.position.x, transform.position.y,-2), Quaternion.identity);

			// prefabDamage.GetComponent<ParticleSystem>().startColor = Color.green;
			Destroy(prefabDamage,1);
			audioManger.Play(audioManger.impactBoss,transform.position);
			// other.GetComponent<EnemyLogic>().addDamage(damage);
			Destroy(myTransform.gameObject);
			
		}

	}*/
}
