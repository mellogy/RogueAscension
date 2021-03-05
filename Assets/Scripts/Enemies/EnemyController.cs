using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Steppable
{
    public PlayerMovement player;
    public Transform movepoint;
    private Vector3 newMove;
    private float moveSpeed = 5f;

    private void Start()
    {
        newMove = transform.position;
    }

    public virtual void Attack()
    {

    }

    public override void Step()
    {
        int xdir = Mathf.FloorToInt(Random.Range(-1, 2));
        int ydir = Mathf.FloorToInt(Random.Range(-1, 2));

        newMove = transform.position + new Vector3(xdir, ydir, 0);
        if (Physics2D.OverlapCircle(newMove, .2f))
        {
            newMove = transform.position;
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, newMove) > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, newMove, moveSpeed * Time.deltaTime); 
        }
    }
}
