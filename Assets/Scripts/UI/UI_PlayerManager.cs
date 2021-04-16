using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerManager : MonoBehaviour
{
    public HealthManager healthManager;
    public PlayerMovement playerMovement;
    public UI_HPManager hpManager;

    public bool updateHealth = true;

    // Update is called once per frame
    void Update()
    {
        if (updateHealth)
        {
            hpManager.UpdateHP();
            updateHealth = false;
        }
    }
}
