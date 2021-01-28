using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckFog : MonoBehaviour
{
    [SerializeField]
    private Tilemap[] maps; //maps affected by fov
    [SerializeField]
    private int sightRadius = 3; //how far can we see
    [SerializeField]
    private Transform emitPoint; //where is this emitting from

    private Color dark = Color.gray;
    private Color lit = Color.white;
    private RaycastHit2D[] hit;

    private int frames = 0;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        frames++;
        if (frames % 4 == 0)
        {
            //TODO: This is DISGUSTINGLY inefficient, please fix asap
            foreach (var map in maps)
            {
                //farthest
                for (int x = (int)emitPoint.position.x - sightRadius - 1; x < (int)emitPoint.position.x + sightRadius + 1; x++)
                {
                    for (int y = (int)emitPoint.position.y - sightRadius - 1; y < (int)emitPoint.position.y + sightRadius + 1; y++ )
                    {
                        map.SetColor(new Vector3Int(x, y, 0), dark);
                    }
                }

                //nearest
                for (int x = (int)emitPoint.position.x - sightRadius; x < (int)emitPoint.position.x + sightRadius; x++)
                {
                    for (int y = (int)emitPoint.position.y - sightRadius; y < (int)emitPoint.position.y + sightRadius; y++)
                    {
                        map.SetColor(new Vector3Int(x, y, 0), lit);
                    }
                }
            }
        }
    }
}
