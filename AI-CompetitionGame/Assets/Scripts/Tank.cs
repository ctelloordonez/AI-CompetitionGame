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
    private float movementInputValue;
    private float turnInputValue;

    // object detection
    public float heightMultiplier;          // Y Offset for the sight
    public float centerSightDist;           // The range of the AI sight
    public float outerSightDist;
    private bool obstacleLeft = false;
    private bool obstacleRight = false;
    private bool obstacleAhead = false;
    public float stoppingDist;

    public Transform turretCanon;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    public Vector3 Targetpoint;

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

    }

    private void FixedUpdate()
    {
        Move();
        Turn();
        CheckSurface();
        TurnTurret();
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

    }

    // Changes the type of ammo the tank is shooting
    public void ChangeAmmo()
    {

    }

    // Checks the surface in order to recognice the terrain and move
    public void CheckSurface()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.green);

        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, centerSightDist))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * centerSightDist, Color.red);

                //Debug.Log("Tank: Terrain is blocking my way upfront");
                obstacleAhead = true;
                float distance = stoppingDist - hit.distance;
                //Debug.Log(distance);
                if (distance > 0)
                {
                    movementInputValue = 0;
                    turnInputValue = 3;
                }
            }
        }
        else
        {
            obstacleAhead = false;
        }


        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right), out hit, outerSightDist))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.red);
                //Debug.Log("Tank: Terrain is blocking my way on the right");
                obstacleRight = true;
            }

        }
        else
            obstacleRight = false;

        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right), out hit, outerSightDist))
        {
            if (hit.collider.gameObject.tag == "Environment")
            {
                Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.red);
                //Debug.Log("Tank: Terrain is blocking my way on the left");
                obstacleLeft = true;
            }
        }
        else
            obstacleLeft = false;
    }
}
