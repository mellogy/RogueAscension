using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    public GameObject[] itemPool;

    void Start()
    {
        Instantiate(itemPool[Random.Range(0, itemPool.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
