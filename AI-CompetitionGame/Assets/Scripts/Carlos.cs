using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carlos : MonoBehaviour
{
    // rotations  
    public float turnSpeed = 180f;          // Turning speed of the tank in degrees per second.
    public float turnTurretSpeed = 90f;     // Turning speed of the turret in degrees per second.


    // movement
    public float speed = 12f;               // Movement speed of the tank.

    private Rigidbody m_Rigidbody;
    private float movementInputValue;
    private float turnInputValue;

    // object detection
    private string obstacleLeft;
    private string obstacleRight;
    private string obstacleAhead;
    private TankVision vision;

    // shoot
    public Rigidbody shell;
    public Transform fireTransform;
    public float shootSpeed;
    public float cooldown;

    private float timeShot;

    // health
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        movementInputValue = 1;
        turnInputValue = 0;
        timeShot = 0;
        health = 100;

        m_Rigidbody = GetComponent<Rigidbody>();
        vision = GetComponent<TankVision>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        obstacleAhead = vision.ObstacleAhead();
        obstacleRight = vision.ObstacleRight();
        obstacleLeft = vision.ObstacleLeft();

        if (obstacleAhead == "Tank" || obstacleLeft == "Tank" || obstacleRight == "Tank")
        {
            if (obstacleAhead == "Tank")
            {
                movementInputValue = 0;
                if (timeShot <= 0)
                    Fire();
            }
        }

        else if (obstacleAhead == "Environment")
        {
            if (obstacleRight == "Environment")
            {
                if (obstacleLeft == "Environment")
                {
                    movementInputValue = 0;
                    turnInputValue = 1;
                }

                else
                    turnInputValue = 1;
            }

            else
            {
                turnInputValue = 1;
            }
        }

        else
        {
            if (obstacleLeft == "Environment")
            {
                if (obstacleRight == "Environment")
                {
                    movementInputValue = 1;
                    turnInputValue = 0;
                }

                else
                    turnInputValue = 1;
            }

            else if (obstacleRight == "Environment")
            {
                turnInputValue = -1;
            }

            else
            {
                movementInputValue = 1;
                turnInputValue = 0;
            }
        }

        Move();
        Turn();
    }

    // Returns the current health of the tank
    public float GetHealth()
    {
        return 0;
    }

    // Applie an specified amount of damage to the tank
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
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
        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = shootSpeed * fireTransform.forward;
        timeShot = cooldown;
    }

    // Changes the type of ammo the tank is shooting
    public void ChangeAmmo()
    {
        
    }
}
