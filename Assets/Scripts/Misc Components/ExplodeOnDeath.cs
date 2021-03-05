using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    public GameObject deathParticles;
    public bool destroysObjects = false;

    private void OnDestroy()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Camera.main.GetComponent<CameraShake>().shakeDuration = 0.2f;

        if (destroysObjects)
        {
            Collider[] thingsToDestroy = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider c in thingsToDestroy)
            {
                if (c.gameObject.GetComponent<Destructable>() != null)
                {
                    Destroy(c.gameObject);
                }
            }
        }

    }
}
