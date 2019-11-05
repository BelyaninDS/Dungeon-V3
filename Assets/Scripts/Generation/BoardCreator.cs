using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public enum TileType
    {
        Wall, Floor,
    }

    public static int minNumRooms, maxNumRooms;
    public static int minRoomWidth, maxRoomWidth;
    public static int minRoomHeight, maxRoomHeight;
    public static int minCorridorLength, maxCorridorLength;


    public int columns;
    public int rows;
    public IntRange numRooms = new IntRange(minNumRooms, maxNumRooms);
    public IntRange roomWidth = new IntRange(minRoomWidth, maxRoomWidth);
    public IntRange roomHeight = new IntRange(minRoomHeight, maxRoomHeight);
    public IntRange corridorLength = new IntRange(minCorridorLength, maxCorridorLength);
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;


    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    private GameObject boardHolder;


    // Start is called before the first frame update
    private void Start()
    {
        boardHolder = new GameObject("BoardHolder");

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValueForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();
    }


    void SetupTilesArray()
    {
        tiles = new TileType[columns][];

        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileType[rows];
        }
    }


    void CreateRoomsAndCorridors()
    {
        rooms = new Room[numRooms.Random];

        corridors = new Corridor[rooms.Length - 1];

        //Create a 1st room and corridor
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for(int i = 0; i < rooms.Length; i++)
        {
            //Create a room
            rooms[i] = new Room();

            //Setup the room based on a previous corridor
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows);

            if(i < corridors.Length)
            {
                //Create a corridor
                corridors[i] = new Corridor();

                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }
    }


    void SetTilesValueForRooms()
    {
        //Go through all the rooms
        for(int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            //For each room go through their width
            for(int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                //For each horizontal tile, go up vertically through room's height 
                for(int k =0;k< currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        //Go through eve corridor
        for (int i = 0; i< corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            //For each corridor go through its length
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                int xCoord = currentCorridor.startXpos;
                int yCoord = currentCorridor.startYpos;


                switch (currentCorridor.direcrion)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.East:
                        yCoord += j;
                        break;
                    case Direction.West:
                        yCoord -= j;
                        break;
                }

                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
