using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellUIManager : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField]
    Image ISlot;
    [SerializeField]
    Image JSlot;
    [SerializeField]
    Image KSlot;
    [SerializeField]
    Image LSlot;

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        if (player.IBound) 
        {
            ISlot.sprite = player.IBound.spellIcon;
            ISlot.enabled = true;
        } 
        else 
        {
            ISlot.enabled = false;
        }

        if (player.JBound)
        {
            JSlot.sprite = player.JBound.spellIcon;
            JSlot.enabled = true;
        }
        else
        {
            JSlot.enabled = false;
        }

        if (player.KBound)
        {
            KSlot.sprite = player.KBound.spellIcon;
            KSlot.enabled = true;
        }
        else
        {
            KSlot.enabled = false;
        }

        if (player.LBound)
        {
            LSlot.sprite = player.LBound.spellIcon;
            LSlot.enabled = true;
        }
        else
        {
            LSlot.enabled = false;
        }
    }
}
