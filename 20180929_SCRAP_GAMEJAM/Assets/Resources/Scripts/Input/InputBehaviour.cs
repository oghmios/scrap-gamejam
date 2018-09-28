using UnityEngine;
using System.Collections;

public class InputBehaviour : MonoBehaviour {
	
	public enum SwipeType { LEFT, RIGHT, UP, DOWN };
	
	private InputBehaviour _parent;
	
	public InputBehaviour parent {
		get { return _parent; }
		set { _parent = value; }
	}
	
	void OnEnable(){
		if( this.transform.parent != null )
			_parent = this.transform.parent.GetComponent<InputBehaviour>();
	}

	
	public delegate void ClickHandler( int button );
	public delegate void DownHandler( 	int button );
	public delegate void UpHandler( 	int button );
	public delegate void DragHandler( int button, Vector3 deltaPosition );
	public delegate void DropHandler( int button, GameObject toPlace );
	public delegate void SwipeHandler( int button, SwipeType type );
	
	public event ClickHandler 	onClick;
	public event DownHandler 	onDown;
	public event UpHandler 		onUp;
	public event DragHandler 	onDrag;
	public event DropHandler 	onDrop;
	public event SwipeHandler   onSwipe;
	
	public void triggerClick(int button){
		if( onClick != null ) onClick(button);
		else if( _parent != null ) _parent.triggerClick(button);
	}
	public void triggerDown(int button){
		if( onDown != null ) onDown(button);
		else if( _parent != null ) _parent.triggerDown(button);
	}
	public void triggerUp(int button){
 		if( onUp != null ) onUp(button);
		else if( _parent != null ) _parent.triggerUp(button);
	}
	public void triggerDrag(int button, Vector3 deltaPosition){
		if( onDrag != null ) onDrag(button, deltaPosition);
		else if( _parent != null ) _parent.triggerDrag(button,deltaPosition);
	}
	public void triggerDrop(int button, GameObject toPlace){
		if( onDrop != null ) onDrop(button, toPlace);
		else if( _parent != null ) _parent.triggerDrop(button,toPlace);
	}	
	public void triggerSwipe( int button, SwipeType type ){
		if( onSwipe != null ) onSwipe( button, type );
		else if( _parent != null ) _parent.triggerSwipe(button,type);
	}
		
}
