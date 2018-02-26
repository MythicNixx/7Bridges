// Chris Blanchard
// Tuesday 1/23/2018
// Controls the players damage taken and health level

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public int startHealth = 3;
    public int currentHealth;
    public Slider healthSlider;

    bool isDead;
    bool damaged;

    void Awake()
    {
        currentHealth = startHealth;
        healthSlider.maxValue = startHealth;
    }

    void Update()
    {
        currentHealth = (int) healthSlider.value;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    //public void TakeDamage()
    //{
    //    currentHealth -= 1;

    //    healthSlider.value = currentHealth;
    //}

    // Removes player movement and any other functions related to player
    void Death()
    {
        isDead = true;
        GetComponent<PlayerController>().enabled = false;
    }
}
