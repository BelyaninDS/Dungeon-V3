using UnityEngine;

public class Room
{
    public int xPos, yPos;
    public int roomWidth, roomHeight;

    public Direction enteringCorridor;

    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        xPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
    }

    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        enteringCorridor = Corridor.direction;

        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        switch (Corridor.direction)
        {
            case Direction.North:
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.direction);
                yPos = corridor.EndPositionY;
                xPos = Random.Range(corridor);


        }

    }
}

