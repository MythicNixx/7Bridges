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
        //resets the flag so damage can be taken again
        damaged = false;
    }

    public void TakeDamage()
    {
        //set flag for damage taken
        damaged = true;

        currentHealth -= 1;

        healthSlider.value = currentHealth;

        // ensures that when player dies, death method runs
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    // Removes player movement and any other functions related to player
    void Death()
    {
        isDead = true;
        GetComponent<PlayerController>().enabled = false;
    }
}
