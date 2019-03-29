using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class JannisBrain : MonoBehaviour
{
    /*
    [Header("References")]
    [Tooltip("The target object will be used for the intercept calculations")]
    private GameObject Target = null; // replace this go with a target found from the search mechanism
    public GameObject closestEnemy = null; // the enemy tank closest to you
    private GameObject[] gos; // game object array for tanks
    public GameObject shooter; // the tower - used for intercept calculation
    private GameObject target; // the target he picked from the enemy array - used for intercept calculation


    [Header("Positions")]
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
    [Tooltip("When True, a tank is inside the detection radius")]
    public bool enemyTankInRange = false;

    [Header("Detection")]
    [Tooltip("Radius for detecting enemy tanks")]
    [SerializeField] private float detectionRadius = 0f;
    [SerializeField] private float detectionCycleTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(scanCycle());
        shooter = this.gameObject;
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

    #region TriggerChecks
    private void OnTriggerStay(Collider other) // just constantly checks if there is an enemy tank in range
    {
        if(other.gameObject.tag == "Tank") 
        {
            enemyTankInRange = true;
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Tank")
        {
            enemyTankInRange = false;
        }
    }

    #endregion

    private IEnumerator scanCycle()
    {
        yield return new WaitForSeconds(detectionCycleTime);
        FindTargetInSurroundingArea();
    }


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






                                     //////////////////// Find closest enemy ////////////////////////
                                                         #region Find closest target 


    /// <summary>
    /// This function uses overlap sphere to find close enemy tanks by checking for the tag of hit objects
    /// The closest hit enemy tank will be set as the go "closestEnemy" and used for the intercept point calculations
    /// </summary>
    private void FindTargetInSurroundingArea()
    {
        if(enemyTankInRange == true && target == null)
        {
            Collider[] col = Physics.OverlapSphere(transform.position, detectionRadius); // draw a sphere at desire point based on player pos + offset and desired radius of effect
            if (col.Length > 0)
            {
                float distance = Mathf.Infinity;
                foreach (Collider hit in col) // checks each object hit
                {
                    if (hit.tag == "Tank" && hit.gameObject != this.gameObject) // if hit object has equal tag to tank tag and unequal to the tank its casted from
                    {
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
    #endregion

    #region Show Gizmos
    void OnDrawGizmosSelected()
    {
        // Draw a sphere at the transform's position
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    #endregion



    //////////////////// Intercept point ////////////////////////

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
*/