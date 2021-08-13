using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float health = 100f;


    // health reduced on damage
    public void TakeDamage(float damage) 
    {
        Health.currentHealth -= damage;
        health -= damage;
        if (health <= 0)
        {
            
            health = 0;
            GetComponent<DeathHandler>().HandleDeath();

        }
    }
   
}
