using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float damage = 100f;
    public float lifeTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Carlos target = other.GetComponent<Carlos>();

        if (target != null)
        {
            target.TakeDamage(damage);
        }

       Destroy(gameObject);
    }
}
