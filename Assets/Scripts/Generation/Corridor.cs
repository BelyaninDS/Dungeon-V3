using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North, East, South, West,
}

public class Corridor
{
    public int startXpos;
    public int startYpos;
    public int corridorLength;
    public Direction direction;

    public int EndPositionX
    {
        get
        {
            if (direction == Direction.North || direction == Direction.South)
                return startXpos;
            if (direction == Direction.East)
                return startXpos + corridorLength - 1;
            return startXpos - corridorLength + 1; 
       }
    }

    public int EndPositionY
    {
        get
        {
            if (direction == Direction.East || direction == Direction.West)
                return startYpos;
            if (direction == Direction.North)
                return startYpos + corridorLength - 1;
            return startYpos - corridorLength + 1;
        }
    }

    public void SetupCorridor(Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns,int rows, bool firstCorridor)
    {
        direction = (Direction)Random.Range(0, 3);

        Direction oppositeDirection = (Direction)(((int)room.enteringCorridor + 2) % 4);

        if(!firstCorridor && direction == oppositeDirection)
        {
            int directionInt = (int)direction;
            directionInt++;
            directionInt = directionInt % 4;
            direction = (Direction)directionInt;
        }

        corridorLength = length.Random;

        int maxLength = length.m_Max;

        //Направление коридора
        switch (direction)
        {
            case Direction.North:
                startXpos = Random.Range(room.xPos, room.xPos + room.roomWidth - 1);
                startYpos = room.yPos + room.roomHeight;
                maxLength = rows - startYpos - roomHeight.m_Min;
                break;

            case Direction.East:
                startXpos = room.xPos + room.roomWidth;
                startYpos = Random.Range(room.yPos, room.yPos + room.roomHeight - 1);
                maxLength = columns - startXpos - roomWidth.m_Min;
                break;

            case Direction.South:
                startXpos = Random.Range(room.xPos, room.xPos + room.roomWidth);
                startYpos = room.yPos - 1;
                maxLength = startYpos - roomHeight.m_Min;
                break;

            case Direction.West:
                startXpos = room.xPos - 1;
                startYpos = Random.Range(room.yPos, room.yPos + room.roomHeight);
                maxLength = startXpos - roomWidth.m_Min;
                break;
        }
        corridorLength = Mathf.Clamp(corridorLength, 1, maxLength);
    }   
}
