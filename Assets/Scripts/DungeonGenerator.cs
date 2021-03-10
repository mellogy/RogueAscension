using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Parameters")]
    [SerializeField]
    private int dungeonWidth = 8;
    [SerializeField]
    private int dungeonHeight = 5;
    [SerializeField]
    private int roomWidth = 10;
    [SerializeField]
    private int roomHeight = 10;
    [SerializeField]
    private int shopsToSpawn = 1;
    [SerializeField]
    private int bonusRoomsToSpawn = 3;
    public int seed = 0;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] fourWayRooms;
    [SerializeField]
    private GameObject[] horizontalRooms;
    [SerializeField]
    private GameObject[] verticalRooms;
    [SerializeField]
    private GameObject[] shops;
    [SerializeField]
    private GameObject[] bonusRooms;
    [SerializeField]
    private GameObject startRoom;
    [SerializeField]
    private GameObject solidRoom;

    [HideInInspector]
    public enum RoomTypes { Solid, FourWay, Horizontal, Vertical, Shop, Bonus, Start }
    [HideInInspector]
    public RoomTypes[,] dungeon;

    private void Start()
    {
        dungeon = GenerateDungeon(Random.Range(0, 1000)); //initialize the dungeon

        //spawn prefabs for each room
        for (int i = 0; i < dungeonWidth; i++)
        {
            for (int j = 0; j < dungeonHeight; j++)
            {
                RoomTypes room = dungeon[i, j];
                GameObject roomToSpawn;

                switch (room) //choose the room to spawn here
                {
                    case RoomTypes.Solid:
                        roomToSpawn = solidRoom;
                        break;
                    case RoomTypes.FourWay:
                        roomToSpawn = fourWayRooms[Random.Range(0, fourWayRooms.Length)];
                        break;
                    case RoomTypes.Horizontal:
                        roomToSpawn = horizontalRooms[Random.Range(0, horizontalRooms.Length)];
                        break;
                    case RoomTypes.Vertical:
                        roomToSpawn = verticalRooms[Random.Range(0, verticalRooms.Length)];
                        break;
                    case RoomTypes.Shop:
                        roomToSpawn = shops[Random.Range(0, shops.Length)];
                        break;
                    case RoomTypes.Bonus:
                        roomToSpawn = bonusRooms[Random.Range(0, bonusRooms.Length)];
                        break;
                    case RoomTypes.Start:
                        roomToSpawn = startRoom;
                        break;
                    default:
                        roomToSpawn = solidRoom;
                        break;
                }

                Instantiate(roomToSpawn, new Vector3(i * roomWidth, 0, j * roomHeight), Quaternion.identity);
            }
        }
    }

    private RoomTypes[,] GenerateDungeon(int seed = 0)
    {
        //Random.InitState(seed);

        //----------------------
        //- Initialization
        //----------------------
        RoomTypes[,] map = new RoomTypes[dungeonWidth, dungeonHeight]; //array to store the room types and spawn appropriately
        int shopsLeft = shopsToSpawn; //int to keep track of shops spawned
        int bonusLeft = bonusRoomsToSpawn; //int to keep track of bonus rooms spawned

        //initialize the array
        for (int i = 0; i < dungeonWidth; i++)
        {
            for (int j = 0; j < dungeonHeight; j++)
            {
                map[i, j] = RoomTypes.Solid;
            }
        }

        int startPos = Random.Range(0, dungeonWidth); //where are we starting?
        Vector2Int controller = new Vector2Int(startPos, 0); //vector2 to store our current position 
        bool pathDone = false; //are we finished creating the solution path?

        //----------------------
        //- Level borders
        //----------------------
        for (int i = -1; i < dungeonWidth + 1; i++)
        {
            for (int j = -1; j < dungeonHeight + 1; j++)
            {
                if (i == -1 || i == dungeonWidth || j == -1 || j == dungeonHeight)
                {
                    Instantiate(solidRoom, new Vector3(i * roomWidth, 0, j * roomHeight), Quaternion.identity);
                }
            }
        }


        //----------------------
        //- Main path generation
        //----------------------
        map[controller.x, controller.y] = RoomTypes.Start; //set the starting room here

        while (!pathDone)
        {

            int moveDir = Random.Range(1, 6);
            if (moveDir == 1 || moveDir == 2)
            {
                //Left
                controller.x -= 1;
                controller.x = Mathf.Clamp(controller.x, 0, dungeonWidth - 1); //clamp x value to dungeon bounds

                if (map[controller.x, controller.y] == RoomTypes.Solid) //only set this room if it hasn't already been given a value
                {
                    map[controller.x, controller.y] = RoomTypes.Horizontal;
                }

            }
            if (moveDir == 3 || moveDir == 4)
            {
                //Right
                controller.x += 1;
                controller.x = Mathf.Clamp(controller.x, 0, dungeonWidth - 1); //clamp x value to dungeon bounds

                if (map[controller.x, controller.y] == RoomTypes.Solid) //only set this room if it hasn't already been given a value
                {
                    map[controller.x, controller.y] = RoomTypes.Horizontal;
                }
            }
            if (moveDir == 5)
            {
                //Down
                map[controller.x, controller.y] = RoomTypes.FourWay;

                controller.y += 1;

                if (controller.y == dungeonHeight) //We've reached the end of the solution path
                {
                    pathDone = true;
                }
                else
                {
                    map[controller.x, controller.y] = RoomTypes.FourWay;
                }
            }
        }

        //----------------------
        //- Extra room generation
        //----------------------

        //TODO: There's probably a better way to do this, maybe refactor later?
        while (shopsLeft > 0) //shops
        {
            int rx = Random.Range(0, dungeonWidth - 1);
            int ry = Random.Range(0, dungeonHeight - 1);

            if (map[rx, ry] == RoomTypes.Solid)
            {
                if (
                    map[Mathf.Clamp(rx - 1, 0, dungeonWidth - 1), ry] == RoomTypes.FourWay ||
                    map[Mathf.Clamp(rx - 1, 0, dungeonWidth - 1), ry] == RoomTypes.Horizontal ||
                    map[Mathf.Clamp(rx + 1, 0, dungeonWidth - 1), ry] == RoomTypes.FourWay ||
                    map[Mathf.Clamp(rx + 1, 0, dungeonWidth - 1), ry] == RoomTypes.Horizontal
                    )
                {
                    map[rx, ry] = RoomTypes.Shop;
                    shopsLeft--;
                }
            }
        }

        while (bonusLeft > 0) //shops
        {
            int rx = Random.Range(0, dungeonWidth - 1);
            int ry = Random.Range(0, dungeonHeight - 1);

            if (map[rx, ry] == RoomTypes.Solid)
            {
                if (
                    map[Mathf.Clamp(rx - 1, 0, dungeonWidth - 1), ry] == RoomTypes.FourWay ||
                    map[Mathf.Clamp(rx - 1, 0, dungeonWidth - 1), ry] == RoomTypes.Horizontal ||
                    map[Mathf.Clamp(rx + 1, 0, dungeonWidth - 1), ry] == RoomTypes.FourWay ||
                    map[Mathf.Clamp(rx + 1, 0, dungeonWidth - 1), ry] == RoomTypes.Horizontal
                    )
                {
                    map[rx, ry] = RoomTypes.Bonus;
                    bonusLeft--;
                }
            }
        }

        //ensure the starting room spawned
        map[startPos, 0] = RoomTypes.Start;

        return map;
    }


}
