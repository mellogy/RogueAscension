using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float currentHealth = 3f;
    public float maxHealth = 3f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(float dmg)
    {
        currentHealth -= dmg;
        checkDeath();
    }

    void checkDeath()
    {
        if (currentHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }

    public void heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
