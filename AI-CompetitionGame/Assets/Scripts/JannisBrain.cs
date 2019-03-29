using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannisBrain : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The target object will be used for the intercept calculations")]
    [SerializeField] private GameObject target; // the target he picked from the enemy array - used for intercept calculation
    private GameObject[] gos; // game object array for tanks
    private GameObject shooter; // the tower - used for intercept calculation
    private Tank tank;


    [Header("Extras needed for references or calculations")]
    [Tooltip("speed of the projectile")]
    public float shotSpeed = 0f;
    private Transform canon;

    [Header("Detection")]
    [Tooltip("Radius for detecting enemy tanks")]
    [SerializeField] private float detectionRadius = 0f;
    private bool fleeing = false;
    private float fleeTime = 3f;


    [Header("Navigation")]
    private string obstacleAhead;
    private string obstacleLeft;
    private string obstacleRight;

    [Header("Movement Values")]
    public float movementInputValue;
    public float turnInputValue;

    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
        canon = tank.turretCanon;
    }

    // Update is called once per frame
    void Update()
    {
        // individual obstacle checks for each ray and changes in movement
        checkFrontForObstacles();
        checkLeftForObstacles();
        checkRightForObstacles();
        // grouped checks
        groupedCheck();
        target = tank.target;
        
    }

    private void FixedUpdate()
    {
            obstacleAhead = tank.ObstacleAhead();
            obstacleLeft = tank.ObstacleLeft();
            obstacleRight = tank.ObstacleRight();
            behaviorStateManager();

    }

    #region Obstacle Checks

    /// <summary>
    /// checks for obstacles on the left and adjusts movement accordingly
    /// </summary>
    private void checkLeftForObstacles()
    {
        if (obstacleLeft != null && obstacleRight == null)
        {
            if (obstacleLeft == "Environment")
            {
                turnInputValue = 1;
            }

            if (obstacleLeft == "Tank")
            {
                // check for health or more
            }
        }
    }

    /// <summary>
    /// checks for obstacles ahead
    /// </summary>
    private void checkFrontForObstacles()
    {
        if (obstacleAhead != null) // if there is an collider hit by the ray
        {
            if (obstacleAhead == "Environment") // if ray hits part of the environment
            {
                movementInputValue = .5f;
            }
            if (obstacleAhead == "Tank") // if ray hits an enemy
            {
                // check for health and more
            }
        }
        else // if there is nothing hit by the ray
        {
            movementInputValue = 1;
        }
    }
    /// <summary>
    /// checks for obstacles on the right and adjusts movement accordingly
    /// </summary>
    private void checkRightForObstacles()
    {
        if(obstacleRight != null && obstacleLeft == null)
        {
            if(obstacleRight == "Environment")
            {
                turnInputValue = -1;
            }

            if(obstacleRight == "Tank")
            {
                // check for health or more
            }
        }
    }


    /// <summary>
    /// specific cases, when multiple checks have to be taken into account
    /// </summary>
    private void groupedCheck()
    {
        if (obstacleRight != null && obstacleLeft != null) // when objects block the both sites at the same time
        {
            if(obstacleRight == "Environment" && obstacleLeft == "Environment")
            {
                turnInputValue = 1;
            }
        }

        if (obstacleAhead == null && obstacleLeft == null && obstacleRight == null)
        {
            movementInputValue = 1;
            turnInputValue = 0;
        }
    }
    #endregion

    #region AI Preset Behaviors

    void behaviorStateManager()
    {
        if(target == null && fleeing == false)
        {
            Wandering();
        }
        if(target != null && fleeing == false)
        {
            attackMode();
            tank.TurnTurret();
        }
        if(target != null)
        {
            tank.TurnTurret();
        }
        if (fleeing == true)
        {
            FleefromEnemy();
        } 

    }

    private void Wandering()
    {
        tank.Move(movementInputValue);
        tank.Turn(turnInputValue);
    }

    private void attackMode()
    {
        tank.Move(movementInputValue);
        Vector3 direction = tank.target.transform.position - tank.turretCanon.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        if(Quaternion.Angle(tank.turretCanon.transform.rotation, lookRotation) <= 1f)
        {
            tank.Fire();
            fleeing = true;
        }
    }

    private void FleefromEnemy()
    {
        fleeTime -= Time.deltaTime;
        if(fleeTime >= -1)
        {
            tank.Move(1);
            tank.Turn(turnInputValue);
        }
        if(fleeTime < 1)
        {
            tank.Move(-.5f);
            tank.Turn(turnInputValue);
        }

        if(fleeTime <= 0)
        {
            fleeing = false;
            fleeTime = 2;
        }
    }
    #endregion


    ///////////////////// Gizmos //////////////////////////////////
    #region Show Gizmos
    void OnDrawGizmosSelected()
    {
        // Draw a sphere at the transform's position
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    #endregion

}
