using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image healthBar;
    float maxHealth = 300;
    public static float health;
    public GameObject tank;

    

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        health = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (tank != null)
            health = tank.GetComponent<Tank>().GetHealth();

        else
            health = 0;

        healthBar.fillAmount = health / maxHealth;
    }
   
}
