using UnityEngine;

public class Room
{
    public int xPos, yPos;
    public int roomWidth, roomHeight;

    public Direction enteringCorridor;

    //Первая комната
    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        //Высота и ширина
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        //Позиция комнаты
        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
    }

    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
    {
        //Направление входящего коридора
        enteringCorridor = corridor.direction;

        //Высота и ширина
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        switch (corridor.direction)
        {
            case Direction.North:
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);
                yPos = corridor.EndPositionY + 1;

                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1,corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            case Direction.East:
                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
                xPos = corridor.EndPositionX + 1;

                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;

            case Direction.South:
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);
                yPos = corridor.EndPositionY - roomHeight;

                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            case Direction.West:
                roomWidth = Mathf.Clamp(roomWidth, 1, corridor.EndPositionX);
                xPos = corridor.EndPositionX - roomWidth;

                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;
        }
    }
}

