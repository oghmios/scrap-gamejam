using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour
{
	public enum Dimensions {X,Y,Z}
	public float speed;
	public Dimensions dimension;

	void FixedUpdate () {

		switch(dimension){
			case Dimensions.X:
				transform.Rotate(speed * Time.deltaTime, 0, 0);
				break;
			case Dimensions.Y:
				transform.Rotate(0, speed * Time.deltaTime,0);
				break;
			case Dimensions.Z:
				transform.Rotate(0, 0, speed * Time.deltaTime);
				break;
		}
	}
}
