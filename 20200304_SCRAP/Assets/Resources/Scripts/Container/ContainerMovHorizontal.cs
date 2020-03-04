using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerMovHorizontal : MonoBehaviour {

    // http://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly

    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    public float timeTakenDuringLerp = 1f;

    /// <summary>
    /// How far the object should move when 'space' is pressed
    /// </summary>
    public float distanceToMove = 10;

    //Whether we are currently interpolating or not
    public bool isLeftToRight;

    //The start and finish positions for the interpolation
    private Vector3 startPosition;
    private Vector3 endPosition;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;

    private Transform myTransform;

    // Use this for initialization
    void Start () {
        myTransform = this.transform;

        if (isLeftToRight)
            setGoToEnd();
        else
            setGoToStart();
    }

    public void setGoToEnd() {


            isLeftToRight = true;
            _timeStartedLerping = Time.time;

            //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
            startPosition = myTransform.position;
            endPosition = myTransform.position + Vector3.right * distanceToMove;
    }

    public void setGoToStart() {

        isLeftToRight = false;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        endPosition = myTransform.position;
        startPosition = myTransform.position + Vector3.left * distanceToMove;

    }

    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {
        if (isLeftToRight)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            /*if (percentageComplete > 0 && percentageComplete < 0.5f)
            {
                startPosition += new Vector3(0, percentageComplete * -2, 0) * 0.25f;
            }
            else if (percentageComplete > 0.5f && percentageComplete < 1)
            {
                startPosition += new Vector3(0, Mathf.Abs(((percentageComplete * 2) - 2)) * 0.25f, 0);
            }*/
            // transform.position = Vector3.Slerp(startPosition, endPosition, percentageComplete);
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                setGoToStart();
            }
        }
        else {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            /*if (percentageComplete > 0 && percentageComplete < 0.5f)
            {
                startPosition += new Vector3(0, percentageComplete * -2, 0)*0.25f;
            }
            else if (percentageComplete > 0.5f && percentageComplete < 1) {
                startPosition += new Vector3(0, Mathf.Abs(((percentageComplete*2)-2))*0.25f, 0);
            }*/
            // transform.position = Vector3.Slerp(endPosition, startPosition, percentageComplete);
            transform.position = Vector3.Lerp(endPosition, startPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                setGoToEnd();
            }
        }
    }
}
