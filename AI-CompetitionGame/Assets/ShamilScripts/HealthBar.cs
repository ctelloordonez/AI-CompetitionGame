﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image healthBar;
    float shamilHealth = 100f;
    public static float health;
    public GameObject tank;

    

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        health = tank.GetComponent<Tank>().GetHealth();

    }
    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / shamilHealth;
    }
   
}
