﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour
{

    bool touching=false;
    public GameObject player;
    public float Modifier;
    //true if light, false if heavy
    // Start is called before the first frame update

    private void Update()
    {
       
    }
    public bool Gettouching()
    {
        return touching;
    }
    private void OnCollisionStay(Collision collision)
    {   
        if(collision.gameObject.layer == 10)
            touching = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10)
            touching = false;
    }
}


