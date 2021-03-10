using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonepickup : MonoBehaviour
{
    private Transform target;
    private bool grav;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                target = c.gameObject.transform;
                grav = true;
            }
        }

        if (grav)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, .05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            other.gameObject.GetComponent<PlayerMovement>().bones++;
            Destroy(gameObject);
        }
    }
}
