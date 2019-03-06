using UnityEngine;
using System.Collections;

public class BossColliderMelee: MonoBehaviour {


	void OnTriggerEnter(Collider other){

		Debug.Log (other.tag);
		if(other.tag == "Player"){

			transform.parent.transform.GetComponent<BossLogic>().setAttack();
			
		}

	}
}
