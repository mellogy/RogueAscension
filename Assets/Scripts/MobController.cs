using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f; //how fast entity moves to target square
    [SerializeField]
    private Transform movePoint; //target entity is moving to
    [SerializeField]
    private LayerMask solid; //layermask for impassable objects
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AIType ai;

    private int dir;
    private enum AIType { randomWander }

    //TODO: This could probably be implemented in a better way
    private bool queueMove = false;


    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null; //ditch the parent to make sure the target stays still (if we didn't do this, it would move w the entity)
        dir = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (queueMove)
        {
            Step();
            queueMove = false;
        }

        if (Vector3.Distance(transform.position, movePoint.position) != 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }

    public void Step()
    {
        switch (ai)
        {
            case AIType.randomWander:
                moveRandom();
                break;
            default:
                moveRandom();
                break;
        }
    }

    private float Lengthdir_x(int len, float dir)
    {
        return len * Mathf.Cos(dir * Mathf.Deg2Rad);
    }

    private float Lengthdir_y(int len, float dir)
    {
        return len * -Mathf.Sin(dir * Mathf.Deg2Rad);
    }

    void moveRandom()
    {

        dir = Random.Range(0, 4);

        int mx = (int)Lengthdir_x(1, 90 * dir);
        int my = (int)Lengthdir_y(1, 90 * dir);

        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(mx, 0, 0), .25f, solid))
        {
            movePoint.position += new Vector3(mx, 0, 0);
        }

        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, my, 0), .25f, solid))
        {
            movePoint.position += new Vector3(0, my, 0);
        }
    }
}
