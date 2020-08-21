using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulateScript : MonoBehaviour
{
    public static ObjectManipulateScript objectManipulateScript;
    GameObject grabbedObject;

    private float slowdownMultiplier = 0.5f;

    LayerMask raycastMask;

    private bool objectGrabbed = false;

    private void Awake()
    {
        objectManipulateScript = this;
        raycastMask = LayerMask.GetMask("MoveableObject");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(objectGrabbed)
            {
                ReleaseObject();
            }
            else
            {
                //Raycasts for object
                RaycastHit hit;
                Physics.Raycast(transform.position, Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.forward, out hit, 1f, raycastMask);
                if(hit.collider != null)
                {
                    GrabObject(hit.collider.gameObject,(transform.position - hit.collider.transform.position).magnitude+0.1f);
                }
            }
        }
    }

    private void GrabObject(GameObject obj,float dist)
    {
        CharacterMovementScript.characterMovementScript.canJump = false;
        objectGrabbed = true;
        grabbedObject = obj;
        obj.GetComponent<Rigidbody>().mass = 0.01f;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        CreateJoint(dist);

        CharacterMovementScript.characterMovementScript.speed *= slowdownMultiplier;
    }

    private void ReleaseObject()
    {
        CharacterMovementScript.characterMovementScript.canJump = true;
        ConfigurableJoint joint = grabbedObject.GetComponent<ConfigurableJoint>();
        Destroy(joint);
        grabbedObject.GetComponent<Rigidbody>().mass = 1f;
        grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        grabbedObject = null;
        objectGrabbed = false;
        CharacterMovementScript.characterMovementScript.speed /= slowdownMultiplier;
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
