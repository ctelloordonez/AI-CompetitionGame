using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnTankDamage : MonoBehaviour
{

    public int playerHealth = 30;
    int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(playerHealth);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Shell")
        {
            playerHealth -= damage;
            Debug.Log("takes Damage" + playerHealth);
        }
    }
}
