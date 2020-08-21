using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour
{
    public bool lightmass;
    public GameObject player;
    public bool grounded;
    //true if light, false if heavy
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
        ObjectManipulateScript a = player.GetComponent<ObjectManipulateScript>();
        if (!a.GetObjectGrabbed())
        {
            Debug.Log(a.GetObjectGrabbed());
            if (grounded && !lightmass)
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            }
            else
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        
    }
    private void OnCollisionStay(Collision collision)
    {
        grounded = true;

    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}


