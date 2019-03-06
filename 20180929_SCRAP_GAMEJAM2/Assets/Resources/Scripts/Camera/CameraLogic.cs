using UnityEngine;
using System.Collections;

public class CameraLogic : MonoBehaviour {

	private Transform playerTransform;
	private Transform myTransform;


	public float mapX = 80;
	public float mapY = 100;

	private float minX;
	private float maxX;
	private float minY;
	private float maxY;

	// Use this for initialization
	void Start () {
	
		myTransform = this.transform;
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		float vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;    
		float horzExtent = vertExtent * Screen.width / Screen.height;


		// Calculations assume map is position at the origin
		minX = horzExtent - mapX / 2;
		maxX = mapX / 2 - horzExtent;
		minY = vertExtent - mapY / 2;
		maxY = mapY / 2 - vertExtent;

	}
	
	// Update is called once per frame
	void Update () {
	
		if(playerTransform!=null)
			myTransform.position = new Vector3(playerTransform.position.x,playerTransform.position.y+5,-25);

	}

	void LateUpdate() {
		var v3 = transform.position;
		v3.x = Mathf.Clamp(v3.x, minX, maxX);
		v3.y = Mathf.Clamp(v3.y, minY, maxY);
		transform.position = v3;
	}
}
