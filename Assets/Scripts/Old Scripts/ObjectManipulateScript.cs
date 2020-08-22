using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulateScript : MonoBehaviour
{
    public static ObjectManipulateScript objectManipulateScript;
    GameObject grabbedObject;

    CapsuleCollider coll;

    private float slowdownMultiplier = 0.5f;

    LayerMask raycastMask;

    private bool objectGrabbed = false;

    private void Awake()
    {
        objectManipulateScript = this;
        raycastMask = LayerMask.GetMask("MoveableObject");
        coll = gameObject.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !CharacterMovementScript.characterMovementScript.isSitting)
        {
            if(objectGrabbed)
            {
                ReleaseObject();
            }
            else
            {
                //Raycasts for object 
                Vector3 dir = Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.forward;
                RaycastHit hit;
                Vector3 p1 = transform.position + coll.center - (coll.bounds.extents.y - coll.radius - 0.1f) * Vector3.up;
                Vector3 p2 = p1 + (2 * coll.bounds.extents.y - 0.1f - coll.radius) * Vector3.up;
                Physics.CapsuleCast(p1, p2, coll.radius-0.1f, dir, out hit, 1f, raycastMask);
           
                if (hit.collider != null)
                {
                    GrabObject(hit.collider.gameObject,(transform.position - hit.collider.transform.position).magnitude+0.2f);
                                    
                }
            }
        }
    }

    private void GrabObject(GameObject obj,float dist)
    {
        CharacterMovementScript.characterMovementScript.canJump = false;
        objectGrabbed = true;
        grabbedObject = obj;
        //obj.GetComponent<Rigidbody>().mass = 0.01f;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        CreateJoint(dist);

        CharacterMovementScript.characterMovementScript.speed *= slowdownMultiplier;
    }

    
   

    private void ReleaseObject()
    {
        CharacterMovementScript.characterMovementScript.canJump = true;
        ConfigurableJoint joint = grabbedObject.GetComponent<ConfigurableJoint>();
        Destroy(joint);
        //grabbedObject.GetComponent<Rigidbody>().mass = 1f;
        grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        grabbedObject = null;
        objectGrabbed = false;
        CharacterMovementScript.characterMovementScript.speed /= slowdownMultiplier;
    }

    public bool GetObjectGrabbed()
    {
        return objectGrabbed; 
    }
    private void CreateJoint(float dist)
    {
        ConfigurableJoint joint = grabbedObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector3.zero;
        joint.enableCollision = true;
        var lim = joint.linearLimit;
        lim.limit = dist;
        joint.linearLimit = lim;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.connectedBody = GetComponent<Rigidbody>();
    }

    public GameObject GetTargetObject()
    {
        return grabbedObject;
    }
}
