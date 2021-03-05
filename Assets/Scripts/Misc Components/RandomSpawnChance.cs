using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnChance : MonoBehaviour
{
    [Tooltip("As a decimal from 0 to 1, with 0 being 0% and 1 being 100% chance of spawning")]
    public float spawnChance = 0.5f;

    void Start()
    {
        float dice = Random.Range(0f, 1f);
        if (dice>spawnChance)
        {
            Destroy(gameObject);
        }
    }
}
