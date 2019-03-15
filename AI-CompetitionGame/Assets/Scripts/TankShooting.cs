using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public Rigidbody shell;
    public Transform fireTransform;
    public float speed;
    public float cooldown;

    private float timeShot;

    // Start is called before the first frame update
    void Start()
    {
        timeShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && timeShot <= 0)
        {
            Fire();
        }

        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime;
        }
    }

    private void Fire()
    {
        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = speed * fireTransform.forward;
        timeShot = cooldown;
    }
}
