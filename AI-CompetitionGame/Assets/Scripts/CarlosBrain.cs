using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlosBrain : MonoBehaviour
{
    Tank tank;

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
    }

    private void FixedUpdate()
    {
        Debug.Log(timeLastTurn);

        obstacleLeft = tank.ObstacleLeft();
        obstacleAhead = tank.ObstacleAhead();
        obstacleRight = tank.ObstacleRight();

        if (obstacleAhead == "Tank" || obstacleLeft == "Tank" || obstacleRight == "Tank")
        {
            if (obstacleAhead == "Tank")
            {
                movementInputValue = 0;
                tank.Fire();
            }
        }

        else if (obstacleAhead == "Environment")
        {
            if (obstacleRight == "Environment" && obstacleLeft == "Environment")     // If there are obsctacle ahead, right and left,
            {
                movementInputValue = 0;                                              // then stop,

                if (!JustTurned())
                {
                    turnInputValue = 1;                                                   // turn right
                    timeLastTurn = 0;
                }
            }

            else
            {
                if(obstacleLeft == "Environment")       // If there is an obstacle ahead and left, then
                {
                    if (!JustTurned())
                    {
                        turnInputValue = 1;                                                   // turn right
                        timeLastTurn = 0;
                    }
                }

                else if(obstacleRight == "Environment") // If there is an obstacle ahead and right, then turn left
                {
                    if (!JustTurned())
                    {
                        turnInputValue = -1;                                                   // turn right
                        timeLastTurn = 0;
                    }
                }

                else                                    // If the obstacle is ahead, then slowdown and turn left
                {
                    movementInputValue = 0.5f;
                    if (!JustTurned())
                    {
                        turnInputValue = -1;                                                   // turn left
                        timeLastTurn = 0;
                    }
                }
            }
        }

        else
        {
            if (obstacleLeft == "Environment")
            {
                if (obstacleRight == "Environment")     // If there is an obstacle right and left, then keep moving without turning
                {
                    movementInputValue = 1;
                    turnInputValue = 0;
                    timeLastTurn = 0;
                }

                else                                    // If there is an obstacle left, then turn right
                {
                    if (!JustTurned())
                    {
                        turnInputValue = 0.5f;                                                   // turn right
                    }
                    turnInputValue = 0.5f;
                    timeLastTurn = 0;
                }
            }

            else if (obstacleRight == "Environment")    // If ther is an obstacle right, then turn left
            {
                if (!JustTurned())
                {
                    turnInputValue = -0.5f;                                                   // turn left
                    timeLastTurn = 0;
                }
            }

            else                                        // if there are not obstacles, then keep moving without turning
            {
                movementInputValue = 1;
                turnInputValue = 0;
            }
        }

        if(timeLastTurn > 5)
        {
            turnInputValue = randomTurn;
            if(timeLastTurn > 5.5f)
                timeLastTurn = 0;
        }
        else
        {
            randomTurn = Random.Range(-1, 1);
        }

        lastTurn = turnInputValue;
    }

    private bool JustTurned()
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
