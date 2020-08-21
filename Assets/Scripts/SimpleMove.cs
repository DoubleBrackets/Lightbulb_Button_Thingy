using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public LayerMask ground;
    private GameObject Obj;
    private Vector3 objdirection;
    private bool grabbed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2, ground))
            {
                print(hit.collider.gameObject.name);
                Obj = hit.collider.gameObject;
                objdirection = transform.position - hit.point;
                grabbed = true;
            }
        }
        if (grabbed)
        {
            Obj.transform.position = (transform.position - objdirection.normalized) * 1.25f;
            Obj.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Collider[] a = Obj.GetComponents<Collider>();
            foreach (Collider c in a)
            {
                c.isTrigger = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && grabbed)
        {
            Obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            grabbed = false;
            Collider[] a = Obj.GetComponents<Collider>();
            foreach (Collider c in a)
            {
                c.isTrigger = false;
            }

            Obj = null;

        }
    }
    public GameObject GetObject()
    {
        return Obj;
    }
}
