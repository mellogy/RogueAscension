using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    //this script marks something as destructable

    public GameObject[] lootPool;
    public int minLootDropped = 1;
    public int maxLootDropped = 3;
    public Sprite particle;
    public GameObject deathParticles;

    public void Explode()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        if (lootPool.Length > 0)
        {
            //drop loot when destroyed if there's any to drop
        }
    }
}
