using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamilAI : MonoBehaviour, ITank
{

   
       


    // movement vars
    public float speed = 12f;             
    private Rigidbody m_Rigidbody;
    private float movementInputValue;
    private float turnInputValue;
    public float turnSpeed;          
    public float turnTurretSpeed;

    // rycst vars

    public float centerSightDist;           
    public float outerSightDist;
    public float stoppingDist;
    private float distanceToObject;

    //ostcls vars
    private bool obstacleLeft;
    private bool obstacleRight;
    private bool obstacleAhead;
    private bool enemyTankAhead;
    private bool enemyTankRight;
    private bool enemyTankLeft;


    //fire vars
    public Rigidbody shell;
    public Transform fireTransform;
    public float shellSpeed;
    public float timeInBetween;
    private float timeShot;



    // Start is called before the first frame update
    void Start()
    {
        movementInputValue = 1;
        turnInputValue = 0;
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (obstacleLeft)
            turnInputValue = 1;
        if (obstacleRight)
            turnInputValue = -1;

        if (obstacleRight && obstacleLeft)
            turnInputValue = 1;
        if (!obstacleAhead && !obstacleLeft && !obstacleRight)
        {
            movementInputValue = 1;
            turnInputValue = 0;
        }
        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
        CheckSurface();
    }

    // Returns the current health of the tank
    public float GetHealth()
    {
        return 0;
    }

    // Applie an specified amount of damage to the tank
    public void TakeDamage(float damage)
    {

    }

    // The tanks moves either forward or backwards
    public void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    // The tank rotates to the right or to the left
    public void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    // Rotates the direction the turret is aiming
    public void TurnTurret()
    {

    }

    // Instantiates and fires a bullet
    public void Fire()
    {
        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation);
        shellInstance.velocity = shellSpeed * fireTransform.forward;
        timeShot = timeInBetween;
    }

    // Changes the type of ammo the tank is shooting
    public void ChangeAmmo()
    {

    }

    // Checks the surface in order to recognice the terrain and move
    public void CheckSurface()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 2.8f, transform.forward * centerSightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 2.8f, (transform.forward + transform.right) * outerSightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 2.8f, (transform.forward - transform.right) * outerSightDist, Color.green);

        if (Physics.Raycast(transform.position + Vector3.up * 3, transform.forward, out hit, centerSightDist))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                print(distanceToObject + " " + hit.collider.gameObject.name);

                //Debug.Log("Tank: Terrain is blocking my way upfront");
                obstacleAhead = true;
                float distance = stoppingDist - hit.distance;
                if (distance > 0)
                {
                    movementInputValue = 0;
                    turnInputValue = 1;
                }
            }
            if (hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
            {
                Fire();
                enemyTankAhead = true;
            }
        }
        else
        {
            obstacleAhead = false;
        }




        if (Physics.Raycast(transform.position + Vector3.up * 2.8f, (transform.forward + transform.right), out hit, outerSightDist))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                
                print(distanceToObject + " " + hit.collider.gameObject.name);
                //Debug.Log("Tank: Terrain is blocking my way on the right");
                obstacleRight = true;
            }
            if (hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
            {
                Fire();
                enemyTankRight = true;
            }

        }
        else
            obstacleRight = false;

        if (Physics.Raycast(transform.position + Vector3.up * 2.8f, (transform.forward - transform.right), out hit, outerSightDist))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                print(distanceToObject + " " + hit.collider.gameObject.name);
                //Debug.Log("Tank: Terrain is blocking my way on the left");
                obstacleLeft = true;
            }
            if (hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
            {
                Fire();
                enemyTankLeft = true;
            }
        }
        else
            obstacleLeft = false;


    }
}

