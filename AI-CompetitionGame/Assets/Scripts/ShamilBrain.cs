using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamilBrain : MonoBehaviour
{
    private Tank tank;
    private float movementInputValue;
    private float turnInputValue;
    private string obstacleAhead;
    private string obstacleLeft;
    private string obstacleRight;
    public float turnSpeed = 180f;
    public float turnTurretSpeed = 90f;

    public float speed = 12f;
    private Rigidbody m_Rigidbody;

    private void Start()
    {
        tank = GetComponent<Tank>();
        m_Rigidbody = GetComponent<Rigidbody>();
        movementInputValue = 1;
        turnInputValue = 0;       
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


        //check for surface
        if(obstacleAhead == "Environment")
        {
            movementInputValue = 0;
            turnInputValue = 1;
        }
        else
        {
            movementInputValue = 1;
            turnInputValue = 0;
        }

        if(obstacleLeft == "Environment")
        {
            movementInputValue = 0;
            turnInputValue = 1;
        }
        else
        {
            movementInputValue = 1;
            turnInputValue = 0;
        }

        if (obstacleRight == "Environment")
        {
            turnInputValue = -1;
            movementInputValue = 0;

        }
        else
        {
            movementInputValue = 1;
            turnInputValue = 0;
        }


        //check for tanks
        if(obstacleAhead == "Tank" || obstacleRight == "Tank" || obstacleLeft == "Tank")
        {
            movementInputValue = 0;
            tank.Fire();
        }

        if(obstacleRight == "Tank")
        {
            tank.TurnTurret();
        }
        if(obstacleLeft == "Tank")
        {
            tank.TurnTurret();
        }
    }
}
