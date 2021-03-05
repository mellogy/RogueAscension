using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) { GameObject.FindGameObjectWithTag("Player"); } //keep looking for the player until we've got it
        transform.position = player.transform.position + new Vector3(0, 10, -5);
    }
}
