using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float currentHealth = 3f;
    public float maxHealth = 3f;
    public bool dropLoot = false;
    public GameObject[] lootPool;
    public int minDrops = 2;
    public int maxDrops = 6;

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
            if (dropLoot)
            {
                int drops = Random.Range(minDrops, maxDrops);
                while (drops>0)
                {
                    GameObject toDrop = lootPool[Random.Range(0, lootPool.Length)];
                    Vector2 rnd = Random.insideUnitCircle;
                    Instantiate(toDrop, transform.position + new Vector3(rnd.x, 0, rnd.y), Quaternion.identity);
                    drops--;
                }
            }

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
