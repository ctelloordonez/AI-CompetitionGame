using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamilAI : MonoBehaviour
{
     
    // movement vars
    public float tankMoveSpeed;             
    private Rigidbody rb;
    private float movementInputValue;
    private float turnInputValue;
    public float tankRotSpeed;

    // raycast vars
    RaycastHit hit;
    public float raycastLength;
    public float stoppingDist;
    private float distanceToObject;

    //obstacle vars
    private bool obstacleLeft = false;
    private bool obstacleRight = false;
    private bool obstacleAhead = false;

    //fire vars
    public Rigidbody shell;
    public Transform fireTransform;
    public float speed;
    public float timeInBetween;
    private float timeShot;


    // Start is called before the first frame update
    void Start()
    {
        timeShot = 0;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (obstacleLeft)
            turnInputValue = 1;
        if (obstacleRight)
            turnInputValue = -1;

        if (!obstacleAhead && !obstacleLeft && !obstacleRight)
        {
            movementInputValue = 1;
            turnInputValue = 0;
        }
        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime;
        }
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {                                      // written to check if the function works
            Fire();
        }
        */
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
        CheckSurface();
       
    }

   

    // The tanks moves either forward or backwards
    public void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * tankMoveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    // The tank rotates to the right or to the left
    public void Turn()
    {
        float turn = turnInputValue * tankRotSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    

    // Instantiates and fires a bullet
    public void Fire()
    {

        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation);
        shellInstance.velocity = speed * fireTransform.forward;
        timeShot = timeInBetween;
    }


    // Checks the surface in order to recognice the terrain and move
    public void CheckSurface()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward) * raycastLength;
        Vector3 left = transform.TransformDirection(Vector3.left) * raycastLength;
        Vector3 right = transform.TransformDirection(Vector3.right) * raycastLength;
        Vector3 back = transform.TransformDirection(Vector3.back) * raycastLength;

        Debug.DrawRay(transform.position + Vector3.up * 3, (forward) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 3, (forward + right) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 3, (forward - right) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 3, (back) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 3, (back + right) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 3, (back - right) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 3, (right) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 3, (left) * raycastLength, Color.green);



        if (Physics.Raycast(transform.position + Vector3.up * 3, (forward), out hit, raycastLength))
        {

            if (hit.collider.gameObject.tag == "Environment")
            {
                obstacleAhead = true;
                float distance = stoppingDist - hit.distance;
                print(distanceToObject + " " + hit.collider.gameObject.name);
                if (distance > 0)
                {
                    movementInputValue = 0;
                    turnInputValue = 1;
                }
            }
            if (hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
            {
                Fire();
            }
        }
        else
        {
            obstacleAhead = false;
        }

        if (Physics.Raycast(transform.position + Vector3.up * 3, (forward + right), out hit, raycastLength))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {

                obstacleRight = true;
            }
            if (hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
            {
                Fire();
            }

        }
        else
            obstacleRight = false;

        if (Physics.Raycast(transform.position + Vector3.up * 3, (forward - right), out hit, raycastLength))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {

                obstacleLeft = true;
            }
            if (hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
            {
                Fire();
            }
        }
        else
            obstacleLeft = false;


    }
}
