using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour, ITank
{
    // rotations  
    public float turnSpeed = 180f;          // Turning speed of the tank in degrees per second.
    public float turnTurretSpeed = 90f;     // Turning speed of the turret in degrees per second.


    // movement
    public float speed = 12f;               // Movement speed of the tank.
    private Rigidbody m_Rigidbody;


    // object detection
    public float heightMultiplier;          // Y Offset for the sight
    public float centerSightDist;           // The range of the AI sight
    public float outerSightDist;
    private bool obstacleLeft = false;
    private bool obstacleRight = false;
    private bool obstacleAhead = false;
    public float stoppingDist;

    // shoot
    public Rigidbody shell;
    public Transform fireTransform;
    public float shootSpeed;
    public float cooldown;
    public float bulletCooldown;

    private float timeShot;
    

    public Transform turretCanon;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    public Vector3 Targetpoint;

    private float health;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        health = 100;
        timeShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.green);

        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime;
        }
        
    }

    // Returns the current health of the tank
    public float GetHealth()
    {
        return health;
    }

    // Applie an specified amount of damage to the tank
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // The tanks moves either forward or backwards
    public void Move(float movementInputValue)
    {
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    // The tank rotates to the right or to the left
    public void Turn(float turnInputValue)
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    // Rotates the direction the turret is aiming
    public void TurnTurret()
    {
        //find the vector pointing from our position to the target
        _direction = (Targetpoint - turretCanon.position).normalized;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        turretCanon.transform.rotation = Quaternion.Slerp(turretCanon.rotation, _lookRotation, Time.deltaTime * turnTurretSpeed);
    }

    // Instantiates and fires a bullet
    public void Fire()
    {
        if (timeShot <= 0)
        {
            timeShot = cooldown;
            Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
            shellInstance.velocity = shootSpeed * fireTransform.forward;
            //timeShot = cooldown;
        }
    }

    public string ObstacleAhead()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, centerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }

    public string ObstacleRight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right), out hit, outerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }

    public string ObstacleLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right), out hit, outerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }
}

