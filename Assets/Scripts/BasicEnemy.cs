using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BasicEnemy : MonoBehaviour
{
    public Transform target;
    CharacterMovementScript a;
    float cooldown;
    float cooldowntime=.25f;
    float forcecounter=0;
    public float forcelimiter;
    public NavMeshAgent me;
    // Start is called before the first frame update
    void Start()
    {
        a = target.GetComponent<CharacterMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown >= 0)
        {
            cooldown -= Time.deltaTime;
        }
        else if(cooldown <= 0)
        {
            cooldown = 0;
            a.enabled = true;
        }
        me.SetDestination(target.position);
        if (Vector3.Distance(target.position,gameObject.transform.position)<2.5)
        {
            gameObject.GetComponent<Animator>().SetBool("Attacking", true);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("Attacking", false);
        }
        if (forcecounter >= forcelimiter)
        {
            Debug.Log("dead");
            gameObject.layer = 11;
            foreach( Transform child in transform)
            {
                if (child.name == "Armature")
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    child.gameObject.layer = 11;
                    if (child.gameObject.GetComponent<Rigidbody>())
                    {
                        child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        child.gameObject.GetComponent<ObjectBehavior>().enabled = true;
                    }
                }
            }
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            Destroy(gameObject.GetComponent<NavMeshAgent>());
            Destroy(gameObject.GetComponent<Animator>());
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float fmod = 20;
        if (gameObject.GetComponent<Animator>().GetBool("Attacking"))
        {
            if (other.gameObject.layer == 8)
            {
                other.gameObject.GetComponent<Rigidbody>().velocity=(-transform.forward*fmod);
                a.enabled = false;
                cooldown = cooldowntime;
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.relativeVelocity * collision.rigidbody.mass).magnitude>=3){
            if (collision.gameObject.layer != 8)
            {
                forcecounter+= (collision.relativeVelocity * collision.rigidbody.mass).magnitude;
            }
         
        }
    }
}
