using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HPManager : MonoBehaviour
{
    private HealthManager hp;
    private GameObject[] hearts;
    private int currentHP;

    public UI_PlayerManager player;
    public GameObject heart;

    private void Start()
    {
        hearts = new GameObject[12];
        hp = player.healthManager;
    }

    public void UpdateHP()
    {
        
        currentHP = Mathf.RoundToInt(hp.currentHealth);
        print(currentHP.ToString());
        
        if (hearts.Length>0)
        {
            foreach(GameObject g in hearts)
            {
                Destroy(g);
            }
        }

        for (int i=1;i<currentHP;i++)
        {
            GameObject h = Instantiate(heart, new Vector2(heart.transform.position.x + (i * 72), heart.transform.position.y), Quaternion.identity);
            h.transform.parent = gameObject.transform;
            hearts[i] = h;
        }
    }
}
