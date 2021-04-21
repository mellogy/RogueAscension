using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    private SpriteMask sightRadius;

    [SerializeField]
    public PlayerSpell JBound;

    [SerializeField]
    public PlayerSpell IBound;

    [SerializeField]
    public PlayerSpell KBound;

    [SerializeField]
    public PlayerSpell LBound;

    public Vector3 dir = new Vector3(0, 0, -1);
    private WorldManager gameMaster;
    public HealthManager healthManager;
    private float health;
    public int bones = 0;
    public int invincibilityFrames = 30;
    private int iframe = 0;
    public UI_PlayerManager uiManager;



    void Start()
    {
        movePoint.parent = null; //ditch the parent to make sure the target stays still (if we didn't do this, it would move w the player)
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<WorldManager>();
        health = healthManager.currentHealth;

        uiManager.updateHealth = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        UseSpells();
        Debug();

        if (health != healthManager.currentHealth)
        {
            if (iframe==0 && health > healthManager.currentHealth)
            {
                //hurt
                uiManager.updateHealth = true;
            }
            health = healthManager.currentHealth;
        }

        if (iframe>0)
        {

        }
        else
        {

        }
    }

    void Debug()
    {

    }

    void MovePlayer()
    {
        //move player towards target if we aren't already there - otherwise, watch for input and move target accordingly
        if (Vector3.Distance(transform.position, movePoint.position) != 0f)
        {
            dir = movePoint.position - transform.position;
            anim.SetFloat("Horizontal", dir.x);
            anim.SetFloat("Vertical", dir.z);
            anim.SetBool("Walk", true);
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Walk", false);
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (Physics.OverlapSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0), .25f, solid).Length == 0)
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                    gameMaster.Step();
                }

            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) //use an elseif to prevent diagonal movement
            {
                if (Physics.OverlapSphere(movePoint.position + new Vector3(0, 0, Input.GetAxisRaw("Vertical")), .25f, solid).Length == 0)
                {
                    movePoint.position += new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
                    gameMaster.Step();

                }
                
            }
        }
    }

    void UseSpells()
    {
        //this section has a lot of repeated code
        //i can't think of a better way to do it right now, but ideally it would be fixed at some point

        if (Input.anyKeyDown)
        {
            string inString = Input.inputString.Substring(0, 1); //make sure we grab the first input only
            PlayerSpell toUse = null;
            bool useSpell = false;

            if (inString.Length > 0)
            {
                switch (inString)
                {
                    case "j":
                        if (JBound)
                        {
                            toUse = JBound;
                            useSpell = true;
                            if (JBound.limitedUse)
                            {
                                JBound.usesLeft--;
                                if (JBound.usesLeft <= 0)
                                {
                                    JBound = null;
                                }
                            }
                        }
                        break;
                    case "i":
                        if (IBound)
                        {
                            toUse = IBound;
                            useSpell = true;
                            if (IBound.limitedUse)
                            {
                                IBound.usesLeft--;
                                if (IBound.usesLeft <= 0)
                                {
                                    IBound = null;
                                }
                            }
                        }
                        break;
                    case "k":
                        if (KBound)
                        {
                            toUse = KBound;
                            useSpell = true;
                            if (KBound.limitedUse)
                            {
                                KBound.usesLeft--;
                                if (KBound.usesLeft <= 0)
                                {
                                    KBound = null;
                                }
                            }
                        }
                        break;
                    case "l":
                        if (LBound)
                        {
                            toUse = LBound;
                            useSpell = true;
                            if (LBound.limitedUse)
                            {
                                LBound.usesLeft--;
                                if (LBound.usesLeft <= 0)
                                {
                                    LBound = null;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }

                if (useSpell)
                {
                    PlayerSpell spell = Instantiate(toUse, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                }

            }
        }
    }
}
