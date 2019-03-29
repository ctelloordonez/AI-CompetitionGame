using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannisBrain : MonoBehaviour
{
    

    #region References and Variables

    [Header("References")]
    [Tooltip("The target object will be used for the intercept calculations")]
    [SerializeField] private GameObject target; // the target he picked to shoot at
    private Tank tank; // reference for the tank script


    [Header("Extras needed for references or calculations")]
    private Transform canon; // the transform of the canon game object of my tank

    [Header("Detection")]
    [Tooltip("Radius for detecting enemy tanks")]
    [SerializeField] private float detectionRadius = 0f; // displays the detection radius of my tank
    private bool fleeing = false; // used to swap between behaviour states // when true, the tank will change its current movement to avoid taking damage
    private float fleeTime = 1f; // the time that the tank will flee


    [Header("Navigation")]
    private string obstacleAhead; 
    private string obstacleLeft;
    private string obstacleRight;

    [Header("Movement Values")]
    private float movementInputValue;
    private float turnInputValue;

    #endregion


    #region Unity Callbacks (Start, Update, FixedUpdate)

    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
        canon = tank.turretCanon;
        detectionRadius = tank.detectionRadius;
        fleeTime = 1f;
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

    #endregion


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
                movementInputValue = 1f;
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

    /// <summary>
    /// handles the changes in behavior of the tank based on gained Intel/Input of the Brain 
    /// and the Sensors to its disposal
    /// </summary>
    void behaviorStateManager()
    {
        if(target == null && fleeing == false) // if the tank has no target or isnt fleeing from one
        {
            Wandering();
        }
        if(target != null && fleeing == false) // if tank has found a target and isnt fleeing from one
        {
            attackMode();
            tank.TurnTurret();
        }
        if(target != null) // if tank has a target, it will always turn, no matter if he is fleeing or not
        {
            tank.TurnTurret();
        }
        if (fleeing == true) // if the tank is fleeing after an engage
        {
            FleefromEnemy();
        } 

    }

    /// <summary>
    /// the tank wanders around the battleground
    /// </summary>
    private void Wandering() 
    {
        tank.Move(movementInputValue);
        tank.Turn(turnInputValue);
    }

    /// <summary>
    /// The tank prepares his attack and starts attacking when possible
    /// </summary>
    private void attackMode()
    {
        tank.Move(movementInputValue);
        Vector3 direction = tank.target.transform.position - tank.turretCanon.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        if(Quaternion.Angle(tank.turretCanon.transform.rotation, lookRotation) <= 2f)
        {
            tank.Fire();
            fleeing = true;
        }
    }

    /// <summary>
    /// After the tank has attacked, he will start a small manouver to possibly dodge bullets of an enemy
    /// </summary>
    private void FleefromEnemy() 
    {
        fleeTime -= Time.deltaTime;
        if(fleeTime > 0)
        {
            tank.Move(-1f);
            tank.Turn(.1f);
        }
       
        if(fleeTime <= 0)
        {
            fleeing = false;
            fleeTime = 1f;
        }
    }
    #endregion


    ///////////////////// Gizmos //////////////////////////////////
    #region Show Gizmos
    void OnDrawGizmosSelected()
    {
        // Draw a sphere at the transform's position to show the detection radius
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    #endregion

}
