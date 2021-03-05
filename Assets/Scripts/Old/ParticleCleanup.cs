using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleanup : MonoBehaviour
{
    float delayTime = 2f;

    private void Start()
    {
        Destroy(gameObject, delayTime);
    }
}
