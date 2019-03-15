using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Radius : MonoBehaviour{

    public float Dradius = 5f;

    bool activateR = true;

    void Start()
    {
        GetComponent<NavMeshAgent>().enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
        radius();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Dradius);
    }

    public void radius()
    {
        if (activateR == true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Dradius, LayerMask.GetMask("enemy"));

            foreach (Collider nearByObject in colliders)
            {

                GetComponent<Tank>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = true;  
                GetComponent<AI>().enabled = true; 
                activateR = false; 
            }
        }
    }
    
}
