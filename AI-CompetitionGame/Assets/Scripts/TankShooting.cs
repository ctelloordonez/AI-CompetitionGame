using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public Rigidbody shell;
    public Transform fireTransform;
    public float speed;
    public float cooldown;
    public float bulletCooldown;

    private bool fired;
    private float timeShot;

    

    // Start is called before the first frame update
    void Start()
    {
        timeShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && timeShot <= 0)
        {
            FireShell();
        }

        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime; 
        }
    }

    private void FireShell()
    {
        fired = true;
        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = speed * fireTransform.forward;
        timeShot = cooldown;
    }   
   }

