﻿using System.Collections;
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
    private GameObject closestEnemy = null; // the enemy tank closest to you
    [SerializeField] private float detectionRadius = 0f;
    [SerializeField] private float detectionCycleTime = 1f;
    public GameObject target = null;
    // object detection
    public float heightMultiplier;          // Y Offset for the sight
    public float centerSightDist;           // The range of the AI sight
    public float outerSightDist;

    // shoot
    public Rigidbody shell;
    public Transform fireTransform;
    public float shootSpeed;
    public float cooldown;
    private float timeShot;

    // turret rotation
    public Transform turretCanon;
    public Vector3 Targetpoint;
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // health
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        health = 300;
        timeShot = 0;
        StartCoroutine(scanCycle());
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

        //rotate over time according to speed until desired rotation is reached
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

    public string ObstacleLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right), out hit, outerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right) * outerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }

    public string ObstacleRight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right), out hit, outerSightDist))
        {
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right) * outerSightDist, Color.red);
            return hit.collider.gameObject.tag;
        }

        return null;
    }


    //////////////////// Find closest enemy ////////////////////////
    #region Find closest target 


    /// <summary>
    /// This function uses overlap sphere to find close enemy tanks by checking for the tag of hit objects
    /// The closest hit enemy tank will be set as the go "closestEnemy" and used for the intercept point calculations
    /// </summary>
    public void FindTargetInSurroundingArea()
    {
        if (target == null)
        {
            Collider[] col = Physics.OverlapSphere(transform.position, detectionRadius); // draw a sphere at desire point based on player pos + offset and desired radius of effect
            if (col.Length > 0)
            {
                float distance = Mathf.Infinity;
                foreach (Collider hit in col) // checks each object hit
                {
                    if (hit.tag == "Tank" && hit.gameObject != this.gameObject) // if hit object has equal tag to tank tag and unequal to the tank its casted from
                    {
                        Debug.Log("checking");
                        float diff = Vector3.Distance(transform.position, hit.transform.position);
                        float curDistance = diff;
                        if (curDistance < distance) // compare the distance between each hit tank. If distance to hit tank is smaller to the ones prev. checked
                        { // do the following
                            closestEnemy = hit.gameObject;
                            distance = curDistance;
                        }
                    }
                }
                target = closestEnemy;
            }
        }
        StartCoroutine(scanCycle());
    }

    private IEnumerator scanCycle()
    {
        yield return new WaitForSeconds(detectionCycleTime);
        FindTargetInSurroundingArea();
    }

    #endregion

}

