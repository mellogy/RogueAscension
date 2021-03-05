using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OLD_DungeonGenerator : MonoBehaviour
{
    //------- Inspector vars
    [Header("Generation Parameters")]
    [SerializeField]
    private int dungeonWidth = 100;
    [SerializeField]
    private int dungeonHeight = 100;
    [SerializeField]
    private Tile[] floorTiles;
    [SerializeField]
    private Tile[] wallTiles; //!!Important!! Make sure that this array is in the appropriate order for tile bitmasking (ref in the Google Drive folder)
    [SerializeField]
    private Tile fogTile;
    [SerializeField]
    private generatorType generator;
    [SerializeField]
    private GameObject[] enemyTypes;
    [SerializeField]
    private Texture2D AllDirRooms;
    [SerializeField]
    private Texture2D LRRooms;
    [SerializeField]
    private Texture2D UDRooms;
    [SerializeField]
    private Texture2D blankRoom;
    [SerializeField]
    private Texture2D bonusRooms;

    [Header("Special Parameters")]
    [SerializeField]
    private int maxShops = 1;
    [SerializeField]
    private Texture2D shops;

    [Header("Object Refs")]
    [SerializeField]
    private Tilemap floorMap; //tilemap for floors
    [SerializeField]
    private Tilemap decoMap; //tilemap for decor
    [SerializeField]
    private Tilemap solidMap; //tilemap for solid objects
    [SerializeField]
    private GameObject playerObj; //player ref
    [SerializeField]
    private LayerMask solid;

    [Header("Debug")]
    [SerializeField]
    private bool noFog;

    private GameObject player;
    private CheckFog fog;
    private bool hasGeneratedEnemies = false;
    private int px;
    private int py;



    //------- Internal vars
    public int[,] wallsMap;
    private enum generatorType { randomWalk, roguelike, maze }
    private Color dark = new Color(.1f, .1f, .1f, 1f);
    private List<GameObject> enemies = new List<GameObject>();


    void Start()
    {
        if (noFog)
        {
            CheckFog fog = gameObject.GetComponent<CheckFog>();
            fog.enabled = false;
        }
        wallsMap = new int[dungeonWidth + 1, dungeonHeight + 1];
        InitFloorTiles();

        switch (generator)
        {
            case generatorType.randomWalk:
                wallsMap = RandomWalkDungeon();
                break;
            case generatorType.roguelike:
                wallsMap = RoguelikeDungeon(10, 10);
                break;
            case generatorType.maze:
                wallsMap = RandomWalkDungeon();
                break;
            default:
                wallsMap = RandomWalkDungeon();
                break;
        }

        InitWallTiles();

        player = Instantiate(playerObj, new Vector3(px - 0.5f, py - 0.5f, 0), Quaternion.identity);
    }

    public void Step()
    {
        foreach (var mob in enemies)
        {
            mob.GetComponent<MobController>().Step();
        }
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
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                Tile tile = floorTiles[Random.Range(0, floorTiles.Length)]; //pick a random tile from the array
                floorMap.SetTile(new Vector3Int(x, y, 0), tile);
                floorMap.SetTileFlags(tilePos, TileFlags.None);
                if (!noFog)
                {
                    floorMap.SetColor(tilePos, dark);
                }

            }
        }
    }

    public void InitWallTiles()
    {
        //reset wall map
        solidMap.ClearAllTiles();

        //set side walls
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                if (x == 0 || y == 0 || x == dungeonWidth - 1 || y == dungeonHeight - 1)
                {
                    wallsMap[x, y] = 1;
                }
            }
        }

        //fill map appropriately 
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
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
                    int north = wallsMap[x, Mathf.Clamp(y + 1, 0, dungeonHeight - 1)];
                    int west = wallsMap[Mathf.Clamp(x - 1, 0, dungeonWidth - 1), y] * 2;
                    int east = wallsMap[Mathf.Clamp(x + 1, 0, dungeonWidth - 1), y] * 4;
                    int south = wallsMap[x, Mathf.Clamp(y - 1, 0, dungeonHeight - 1)] * 8;
                    int tileIndex = north + east + south + west;

                    Vector3Int tilePos = new Vector3Int(x, y, 0);

                    Tile tile = wallTiles[tileIndex];
                    solidMap.SetTile(tilePos, tile);
                    solidMap.SetTileFlags(tilePos, TileFlags.None);
                    if (!noFog)
                    {
                        solidMap.SetColor(tilePos, dark);
                    }

                }
                else if (!hasGeneratedEnemies)
                {
                    int d = Random.Range(0, 25);
                    if (d == 1)
                    {
                        if (!Physics2D.OverlapCircle(new Vector2(x + 0.5f, y + 0.5f), .2f))
                        {
                            GameObject en = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length)], new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                        }
                    }
                }

            }
        }
        hasGeneratedEnemies = true;

    }

    /* 
     * ---------------------------------
     * ----- Generation algorithms -----
     * ---------------------------------
     */
    // -------------------- Random Walk
    private int[,] RandomWalkDungeon()
    {
        int enemyOdds = 10; //odds of spawning an enemy, 1/x

        //array to hold room layout
        int[,] map = new int[dungeonWidth, dungeonHeight];
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

        px = cx;
        py = cy;

        //how many steps should we take?
        int steps = 1500;

        //odds of changing direction as a 1/x chance
        int odds = 2;

        for (int i = 0; i < steps; i++)
        {
            //carve current spot in map
            map[cx, cy] = 0;

            if (Random.Range(1, enemyOdds + 1) == enemyOdds)
            {
                GameObject enemy;
                enemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length)], new Vector3(cx + 0.5f - dungeonWidth / 2, cy + 0.5f - dungeonHeight / 2, 0), Quaternion.identity);
                enemies.Add(enemy);
            }

            //are we gonna change direction?
            if (Random.Range(1, odds + 1) == odds)
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
    //----------------- Roguelike/Spelunky Like
    private int[,] RoguelikeDungeon(int roomWidth, int roomHeight)
    {
        //----------------------
        //- Initialization
        //----------------------
        int[,] map = new int[dungeonWidth, dungeonHeight];

        //TODO: is there really no easier way to clear an array? this feels inefficient
        for (int i = 0; i < dungeonWidth; i++)
        {
            for (int j = 0; j < dungeonHeight; j++)
            {
                map[i, j] = 1;
            }
        }

        //figure out how many rooms wide/tall the level is, then make a new array to store them
        int genWidth = (int)(dungeonWidth / roomWidth);
        int genHeight = (int)(dungeonHeight / roomHeight);
        Texture2D[,] rooms = new Texture2D[genWidth, genHeight];


        //clear array with bonus/optional rooms to start
        for (int x = 0; x < genWidth; x++)
        {
            for (int y = 0; y < genHeight; y++)
            {
                rooms[x, y] = bonusRooms;
            }
        }

        //start pos set to a random x value in the possible generator width
        int startPos = Random.Range(0, genWidth);

        //controller x/y
        int cx = startPos;
        int cy = 0;

        //set the first room as open, then move the player there
        rooms[cx, cy] = LRRooms;
        px = (cx * roomWidth) + (roomWidth / 2);
        py = (cy * roomHeight) + (roomHeight / 2);

        //var for checking to see if the main solution path is ready or not
        bool pathDone = false;

        //----------------------
        //- Main path generation
        //----------------------
        while (!pathDone)
        {
            int moveDir = Random.Range(1, 6);
            if (moveDir == 1 || moveDir == 2)
            {
                //Left
                if (rooms[Mathf.Clamp(cx - 1, 0, genWidth - 1), cy] == bonusRooms)
                {
                    cx -= 1;
                    cx = Mathf.Clamp(cx, 0, genWidth - 1);
                    rooms[cx, cy] = LRRooms;
                }
            }
            if (moveDir == 3 || moveDir == 4)
            {
                //Right
                if (rooms[Mathf.Clamp(cx + 1, 0, genWidth - 1), cy] == bonusRooms)
                {
                    cx += 1;
                    cx = Mathf.Clamp(cx, 0, genWidth - 1);
                    rooms[cx, cy] = LRRooms;
                }
            }
            if (moveDir == 5)
            {
                //Down
                rooms[cx, cy] = AllDirRooms;

                cy += 1;
                if (cy >= genHeight)
                {
                    //done!
                    pathDone = true;
                }
                else
                {
                    rooms[cx, cy] = AllDirRooms;
                }
            }
        }

        //----------------------
        //- Extra room generation
        //----------------------
        if (maxShops > 0)
        {
            int shopsMade = 0;
            while (shopsMade < maxShops)
            {
                //pick a random spot in the map
                int rx = Random.Range(0, genWidth);
                int ry = Random.Range(0, genHeight);

                //is this tile neighboring a 4-way intersection?
                if (
                    rooms[Mathf.Clamp(rx - 1, 0, genWidth-1), ry] == AllDirRooms ||
                    rooms[Mathf.Clamp(rx + 1, 0, genWidth-1), ry] == AllDirRooms ||
                    rooms[rx, Mathf.Clamp(ry - 1, 0, genHeight-1)] == AllDirRooms ||
                    rooms[rx, Mathf.Clamp(ry + 1, 0, genHeight-1)] == AllDirRooms )
                {
                    //if so, set a shop here and increment the counter
                    rooms[rx, ry] = shops;
                    shopsMade++;
                }
            }
        }

        for (int x = 0; x < genWidth; x++)
        {
            for (int y = 0; y < genHeight; y++)
            {
                int[,] room = SpriteToRoom(rooms[x, y]);
                int mx = x * roomWidth;
                int my = y * roomHeight;
                for (int i = 0; i < roomWidth; i++)
                {
                    for (int j = 0; j < roomHeight; j++)
                    {
                        if (room[i, j] > 1)
                        {
                            int d = Random.Range(0, room[i, j] + 1);
                            if (d == room[i, j])
                            {
                                map[mx + i, my + j] = 1;
                            }
                            else
                            {
                                map[mx + i, my + j] = 2;
                            }
                        }
                        else
                        {
                            map[mx + i, my + j] = room[i, j];
                        }
                    }
                }
            }
        }

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

    //set a region of a 2d array
    private void SetArrayRegion(int[,] array, int x1, int y1, int x2, int y2, int val)
    {
        for (int i = x1; i < x2; i++)
        {
            for (int j = y1; j < y2; j++)
            {
                array[i, j] = val;
            }
        }
    }

    //convert a particularly formatted sprite into an int array
    private int[,] SpriteToRoom(Texture2D spr)
    {
        int sprWidth = 10;
        int sprHeight = 10;
        //map of pixel colors to their corresponding array values
        Dictionary<Color, int> colorMap = new Dictionary<Color, int>
        {
            { Color.white, 0 },
            { Color.black, 1 },
            { Color.red, 2 }
        };

        int rndRange = spr.width / sprWidth;
        int sprX = Random.Range(0, rndRange) * sprWidth;


        //Color[] colors = spr.GetPixels(0, 0, sprWidth, sprHeight);
        int[,] r = new int[sprWidth, sprHeight];
        for (int x = sprX; x < sprX + sprWidth; x++)
        {
            for (int y = 0; y < sprHeight; y++)
            {
                Color c = spr.GetPixel(x, y);
                r[x - sprX, y] = colorMap[c];
            }
        }

        return r;
    }
}
