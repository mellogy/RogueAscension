using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField]
    TextMeshProUGUI boneText;
    [SerializeField]
    TextMeshProUGUI bombText;

    // Update is called once per frame
    void Update()
    {
        bombText.text = "0";

        if (!player)
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        if (player.IBound) 
        {
            ISlot.sprite = player.IBound.spellIcon;
            ISlot.enabled = true;

            if (player.IBound.GetComponent<Bomb>())
            {
                bombText.text = player.IBound.usesLeft.ToString();
            }
            
        } 
        else 
        {
            ISlot.enabled = false;
        }

        if (player.JBound)
        {
            JSlot.sprite = player.JBound.spellIcon;
            JSlot.enabled = true;

            if (player.JBound.GetComponent<Bomb>())
            {
                bombText.text = player.JBound.usesLeft.ToString();
            }
        }
        else
        {
            JSlot.enabled = false;
        }

        if (player.KBound)
        {
            KSlot.sprite = player.KBound.spellIcon;
            KSlot.enabled = true;

            if (player.KBound.GetComponent<Bomb>())
            {
                bombText.text = player.KBound.usesLeft.ToString();
            }
        }
        else
        {
            KSlot.enabled = false;
        }

        if (player.LBound)
        {
            LSlot.sprite = player.LBound.spellIcon;
            LSlot.enabled = true;

            if (player.LBound.GetComponent<Bomb>())
            {
                bombText.text = player.LBound.usesLeft.ToString();
            }
        }
        else
        {
            LSlot.enabled = false;
        }

        boneText.text = player.bones.ToString();
        

    }
}
