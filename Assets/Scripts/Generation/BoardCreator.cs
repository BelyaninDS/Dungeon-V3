using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public enum TileType
    {
        Wall, Floor, TopWall, BorderWall,
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
    public GameObject[] outerWallTiles;

    public GameObject[] borderWallTiles;

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
            for(int j = -1; j <= currentRoom.roomWidth + 1; j++)
            {
                int xCoord = currentRoom.xPos + j;
                
                //Для каждого горизонтального тайла идем наверх
                for (int k = -1;k <= currentRoom.roomHeight + 1; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    //Если на месте уже есть стена или пол, ничего не делаем
                    if (tiles[xCoord][yCoord] == TileType.TopWall || tiles[xCoord][yCoord] == TileType.Floor || tiles[xCoord][yCoord] == TileType.BorderWall)
                        continue;

                    if(j == -1)
                        tiles[xCoord][yCoord] = TileType.BorderWall;                                //Южная граница

                    else if(j == currentRoom.roomWidth + 1)
                        tiles[xCoord][yCoord] = TileType.BorderWall;                                //Северная граница

                    else if(k == -1)
                        tiles[xCoord][yCoord] = TileType.BorderWall;                                //Западная граница

                    else if(k == currentRoom.roomHeight)
                        tiles[xCoord][yCoord] = TileType.TopWall;           //Задаем тайлы верхних стен   

                    else if (k == currentRoom.roomHeight + 1)
                        tiles[xCoord][yCoord] = TileType.BorderWall;                                //Восточная граница
                    else
                        tiles[xCoord][yCoord] = TileType.Floor;                                                 //Задаем тайлы пола на указанные позиции

                                    
                }
            }

            for(int j = -1;j <= currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;
                for (int k = -1;k <= currentRoom.roomHeight + 1; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                

          
                                             
                    
                    
                    
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

                //Задаем стены и граниы коридора
                if (currentCorridor.direction == Direction.North || currentCorridor.direction == Direction.South)
                {
                    if (tiles[xCoord + 1][yCoord] == TileType.TopWall || tiles[xCoord + 1][yCoord] == TileType.Floor || tiles[xCoord + 1][yCoord] == TileType.BorderWall)
                        continue;
                    else
                        tiles[xCoord + 1][yCoord] = TileType.BorderWall;

                    if (tiles[xCoord - 1][yCoord] == TileType.TopWall || tiles[xCoord - 1][yCoord] == TileType.Floor || tiles[xCoord - 1][yCoord] == TileType.BorderWall)
                        continue;
                    else
                        tiles[xCoord - 1][yCoord] = TileType.BorderWall;
                                   
                }
                else
                {
                    if (tiles[xCoord][yCoord + 1] == TileType.TopWall || tiles[xCoord][yCoord + 1] == TileType.Floor)
                        continue;
                    else
                    {
                        tiles[xCoord][yCoord + 1] = TileType.TopWall;
                        tiles[xCoord][yCoord + 2] = TileType.BorderWall;
                    }

                    if (tiles[xCoord][yCoord - 1] == TileType.TopWall || tiles[xCoord][yCoord - 1] == TileType.Floor || tiles[xCoord][yCoord - 1] == TileType.BorderWall)
                        continue;
                    else
                        tiles[xCoord][yCoord - 1] = TileType.BorderWall;
                    
                }
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

                else if (tiles[i][j] == TileType.BorderWall)
                    InstantiateFromArray(borderWallTiles, i, j);
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


