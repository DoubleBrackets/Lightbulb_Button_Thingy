using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FaceAnimationScript : MonoBehaviour
{
    FaceAnimationScript faceAnimationScript;
    public TextMeshPro faceText;

    Rigidbody rb;

    string currentState = "default";

    [Serializable]
    public struct FaceState
    {
        public string face;
        public string key;
    }

    public FaceState[] states;

    private void Awake()
    {
        faceAnimationScript = this;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SimpleMove a = gameObject.GetComponent<SimpleMove>();
        if (CharacterMovementScript.characterMovementScript.isSitting || CharacterMovementScript.characterMovementScript.isChangingPosition)
        {
            ChangeFaceState("sitting");
        }
        else if (a.GetObject() != null)
        {
            ChangeFaceState("moveobject");
        }
        else if (rb.velocity.y > 0 && !CharacterMovementScript.characterMovementScript.isGrounded)
        {
            ChangeFaceState("jump");
        }
        else if(rb.velocity.y < 0 && !CharacterMovementScript.characterMovementScript.isGrounded)
        {
            ChangeFaceState("fall");
        }
        else
        {
            ChangeFaceState("default");
        }
    }

    public void ChangeFaceState(string key)
    {
        if (key == currentState)
            return;
        foreach(FaceState state in states)
        {
            if(state.key == key)
            {
                currentState = key;
                faceText.text = state.face;
            }
        }
    }
    
}
