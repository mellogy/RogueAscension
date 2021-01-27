using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    //------- Inspector vars
    [Header("Dungeon Parameters")]
    [SerializeField]
    private int dungeonWidth = 100;
    [SerializeField]
    private int dungeonHeight = 100;
    [SerializeField]
    private Tile[] floorTiles; 
    [SerializeField]
    private Tile[] wallTiles; //!!Important!! Make sure that this array is in the appropriate order for tile bitmasking (ref in the Google Drive folder)
    [SerializeField]
    private generatorType generator;

    [Header("Object Refs")]
    [SerializeField]
    private Tilemap floorMap; //tilemap for floors
    [SerializeField]
    private Tilemap decoMap; //tilemap for decor
    [SerializeField]
    private Tilemap solidMap; //tilemap for solid objects
    [SerializeField]
    private Tilemap fogMap; //tilemap for fog of war
    [SerializeField]
    private GameObject player; //player ref for fog of war

    //------- Internal vars
    private int[,] wallsMap;
    private enum generatorType { randomWalk, roguelike, maze }

    //class used for the roguelike generator to make things easier down the line
    private class RectangularRoom
    {
        public int x, y, width, height;
        internal int x2, y2;
        
        void Awake()
        {
            x2 = x + width;
            y2 = y + height;
        }

        public Vector2Int center
        {
            get
            {
                int centerX = (int)((x + x2) / 2);
                int centerY = (int)((y + y2) / 2);

                return new Vector2Int(centerX, centerY);
            }
        }


    }


    void Start()
    {
        InitFloorTiles();

        switch (generator)
        {
            case generatorType.randomWalk:
                wallsMap = RandomWalkDungeon();
                break;
            case generatorType.roguelike:
                wallsMap = RoguelikeDungeon(6,10,50);
                break;
            case generatorType.maze:
                wallsMap = RandomWalkDungeon();
                break;
            default:
                wallsMap = RandomWalkDungeon();
                break;
        }
        
        InitWallTiles();
    }

    /* 
     * -------------------------
     * ----- Tilemap inits -----
     * -------------------------
     */
    private void InitFloorTiles()
    {
        //reset and floodfill floor map
        floorMap.ClearAllTiles();
        for (int x = -(dungeonWidth / 2); x < (dungeonWidth / 2); x++)
        {
            for (int y = -(dungeonHeight / 2); y < (dungeonHeight / 2); y++)
            {
                Tile tile = floorTiles[Random.Range(0, floorTiles.Length)]; //pick a random tile from the array
                floorMap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private void InitWallTiles()
    {
        //reset wall map
        solidMap.ClearAllTiles();

        //fill map appropriately 
        for (int x = 1; x < dungeonWidth - 1; x++)
        {
            for (int y = 1; y < dungeonHeight - 1; y++)
            {
                /* ----- Tile bitmasking
                 * We use the map we made earlier to add up neighboring cells using binary values like this:
                 * = 1 =
                 * 2 X 4
                 * = 8 =
                 * And then use that result (north + west + east + south) to fetch the correct index from the wall tiles array we made earlier
                 * (which is why it needs to be in a specific order)
                 */

                if (wallsMap[x, y] == 1)
                { //only do this if the current tile should be a wall

                    //the y values are swapped here, for some reason. i don't know why. but it doesn't work otherwise, so don't touch. 
                    int north = wallsMap[x, y + 1];
                    int west = wallsMap[x - 1, y] * 2;
                    int east = wallsMap[x + 1, y] * 4;
                    int south = wallsMap[x, y - 1] * 8;
                    int tileIndex = north + east + south + west;

                    Tile tile = wallTiles[tileIndex];
                    solidMap.SetTile(new Vector3Int(x - (dungeonWidth / 2), y - (dungeonHeight / 2), 1), tile);
                }
            }
        }
    }

    /* 
     * ---------------------------------
     * ----- Generation algorithms -----
     * ---------------------------------
     */
    private int[,] RandomWalkDungeon()
    {
        //array to hold room layout
        int[,] map = new int[dungeonWidth,dungeonHeight];
        //TODO: is there really no easier way to clear an array? this feels inefficient
        for (int i = 0; i < dungeonWidth; i++)
        {
            for (int j = 0; j < dungeonHeight; j++)
            {
                map[i, j] = 1;
            }
        }

        //controller x/y/direction
        int cx = dungeonWidth / 2;
        int cy = dungeonHeight / 2;
        int cdir = Random.Range(0, 4);

        //how many steps should we take?
        int steps = 1500;

        //odds of changing direction as a 1/x chance
        int odds = 2;

        for (int i=0; i<steps; i++)
        {
            //carve current spot in map
            map[cx, cy] = 0;

            //are we gonna change direction?
            if (Random.Range(1,odds+1) == odds)
            {
                cdir = Random.Range(0, 4);
            }

            //move controller and clamp to dungeon bounds (we use a border of 2 to ensure there's room for outer walls later)
            cx += (int)Lengthdir_x(1, 90 * cdir);
            cy += (int)Lengthdir_y(1, 90 * cdir);

            cx = Mathf.Clamp(cx, 2, dungeonWidth - 2);
            cy = Mathf.Clamp(cy, 2, dungeonHeight - 2);
        }

        return map;
    }

    private int[,] RoguelikeDungeon(int minRoomSize, int maxRoomSize, int maxRooms)
    {
        int[,] map = new int[dungeonWidth, dungeonHeight];
        RectangularRoom[] rooms = new RectangularRoom[maxRooms];
        Vector2[] roomCenters = new Vector2[maxRooms];

        //TODO: is there really no easier way to clear an array? this feels inefficient
        for (int i = 0; i < dungeonWidth; i++)
        {
            for (int j = 0; j < dungeonHeight; j++)
            {
                map[i, j] = 1;
            }
        }

        //add rooms to the map
        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize + 1); //add 1 to account for exclusive range
            int roomHeight = Random.Range(minRoomSize, maxRoomSize + 1);
            int roomX1 = Random.Range(2, dungeonWidth - roomWidth - 1);
            int roomY1 = Random.Range(2, dungeonHeight - roomHeight - 1);
            int roomX2 = roomX1 + roomWidth;
            int roomY2 = roomY1 + roomHeight;

            for (int rx = roomX1 + 1; rx < roomX1 + roomWidth - 1; rx++)
            {
                for (int ry = roomY1 + 1; ry < roomY1 + roomHeight - 1; ry++)
                {
                    map[rx, ry] = 0;
                }
            }

            roomCenters[i] = new Vector2((roomX1 + roomX2) / 2, (roomY1 + roomY2) / 2);

        }

        //add tunnels
        for (int i=1;i<maxRooms;i++)
        {
            Vector2 pointA = roomCenters[i];
            Vector2 pointB = roomCenters[i - 1];

            for (int tx = (int)pointA.x; tx < (int)pointB.x; tx++)
            {
                for (int ty = (int)pointA.y; ty < (int)pointA.y; ty++)
                {
                    map[tx, ty] = 0;
                }
            }
        }

        player.transform.position = new Vector2(roomCenters[0].x - 50 + 0.5f, roomCenters[0].y - 50 + 0.5f);
        return map;
    }

    /* 
     * ----------------------------
     * ----- Helper functions -----
     * ----------------------------
     */

    //Takes a length and an angle in degrees and returns the x/y component of the vector
    private float Lengthdir_x(int len, float dir)
    {
        return len * Mathf.Cos(dir * Mathf.Deg2Rad);
    }

    private float Lengthdir_y(int len, float dir)
    {
        return len * -Mathf.Sin(dir * Mathf.Deg2Rad);
    }

    //checks if two rooms intersect
    private bool CheckRoomIntersect(RectangularRoom room1, RectangularRoom room2)
    {
        return (
            room1.x <= room2.x2 &&
            room1.x2 >= room2.x &&
            room1.y <= room2.y2 &&
            room1.y2 >= room2.y
        );
    }

    //dig an L-shaped tunnel between two rooms
    private void TunnelBetween(RectangularRoom room1, RectangularRoom room2, int[,] map)
    {
        int dice = Random.Range(0, 2);
        if (dice==0)
        {
            //dig horizontally, then vertically
            SetArrayRegion(map, room1.center.x, room1.center.y, room2.center.x, room1.center.y, 0);
            SetArrayRegion(map, room2.center.x, room1.center.y, room2.center.x, room2.center.y, 0);
        }
        else
        {
            //dig vertically, then horizontally
            SetArrayRegion(map, room1.center.x, room1.center.y, room1.center.x, room2.center.y, 0);
            SetArrayRegion(map, room2.center.x, room2.center.y, room1.center.x, room2.center.y, 0);
        }
    }

    //set a region of a 2d array
    private void SetArrayRegion(int[,] array, int x1, int y1, int x2, int y2, int val)
    {
        for (int i=x1;i<x2;i++)
        {
            for (int j=y1;j<y2;j++)
            {
                array[i, j] = val;
            }
        }
    }
}
