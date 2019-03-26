using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShamilAI : MonoBehaviour, ITank
{

    // movement vars
    public float speed;             
    private Rigidbody rb;
    private float movementInputValue;
    private float turnInputValue;
    public float turnSpeed;          
   
    // rycst vars
    RaycastHit hit;
    public float centerSightDist;           
    public float outerSightDist;
    public float stoppingDist;
    private float distanceToObject;
    public float raycastLength;

    //obstacls vars
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

    // Player health vars
    public int tankHealth = 100;
    public int shellDamage;
    public int bulletDamage;

    //particle effects and UI elements
    public GameObject explosionEffect;
    public GameObject text;



    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
        movementInputValue = 1;
        turnInputValue = 0;
        rb = GetComponent<Rigidbody>();
        Debug.Log(tankHealth);
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

    // Apply an specified amount of damage to the tank
    public void TakeDamage(float damage)
    {
        //used different function
       

    }
    //tank take damage function
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Shell")
        {
            tankHealth -= shellDamage;
            Debug.Log(tankHealth);
            HealthBar.health -= 25f;
           
            if (HealthBar.health == 0)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                text.SetActive(true);
                Destroy(gameObject,0.3f);
            }
        }
        else if(collision.gameObject.tag == "Bullet")
        {
            tankHealth -= bulletDamage;
            HealthBar.health -= 10f;
            Debug.Log(tankHealth);
            if(HealthBar.health == 0)
            {
                Destroy(gameObject,0.3f);
            }
        }
    }

    // The tanks moves either forward or backwards
    public void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    // The tank rotates to the right or to the left
    public void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    // Rotates the direction the turret is aiming
    public void TurnTurret()
    {
        //different script used for this function and attached to turret. 
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
    

   //AI obstacle check with raycast
    public void CheckSurface()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * raycastLength;
        Vector3 left = transform.TransformDirection(Vector3.left) * raycastLength;
        Vector3 right = transform.TransformDirection(Vector3.right) * raycastLength;
        Vector3 back = transform.TransformDirection(Vector3.back) * raycastLength;


        Debug.DrawRay(transform.position + Vector3.up * 2.8f, transform.forward * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 2.8f, (transform.forward + transform.right) * raycastLength, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 2.8f, (transform.forward - transform.right) * raycastLength, Color.green);
      

        if (Physics.Raycast(transform.position + Vector3.up * 3, transform.forward, out hit, raycastLength))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                Debug.Log(distanceToObject + " " + hit.collider.gameObject.name);
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
               
                enemyTankAhead = true;           

            }
        }
        else
        {
            obstacleAhead = false;
        }



        if (Physics.Raycast(transform.position + Vector3.up * 2.8f, (transform.forward + transform.right), out hit, raycastLength))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                
                print(distanceToObject + " " + hit.collider.gameObject.name);
                //Debug.Log("Tank: Terrain is blocking my way on the right");
                obstacleRight = true;
            }
            if (hit.collider.gameObject.tag == "EnemyTank")
            {
                enemyTankRight = true;   
            }
        }
        else
            obstacleRight = false;



        if (Physics.Raycast(transform.position + Vector3.up * 2.8f, (transform.forward - transform.right), out hit, raycastLength))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                print(distanceToObject + " " + hit.collider.gameObject.name);
                obstacleLeft = true;
            }
            if (hit.collider.gameObject.tag == "EnemyTank")
            {              
                enemyTankLeft = true;   
            }
           
        }
        else
            obstacleLeft = false;

    }
}

