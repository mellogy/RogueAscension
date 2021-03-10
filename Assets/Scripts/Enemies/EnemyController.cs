using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Steppable
{
    public PlayerMovement player;
    public Transform movepoint;
    public enum MovementPattern { ChasePatrol, ChaseRandom, SetPath, RandomPath }
    public MovementPattern movementPattern = MovementPattern.RandomPath;
    public Vector3[] path;
    public LayerMask solidLayer;

    private Vector3 newMove;
    private float moveSpeed = 5f;
    private int pathIndex = 0;

    private void Start()
    {
        newMove = transform.position;
        player = FindObjectOfType<PlayerMovement>();
    }

    public virtual void Attack()
    {

    }

    public override void Step()
    {
        switch (movementPattern)
        {
            case MovementPattern.ChasePatrol:
                moveChase();
                break;
            case MovementPattern.ChaseRandom:
                moveChase();
                break;
            case MovementPattern.SetPath:
                moveSetPath();
                break;
            case MovementPattern.RandomPath:
                moveRandomPath();
                break;
            default:
                break;
        }

        if (Physics.CheckSphere(newMove, .2f, solidLayer))
        {
            newMove = transform.position;
        }
    }

    void moveSetPath()
    {
        pathIndex++;
        if (pathIndex >= path.Length)
        {
            pathIndex = 0;
        }

        newMove = transform.position + path[pathIndex];
    }

    void moveRandomPath()
    {
        newMove = transform.position + path[Random.Range(0, path.Length)];
    }

    void moveChase()
    {
        var viewDir = transform.position - player.gameObject.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, viewDir, out hit, Mathf.Infinity))
        {
            if (hit.transform == player.gameObject.transform)
            {
                //sees the player
                Vector3 toPlayer = Vector3.MoveTowards(transform.position, player.gameObject.transform.position, 1);
                newMove = Vector3.Normalize(SnapTo(toPlayer, 90));
            }
            else
            {
                if (movementPattern == MovementPattern.ChasePatrol)
                {
                    moveSetPath();
                } 
                else
                {
                    moveRandomPath();
                }
            }
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, newMove) > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, newMove, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z); //i know this is weird, but it keeps the enemies from sinking into the floor
        }
    }



    Vector3 SnapTo(Vector3 v3, float snapAngle)
    {
        float angle = Vector3.Angle(v3, Vector3.up);
        if (angle < snapAngle / 2.0f)          // Cannot do cross product 
            return Vector3.up * v3.magnitude;  //   with angles 0 & 180
        if (angle > 180.0f - snapAngle / 2.0f)
            return Vector3.down * v3.magnitude;

        float t = Mathf.Round(angle / snapAngle);
        float deltaAngle = (t * snapAngle) - angle;

        Vector3 axis = Vector3.Cross(Vector3.up, v3);
        Quaternion q = Quaternion.AngleAxis(deltaAngle, axis);
        return q * v3;
    }
}
