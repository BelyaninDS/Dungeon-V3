using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public enum TileType
    {
        Wall, Floor, TopWall, BottomWall, LeftWall, RightWall
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
    public GameObject[] topWallTiles;
    public GameObject[] bottomWallTiles;
    public GameObject[] leftWallTiles;
    public GameObject[] rightWallTiles;
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
        //Массив комнат рандомной размерности
        rooms = new Room[numRooms.Random];

        corridors = new Corridor[rooms.Length - 1];

        //Первая комната и коридор
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for(int i = 1; i < rooms.Length; i++)
        {
            //Новая комната
            rooms[i] = new Room();

            //Настройка комнаты на основании положения предыдущего коридора
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i-1]);

            if(i < corridors.Length)
            {
                //Создаем коридор
                corridors[i] = new Corridor();

                //Создаем комнату на основе последнего коридора
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }
    }


    void SetTilesValueForRooms()
    {
        //Прогоняем все комнаты
        for(int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];        //Текущая комната

            //По всей ширине текущей комнаты
            for(int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;
                tiles[xCoord][currentRoom.yPos + currentRoom.roomHeight] = TileType.TopWall ;
                tiles[xCoord][currentRoom.yPos - 1] = TileType.BottomWall;
                tiles[currentRoom.xPos + currentRoom.roomWidth][currentRoom.yPos + currentRoom.roomHeight] = TileType.RightWall;
                tiles[currentRoom.xPos - 1][currentRoom.yPos + currentRoom.roomHeight] = TileType.LeftWall;
                //Для каждого горизонтального тайла идем наверх
                for (int k = 0;k< currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    tiles[xCoord][yCoord] = TileType.Floor;        //Задаем тайлы пола на указанные позиции
                    tiles[currentRoom.xPos - 1][yCoord] = TileType.LeftWall;
                    tiles[currentRoom.xPos + currentRoom.roomWidth][yCoord] = TileType.RightWall;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        //Прогоняем все коридоры
        for (int i = 0; i< corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];        //Текущий коридор

            //Прогоняем по всей длине коридора
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                int xCoord = currentCorridor.startXpos;
                int yCoord = currentCorridor.startYpos;


                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    
                    case Direction.East:
                        xCoord += j;
                        break;

                    case Direction.South:
                        yCoord -= j;
                        break;

                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            for(int j = 0; j < tiles[i].Length; j++)
            {
                
                if (tiles[i][j] == TileType.Floor)
                    InstantiateFromArray(floorTiles, i, j);

                else if (tiles[i][j] == TileType.Wall)
                    InstantiateFromArray(wallTiles, i, j);

                else if (tiles[i][j] == TileType.TopWall)
                    InstantiateFromArray(topWallTiles, i, j);

                else if (tiles[i][j] == TileType.BottomWall)
                    InstantiateFromArray(bottomWallTiles, i, j);

                else if (tiles[i][j] == TileType.LeftWall)
                    InstantiateFromArray(leftWallTiles, i, j);

                else if (tiles[i][j] == TileType.RightWall)
                    InstantiateFromArray(rightWallTiles, i, j);

            }
        }
    }


    void InstantiateOuterWalls()
    {
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX -1f,bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord,float startingY,float endingY)
    {
        float currentY = startingY;

        while(currentY <= endingY)
        {
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }

    
    void InstantiateHorizontalOuterWall(float startingX,float endingX,float yCoord)
    {
        float currentX = startingX;

        while(currentX <= endingX)
        {
            InstantiateFromArray(outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }

    
    void InstantiateFromArray(GameObject[] prefabs,float xCoord, float yCoord)
    {
        int randomIndex = Random.Range(0, prefabs.Length);

        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        tileInstance.transform.parent = boardHolder.transform;
    }
}
