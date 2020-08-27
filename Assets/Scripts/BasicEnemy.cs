using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BasicEnemy : MonoBehaviour
{
    private Transform target;

    CharacterMovementScript a;

    float cooldown;
    float cooldowntime=0.8f;

    float immunityTime = 0.4f;
    float immunityTimer = 0f;

    float forcecounter=0;
    public float forcelimiter;
    private float forceLimit = 5f;
    public NavMeshAgent me;
    // Start is called before the first frame update
    void Start()
    {
        target = CharacterMovementScript.characterMovementScript.gameObject.transform;
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
            a.Stunned(false);
            immunityTimer = immunityTime;
        }
        if (immunityTimer >= 0)
        {
            immunityTimer -= Time.deltaTime;
        }
        else if (immunityTimer <= 0)
        {
            immunityTimer = 0;
            a.isInvuln = false;
        }

        me.SetDestination(target.position);
        if (Vector3.Distance(target.position,gameObject.transform.position)<1.5 && !a.isInvuln)
        {
            gameObject.GetComponent<Animator>().SetBool("Attacking", true);
            KickTarget();
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

    private void KickTarget()
    {
        Rigidbody rb = target.gameObject.GetComponent<Rigidbody>();
        float fmod = 20;
        if (rb != null )
        {
            a.isInvuln = true;
            rb.velocity = (-transform.forward * fmod);
            a.Stunned(true);
            cooldown = cooldowntime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            if (collision.gameObject.layer != 8)
            {
                float mag = (collision.relativeVelocity * collision.rigidbody.mass).magnitude;
                print(mag);
                if (mag > forceLimit)
                {
                    forcecounter += mag;
                }
            }
        }
    }
}
