using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{

    // rycst vars
    RaycastHit hit;
    public float centerSightDist;
    public float outerSightDist;
    private float distanceToObject;
    public float raycastLength;

    public float rotSpeed;
    private bool rotLeft;
    private bool rotRight;

    //fire vars
    public Rigidbody shell;
    public Transform fireTransform;
    public float shellSpeed;
    public float shellCooldown;
    private float timeShot;
    public Rigidbody bullet;
    public float bulletSpeed;
    public float bulletCooldown;
    private float timeShotBullet;

    Quaternion startRotPos;

    private void Start()
    {
        startRotPos = transform.rotation;
    }

    public void Update()
    {
        if (timeShot > 0)
        {
            timeShot -= Time.deltaTime;
        }
        if (timeShotBullet > 0)
        {
            timeShotBullet -= Time.deltaTime;
        }
       // transform.Rotate(0, rotSpeed, 0);

    }

    public void FireShell()
    {
        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation);
        shellInstance.velocity = shellSpeed * fireTransform.forward;
        timeShot = shellCooldown;
    }
    public void FireBullet()
    {
        Rigidbody bulletInstance = Instantiate(bullet, fireTransform.position, fireTransform.rotation);
        bulletInstance.velocity = bulletSpeed * fireTransform.forward;
        timeShotBullet = bulletCooldown;
    }

    void RotateRight()
    {
        transform.Rotate(0, rotSpeed, 0);
        rotRight = true;
        FireShell();
    }
    
    void RotateLeft()
    {
        
        transform.Rotate(0, -rotSpeed, 0);
        rotLeft = true;
    }
    

    private void FixedUpdate()
    {
        CheckForTank();
        
    }
    
   public void GoToPrevPos()
    {
        
    }
    


    public void CheckForTank()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * raycastLength;
        Vector3 left = transform.TransformDirection(Vector3.left) * raycastLength;
        Vector3 right = transform.TransformDirection(Vector3.right) * raycastLength;
       
        Debug.DrawRay(transform.position + Vector3.up * 1.7f, (transform.forward + transform.right) * raycastLength, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 1.7f, (transform.forward - transform.right) * raycastLength, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 1.7f, transform.forward * raycastLength, Color.red);


        if (Physics.Raycast(transform.position + Vector3.up * 1.7f, transform.forward, out hit, raycastLength))
        {

            if (hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
            {

                FireShell();
               
            }
            if(hit.collider.gameObject.tag == "EnemyTank" && timeShotBullet <=0)
            {
                FireBullet();
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up * 1.7f, (transform.forward + transform.right), out hit, raycastLength))
        {
           
            if (hit.collider.gameObject.tag == "EnemyTank")
            {

                if(hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
                {
                    FireShell();
                  
                }
                if (hit.collider.gameObject.tag == "EnemyTank" && timeShotBullet <= 0)
                {
                    FireBullet();
                }
                RotateRight();
                Debug.Log("rightTank");
                
            }
        }
        else
        {
            //transform.rotation = startRotPos;
            
        }

        if (Physics.Raycast(transform.position + Vector3.up * 1.7f, (transform.forward - transform.right), out hit, raycastLength))
        {

            if (hit.collider.gameObject.tag == "EnemyTank")
            {

                if(hit.collider.gameObject.tag == "EnemyTank" && timeShot <= 0)
                {
                    FireShell();
                }
                if (hit.collider.gameObject.tag == "EnemyTank" && timeShotBullet <= 0)
                {
                    FireBullet();
                }
                RotateLeft();
                Debug.Log("leftWorks");
            }
        }
        rotLeft = false;
    }
}
