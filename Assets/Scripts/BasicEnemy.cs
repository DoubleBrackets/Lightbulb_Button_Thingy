using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BasicEnemy : MonoBehaviour
{
    NavMeshAgent me;
    public Transform target;
    CharacterMovementScript a;
    float cooldown;
    float cooldowntime=.25f;
    // Start is called before the first frame update
    void Start()
    {
        me = gameObject.GetComponent<NavMeshAgent>();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        float fmod = 20;
        Debug.Log("okay");
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
}
