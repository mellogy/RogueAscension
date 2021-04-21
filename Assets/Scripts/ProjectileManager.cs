using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProjectileManager : Steppable
{
    public Vector3 moveDirection = Vector3.zero;
    public float damage;
    public float duration = 8f;

    private float life = 0f;
    private GameObject target;
    private Transform movePoint;
    private float movementSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        target = new GameObject();
        target.transform.position = transform.position;
        movePoint = target.transform;
        transform.rotation = Quaternion.Euler(60, 0, 0);
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, movePoint.position) != 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);
        }
    }

    public override void Step()
    {
        life++;
        if (life >= duration)
        {
            if (gameObject.GetComponent<ExplodeOnDeath>())
            {
                gameObject.GetComponent<ExplodeOnDeath>().Explode();
            }

            Destroy(gameObject);
        }

        target.transform.position += moveDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(target);
        Destroy(gameObject);
    }
}
