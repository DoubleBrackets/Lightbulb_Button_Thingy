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

    string currentState = "";

    private bool isBlinking = false;

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
        StartCoroutine(BlinkCycle());
    }

    private void Update()
    {
        if (PlayerButtonScript.playerButtonScript.isFlashing)
        {
            ChangeFaceState("flashing");
        }
        else if (CharacterMovementScript.characterMovementScript.isStunned)
        {
            ChangeFaceState("stunned");
        }
        else if (CharacterMovementScript.characterMovementScript.isSitting || CharacterMovementScript.characterMovementScript.isChangingPosition)
        {
            ChangeFaceState("sitting");
        }
        else if(SimpleMove.simpleMove.isThrowing)
        {
            ChangeFaceState("throwobject");
        }
        else if (SimpleMove.simpleMove.IsGrabbed())
        {
            ChangeFaceState("moveobject");
        }
        else if (rb.velocity.y > 0 && !CharacterMovementScript.characterMovementScript.isGrounded)
        {
            ChangeFaceState("jump");
        }
        else if (rb.velocity.y < 0 && !CharacterMovementScript.characterMovementScript.isGrounded)
        {
            ChangeFaceState("jump");
        }
        else if (isBlinking)
        {
            Blink();
        }
        else
        {
            ChangeFaceState("default");
        }
    }

    IEnumerator BlinkCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.5f + UnityEngine.Random.Range(0.5f,2f));
            isBlinking = true;
            yield return new WaitForSeconds(0.25f);
            isBlinking = false;
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

    private void Blink()
    {
        currentState = "blink";
        string currentFace = faceText.text;
        currentFace = "-" + currentFace[1] + "-";
        faceText.text = currentFace;
    }
    
}
