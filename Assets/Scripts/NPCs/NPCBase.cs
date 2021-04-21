using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBase : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    string[] greetings = new string[] { "heya", "yo", "hi", "whats up" };
    int talkTimer = 0;

    void Update()
    {
        if (talkTimer>0)
        {
            talkTimer--;
            
        }
        dialogueText.color = new Color(0, 0, 0, talkTimer);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            ShowDialogue();
        }
    }

    void ShowDialogue()
    {
        dialogueText.text = greetings[Random.Range(0, greetings.Length)];
        talkTimer = 360;
    }
}
