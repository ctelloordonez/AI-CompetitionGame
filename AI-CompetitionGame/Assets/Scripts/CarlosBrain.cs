using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlosBrain : MonoBehaviour
{
    Tank tank;

    float movementInputValue;
    float turnInputValue;

    string obstacleLeft;
    string obstacleAhead;
    string obstacleRight;

    private void Start()
    {
        tank = GetComponent<Tank>();

        movementInputValue = 1;
        turnInputValue = 0;
        obstacleLeft = null;
        obstacleAhead = null;
        obstacleRight = null;
    }

    private void Update()
    {
        tank.Move(movementInputValue);
        tank.Turn(turnInputValue);
    }

    private void FixedUpdate()
    {
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
            if (obstacleRight == "Environment" && obstacleLeft == "Environment")     // If there are obsctacle ahead, right and left, then stop and turn left
            {
                movementInputValue = 0;
                turnInputValue = 1;
            }

            else
            {
                if(obstacleLeft == "Environment")       // If there is an obstacle ahead and left, then turn right
                {
                    turnInputValue = -1;
                }

                else if(obstacleRight == "Environment") // If there is an obstacle ahead and right, then turn left
                {
                    turnInputValue = 1;
                }

                else                                    // If the obstacle is ahead, then slowdown and turn left
                {
                    movementInputValue = 0.5f;
                    turnInputValue = 1;
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
                }

                else                                    // If there is an obstacle left, then turn right
                {
                    turnInputValue = -0.5f;
                }
            }

            else if (obstacleRight == "Environment")    // If ther is an obstacle right, then turn left
            {
                turnInputValue = 0.5f;
            }

            else                                        // if there are not obstacles, then keep moving without turning
            {
                movementInputValue = 1;
                turnInputValue = 0;
            }
        }
    }
}
