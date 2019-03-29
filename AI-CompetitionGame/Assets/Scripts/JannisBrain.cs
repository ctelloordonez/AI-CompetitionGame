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

    [Header("Intercept Point")]
    //locations 
    [Tooltip("the calculated point the tank will shoot at")]
    public Vector3 interceptPoint;
    private Vector3 shooterPosition; // tower position
    private Vector3 targetPosition; // target position
    //velocities
    private Vector3 shooterVelocity; // tower velocity
    private Vector3 targetVelocity; // target velocity, when working with nav mesh, access the agent velocity, not rigidbody

    [Header("Extras needed for references or calculations")]
    [Tooltip("speed of the projectile")]
    public float shotSpeed = 0f;
    private Transform canon;

    [Header("Detection")]
    [Tooltip("Radius for detecting enemy tanks")]
    [SerializeField] private float detectionRadius = 0f;

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
        shooter = this.gameObject;
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
                movementInputValue = 0;
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
        if(target == null)
        {
            Wandering();
        }
        if(target != null)
        {
            attackMode();
        }
    }

    private void Wandering()
    {
        tank.Move(movementInputValue);
        tank.Turn(turnInputValue);
    }

    private void attackMode()
    {
        tank.Targetpoint = target.transform.position;
        tank.TurnTurret();
        tank.Move(movementInputValue);
        Vector3 direction = target.transform.position - canon.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        if(Quaternion.Angle(canon.rotation, lookRotation) <= 1f)
        {
            tank.Fire();
            Debug.Log("could fire");
            target = null;
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



    //////////////////// Intercept point /////////////////////////

    private void CallInterceptPoint()
    {
        shooterPosition = shooter.transform.position;
        targetPosition = target.transform.position;
        shooterVelocity = shooter.GetComponent<Rigidbody>() ? shooter.GetComponent<Rigidbody>().velocity : Vector3.zero;
        targetVelocity = target.GetComponent<Rigidbody>() ? target.GetComponent<Rigidbody>().velocity : Vector3.zero;
        interceptPoint = FirstOrderIntercept
        (
            shooterPosition,
            shooterVelocity,
            shotSpeed,
            targetPosition,
            targetVelocity
        );
    }


    #region Intercept Point calculation

    /// <summary>
    /// This Vector 3 calculates a point in the "future" on which a shot projectile of the tank is able to hit 
    /// the targeted enemy tank. This point is calculated based on the current enemy tank and its own velocity and takes the travel 
    /// time of the bullet into account.
    /// Any changes in velocity on the enemy site can lead into the projectile missing its target
    /// </summary>
    /// <param name="shooterPosition"></param>
    /// <param name="shooterVelocity"></param>
    /// <param name="shotSpeed"></param>
    /// <param name="targetPosition"></param>
    /// <param name="targetVelocity"></param>
    /// <returns></returns>
    public static Vector3 FirstOrderIntercept
        (
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity
        )

    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }
    //first-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }

    #endregion



}
