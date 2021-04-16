using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) { 
            player = FindObjectOfType<PlayerMovement>().gameObject; //keep looking for the player until we've got it
        }
        transform.position = player.transform.position + new Vector3(0, 10, -5);
    }
}
