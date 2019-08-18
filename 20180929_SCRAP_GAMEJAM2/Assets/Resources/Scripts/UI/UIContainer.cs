using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour {

    // on the character
    public Text mTextOverHead;
    private Transform mTransform;
    private Transform mTextOverTransform;
    void Awake()
    {
        mTransform = transform;
        mTextOverTransform = mTextOverHead.transform;
    }
    void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(mTransform.position);
        // add a tiny bit of height?
        screenPos.y += 2; // adjust as you see fit.
        mTextOverTransform.position = screenPos;
    }
}
