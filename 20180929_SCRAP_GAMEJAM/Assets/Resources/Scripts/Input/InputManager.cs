using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	public float clickSensitivity = 0.5f;
	
	//private bool down = false;
	private bool dragging = false;
	private bool touchEnded = true;
	private Vector2 touchPosEnded;
	private InputBehaviour selectedInput = null;
	private Vector3 lastPosition,initialScreenPosition;
	private float downTime = 0; 
	public float minimumMagnitude = 0.05f;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
		/*Touch handling: */
		if( Input.touchCount == 1 ){
			Touch touch = Input.touches[0];
			if( touch.phase == TouchPhase.Began ){
				touchEnded=false; touchPosEnded = touch.position;
				handleDown(touch.fingerId, touch.position);
			}else if( selectedInput != null && touch.phase == TouchPhase.Moved ){
				touchPosEnded = touch.position;
				handleDrag( touch.fingerId, touch.position );
			}else if( selectedInput != null && touch.phase == TouchPhase.Ended ) {
				touchEnded = true;
				handleUp( touch.fingerId, touch.position );
			}
		} else if( Input.touchCount == 0 ){
			
			/*Mouse handling*/
			if( Input.GetMouseButtonDown(0) ){
				handleDown(0, Input.mousePosition);
			} else if( selectedInput != null && !touchEnded){ //Release touches
				touchEnded = true;
				handleUp( 0, touchPosEnded );
			} else if( selectedInput != null ){
				handleDrag(0, Input.mousePosition );
				if(  Input.GetMouseButtonUp(0) )
					handleUp(0, Input.mousePosition);
			}
		}
				
	}
	
	
	private void handleDown(int button, Vector3 position){
		selectedInput = getGameObject( position, out lastPosition );
		initialScreenPosition = position;
		if( selectedInput != null ){
			downTime = Time.time;
			//Debug.Log("Down [" + button + "]" );
			selectedInput.triggerDown( button );
		}
	}
	private void handleDrag( int button, Vector3 position ){
		Vector3 delta = getDelta( position, ref lastPosition );
		if( delta.magnitude> minimumMagnitude){
			dragging = true;
			selectedInput.triggerDrag( button, delta );
		}
	}
	private void handleUp( int button, Vector3 position ){
		selectedInput.triggerUp(button);
		Vector3 hitPosition;
		InputBehaviour triggered = getGameObject(position, out hitPosition);
		GameObject landingObject = null;
		if( triggered != null ) landingObject = triggered.gameObject;
		
		if( dragging ){
			dragging = false;
			//Vector3 delta = getDelta( position, ref lastPosition );
			selectedInput.triggerDrop( button, landingObject );
			
			if( (Time.time - downTime) < clickSensitivity ){
				//Gestures
				InputBehaviour.SwipeType gesture;
				Vector3 normDirection = Vector3.Normalize( position - initialScreenPosition );
				if( Mathf.Abs( normDirection.y ) < Mathf.Abs( normDirection.x ) ){
					//Horizontal
					if( normDirection.x > 0 ){
						gesture = InputBehaviour.SwipeType.RIGHT;
					} else {
						gesture = InputBehaviour.SwipeType.LEFT;
					}
				} else {
					//Vertical
					if( normDirection.y > 0 ){
						gesture = InputBehaviour.SwipeType.UP;
					}else{
						gesture = InputBehaviour.SwipeType.DOWN;
					}
				}
				selectedInput.triggerSwipe( button, gesture );
			}
			
		}else if( (Time.time - downTime) < clickSensitivity ){
				if( triggered == selectedInput )	//Avoid fast move
					selectedInput.triggerClick(button);
		}
		selectedInput = null; 
	}
	//private void handleClick(){}
	//private void handleDrop(){}
	
	
	
	private InputBehaviour getGameObject( Vector3 screenPos, out Vector3 point ){
		InputBehaviour result = null;
		
		// Construct a ray from the current touch coordinates
		Ray ray = GetComponent<Camera>().ScreenPointToRay( screenPos );
		RaycastHit hit;
		if ( Physics.Raycast( ray, out hit ) ) {
			result = hit.transform.gameObject.GetComponent<InputBehaviour>();
			point = hit.point;
		} else {
			point = Vector3.zero;
			//distance = 0;
		}
		
		return result;
	}
	
	private Vector3 getDelta( Vector3 current, ref Vector3 last ) {
		Vector3 result, destination;
		
		destination = GetComponent<Camera>().ScreenToWorldPoint( new Vector3( current.x, current.y, last.z - GetComponent<Camera>().transform.position.z ) );
		result = new Vector3( destination.x - last.x, destination.y - last.y );
		last.x = destination.x;
		last.y = destination.y;
		
		return result;
	}
	
	
}
