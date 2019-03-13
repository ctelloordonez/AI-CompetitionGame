using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour{

    public float LookRadius = 10f;

    Transform target;
    NavMeshAgent agent;

    #region Singleton
    public static AI instance;

    private void Awake()
    {
        instance = this;
    }

#endregion

    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        target = AI.instance.Enemy.transform;
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= LookRadius)
        {
            agent.SetDestination(target.position); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}
