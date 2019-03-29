using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlosBrain : MonoBehaviour
{
    public Transform ForwardFirePoint;

    Tank tank;
    GameObject target;
    Transform turret;

    float movementInputValue;
    float turnInputValue;
    float lastTurn;
    float randomTurn;
    float timeLastTurn;

    string obstacleLeft;
    string obstacleAhead;
    string obstacleRight;

    private void Start()
    {
        tank = GetComponent<Tank>();
        turret = tank.turretCanon.transform;

        movementInputValue = 1;
        turnInputValue = 0;
        lastTurn = turnInputValue;
        timeLastTurn = 0;

        obstacleLeft = null;
        obstacleAhead = null;
        obstacleRight = null;
    }

    private void Update()
    {
        timeLastTurn += Time.deltaTime;
        timeLastTurn = timeLastTurn % 60;

        tank.Move(movementInputValue);
        tank.Turn(turnInputValue);

        target = tank.target;

        if (target != null)                 // if a target is in range, rotate the turret aiming it
        {
            tank.Targetpoint = target.transform.position;
            tank.Targetpoint.y = turret.position.y;
            tank.TurnTurret();

            Vector3 aimDirection = target.transform.position - turret.position;
            Quaternion lookRotation = Quaternion.LookRotation(aimDirection);

            float angle = Quaternion.Angle(turret.rotation, lookRotation);
            if (angle < 15f)
            {
                tank.Fire();
            }
        }

        else                                // if not, rotate the turret back to the forward position
        {
            tank.Targetpoint = ForwardFirePoint.position;

            Vector3 aimDirection = tank.Targetpoint - turret.position;
            Quaternion lookRotation = Quaternion.LookRotation(aimDirection);

            float angle = Quaternion.Angle(turret.rotation, lookRotation);
            if (angle > 1f)
                tank.TurnTurret();
        }
    }

    private void FixedUpdate()
    {
        obstacleLeft = tank.ObstacleLeft();
        obstacleAhead = tank.ObstacleAhead();
        obstacleRight = tank.ObstacleRight();

        if (obstacleAhead == "Tank" || obstacleLeft == "Tank" || obstacleRight == "Tank")       // If there is other tank in front, it turns towards the opposite direction
        {
            if (obstacleAhead == "Tank")
            {
                turnInputValue = -1;
            }

            else if (obstacleLeft == "Tank")
            {
                turnInputValue = 1;
            }

            else if (obstacleRight == "Tank")
            {
                turnInputValue = -1;
            }
        }
            
        else if (obstacleAhead == "Environment")                                     // If there are obsctacle ahead, 
        {
            if (obstacleRight == "Environment" && obstacleLeft == "Environment")     //right and left,
            {
                movementInputValue = 0;                                              // then stop,

                if (!JustTurned())
                {
                    turnInputValue = 1;                                                   // and turn right.
                    timeLastTurn = 0;
                }
            }

            else                                                                    // If there are obsctacle ahead, 
            {
                if (obstacleLeft == "Environment")                                  // and left, then
                {
                    if (!JustTurned())
                    {
                        turnInputValue = 1;                                         // turn right
                        timeLastTurn = 0;
                    }
                }

                else if (obstacleRight == "Environment") // If there is an obstacle ahead and right, 
                {
                    if (!JustTurned())
                    {
                        turnInputValue = -1;                                                   // then turn left
                        timeLastTurn = 0;
                    }
                }

                else                                    // If the obstacle is ahead, 
                {
                    movementInputValue = 0.5f;          // then slowdown and turn left
                    if (!JustTurned())
                    {
                        turnInputValue = -1;            // and turn left
                        timeLastTurn = 0;
                    }
                }
            }
        }

        else
        {
            if (obstacleLeft == "Environment")
            {
                if (obstacleRight == "Environment")     // If there is an obstacle right and left, 
                {
                    movementInputValue = 1;             // then keep moving 
                    turnInputValue = 0;                 // without turning
                    timeLastTurn = 0;
                }

                else                                    // If there is an obstacle left, 
                {
                    if (!JustTurned())
                    {
                        turnInputValue = 0.5f;          // then turn right
                        timeLastTurn = 0;
                    }
                }
            }

            else if (obstacleRight == "Environment")    // If ther is an obstacle right,
            {
                if (!JustTurned())
                {
                    turnInputValue = -0.5f;             // then turn left
                    timeLastTurn = 0;
                }
            }

            else                                        // if there are not obstacles, then keep moving without turning
            {
                movementInputValue = 1;
                turnInputValue = 0;
            }
        }

        if (timeLastTurn > 5)                           // If it has not turned in five seconds, 
        {
            turnInputValue = randomTurn;                // then turn with a random direction during 0.5 seconds.
            if (timeLastTurn > 5.5f)
                timeLastTurn = 0;
        }
        else
        {
            randomTurn = Random.Range(-0.75f, 0.75f);
        }

        lastTurn = turnInputValue;
    }

    private bool JustTurned()                           // Check if the tank has turned 0.25 seconds ago
    {
        if (timeLastTurn < 0.25)
        {
            if (lastTurn != 0)
            {
                turnInputValue = lastTurn;
                timeLastTurn = 0;
            }
            return true;
        }
        return false;
    }
}
