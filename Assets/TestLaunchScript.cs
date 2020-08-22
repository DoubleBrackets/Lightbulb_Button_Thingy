using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLaunchScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 50, 0);
        }
    }
}
