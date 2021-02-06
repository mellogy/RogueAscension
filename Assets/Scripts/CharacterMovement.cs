using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float movementSpeed = 5f; //how fast player moves to target square

    [SerializeField]
    public Transform movePoint; //target player is moving to

    [SerializeField]
    private LayerMask solid; //layermask for impassable objects

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private DungeonGenerator gameMaster;

    [SerializeField]
    private SpriteMask sightRadius;

    [SerializeField]
    private float health = 3f;


    void Start()
    {
        movePoint.parent = null; //ditch the parent to make sure the target stays still (if we didn't do this, it would move w the player)
    }

    // Update is called once per frame
    void Update()
    {
        //move player towards target if we aren't already there - otherwise, watch for input and move target accordingly
        if (Vector3.Distance(transform.position, movePoint.position) != 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);
            anim.SetBool("Walk", true);
        } 
        else
        {
            anim.SetBool("Walk", false);

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0), .25f, solid)) 
                { 
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                }
                //gameMaster.Step();
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) //use an elseif to prevent diagonal movement
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, Input.GetAxisRaw("Vertical"), 0), .25f, solid))
                {
                    movePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                }
                //gameMaster.Step();
            }
        }
    }
}
