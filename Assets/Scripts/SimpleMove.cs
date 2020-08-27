using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public static SimpleMove simpleMove;

    public LayerMask ground;
    private GameObject Obj;
    private Vector3 objdirection;
    private bool grabbed;
    public Transform holder;
    private float cooldown;
    private float cooldowntime;

    public float ForceMod;

    public bool isThrowing = false;

    int prevLayer;
    private void Awake()
    {
        simpleMove = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cooldowntime = .3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !CharacterMovementScript.characterMovementScript.isSitting && !CharacterMovementScript.characterMovementScript.isStunned)
        {
            if (Physics.BoxCast(transform.position-transform.up, new Vector3(0.25f,2,0.25f),transform.forward, out hit, Quaternion.identity,2, ground))
            {
                if (hit.collider.gameObject.GetComponent<ObjectBehavior>() == null)
                    return;
                if(hit.collider.gameObject.layer == 14)//Can only grab rope by the ends
                {
                    hit.collider.gameObject.transform.SetParent(null);
                    //Remove any attached joints when picking up rope
                    FixedJoint ropeAttachJoint = hit.collider.gameObject.GetComponent<FixedJoint>();
                    if (ropeAttachJoint != null)
                        Destroy(ropeAttachJoint);
                }
                //Transform work
                Obj = hit.collider.gameObject;
                prevLayer = Obj.layer;
                foreach(Transform obj in Obj.transform)
                    obj.gameObject.layer = 15;
                objdirection = transform.position - hit.point;
                grabbed = true;
                cooldown = cooldowntime;
                Obj.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                //Effects and movement slowdowns
                CharacterMovementScript.characterMovementScript.rotationFactor *= 5;
                CharacterMovementScript.characterMovementScript.speed /= 2f;
                PlayerParticleManager.playerParticleManager.PlayParticle("GrabParticles");
            }
        }
        if (grabbed)
        {
            holder.localPosition = new Vector3(holder.localPosition.x, Obj.GetComponent<ObjectBehavior>().Modifier, holder.localPosition.z); ;
            Obj.transform.position = holder.position;
            if (Input.GetMouseButtonDown(1))//Throwing
            {
                StartCoroutine(IsThrowingState());
                PlayerParticleManager.playerParticleManager.PlayParticle("LaunchParticles");
                Obj.GetComponent<Rigidbody>().AddForce((gameObject.transform.forward*ForceMod)/Obj.GetComponent<Rigidbody>().mass,ForceMode.VelocityChange);
                ReleaseObject();
            }
        }
        
    }

    IEnumerator IsThrowingState()
    {
        isThrowing = true;
        yield return new WaitForSeconds(0.75f);
        isThrowing = false;
    }
       

    private void LateUpdate()
    {
        if (Obj != null)
        {
            if(Input.GetKeyDown(KeyCode.E) && Obj.layer == 14)//Attaching rope to a moveableObject
            {
                RaycastHit hit;
                //raycast for targets
                Physics.BoxCast(transform.position-transform.up, new Vector3(0.4f, 2, 0.4f), transform.forward, out hit, Quaternion.identity, 3, LayerMask.GetMask("MoveableObject"));
                if(hit.collider != null)
                {
                    Rigidbody hitRb = hit.collider.GetComponent<Rigidbody>();
                    if (hitRb != null)
                    {
                        //Create joint and attach
                        Obj.transform.SetParent(hit.collider.gameObject.transform);
                        Obj.transform.position = hit.point;
                        FixedJoint ropeJoint = Obj.AddComponent<FixedJoint>();
                        ropeJoint.connectedBody = hitRb;
                        ReleaseObject();
                        return;
                    }              
                }
            }
            //Release object on mouserelease or stunned
            if ((Input.GetMouseButtonUp(0) || CharacterMovementScript.characterMovementScript.isStunned )&& grabbed)
            {
                ReleaseObject();
            }
            //Release object if hits terrain
            else if (Obj.GetComponent<ObjectBehavior>().Gettouching() && cooldown<=0)
            {
                ReleaseObject();
            }
        }
    }

    private void ReleaseObject()
    {
        Obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        CharacterMovementScript.characterMovementScript.rotationFactor /= 5;
        CharacterMovementScript.characterMovementScript.speed *= 2f;
        PlayerParticleManager.playerParticleManager.StopParticle("GrabParticles");
        Obj.layer = prevLayer;
        foreach (Transform obj in Obj.transform)
            obj.gameObject.layer = prevLayer;
        grabbed = false;
        Obj = null;

    }
    public GameObject GetObject()
    {
        return Obj;
    }
    public bool IsGrabbed()
    {
        return grabbed;
    }
}
