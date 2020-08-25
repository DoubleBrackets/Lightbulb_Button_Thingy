using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BasicEnemy : MonoBehaviour
{
    NavMeshAgent me;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        me = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        me.SetDestination(target.position);
    }
}
