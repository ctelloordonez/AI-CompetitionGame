using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamilBrain : MonoBehaviour
{
    //get component of Tank.cs
    private Tank tank;

    //movement vars
    private float movementInputValue;
    private float turnInputValue;

    //obstacles 
    private string obstacleAhead;
    private string obstacleLeft;
    private string obstacleRight;

    //tank and turret rotation
    private float turnSpeed = 180f;
    private float turnTurretSpeed = 90f;

    //speed vars
    private float speed = 12f;
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

        if(tank.target != null)
        {
            tank.TurnTurret();
            Vector3 direction = tank.target.transform.position - tank.turretCanon.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            if (Quaternion.Angle(tank.turretCanon.transform.rotation, lookRotation) <= 1f)
            {
                tank.Fire();
               
            }
        }
        else if(tank.target = null)
        {
            tank.TurnTurret();
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.zero);
            if (Quaternion.Angle(tank.turretCanon.transform.rotation, lookRotation) <= 1f)
            {
                turnInputValue = 0;

            }
        }


    }

    private void FixedUpdate()
    {
        obstacleLeft = tank.ObstacleLeft();
        obstacleAhead = tank.ObstacleAhead();
        obstacleRight = tank.ObstacleRight();


        //check for obstacles 
        if(obstacleAhead == "Environment" && obstacleRight == "Environment") //turns left if thers obstcale on right and ahead
        {
           // movementInputValue = 0;
            turnInputValue = -1;
        }

        else if(obstacleAhead == "Environment" && obstacleLeft == "Environment")  // if there are obstacles on left and ahead turn right
        {
           // movementInputValue = 0;   //tank stops and turns right;
            turnInputValue = 1;
        }
        else if(obstacleAhead == "Environment" && obstacleLeft == "Environment" && obstacleRight == "Environment")
        {
            movementInputValue = -1;
            turnInputValue = 1;
        }
        else if(obstacleLeft == "Environment" && obstacleRight == "Environment")
        {
            movementInputValue = -1;  //tanks goes back if there right and left obstacles
            
        }
        else
        {
            if (obstacleLeft == "Environment")
            {
                movementInputValue = 0;
                turnInputValue = 1;
            }
           
            else  if (obstacleRight == "Environment")
            {
                turnInputValue = -1;
                movementInputValue = 0;

            }
            else
            {
                movementInputValue = 1;
                turnInputValue = 0;
            }
        }
 

        //check for tanks
        if(obstacleAhead == "Tank" || obstacleRight == "Tank" || obstacleLeft == "Tank" && !this.gameObject)
        {
            movementInputValue = 0;
            tank.Fire();
        }

        if(obstacleRight == "Tank" && !this.gameObject)
        {
            tank.TurnTurret();    //turns the turret to target point (right)
           
            Vector3 direction = tank.target.transform.position - tank.turretCanon.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            if(Quaternion.Angle(tank.turretCanon.transform.rotation, lookRotation) <= 1f)
            {
                tank.Fire();
            }
        }
        if(obstacleLeft == "Tank" && !this.gameObject)
        {
            tank.TurnTurret();    //turns the turret to target point (right)
            Vector3 direction = tank.target.transform.position - tank.turretCanon.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            if (Quaternion.Angle(tank.turretCanon.transform.rotation, lookRotation) <= 1f)
            {
                tank.Fire();
            }
        }
    }

    void TurntTurretOriginalPos()
    {
        //resets the position of turret
    }
}
