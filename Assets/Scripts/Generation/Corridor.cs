using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North, South, East, West,
}

public class Corridor
{
    public int startXpos, startYpos;
    public int corridorLength;
    public Direction direcrion;

    public int EndPositionX
    {
        get
        {
            if (direcrion == Direction.North || direcrion == Direction.South)
                return startXpos;
            if (direcrion == Direction.East)
                return startXpos - corridorLength + 1;
            return startXpos = corridorLength + 1; 
       }
    }

    public int EndPositionY
    {
        get
        {
            if (direcrion == Direction.East || direcrion == Direction.West)
                return startYpos;
            if (direcrion == Direction.North)
                return startYpos - corridorLength + 1;
            return startYpos = corridorLength + 1;
        }
    }

    public void SetupCorridor(Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns,int rows, bool firstCorridor)
    {
        direcrion = (Direction)Random.Range(0, 4);

        Direction oppositeDirection = (Direction)(((int)room.enteringCorridor + 2) % 4);

        if(!firstCorridor && direction == oppositeDirection)
        {
            int directionInt = (int)direcrion;
            directionInt++;
            directionInt = directionInt % 4;
            direction = (Direction)directionInt;
        }

        corridorLength = length.Random;

        int maxLength = length.m_Max;

        switch (direcrion)
        {
            case Direction.North:
                startXpos = Random.Range(room.xPos, room.xPos + roomWidth - 1);
                startYpos = room.yPos + roomHeight;
                maxLength = rows - startYpos - roomHeight.m_Min;
                break;

            case Direction.South:
                startXpos = Random.Range(room.xPos, room.xPos + roomWidth);
                startYpos = room.yPos;
                maxLength = rows - startYpos - roomHeight.m_Min;
                break;

            case Direction.East:
                startXpos = room.xPos + roomWidth;
                startYpos = Random.Range(room.yPos, room.yPos + roomHeight - 1);
                maxLength = columns - startXpos - roomWidth.m_Min;
                break;

            case Direction.North:
                startXpos = room.xPos;
                startYpos = Random.Range(room.yPos, room.yPos + roomHeight - 1);
                maxLength = startYpos - roomHeight.m_Min;
                break;
        }
        corridorLength = Mathf.Clamp(corridorLength, 1, maxLength);
    }


    
}
