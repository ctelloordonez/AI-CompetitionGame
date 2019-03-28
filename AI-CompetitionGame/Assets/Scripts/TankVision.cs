using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankVision : MonoBehaviour
{
    public float heightMultiplier;      // y Offset for the sight
    public float centerSightDist;       // The range of the AI sight
    public float outerSightDist;

    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.green); 
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.green); 
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.green);
    }

    public string ObstacleAhead()
    {
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, centerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }

    public string ObstacleLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right), out hit, outerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }

    public string ObstacleRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right), out hit, outerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }
}
