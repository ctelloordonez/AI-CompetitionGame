using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour, ITank
{
    public float speed = 12f;               // Movement speed of the tank.
    public float turnSpeed = 180f;          // Turning speed of the tank in degrees per second.
    public float turnTurretSpeed = 90f;     // Turning speed of the turret in degrees per second.

    private Rigidbody m_Rigidbody;
    private float movementInputValue;
    private float turnInputValue;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();   
    }

    // Update is called once per frame
    void Update()
    {
        movementInputValue = Input.GetAxis("Vertical");
        turnInputValue = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    public float GetHealth()
    {
        return 0;
    }

    public void TakeDamage(float damage)
    {

    }

    public void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    public void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    public void TurnTurret()
    {

    }

    public void Fire()
    {

    }

    public void ChangeAmmo()
    {

    }

    public void CheckSurface()
    {

    }
}
