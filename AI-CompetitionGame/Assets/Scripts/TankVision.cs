using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankVision : MonoBehaviour
{
    public float heightMultiplier;
    public float centerSightDist;
    public float outerSightDist;

    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.green); 
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.green); 
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.green);
    }

    public bool CheckAhead()
    {
        if (Physics.Raycast (transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, centerSightDist))
        {
            if (hit.collider.gameObject.tag == "Enviroment")
            {
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.red);
                return true;
            }
        }

        return false;
    }

    public bool CheckRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right), out hit, outerSightDist))
        {
            if (hit.collider.gameObject.tag == "Enviroment")
            {
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.red);
                return true;            
            }
        }

        return false;
    }

    public bool CheckLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right), out hit, outerSightDist))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.red);
                return true;
            }
        }

        return false;
    }
}
