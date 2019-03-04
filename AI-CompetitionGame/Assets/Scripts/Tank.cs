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

    // Returns the current health of the tank
    public float GetHealth()
    {
        return 0;
    }

    // Applie an specified amount of damage to the tank
    public void TakeDamage(float damage)
    {

    }

    // The tanks moves either forward or backwards
    public void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    // The tank rotates to the right or to the left
    public void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    // Rotates the direction the turret is aiming
    public void TurnTurret()
    {

    }

    // Instantiates and fires a bullet
    public void Fire()
    {

    }

    // Changes the type of ammo the tank is shooting
    public void ChangeAmmo()
    {

    }

    // Checks the surface in order to recognice the terrain and move
    public void CheckSurface()
    {

    }
}
