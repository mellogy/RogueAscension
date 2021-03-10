using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPool;

    void Start()
    {
        Instantiate(enemyPool[Random.Range(0, enemyPool.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
