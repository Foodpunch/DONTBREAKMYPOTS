using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomData : TileData {

    //define how much space and tiles room takes
    //starts from center of the room
    public enum RoomTypes
    {
        Room1,
        Room2,
        Room3,
    };
    public RoomTypes roomType;
    public bool isConnected;   
}

public struct RoomDimensions
{

    //Edge of room is equals to its center +/- of its extents.

    public Vector2 roomCenter;

    public float leftEdge;
    public float rightEdge;
    public float topEdge;
    public float bottomEdge;

    public RoomDimensions(Collider2D room)                     //dimensions of the room
    {
        roomCenter = room.bounds.center;                        //center of the room
        leftEdge = roomCenter.x - room.bounds.extents.x;        //left side of the room
        rightEdge = roomCenter.x + room.bounds.extents.x;       //right side of he room
        topEdge = roomCenter.y + room.bounds.extents.y;         //top side of the room
        bottomEdge = roomCenter.y - room.bounds.extents.y;      //bottom edge of the room
    }
   
}
