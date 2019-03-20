using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{

    // rycst vars
    RaycastHit hit;
    public float centerSightDist;
    public float outerSightDist;
    private float distanceToObject;
    public float raycastLength;

    public float rotSpeed;
    private bool left;
    private bool right;

    //fire vars
    public Rigidbody shell;
    public Transform fireTransform;
    public float shellSpeed;
    public float timeInBetween;
    private float timeShot;



    public void Update()
    {
        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime;
        }
    }

    public void Fire()
    {
        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation);
        shellInstance.velocity = shellSpeed * fireTransform.forward;
        timeShot = timeInBetween;
    }

    public void RotateLeft()
    {
        if (left = true)
        {
            transform.Rotate(0, -rotSpeed, 0);
            left = true;
        }
    }

    public void RotateRight()
    {
        if (right = true)
        {
            transform.Rotate(0, rotSpeed, 0);
            right = true;
        }
       
    }

    private void FixedUpdate()
    {
        CheckForTank();
    }

    public void RotateStop()
    {
        left = false;
        right = false;
    }

    public void CheckForTank()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * raycastLength;
        Vector3 left = transform.TransformDirection(Vector3.left) * raycastLength;
        Vector3 right = transform.TransformDirection(Vector3.right) * raycastLength;
       
        Debug.DrawRay(transform.position + Vector3.up * 1.7f, (transform.forward + transform.right) * raycastLength, Color.yellow);
        Debug.DrawRay(transform.position + Vector3.up * 1.7f, (transform.forward - transform.right) * raycastLength, Color.yellow);


        if (Physics.Raycast(transform.position + Vector3.up * 1.7f, (transform.forward + transform.right), out hit, raycastLength))
        {
           
            if (hit.collider.gameObject.tag == "EnemyTank")
            {

                if(hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
                {
                    Fire();
                }

                RotateRight();
                Debug.Log("rightTank");
                
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up * 1.7f, (transform.forward - transform.right), out hit, raycastLength))
        {

            if (hit.collider.gameObject.tag == "EnemyTank")
            {

                if(hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
                {
                    Fire();
                }
                RotateLeft();
                Debug.Log("leftWorks");
            }
        }
    }
}
