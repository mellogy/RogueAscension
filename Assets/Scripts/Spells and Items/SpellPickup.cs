using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    public PlayerSpell mySpell;
    public Sprite myIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (mySpell.limitedUse == true)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerMovement p = other.gameObject.GetComponent<PlayerMovement>();

                if (p.JBound == mySpell)
                {
                    p.JBound.usesLeft++;
                    Destroy(gameObject);
                }
                else if (p.IBound == mySpell)
                {
                    p.IBound.usesLeft++;
                    Destroy(gameObject);
                }
                else if (p.LBound == mySpell)
                {
                    p.LBound.usesLeft++;
                    Destroy(gameObject);
                }
                else if (p.KBound == mySpell)
                {
                    p.KBound.usesLeft++;
                    Destroy(gameObject);
                }
                else
                {
                    getSpell(other);
                }

            }
        }
        else
        {
            getSpell(other);
        }
        
    }

    void getSpell(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMovement p = other.gameObject.GetComponent<PlayerMovement>();

            if (p.JBound == null)
            {
                p.JBound = mySpell;
                p.JBound.spellIcon = myIcon;
                p.JBound.player = p;
                Destroy(gameObject);
            }
            else if (p.IBound == null)
            {
                p.IBound = mySpell;
                p.IBound.spellIcon = myIcon;
                p.IBound.player = p;
                Destroy(gameObject);
            }
            else if (p.LBound == null)
            {
                p.LBound = mySpell;
                p.LBound.spellIcon = myIcon;
                p.LBound.player = p;
                Destroy(gameObject);
            }
            else if (p.KBound == null)
            {
                p.KBound = mySpell;
                p.KBound.spellIcon = myIcon;
                p.KBound.player = p;
                Destroy(gameObject);
            }
            else
            {
                p.JBound = mySpell;
                p.JBound.spellIcon = myIcon;
                p.JBound.player = p;
                Destroy(gameObject);
            }

        }
    }
}
